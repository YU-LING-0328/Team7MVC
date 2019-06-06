using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Repositories;
using Team7MVC.Models;
using Team7MVC.ViewModels;

namespace Team7MVC.Controllers
{
    public class AdminController : Controller
    {
        public readonly AdminRepository _repo;

        public AdminController()
        {
            _repo = new AdminRepository();
        }
        
        //Dashboard
        public ActionResult Index()
        {
            MonthSaleViewModel monthSaleViewModels;
            monthSaleViewModels = _repo.GetMonthSale();
            monthSaleViewModels.Year_Sale = _repo.GetYearSale();
            monthSaleViewModels.Customer_Count = _repo.GetCustomer_Count();
            monthSaleViewModels.Order_Count = _repo.GetOrder_Count();
            monthSaleViewModels.Origin_Count = _repo.GetOrigin_Count();


            return View(monthSaleViewModels);
        }

        [HttpGet]
        public ActionResult Product()
        {
            var products = _repo.GetAllProducts();
            

            return View(products);
        }

        [HttpPost]
        public ActionResult Product(string SortBy)
        {
            var products = _repo.GetAllProducts(SortBy);

            return View(products);
        }

        [HttpGet]
        public ActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductCreate(string ProductName, string Origin, int Year, int Capacity, decimal UnitPrice, int Stock, string Grade, string Variety, string Area, string Picture, string Introduction, int CategoryId)
        {
            Products product = new Products()
            {
                ProductName = ProductName,
                Origin = Origin,
                Year = Year,
                Capacity = Capacity,
                UnitPrice = UnitPrice,
                Stock = Stock,
                Grade = Grade,
                Variety = Variety,
                Area = Area,
                Picture = Picture,
                Introduction = Introduction,
                CategoryID = CategoryId
            };

            _repo.CreateProduct(product);

            return RedirectToAction("Product");
        }

        [HttpGet]
        public ActionResult ProductEdit(int Id)
        {
            var product = _repo.GetProduct(Id);
            return View(product);
        }

        [HttpPost]
        public ActionResult ProductEdit(int Id, string ProductName, string Origin, int Year, int Capacity, decimal UnitPrice, int Stock, string Grade, string Variety, string Area, string Picture, string Introduction, int CategoryId)
        {
            Products product = new Products()
            {
                ProductID = Id,
                ProductName = ProductName,
                Origin = Origin,
                Year = Year,
                Capacity = Capacity,
                UnitPrice = UnitPrice,
                Stock = Stock,
                Grade = Grade,
                Variety = Variety,
                Area = Area,
                Picture = Picture,
                Introduction = Introduction,
                CategoryID = CategoryId
            };

            _repo.UpdateProduct(product);

            return RedirectToAction("Product");
        }

        [HttpGet]
        public ActionResult ProductDelete(int Id)
        {
            _repo.DeleteProduct(Id);

            return RedirectToAction("Product");
        }

        public ActionResult Order()
        {
            var orders = _repo.GetAllOrders();

            return View(orders);
        }

        // GET: Customers
        public ActionResult Customer()
        {
            var customers = _repo.GetAllCustomers();

            return View(customers);
        }

        public ActionResult Messages()
        {
            var messages = _repo.GetAllMessages();

            return View(messages);
        }
    }
}