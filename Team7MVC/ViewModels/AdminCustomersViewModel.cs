using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class AdminCustomersViewModel
    {
        [Display(Name = "會員ID")]
        public int CustomerID { get; set; }
        [Display(Name = "帳號")]
        public string Account { get; set; }
        [Display(Name = "名字")]
        public string CustomerName { get; set; }
        [Display(Name = "性別")]
        public string Gender { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "手機")]
        public string Phone { get; set; }
        [Display(Name = "消費總額")]
        public decimal TotalCost { get; set; }
    }
}