using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.Models
{
    public class Products
    {
        [Display(Name = "商品編號")]
        public int ProductID { get; set; }
        [Display(Name = "名稱")]
        public string ProductName { get; set; }
        [Display(Name = "產地")]
        public string Origin { get; set; }
        [Display(Name = "年份")]
        public int Year { get; set; }
        [Display(Name = "容量")]
        public int Capacity { get; set; }
        [Display(Name = "價格")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "庫存")]
        public int Stock { get; set; }
        [Display(Name = "等級")]
        public string Grade { get; set; }
        [Display(Name = "品種")]
        public string Variety { get; set; }
        [Display(Name = "產區")]
        public string Area { get; set; }
        [Display(Name = "圖片")]
        public string Picture { get; set; }
        [Display(Name = "商品詳情")]
        public string Introduction { get; set; }
        [Display(Name = "類別")]
        public int CategoryID { get; set; }
    }
}