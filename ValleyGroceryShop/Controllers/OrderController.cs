using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValleyGroceryShop.Models;
using ValleyGroceryShop.Models.ViewModel;

namespace ValleyGroceryShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        public ActionResult ManageOrder()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<OrderViewModel> lstorder = new List<OrderViewModel>();
                var empList = db.tblOrders.Select(x => new { OrderId = x.OrderId, Firstname = x.Firstname, Lastname = x.Lastname, Address = x.Address, Phone = x.Phone, Total = x.Total, OrderDate = x.OrderDate, DeliveredStatus = x.DeliveredStatus }).ToList();
                foreach (var item in empList)
                {
                    lstorder.Add(new OrderViewModel() { OrderId = item.OrderId, Firstname = item.Firstname, Lastname = item.Lastname, Address = item.Address, Phone = item.Phone, Total = item.Total.ToString(), OrderDate = item.OrderDate.ToString(), DeliveredStatus = item.DeliveredStatus });
                }
                return Json(new { data = lstorder }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]

        public ActionResult ViewOrder(int id)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {

                List<OrderDetailsViewModel> lstod = new List<OrderDetailsViewModel>();
                var empList = db.tblOrderDetails.Where(x => x.OrderId == id).ToList();
                foreach (tblOrderDetail product in empList)
                {
                    lstod.Add(new OrderDetailsViewModel() { ProductId = Convert.ToInt32(product.ProductId), ProductName = product.tblProduct.ProductName, Quantity = Convert.ToInt32(product.Quantity), UnitPrice = Convert.ToDecimal(product.UnitPrice) });
                }
                Session.Add("productlist", lstod);
                Session.Add("orderid", id);
                return View(lstod);
            }
        }
        [HttpPost, ActionName("ViewOrder")]
        public ActionResult ViewOrder_Post(int id)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {

                tblOrder sm = db.tblOrders.Where(x => x.OrderId == id).FirstOrDefault();
                sm.DeliveredStatus = "Confirmed";

                db.SaveChanges();
                return RedirectToAction("ManageOrder", "Order");
            }

        }
        VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities();
        public ActionResult PrintBill()
        {
            List<OrderDetailsViewModel> lst = null;
            if (Session["productlist"] != null)
            {
                lst = (List<OrderDetailsViewModel>)Session["productlist"];
                ViewBag.orderlst = lst;
                if (Session["orderid"] != null)
                {
                    int oid = Convert.ToInt32(Session["orderid"].ToString());
                    BillViewModel blv = new BillViewModel();
                    tblOrder tbo = db.tblOrders.Where(o => o.OrderId == oid).FirstOrDefault();
                    ViewBag.Fullname = tbo.Firstname + " " + tbo.Lastname;
                    ViewBag.Phone = tbo.Phone;
                    ViewBag.Address = tbo.Address;
                    ViewBag.OrderDate = tbo.OrderDate;

                }

            }
            return View(lst);


        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
                {
                    // ViewBag.Menus = db.tblMenus.ToList();
                    return View(new tblCategory());
                }
            }
            else
            {
                using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
                {
                    // ViewBag.Menus = db.tblMenus.ToList();
                    return View(db.tblCategories.Where(x => x.CategoryId == id).FirstOrDefault());
                }
            }
        }

        [HttpPost]

        public ActionResult AddOrEdit(tblCategory sm)
        {
            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                if (sm.CategoryId == 0)
                {
                    db.tblCategories.Add(sm);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(sm).State = EntityState.Modified;
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
                tblOrder sm = db.tblOrders.Where(x => x.OrderId == id).FirstOrDefault();
                db.tblOrders.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}