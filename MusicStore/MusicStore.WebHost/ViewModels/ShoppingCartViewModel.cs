using MusicStore.WebHost.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.WebHost.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IReadOnlyCollection<Cart> CartItems { get; set; }

        [UIHint("Price")]
        public decimal CartTotal { get; set; }
    }
}