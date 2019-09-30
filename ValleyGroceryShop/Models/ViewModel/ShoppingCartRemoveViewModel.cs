using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValleyGroceryShop.Models.ViewModel
{
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ProductCount { get; set; }
        public int DeleteId { get; set; }
    }
}