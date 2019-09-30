using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValleyGroceryShop.Models
{
    public static class MyMenu
    {
        public static List<tblCategory> GetMenus()
        {
            using (var context = new VALLEYSTOREDBEntities())
            {
                return context.tblCategories.ToList();
            }
        }



    }
}