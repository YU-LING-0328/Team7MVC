using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Models;
using Team7MVC.Repositories;
using Team7MVC.ViewModels;

namespace Team7MVC.Controllers
{
    public class ProductController : Controller
    {
        public readonly ProductRepository _repo;

        public ProductController()
        {
            _repo = new ProductRepository();
        }
        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            List<Products> products;

            products = _repo.GetProducts();

            return View(products);
        }



        #region 產品搜尋

        [HttpPost]
        public ActionResult SearchProductName(string search)
        {
            List<Products> products;

            if (search != null)
            {
                products = _repo.GetProducts(search);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchProductYear(int? Year_s, int? Year_e)
        {
            List<Products> products;

            if (Year_e == null && Year_s != null)
            {
                products = _repo.GetProducts(Year_s);
            }
            else if (Year_s != null)
            {
                products = _repo.GetProducts(Year_s, Year_e);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchProductPrice(decimal? Price_s, decimal? Price_e)
        {
            List<Products> products;

            if (Price_e == null && Price_s != null)
            {
                products = _repo.GetProducts(Price_s);
            }
            else if (Price_s != null)
            {
                products = _repo.GetProducts(Price_s, Price_e);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        #endregion

        #region 產品分類

        [HttpPost]
        public ActionResult ProductOrigin(string[] Origin)
        {
            List<Products> products;

            if (Origin != null)
            {
                products = _repo.GetProducts(Origin, 1);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        [HttpPost]
        public ActionResult ProductCategory(string[] Category)
        {
            List<Products> products;

            if (Category != null)
            {
                products = _repo.GetProducts(Category, 2);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult NewProduct()
        {
            List<Products> products;


            products = _repo.GetNewProducts();

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult HotProduct()
        {
            List<Products> products;


            products = _repo.GetHotProducts();

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult ExpensiveProduct()
        {
            List<Products> products;


            products = _repo.GetExpensiveProducts();

            ViewData.Model = products;
            return View("Index");
        }

        #endregion

        [HttpGet]
        public ActionResult ProductDetail(int Id)
        {
            var product = _repo.GetProductById(Id);
            return View(product);
        }

        [HttpPost]
        public ActionResult ProductDetail(int ProductId, int buyQty)
        {
            //var product = _repo.GetProductById(Id);

            //ShopListsViewModel shopLists = new ShopListsViewModel
            //{
            //    ProductId = product.ProductID,
            //    Price = product.UnitPrice,
            //    Quantity = buyQty
            //};

            _repo.CreateShoppingCartData(User.Identity.Name, ProductId, buyQty);

            return RedirectToAction("ShoppingCart");
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

            return RedirectToAction("OrderDetail", "OrderDetail");
        }
    }
}