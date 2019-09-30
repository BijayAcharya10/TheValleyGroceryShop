using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValleyGroceryShop.Models
{
    public static class ItemDB
    {
        public static List<tblProduct> GetAllSpecialItem()
        {
            using (var context = new VALLEYSTOREDBEntities())
            {
                return context.tblProducts.OrderByDescending(e => e.ProductId).Where(s => s.IsSpecial == true).Take(4).ToList();
            }
        }
        public static List<tblProduct> SpecialItem()
        {
            using (var context = new VALLEYSTOREDBEntities())
            {
                return context.tblProducts.OrderByDescending(e => e.ProductId).Where(s => s.IsSpecial == false).Take(6).ToList();
            }
        }
        public static List<tblProduct> GetAllItems()
        {
            using (var context = new VALLEYSTOREDBEntities())
            {
                return context.tblProducts.Where(s => s.IsSpecial == false).Take(4).ToList();
            }
        }

        public static List<tblProduct> GetItem()
        {
            using (var context = new VALLEYSTOREDBEntities())
            {
                return context.tblProducts.OrderByDescending(e => e.ProductId).Where(s => s.CategoryId == 5).Take(4).ToList();
            }
        }

        public static List<tblProduct> GetOtherItems()
        {
            using (var context = new VALLEYSTOREDBEntities())
            {
                return context.tblProducts.OrderByDescending(e => e.ProductId).Where(s => s.CategoryId == 4).Take(4).ToList();
            }
        }

    }
}