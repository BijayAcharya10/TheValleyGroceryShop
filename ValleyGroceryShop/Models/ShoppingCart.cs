using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ValleyGroceryShop.Models
{
    public class ShoppingCart
    {
        VALLEYSTOREDBEntities db = new VALLEYSTOREDBEntities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls 
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(tblProduct product)
        {
            // Get the matching cart and album instances 
            var cartItem = db.tblCarts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists 
                cartItem = new tblCart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.tblCarts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart,  
                // then add one to the quantity 
                cartItem.Count++;
            }
            // Save changes 
            db.SaveChanges();
        }

        public int UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cartItem = db.tblCarts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartCount > 0)
                {
                    cartItem.Count = cartCount;
                    itemCount = Convert.ToInt32(cartItem.Count);
                }
                else
                {
                    db.tblCarts.Remove(cartItem);
                }
                // Save changes 
                db.SaveChanges();
            }
            return itemCount;
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart 
            var cartItem = db.tblCarts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                //if (cartItem.Count > 1)
                //{
                //    cartItem.Count--;
                //    itemCount = cartItem.Count;
                //}
                //else
                //{
                db.tblCarts.Remove(cartItem);
                //}
                // Save changes 
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.tblCarts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.tblCarts.Remove(cartItem);
            }
            // Save changes 
            db.SaveChanges();
        }

        public List<tblCart> GetCartItems()
        {
            return db.tblCarts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
            //Forum: if you don't inclde the Album then you get a nullref execption when adding to the cart.
            //return storeDB.Carts.Include("Album").Where(
            //    c => c.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up 
            int? count = (from cartItems in db.tblCarts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null 
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get  
            // the current price for each of those albums in the cart 
            // sum all album price totals to get the cart total 
            decimal? total = (from cartItems in db.tblCarts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.tblProduct.SellingPrice).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(tblOrder order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart,  
            // adding the order details for each 
            foreach (var item in cartItems)
            {
                var orderDetail = new tblOrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.tblProduct.SellingPrice,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart 
                orderTotal += Convert.ToDecimal(item.Count * (item.tblProduct.SellingPrice));

                db.tblOrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count 
            order.Total = orderTotal;

            // Save the order 
            db.SaveChanges();
            // Empty the shopping cart 
            EmptyCart();
            // Return the OrderId as the confirmation number 
            return order.OrderId;
        }

        //We're using HttpContextBase to allow access to cookies. 
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class 
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie 
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to 
        // be associated with their username 
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.tblCarts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (tblCart item in shoppingCart)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }

    }
}