using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Controllers
{
    public class ShoppingCartController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            var totalItems = await shoppingCart.GetCartItems(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var cartTotal = await shoppingCart.GetTotal(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = totalItems,
                CartTotal = cartTotal
            };
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public async Task<IActionResult> AddToCart([FromRoute] int id, [FromServices] MusicStoreDbContext dbContext, [FromServices] ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            // Retrieve the album from the database
            Album addedAlbum = await dbContext.Albums
                .SingleAsync(album => album.AlbumId == id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // Add it to the shopping cart
            await shoppingCart.AddToCart(addedAlbum, cancellationToken);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int id, [FromServices] ShoppingCart shoppingCart, [FromServices] MusicStoreDbContext dbContext, CancellationToken cancellationToken = default)
        {
            // Get the name of the album to display confirmation
            Cart cartRecord = await dbContext.Carts
                                        .SingleAsync(item => item.RecordId == id);

            string albumName = cartRecord.Album.Title;

            // Remove from cart
            int itemCount = await shoppingCart.RemoveFromCart(id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel()
            {
                Message = albumName+ "has been removed from your shopping cart.",
                CartTotal = await shoppingCart.GetTotal(),
                CartCount = await shoppingCart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        //[ChildActionOnly]
        public ActionResult CartSummary([FromServices] ShoppingCart shoppingCart)
        {
            ViewData["CartCount"] = shoppingCart.GetCount();
            return PartialView(shoppingCart);
        }
    }
}