using ValleyGroceryShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ValleyGroceryShop.Models;
using ValleyGroceryShop.Models.ViewModel;
using PagedList;

namespace ValleyGroceryShop.Controllers
{
    public class HomeController : Controller
    {
        VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities();
      
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult ViewItem(int id)
        {
            return PartialView("_ViewItem", db.tblProducts.Find(id));
        }
        public ActionResult ProductList(string search, int? page, int id = 0)
        {

            if (id != 0)
            {

                return View(db.tblProducts.Where(p => p.CategoryId == id).ToList().ToPagedList(page ?? 1, 4));
            }
            else
            {
                if (search != "")
                {
                    return View(db.tblProducts.Where(x => x.Description.Contains(search) || x.ProductName.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, 4));
                }
                else
                {
                    return View(db.tblProducts.ToList().ToPagedList(page ?? 1, 4));
                }

            }

        }

        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(UserViewModel uv)
        {
            tblUser tbl = db.tblUsers.Where(u => u.Username == uv.Username).FirstOrDefault();
            if (tbl != null)
            {
                return Json(new { success = false, message = "User Already Registerd" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                tblUser tb = new tblUser();
                tb.Username = uv.Username;
                tb.Password = uv.Password;
                tb.Email = uv.Email;
                db.tblUsers.Add(tb);
                db.SaveChanges();

                tblUserRole ud = new tblUserRole();
                ud.UserId = tb.UserId;
                ud.RoleId = 2;
                db.tblUserRoles.Add(ud);
                db.SaveChanges();
                return Json(new { success = true, message = "User Registered Successfully" }, JsonRequestBehavior.AllowGet);
               
            }


        }

       [Authorize]
        public ActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel ch)
        {
            string username = Session["username"].ToString();

            tblUser us = db.tblUsers.Where(u => u.Username == username && u.Password == ch.OldPassword).FirstOrDefault();
            if (us != null)
            {
                us.Password = ch.NewPassword;
                db.SaveChanges();

            }
            else
            {
                return Json(new { success = false, message = "You Enter Wrong Password" }, JsonRequestBehavior.AllowGet);
            }
           return Json(new { success = true, message = "Password Changed Successfully" }, JsonRequestBehavior.AllowGet);
           

        }



        public ActionResult ForgetPassword()
        {
            return View();


        }
        [ValidateOnlyIncomingValuesAttribute]//if you have validated all properties in viewmodel but have to restrict for only two properties
        [HttpPost]
        public ActionResult ForgetPassword(UserViewModel uv)
        {
            if (ModelState.IsValid)
            {
                //https://www.google.com/settings/security/lesssecureapps
                //Make Access for less secure apps=true from the sender gmail account

                string from = "aayanacharya2000@gmail.com";
                using (MailMessage mail = new MailMessage(from, uv.Email))
                {
                    try
                    {
                        tblUser tb = db.tblUsers.Where(u => u.Email == uv.Email).FirstOrDefault();
                        if (tb != null)
                        {
                            mail.Subject = "Password Recovery";
                            mail.Body = "Use this password for your login:" + tb.Password;

                            mail.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential networkCredential = new NetworkCredential(from, "frankline@123");
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkCredential;
                            smtp.Port = 587;
                            smtp.Send(mail);
                            ViewBag.Message = "Your password is sent to your mail address";
                        }
                        else
                        {
                            ViewBag.Message = "This email doesnt exist in database";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }

                }

            }
            return View();

        }
    }
}