using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValleyGroceryShop.Models;
using ValleyGroceryShop.Models.ViewModel;

namespace ValleyGroceryShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ManageProduct()
        {
            return View();
        }

        public JsonResult GetData()
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<ProductViewModel> lstitem = new List<ProductViewModel>();
                var lst = db.tblProducts.Include("tblCategory").ToList();
                foreach (var item in lst)
                {
                    lstitem.Add(new ProductViewModel() { ProductId = item.ProductId, CategoryName = item.tblCategory.CategoryName, ProductName = item.ProductName, Description = item.Description, UnitPrice = item.UnitPrice, SellingPrice = item.SellingPrice, Photo = item.Photo });
                }
                return Json(new { data = lstitem }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
                {
                    ViewBag.Categories = db.tblCategories.ToList();
                    ViewBag.Action = "Create New Product";
                    return View(new ProductViewModel());
                }
            }
            else
            {
                using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
                {
                    ViewBag.Action = "Edit Product";
                    ViewBag.Categories = db.tblCategories.ToList();
                    tblProduct item = db.tblProducts.Where(i => i.ProductId == id).FirstOrDefault();
                    ProductViewModel itemvm = new ProductViewModel();
                    itemvm.ProductId = item.ProductId;
                    itemvm.CategoryId = Convert.ToInt32(item.CategoryId);
                    itemvm.ProductName = item.ProductName;
                    itemvm.UnitPrice = item.UnitPrice;
                    itemvm.SellingPrice = item.SellingPrice;
                    itemvm.Description = item.Description;
                    itemvm.Photo = item.Photo;
                    itemvm.IsSpecial = item.IsSpecial;
                    return View(itemvm);
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(ProductViewModel ivm)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                if (ivm.ProductId == 0)
                {
                    tblProduct itm = new tblProduct();
                    itm.CategoryId = Convert.ToInt32(ivm.CategoryId);
                    itm.ProductName = ivm.ProductName;
                    itm.UnitPrice = ivm.UnitPrice;
                    itm.SellingPrice = ivm.SellingPrice;
                    itm.Description = ivm.Description;
                    itm.Description = ivm.Description;
                    HttpPostedFileBase fup = Request.Files["Photo"];
                    if (fup != null)
                    {
                        if (fup.FileName != "")
                        {
                            fup.SaveAs(Server.MapPath("~/rootcss/images" + fup.FileName));
                            itm.Photo = fup.FileName;
                        }
                    }
                    db.tblProducts.Add(itm);
                    db.SaveChanges();
                    ViewBag.Message = "Created Successfully";
                }
                else
                {
                    tblProduct itm = db.tblProducts.Where(i => i.ProductId == ivm.ProductId).FirstOrDefault();
                    itm.CategoryId = Convert.ToInt32(ivm.CategoryId);
                    itm.ProductName = ivm.ProductName;
                    itm.UnitPrice = ivm.UnitPrice;
                    itm.SellingPrice = ivm.SellingPrice;
                    itm.Description = ivm.Description;
                    itm.IsSpecial = ivm.IsSpecial;
                    HttpPostedFileBase fup = Request.Files["Photo"];
                    if (fup != null)
                    {
                        if (fup.FileName != "")
                        {
                            fup.SaveAs(Server.MapPath("~/rootcss/images" + fup.FileName));
                            itm.Photo = fup.FileName;
                        }
                    }

                    db.SaveChanges();
                    ViewBag.Message = "Updated Successfully";

                }
                ViewBag.Categories = db.tblCategories.ToList();
                return View(new ProductViewModel());

            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                tblProduct sm = db.tblProducts.Where(x => x.ProductId == id).FirstOrDefault();
                db.tblProducts.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}