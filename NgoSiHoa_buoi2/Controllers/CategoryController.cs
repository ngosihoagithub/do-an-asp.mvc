using NgoSiHoa_buoi2.Contex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgoSiHoa_buoi2.Controllers
{
    public class CategoryController : Controller
    {
        NgoSiHoa_2121110370Entities obj = new NgoSiHoa_2121110370Entities();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory=obj.Categories.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int Id)
        {
            var lstProduct = obj.Products.Where( n => n.CategoryId == Id).ToList();
            return View(lstProduct);
        }
    }
}