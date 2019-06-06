using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Repositories;
using Team7MVC.ViewModels;

namespace Team7MVC.Controllers
{
    public class OrderDetailController : Controller
    {
        public readonly OrderDetailRepository _repo;

        public OrderDetailController()
        {
            _repo = new OrderDetailRepository();
        }

        // GET: OrderDetail
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderDetail
        [HttpGet]
        public ActionResult OrderDetail()
        {
            OrderDetailViewModel orderdetailList;
            orderdetailList = _repo.OrderDetailList(User.Identity.Name);

            return View(orderdetailList);
        }
    }
}