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

        [HttpGet]
        public ActionResult ProductPage(int? Id)
        {
            List<Products> products;
            if (Id == 1)
            {
                products = _repo.Getproducts(Id);
            }
            else if (Id == 2)
            {
                products = _repo.Getproducts(Id);
            }
            else if (Id == 3)
            {
                products = _repo.Getproducts(Id);
            }
            else
            {
                products = _repo.GetProducts();
            }

            return View(products);
        }

        [HttpPost]
        public ActionResult ProductPage(string search, int? Year_s, int? Year_e, decimal? Price_s, decimal? Price_e, string[] Origin, string[] Category)
        {
            List<Products> products;

            if (search != null)
            {
                products = _repo.GetProducts(search);

            }
            else if (Year_e == null && Year_s != null)
            {
                products = _repo.GetProducts(Year_s);
            }
            else if (Year_s != null)
            {
                products = _repo.GetProducts(Year_s, Year_e);
            }
            else if (Price_e == null && Price_s != null)
            {
                products = _repo.GetProducts(Price_s);
            }
            else if (Price_s != null)
            {
                products = _repo.GetProducts(Price_s, Price_e);
            }
            else if (Origin != null)
            {
                products = _repo.GetProducts(Origin, 1);
            }
            else if (Category != null)
            {
                products = _repo.GetProducts(Category, 2);
            }
            else
            {
                products = _repo.GetProducts();
            }


            return View(products);
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