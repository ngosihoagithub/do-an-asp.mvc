using NgoSiHoa_buoi2.Contex;
using NgoSiHoa_buoi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgoSiHoa_buoi2.Controllers
{
    public class PaymentController : Controller
    {
        NgoSiHoa_2121110370Entities obj = new NgoSiHoa_2121110370Entities();

        // GET: Payment
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Lấy thông tin giỏ hàng từ session
                var lstCart = (List<CartModel>)Session["cart"];
                //Gán cho đối tượng Order
                Order objOrder = new Order();
                objOrder.Name = "DonHang-"+ DateTime.Now.ToString("yyyyMMddHHmmss");
                //objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnutc = DateTime.Now;
                objOrder.Status = 1;
                obj.Orders.Add(objOrder);
                obj.SaveChanges();
                //lấy OrderId vừa mới lưu vào bảng OrderDetail
                int OrderId = objOrder.Id;
                List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
                foreach (var item in lstCart)
                {
                    OrderDetail obj = new OrderDetail();
                    obj.Quantity = item.Quantity;
                    obj.OrderId = OrderId;
                    obj.ProductId = item.Product.Id;
                    lstOrderDetail.Add(obj);
                }
                obj.OrderDetails.AddRange(lstOrderDetail);
                obj.SaveChanges();
                
             
            }
            Session.Remove("cart");
            return View();
        }
    }
}
