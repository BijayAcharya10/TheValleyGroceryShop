using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ValleyGroceryShop.Controllers
{
    public class AdminController : Controller
    {
       [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}