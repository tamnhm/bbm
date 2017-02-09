using Babymart.DAL.Tags;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Babymart.DAL.Administrator;
using Babymart.Models.Module.Enum;
using Babymart.DAL.Customer;
using Babymart.Infractstructuree;
using System.Web.Mvc;

namespace Babymart.Infractstructure
{
    public static class UtilsBB
    {
        static babymart_vnEntities db = new babymart_vnEntities();
        static IAdministratorRepository _administratorRepository = new AdministratorRepository(new babymart_vnEntities());
        static ITagsRepository _iTagsRepository = new TagsRepository(new babymart_vnEntities());
        static ICustomer _iCustomer = new Customer(new babymart_vnEntities());
        public static adminModel GetAdminStrator()
        {
            var tmpAdim = HttpContext.Current.Session[Convert.ToString("AdminStrator")];
            if (tmpAdim == null)
                return null;
            var adminStrator = (admin)HttpContext.Current.Session[Convert.ToString("AdminStrator")];
            return Mapper.Map<adminModel>(adminStrator);
        }
        public static bool GetAdminStratorRole(int roleAdmin)
        {
            var tmpAdim = HttpContext.Current.Session[Convert.ToString("AdminStrator")];
            if (tmpAdim == null)
                return false;
            var adminStrator = (admin)HttpContext.Current.Session[Convert.ToString("AdminStrator")];
            var role = _administratorRepository.AdminRole(adminStrator.id);
            if (role != null && role.Count > 0)
            {
                var ishasRoleAdmin = role.Where(x => x.Role == (int)RoleAdmin.Admin).FirstOrDefault();
                if (ishasRoleAdmin != null)
                    return true;
                var ishasRole = role.Where(x => x.Role == roleAdmin).FirstOrDefault();
                if (ishasRole != null)
                    return true;
                return false;
            }
            return false;
        }
        public static int GetStoreId()
        {
            return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["StoreID"]);
        }

        public static void AddToCart(int id, int cartCount)
        {
            if (HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] != null)
            {
                var cartCur = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
                var tmp = new List<ModelSessionCart>();
                if (cartCur != null)
                {
                    var indexOf = cartCur.FindIndex(a => a.ProductId == id);

                    if (indexOf < 0)
                    {
                        cartCur.Add(new ModelSessionCart
                        {
                            ProductId = id,
                            Count = cartCount
                        });
                    }
                    else
                    {

                        foreach (var item in cartCur)
                        {
                            if (id == item.ProductId)
                            {
                                item.Count += cartCount;
                            }
                        }
                    }

                    //foreach (var item in cartCur)
                    //{
                    //    if (id == item.ProductId)
                    //    {
                    //        item.Count += cartCount;
                    //    }
                    //    else
                    //    {
                    //        tmp.Add(new ModelSessionCart
                    //        {
                    //            ProductId = id,
                    //            Count = cartCount
                    //        });
                    //    }
                    //}
                }
                HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] = cartCur;
            }
            else
            {
                var model = new List<ModelSessionCart>();
                model.Add(new ModelSessionCart
                {
                    ProductId = id,
                    Count = cartCount
                });
                HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] = model;
            }
        }
        public static List<ModelCart> GetCartItem()
        {
            List<ModelCart> data = new List<ModelCart>();
            var currentCart = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
            if (currentCart != null)
            {
                foreach (var item in currentCart)
                {
                    data.Add(GetItem((int)item.ProductId, item.Count));
                }
            }
            return data;
        }
        public static ModelCart GetItem(int id, int count)
        {
            var bienthe = db.shop_bienthe.Find(id);

            var q = from product in db.shop_sanpham
                    where product.id == bienthe.idsp
                    && product.hide == false
                    select product;

            var sanpham = q.FirstOrDefault();
            var cart = new ModelCart();
            cart.Count = count;
            cart.ProductId = id;
            cart.RecordId = id;
            cart.shop_bienthe = new ModelBienthe();
            if (bienthe.giasosanh != null && bienthe.giasosanh > 0)
            {
                if (bienthe.shop_sanpham.ischecksaleoff == true)
                {
                    var kh = HttpContext.Current.Session[Convert.ToString("khachhang")];
                    if (kh != null)
                    {
                        cart.shop_bienthe.gia = bienthe.gia;
                        cart.shop_bienthe.giasosanh = bienthe.giasosanh;
                    }
                    else
                    {
                        cart.shop_bienthe.gia = bienthe.giasosanh;
                    }
                }
                else
                {
                    cart.shop_bienthe.gia = bienthe.gia;
                    cart.shop_bienthe.giasosanh = bienthe.giasosanh;
                }

            }
            else
            {
                cart.shop_bienthe.gia = bienthe.gia;
                cart.shop_bienthe.giasosanh = bienthe.giasosanh;
            }
            cart.shop_bienthe.giasosanh2 = bienthe.giasosanh;
            cart.shop_bienthe.spurl = bienthe.shop_sanpham.spurl;
            cart.shop_bienthe.id = bienthe.id;
            cart.shop_bienthe.title = UtilsBB.HasCookieLangEn(bienthe.title_us) ? bienthe.title_us : bienthe.title;
            cart.shop_bienthe.imgsp = bienthe.shop_sanpham.shop_image.Count > 0 ? bienthe.shop_sanpham.shop_image.FirstOrDefault().url : null;
            cart.shop_bienthe.tensp = UtilsBB.HasCookieLangEn() ? bienthe.shop_sanpham.tensp_us : bienthe.shop_sanpham.tensp;
            cart.shop_bienthe.kg = bienthe.shop_sanpham.kg;
            cart.shop_bienthe.chieucao = bienthe.shop_sanpham.chieucao;
            cart.shop_bienthe.chieudai = bienthe.shop_sanpham.chieudai;
            cart.shop_bienthe.chieurong = bienthe.shop_sanpham.chieurong;
            cart.shop_bienthe.idsp = bienthe.idsp;
            cart.shop_bienthe.masp = bienthe.shop_sanpham.masp;
            cart.shop_bienthe.ischecksaleoff = sanpham.ischecksaleoff;
            return cart;
        }
        public static double GetTotalKg()
        {
            double total = 0;
            var currentCart = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
            foreach (var item in currentCart)
            {
                var bienthe = db.shop_bienthe.Find(item.ProductId);
                if (bienthe.shop_sanpham.kg == null)
                    bienthe.shop_sanpham.kg = 0;
                total += bienthe.shop_sanpham.kg.Value * item.Count;
            }
            return total;
        }
        public static double GetTotalW_H_LCart()
        {
            double total = 0;
            var currentCart = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
            foreach (var item in currentCart)
            {
                var bienthe = db.shop_bienthe.Find(item.ProductId);
                if (bienthe.shop_sanpham.chieucao == null)
                    bienthe.shop_sanpham.chieucao = 0;
                if (bienthe.shop_sanpham.chieudai == null)
                    bienthe.shop_sanpham.chieudai = 0;
                if (bienthe.shop_sanpham.chieurong == null)
                    bienthe.shop_sanpham.chieurong = 0;
                total += ((bienthe.shop_sanpham.chieucao.Value * bienthe.shop_sanpham.chieurong.Value * bienthe.shop_sanpham.chieudai.Value) / 6000) * item.Count;
            }
            return total;
        }

        public static decimal GetTotal()
        {
            var currentCart = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];

            decimal? sum = 0;
            if (currentCart != null)
            {
                foreach (var item in currentCart)
                {
                    var bienthe = db.shop_bienthe.Find(item.ProductId);
                    if (bienthe.giasosanh != null && bienthe.giasosanh > 0)
                    {
                        if (bienthe.shop_sanpham.ischecksaleoff == true)
                        {
                            var kh = HttpContext.Current.Session[Convert.ToString("khachhang")];
                            if (kh != null)
                            {
                                sum += (bienthe.gia * item.Count);
                            }
                            else
                            {
                                sum += (bienthe.giasosanh * item.Count);
                            }
                        }
                        else
                        {
                            sum += (bienthe.gia * item.Count);
                        }
                    }
                    else
                    {
                        sum += (bienthe.gia * item.Count);
                    }
                }
            }


            return sum ?? decimal.Zero;

        }
        public static void EmptyCart()
        {
            HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] = null;
        }
        public static void RemoveFromCarts(int id)
        {
            if (HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] != null)
            {
                var cartCur = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
                if (cartCur != null)
                {
                    var tmpRemove = cartCur.Single(o => o.ProductId == id);
                    cartCur.Remove(tmpRemove);
                }
                HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] = cartCur;
            }
        }
        public static int GetCount()
        {
            var currentCart = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
            if (currentCart != null)
            {
                var pro = 1;

                foreach (var item in currentCart)
                {
                    pro += item.Count;
                }
                return pro;
            }

            return 0;
        }
        public static void UpdateCartCount(int id, int cartCount)
        {
            if (HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] != null)
            {
                var cartCur = (List<ModelSessionCart>)HttpContext.Current.Session[Convert.ToString("ModelSessionCart")];
                if (cartCur != null)
                {
                    foreach (var item in cartCur)
                    {
                        if (id == item.ProductId)
                        {
                            item.Count = cartCount;
                        }
                    }
                }
                HttpContext.Current.Session[Convert.ToString("ModelSessionCart")] = cartCur;
            }
        }
        public static HttpCookie GetHttpCookieLang()
        {
            MyCookie cookieLang = new MyCookie("CookieLang");
            var langCookie = cookieLang.GetCookie();
            return langCookie;
        }
        public static bool HasCookieLangEn()
        {
            var langCookie = GetHttpCookieLang();
            if (langCookie != null && langCookie.Value == "en")
                return true;
            return false;
        }
        public static bool HasCookieLangEn(string value)
        {
            var langCookie = GetHttpCookieLang();
            if (!string.IsNullOrEmpty(value) && langCookie != null && langCookie.Value == "en")
                return true;
            return false;
        }
        public static void SetCookie_User()
        {
            if (HttpContext.Current.Session["khachhang"] == null)
            {
                var cookie = HttpContext.Current.Request.Cookies["Cookie_customer"];
                if (cookie != null)
                {
                    var u = _iCustomer.Login(cookie["Cookie_customer_tendn"], cookie["Cookie_customer_matkhau"]);
                    if (u != null)
                    {
                        HttpContext.Current.Session["khachhang"] = Mapper.Map<ModeCustomerPost>(u);
                    }

                }
            }
        }
        public static bool IsMobileDevice()
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
                return true;
            return
                false;
        }
        public static ContentResult JsonMaxValue(object result)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return new ContentResult
            {
                Content = serializer.Serialize(result),
                ContentType = "application/json"
            };
        }
        //protected override JsonResult JsonMaxValue(object data, 
        //    string contentType,
        //    System.Text.Encoding contentEncoding, 
        //    JsonRequestBehavior behavior)
        //{
        //    return new JsonResult()
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = Int32.MaxValue
        //    };
        //}
    }


}