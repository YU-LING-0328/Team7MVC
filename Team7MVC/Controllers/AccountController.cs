using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.ViewModels;
using Team7MVC.Repositories;
using System.Web.Security;
using Team7MVC.Models;

namespace Team7MVC.Controllers
{
    public class AccountController : Controller
    {
        public readonly AccountRepository _repo;

        public AccountController()
        {
            _repo = new AccountRepository();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            string account = HttpUtility.HtmlEncode(loginVM.Account);
            string password = HttpUtility.HtmlEncode(loginVM.Password);

            Customers user = _repo.Login(account, password);

            if (user == null)
            {
                ModelState.AddModelError("", "無效的帳號或密碼。");
                return View();
            }

            var ticket = new FormsAuthenticationTicket(
            version: 1,
            name: user.Account.ToString(), //可以放使用者Id
            issueDate: DateTime.UtcNow,//現在UTC時間
            expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
            isPersistent: true,// 是否要記住我 true or false
            userData: "", //可以放使用者角色名稱
            cookiePath: FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密

            // Create the cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);

            // Redirect back to original URL.
            var url = FormsAuthentication.GetRedirectUrl(account, true);

            //Response.Redirect(FormsAuthentication.GetRedirectUrl(name, true));

            //return Redirect(FormsAuthentication.GetRedirectUrl(account, true));
            return RedirectToAction("Index", "Wine");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                _repo.Register(registerVM);
                return RedirectToAction("Index", "Wine");
            }
            return View(registerVM);
        }
    }
}