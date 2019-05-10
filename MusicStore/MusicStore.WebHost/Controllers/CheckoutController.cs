using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private const string PromoCode = "FREE";

        private readonly IOrderRepository _orderRepository;

        public CheckoutController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        [HttpGet]
        public IActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddressAndPayment([FromForm] Order order, [FromServices] ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return View(order);

            try
            {
                //judge PromoCode
                if (!string.Equals(order.PromoCode, PromoCode, StringComparison.OrdinalIgnoreCase))
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    await _orderRepository.AddOrder(order, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();

                    //Process the order
                    await shoppingCart.CreateOrder(order, cancellationToken);

                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public async Task<IActionResult> Complete(int id, CancellationToken cancellationToken = default)
        {
            // Validate customer owns this order
            var order = await _orderRepository.FindOrder(id, cancellationToken);
            bool isValid = order != null;
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}