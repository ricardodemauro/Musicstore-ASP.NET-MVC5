using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Models
{
    public class ShoppingCart
    {
        private const string CART_SESSION_KEY = "CartId";

        private readonly MusicStoreDbContext _dbContext;
        private readonly ISessionProvider _session;
        private readonly IClaimsPrincipalProvider _principal;

        public string ShoppingCartId { get; private set; }

        public ShoppingCart(MusicStoreDbContext dbContext, ISessionProvider session, IClaimsPrincipalProvider principal)
        {
            _dbContext = dbContext;
            _session = session;
            _principal = principal;
        }

        public async Task PopupaleShoppingCartId(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ShoppingCartId))
                ShoppingCartId = await GetCartId(_principal.Principal, cancellationToken);
        }


        public async Task AddToCart(Album album, CancellationToken cancellationToken = default)
        {
            await PopupaleShoppingCartId(cancellationToken).ConfigureAwait(false);

            // Get the matching cart and album instances
            var cartItem = await _dbContext.Carts.SingleOrDefaultAsync(
                c => c.CartId == ShoppingCartId
                && c.AlbumId == album.AlbumId, cancellationToken).ConfigureAwait(false);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart()
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                await _dbContext.Carts.AddAsync(cartItem).ConfigureAwait(false);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> RemoveFromCart(int id, CancellationToken cancellationToken = default)
        {
            await PopupaleShoppingCartId(cancellationToken).ConfigureAwait(false);

            // Get the cart
            var cartItem = await _dbContext.Carts.SingleAsync(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id, cancellationToken).ConfigureAwait(false);

            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _dbContext.Carts.Remove(cartItem);
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            return itemCount;
        }

        public async Task EmptyCart()
        {
            await PopupaleShoppingCartId().ConfigureAwait(false);

            var cartItems = _dbContext.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                _dbContext.Carts.Remove(cartItem);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Cart>> GetCartItems(CancellationToken cancellationToken = default)
        {
            await PopupaleShoppingCartId(cancellationToken).ConfigureAwait(false);

            return await _dbContext.Carts
                .Where(cart => cart.CartId == ShoppingCartId)
                .Include(x => x.Album)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCount()
        {
            await PopupaleShoppingCartId().ConfigureAwait(false);

            // Get the count of each item in the cart and sum them up
            int? count = await (from cartItems in _dbContext.Carts
                                where cartItems.CartId == ShoppingCartId
                                select (int?)cartItems.Count).SumAsync();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public async Task<decimal> GetTotal(CancellationToken cancellationToken = default)
        {
            await PopupaleShoppingCartId(cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            // Multiply album price by count of that album to get
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = await (from cartItem in _dbContext.Carts
                                    where cartItem.CartId == ShoppingCartId
                                    select (int?)cartItem.Count * cartItem.Album.Price)
                                .SumAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return total ?? 0;
        }

        public async Task<int> CreateOrder(Order order, CancellationToken cancellationToken = default)
        {
            await PopupaleShoppingCartId(cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            //order have create and is going to update information
            decimal orderTotal = 0;
            var cartItem = await GetCartItems(cancellationToken);

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItem)
            {
                var orderDetail = new OrderDetail()
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);
                _dbContext.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            // Save the order

            await _dbContext.SaveChangesAsync();
            // Empty the shopping cart

            await EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;

        }

        // We're using HttpContextBase to allow access to cookies.
        public async Task<string> GetCartId(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            string cartSessionValue = await _session.GetStringAsync(CART_SESSION_KEY, cancellationToken);
            if (string.IsNullOrEmpty(cartSessionValue))
            {
                // Generate a new random GUID using System.Guid class
                cartSessionValue = Guid.NewGuid().ToString();
                await _session.SetStringAsync(CART_SESSION_KEY, cartSessionValue, cancellationToken);
            }
            return cartSessionValue;
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public async Task MigrateCart(string userName, CancellationToken cancellationToken = default)
        {
            await PopupaleShoppingCartId().ConfigureAwait(false);

            IReadOnlyList<Cart> shoppingCart = await _dbContext.Carts.Where(c => c.CartId == ShoppingCartId).ToListAsync(cancellationToken);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}