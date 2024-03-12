using NgoSiHoa_buoi2.Contex;
using NgoSiHoa_buoi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NgoSiHoa_buoi2.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        NgoSiHoa_2121110370Entities obj = new NgoSiHoa_2121110370Entities();
        private Contex.User _users;

        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();

            var lstProduct = obj.Products.ToList();
            var lstCategory = obj.Categories.ToList();

            objHomeModel.ListProducts = lstProduct;
            objHomeModel.ListCategory = lstCategory;

            return View(objHomeModel);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = obj.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    obj.Configuration.ValidateOnSaveEnabled = false;
                    obj.Users.Add(_user);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }
      
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = obj.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }

}
