﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using MusicStore.WebHost.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IAlbumRepository albumRepository, ShoppingCart shoppingCart)
        {
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
            _shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var totalItems = await _shoppingCart.GetCartItems(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var cartTotal = await _shoppingCart.GetTotal(cancellationToken);

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
        public async Task<IActionResult> AddToCart([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            // Retrieve the album from the database
            Album addedAlbum = await _albumRepository.FindAlbum(id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // Add it to the shopping cart
            await _shoppingCart.AddToCart(addedAlbum, cancellationToken);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int id, [FromServices] MusicStoreDbContext dbContext, CancellationToken cancellationToken = default)
        {
            // Get the name of the album to display confirmation
            Cart cartRecord = await dbContext.Carts
                                        .SingleAsync(item => item.RecordId == id);

            string albumName = cartRecord.Album.Title;

            // Remove from cart
            int itemCount = await _shoppingCart.RemoveFromCart(id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel()
            {
                Message = albumName + "has been removed from your shopping cart.",
                CartTotal = await _shoppingCart.GetTotal(),
                CartCount = await _shoppingCart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        ////
        //// GET: /ShoppingCart/CartSummary
        ////[ChildActionOnly]
        //public ActionResult CartSummary()
        //{
        //    ViewData["CartCount"] = _shoppingCart.GetCount();
        //    return PartialView(shoppingCart);
        //}
    }
}