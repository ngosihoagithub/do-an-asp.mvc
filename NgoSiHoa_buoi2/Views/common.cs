using NgoSiHoa_buoi2.Contex;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgoSiHoa_buoi2.Views
{
    public class common
    {
        [NonAction]
        public SelectList ToSelectLiss(DataTable table, string valueField, string textField)
        { List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[textField].ToString()
                });
            }
            return new SelectList(list,"Value","Text");
        }
    }

    namespace NgoSiHoa_buoi2.Controllers
    {
        public class ProductController : Controller
        {
            NgoSiHoa_2121110370Entities obj = new NgoSiHoa_2121110370Entities();
            // GET: Product
            public ActionResult Index()
            {
                var lstProduct = obj.Products.ToList();
                return View(lstProduct);
            }
            //Detail
            public ActionResult Detail(int id)
            {
                //select * from product where =id
                var objProducts = obj.Products.Where(n => n.Id == id).FirstOrDefault();
                return View(objProducts);
            }
            //Create
            [HttpGet]
            public ActionResult Create()
            {
                return View();
            }
            //post:create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Product objProduct, HttpPostedFileBase file)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Lấy tên tập tin
                            string fileName = Path.GetFileName(file.FileName);
                            // Lưu tập tin vào thư mục "Images" trong dự án
                            string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            file.SaveAs(path);
                            // Lưu tên tập tin vào trường ImageFileName của đối tượng sản phẩm
                            objProduct.Hinh = fileName;
                        }

                        obj.Products.Add(objProduct);
                        obj.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi nếu có
                        ModelState.AddModelError("", "An error occurred while saving the product.");
                        // Log lỗi nếu cần
                    }
                }

                return View(objProduct);
            }
            //Delete
            [HttpGet]
            public ActionResult Delete(int id, String a)
            {
                var objproduct = obj.Products.FirstOrDefault(n => n.Id == id);
                return View(objproduct);
            }
            public ActionResult Delete(int id)
            {
                var objProduct = obj.Products.FirstOrDefault(n => n.Id == id);
                if (objProduct != null)
                {
                    obj.Products.Remove(objProduct);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            public ActionResult Edit(int id)
            {
                var objProduct = obj.Products.FirstOrDefault(n => n.Id == id);
                if (objProduct != null)
                {
                    return View(objProduct);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            //Edit
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Product model, HttpPostedFileBase file)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Tìm đối tượng quyen cần chỉnh sửa
                        var existingProduct = obj.Products.FirstOrDefault(n => n.Id == model.Id);
                        if (existingProduct != null)
                        {
                            // Kiểm tra xem có tệp ảnh mới được tải lên không
                            if (file != null && file.ContentLength > 0)
                            {
                                try
                                {
                                    // Tạo đường dẫn để lưu trữ ảnh
                                    string path = Server.MapPath("~/Images/");
                                    // Lưu trữ tên file mới
                                    string fileName = Path.GetFileName(file.FileName);
                                    // Đường dẫn đầy đủ để lưu trữ ảnh
                                    string fullPath = Path.Combine(path, fileName);
                                    // Lưu ảnh vào thư mục
                                    file.SaveAs(fullPath);
                                    // Gán tên ảnh mới vào trường sHinh của đối tượng quyen
                                    existingProduct.Hinh = fileName;
                                }
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("", "Lỗi khi tải lên ảnh: " + ex.Message);
                                    return View(model);
                                }
                            }

                            // Cập nhật thông tin của đối tượng quyen
                            existingProduct.Name = model.Name;
                            existingProduct.Detail = model.Detail;
                            existingProduct.Price = model.Price;
                            existingProduct.Status = model.Status;

                            // Lưu thay đổi vào cơ sở dữ liệu
                            obj.SaveChanges();

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi cập nhật quyen: " + ex.Message);
                    }
                }

                return View(model);
            }
            public ActionResult CapNhatHinhAnh(int idSanPham)
            {
                {
                    ViewBag.idSanPham = idSanPham;
                    return View();
                }
            }
            [HttpPost]
            public ActionResult ThemAnh(int id, HttpPostedFileBase file)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = id.ToString() + Path.GetFileName(file.FileName);
                        string fullPath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(fullPath);

                        var objProduct = obj.Products.FirstOrDefault(n => n.Id == id);
                        if (objProduct != null)
                        {
                            objProduct.Hinh = fileName;
                            obj.SaveChanges();
                            ViewBag.Message = "Tên ảnh đã được lưu vào cơ sở dữ liệu thành công.";
                        }
                        else
                        {
                            ViewBag.Message = "Không tìm thấy đối tượng quyen có id tương ứng.";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Vui lòng chọn một ảnh.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Lỗi khi lưu tên ảnh vào cơ sở dữ liệu: " + ex.Message;
                }

                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}