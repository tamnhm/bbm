using System.Collections.Generic;
using Babymart.Models;
using Babymart.Models.Module;

namespace Babymart.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ModelCart> CartItemModel { get; set; }

        public List<cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public string  KgTotal { get; set; }
    }
}