using AutoMapper;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Babymart.Models
{
    public class ShoppingCart
    {
        babymart_vnEntities db = new babymart_vnEntities();
        public string ShoppingCartId { get; set; }
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
        public void AddToCart(shop_bienthe sanphambt, int cartCount)
        {
            //db.carts.SingleOrDefault
            // Get the matching cart and sanpham instances


            var cartItem = db.carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == sanphambt.id);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new cart
                {
                    ProductId = sanphambt.id,
                    CartId = ShoppingCartId,
                    Count = cartCount,
                    DateCreated = DateTime.Now
                };
                db.carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count = cartItem.Count + cartCount;
            }
            // Save changes
            db.SaveChanges();
        }
        public int UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cartItem = db.carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartCount > 0)
                {
                    cartItem.Count = cartCount;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.carts.Remove(cartItem);
                }
                // Save changes 
                db.SaveChanges();
            }
            return itemCount;
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.carts.Single(
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
                db.carts.Remove(cartItem);
                //}
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            babymart_vnEntities db = new babymart_vnEntities();
            var cartItems = db.carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }
        public List<ModelCart> GetCartItem()
        {
            var tmp = db.carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
            List<ModelCart> data = new List<ModelCart>();
            foreach (var item in tmp)
            {
                var cart = new ModelCart();
                cart.CartId = item.CartId;
                cart.Count = item.Count;
                cart.ProductId = item.ProductId;
                cart.DateCreated = item.DateCreated;
                cart.RecordId = item.RecordId;
                //var i = item.shop_bienthe.gia;
                cart.shop_bienthe = new ModelBienthe();

                if (item.shop_bienthe.giasosanh != null && item.shop_bienthe.giasosanh > 0)
                {
                    if (item.shop_bienthe.shop_sanpham.ischecksaleoff == true)
                    {
                        var kh = HttpContext.Current.Session[Convert.ToString("khachhang")];
                        if (kh != null)
                        {
                            cart.shop_bienthe.gia = item.shop_bienthe.gia;
                            cart.shop_bienthe.giasosanh = item.shop_bienthe.giasosanh;
                        }
                        else
                        {
                            cart.shop_bienthe.gia = item.shop_bienthe.giasosanh;
                        }
                    }
                    else
                    {
                        cart.shop_bienthe.gia = item.shop_bienthe.gia;
                        cart.shop_bienthe.giasosanh = item.shop_bienthe.giasosanh;
                    }

                }
                else
                {
                    cart.shop_bienthe.gia = item.shop_bienthe.gia;
                    cart.shop_bienthe.giasosanh = item.shop_bienthe.giasosanh;
                }

                cart.shop_bienthe.id = item.shop_bienthe.id;
                cart.shop_bienthe.title = item.shop_bienthe.title;
                cart.shop_bienthe.imgsp = item.shop_bienthe.shop_sanpham.shop_image.Count > 0 ? item.shop_bienthe.shop_sanpham.shop_image.FirstOrDefault().url : null;
                cart.shop_bienthe.tensp = item.shop_bienthe.shop_sanpham.tensp;
                cart.shop_bienthe.kg = item.shop_bienthe.shop_sanpham.kg;
                cart.shop_bienthe.chieucao = item.shop_bienthe.shop_sanpham.chieucao;
                cart.shop_bienthe.chieudai = item.shop_bienthe.shop_sanpham.chieudai;
                cart.shop_bienthe.chieurong = item.shop_bienthe.shop_sanpham.chieurong;
                cart.shop_bienthe.idsp = item.shop_bienthe.idsp;
                cart.shop_bienthe.masp = item.shop_bienthe.shop_sanpham.masp;
                cart.shop_bienthe.ischecksaleoff = item.shop_bienthe.shop_sanpham.ischecksaleoff;
                data.Add(cart);
            }
            return data;
        }
        public List<cart> GetCartItems()
        {
            return db.carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();

        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {

            //decimal? total = (from cartItems in db.carts
            //                  where cartItems.CartId == ShoppingCartId
            //                  select (int?)cartItems.Count *
            //                  cartItems.shop_bienthe.gia).Sum();

            var tmp = (from cartItems in db.carts where cartItems.CartId == ShoppingCartId select cartItems).ToList();
            //var tomp = Mapper.Map<List<ModelCart>>(tmp.ToList());
            //decimal? total = (from cartItems in db.carts
            //                  where cartItems.CartId == ShoppingCartId
            //                  select (int?)cartItems.Count *
            //                  cartItems.shop_bienthe.gia).Sum();

            decimal? sum = 0;
            foreach (var item in tmp)
            {
                if (item.shop_bienthe.giasosanh != null && item.shop_bienthe.giasosanh > 0)
                {
                    if (item.shop_bienthe.shop_sanpham.ischecksaleoff == true)
                    {
                        var kh = HttpContext.Current.Session[Convert.ToString("khachhang")];
                        if (kh != null)
                        {
                            sum += (item.shop_bienthe.gia * item.Count);
                        }
                        else
                        {
                            sum += (item.shop_bienthe.giasosanh * item.Count);
                        }
                    }
                    else
                    {
                        sum += (item.shop_bienthe.gia * item.Count);
                    }
                }
                else
                {
                    sum += (item.shop_bienthe.gia * item.Count);
                }
            }

            return sum ?? decimal.Zero;
        }
        public double? GetTotalKg()
        {
            var a = (from cartItems in db.carts
                     where cartItems.CartId == ShoppingCartId
                     select (int?)cartItems.Count *
                     cartItems.shop_bienthe.shop_sanpham.kg).Sum();
            double? total = (from cartItems in db.carts
                             where cartItems.CartId == ShoppingCartId
                             select (int?)cartItems.Count *
                             cartItems.shop_bienthe.shop_sanpham.kg).Sum();
            var ac = (from cartItems in db.carts
                      where cartItems.CartId == ShoppingCartId
                      select cartItems);

            return total;
        }
        //public int CreateOrder(DONDATHANG order)
        //{
        //    decimal orderTotal = 0;

        //    var cartItems = GetCartItems();
        //    // Iterate over the items in the cart, 
        //    // adding the order details for each
        //    foreach (var item in cartItems)
        //    {
        //        var orderDetail = new OrderDetail
        //        {
        //            ProductId = item.ProductId,
        //            OrderId = order.OrderId,
        //            UnitPrice = item.sanpham.Price,
        //            Quantity = item.Count
        //        };
        //        // Set the order total of the shopping cart
        //        orderTotal += (item.Count * item.sanpham.Price);

        //        db.OrderDetails.Add(orderDetail);

        //    }
        //    // Set the order's total to the orderTotal count
        //    order.Total = orderTotal;

        //    // Save the order
        //    db.SaveChanges();
        //    // Empty the shopping cart
        //    EmptyCart();
        //    // Return the OrderId as the confirmation number
        //    return order.OrderId;
        //}
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
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
            var shoppingCart = db.carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }
    }
}