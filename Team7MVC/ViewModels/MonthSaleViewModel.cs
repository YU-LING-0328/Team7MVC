using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class MonthSaleViewModel
    {
        public string[] Month { get; set; }
        public decimal[] Amount { get; set; }
        public decimal Year_Sale { get; set; }
        public int Customer_Count { get; set; }
        public int Order_Count { get; set; }
        public HotOrigin Origin_Count { get; set; }
    }

    public class HotOrigin
    {
        public string Origin { get; set; }
        public int OriginsTotal { get; set; }
    }
}