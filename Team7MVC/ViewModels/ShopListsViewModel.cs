using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class ShopListsViewModel
    {
        public string Picture { get; set; }
        public string ProductName { get; set; }
        public int Year { get; set; }
        public string Origin { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
        public decimal TotalCost { get; set; }
    }
}