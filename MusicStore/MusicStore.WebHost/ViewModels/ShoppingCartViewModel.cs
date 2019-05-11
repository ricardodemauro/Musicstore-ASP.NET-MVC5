using MusicStore.WebHost.Models;
using System.Collections.Generic;

namespace MusicStore.WebHost.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IReadOnlyCollection<Cart> CartItems { get; set; }

        public decimal CartTotal { get; set; }
    }
}