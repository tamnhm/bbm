using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.ViewModels;
using Babymart.Models.Module;
using Babymart.Infractstructure;
namespace Babymart.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        babymart_vnEntities db = new babymart_vnEntities();

        public ActionResult Index()
        {
            var cartItem = UtilsBB.GetCartItem();
            if (cartItem.Count <= 0)
            {
                this.Session["ModelSessionCart"] = null;
            }
            var viewModel = new ShoppingCartViewModel
                       {
                           CartItemModel = cartItem,
                           CartTotal = UtilsBB.GetTotal()
                       };
            return View(viewModel);
        }
        public ActionResult ChoosePay()
        {
            return View();
        }
        public ActionResult AddToCart(int id, int cartCount)
        {
            UtilsBB.AddToCart(id, cartCount);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            var a = UtilsBB.GetCartItem();
            var cartitem = UtilsBB.GetItem(id, cartCount);
            int count = UtilsBB.GetCount();
            try
            {
                UtilsBB.UpdateCartCount(id, cartCount);
                results = new ShoppingCartRemoveViewModel
                          {
                              CartTotal = UtilsBB.GetTotal(),
                              CartCount = count,
                              ItemCount = count,
                              DeleteId = id,
                              Price = string.Format("{0:0,0 đ}", cartitem.shop_bienthe.gia * cartCount),
                              CartTotals = string.Format("{0:0,0 đ}", UtilsBB.GetTotal())
                          };
            }
            catch
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Error occurred or invalid input...",
                    CartTotal = -1,
                    CartCount = -1,
                    ItemCount = -1,
                    DeleteId = id
                };
            }
            return Json(results);
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            UtilsBB.RemoveFromCarts(id);
            var sum = UtilsBB.GetTotal();
            var results = new ShoppingCartRemoveViewModel
            {

                CartTotal = sum,
                CountPro = UtilsBB.GetCount(),
                DeleteId = id
            };
            if (sum <= 0)
            {
                this.Session["ModelSessionCart"] = null;
            }
            return Json(results);

            //return RedirectToAction("Index");
        }

    }
}
