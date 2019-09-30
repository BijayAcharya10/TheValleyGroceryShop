using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValleyGroceryShop.Models;
using ValleyGroceryShop.Models.ViewModel;

namespace ValleyGroceryShop.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult ManageCategory()
        {
            return View();
        }

        public JsonResult GetData()
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<CategoryViewModel> lst = new List<CategoryViewModel>();
                var catList = db.tblCategories.ToList();
                foreach (var item in catList)
                {
                    lst.Add(new CategoryViewModel() { CategoryId = item.CategoryId, CategoryName = item.CategoryName });
                }
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
                {
                    ViewBag.Action = "New Category";
                    return View(new CategoryViewModel());
                }
            }
            else
            {
                using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
                {
                    CategoryViewModel sub = new CategoryViewModel();
                    var menu = db.tblCategories.Where(x => x.CategoryId == id).FirstOrDefault();
                    sub.CategoryId = menu.CategoryId;
                    sub.CategoryName = menu.CategoryName;
                    ViewBag.Action = "Edit Category";
                    return View(sub);
                }
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(CategoryViewModel sm)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                if (sm.CategoryId == 0)
                {
                    tblCategory tb = new tblCategory();
                    tb.CategoryName = sm.CategoryName;
                    db.tblCategories.Add(tb);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblCategory tbm = db.tblCategories.Where(m => m.CategoryId == sm.CategoryId).FirstOrDefault();
                    tbm.CategoryName = sm.CategoryName;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                tblCategory sm = db.tblCategories.Where(x => x.CategoryId == id).FirstOrDefault();
                db.tblCategories.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}