using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ValleyGroceryShop.Models;
using ValleyGroceryShop.Models.ViewModel;

namespace ValleyGroceryShop.Controllers
{
    public class AccountController : Controller
    {
        VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel l, string ReturnUrl = "")//return to the url
        {

            using (VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities())
            {
                var users = db.tblUsers.Where(a => a.Username == l.Username && a.Password == l.Password).FirstOrDefault();
                if (users != null)
                {

                    //saving value in session
                    Session.Add("username", users.Username);
                    FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        Session.Add("userid", users.UserId);
                        if (users.tblUserRoles.Where(r => r.RoleId == 1).FirstOrDefault() != null)//Redirecting to dashboard and userview
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User");
                }
            }
            return View();

        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}