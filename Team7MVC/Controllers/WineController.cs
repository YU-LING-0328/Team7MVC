using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Team7MVC.ViewModels;
using Team7MVC.Repositories;
using Team7MVC.Models;

namespace Team7MVC.Controllers
{
    public class WineController : Controller
    {
        public readonly WineRepository _repo;
        public readonly MessageRepository mess_repo;

        public WineController()
        {
            _repo = new WineRepository();
            mess_repo = new MessageRepository();
        }

        // GET: Wine
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            List<ShopListsViewModel> shopLists;
            shopLists = _repo.ShopList(User.Identity.Name);

            return View(shopLists);
        }

        [HttpPost]
        public ActionResult ShoppingCart(string nothing)
        {
            return RedirectToAction("Payment");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Payment()
        {
            int CustomerId = _repo.GetCustomerID(User.Identity.Name);
            ViewData["CustomerId"] = CustomerId;
            return View();
        }

        [HttpPost]
        public ActionResult Payment(DateTime RequiredDate, string ShipName, string ShipAddress, decimal Freight, string PayWay)
        {
            Orders orders = new Orders()
            {
                OrderDate = DateTime.Now,
                RequiredDate = RequiredDate,
                ShipperID = 1,
                ShipName = ShipName,
                ShipAddress = ShipAddress,
                Freight = Freight,
                PayWay = PayWay,
                PayDate = DateTime.Now
            };

            _repo.Payment(orders, User.Identity.Name);

            return RedirectToAction("Index", "Wine");
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string name, string email, string phone, string questionCategory, string comments)
        {
            ViewData["Name"] = name;
            ViewData["Email"] = email;
            ViewData["Phone"] = phone;
            ViewData["QuestionCategory"] = questionCategory;
            ViewData["Comments"] = comments;

            Messages m = new Messages
            {
                Name = name,
                Email = email,
                Phone = phone,
                QuestionCategory = questionCategory,
                Comments = comments,
                Datetime = DateTime.Now
            };

            CreateMessagesData(m);
            return View("index");
            //var question = new Questions { Name = name, Email = email, Phone = phone, QuestionCategory = questionCategory, Comments = comments, Datetime = DateTime.Now };

            //var ques = _repo.CreateQuestions(question);
            //return View(ques);         
        }

        public int CreateMessagesData(Messages m)
        {
            //var question = new Questions { Name = name, Email = email, Phone = phone, QuestionCategory = questionCategory, Comments = comments, Datetime = DateTime.Now };

            return mess_repo.CreateMessages(m);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Login(string ActionName)
        {
            return View(ActionName);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Wine");
        }

        // GET: Wine
        public ActionResult Inside_the_macallane()
        {
            return View();
        }

    }
}