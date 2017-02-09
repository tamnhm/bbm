using System.Web.Mvc;
using System.Web.Routing;

namespace Babymart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*Shop*/
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Message_Error",
              url: "404.html",
              defaults: new { controller = "Message", action = "Error" }
          );
            routes.MapRoute(
              name: "Message_Error_Direct",
              url: "404-errors.html",
              defaults: new { controller = "Message", action = "Errordirect" }
          );
            routes.MapRoute(
          name: "CollectionProduct",
          url: "chuyen-muc/{urlcollection}.html",
          defaults: new { controller = "Shop", action = "CollectionProduct", urlcollection = UrlParameter.Optional }
          );
            routes.MapRoute(
        name: "CollectionProductEN",
        url: "en/group/{urlcollection}.html",
        defaults: new { controller = "Shop", action = "CollectionProduct", urlcollection = UrlParameter.Optional, en = "en" }
        );
            routes.MapRoute(
            name: "Plan",
            url: "goi-do-so-sinh.html",
            defaults: new { controller = "Plan", action = "Index" }
            );
            routes.MapRoute(
           name: "PlanEN",
           url: "en/new-baby-checklist.html",
           defaults: new { controller = "Plan", action = "Index", en = "en" }
           );
            routes.MapRoute(
          name: "CollectionParentProduct",
          url: "danh-muc/{urlcollection}.html",
          defaults: new { controller = "Shop", action = "CollectionParentProduct", urlproduct = UrlParameter.Optional }
          );
            routes.MapRoute(
         name: "CollectionParentProductEn",
         url: "en/category/{urlcollection}.html",
         defaults: new { controller = "Shop", action = "CollectionParentProduct", urlproduct = UrlParameter.Optional, en = "en" }
         );
            routes.MapRoute(
          name: "GetProByBrand",
          url: "thuong-hieu/{url}.html",
          defaults: new { controller = "Shop", action = "GetProByBrand", url = UrlParameter.Optional }
          );
            routes.MapRoute(
         name: "GetProByBrandEN",
         url: "en/lable/{url}.html",
         defaults: new { controller = "Shop", action = "GetProByBrand", url = UrlParameter.Optional, en = "en" }
         );
            routes.MapRoute(
            name: "SaleoffProduct",
            url: "san-pham/khuyen-mai.html",
            defaults: new { controller = "Shop", action = "ProductSaleoff" }
            );
            routes.MapRoute(
           name: "SaleoffProductEN",
           url: "en/product/saleoff.html",
           defaults: new { controller = "Shop", action = "ProductSaleoff", en = "en" }
           );
            routes.MapRoute(
            name: "NotSaleoffProduct",
            url: "san-pham/het-han-khuyen-mai.html",
            defaults: new { controller = "Shop", action = "ProductNotSaleoff" }
            );
            routes.MapRoute(
            name: "ProductbyTags",
            url: "san-pham/tags/{tag}",
            defaults: new { controller = "Shop", action = "ProductbyTags", tag = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "ProductbyTagsEN",
            url: "en/san-pham/tags/{tag}",
            defaults: new { controller = "Shop", action = "ProductbyTags", tag = UrlParameter.Optional, en = "en" }
            );
            routes.MapRoute(
           name: "DetailProduct",
           url: "tin-tuc/{urlproduct}.html",
           defaults: new { controller = "Shop", action = "DeatialProduct", urlproduct = UrlParameter.Optional }
           );
            routes.MapRoute(
          name: "DetailProductEN",
          url: "en/product/{urlproduct}.html",
          defaults: new { controller = "Shop", action = "DeatialProduct", urlproduct = UrlParameter.Optional, en = "en" }
          );
            routes.MapRoute(
              name: "Search",
              url: "tim-kiem.html",
              defaults: new { controller = "Shop", action = "Search" }
          );
            routes.MapRoute(
              name: "Home",
              url: "home.html",
              defaults: new { controller = "Shop", action = "Home" }
          );
            routes.MapRoute(
            name: "HomeEN",
            url: "en",
            defaults: new { controller = "Shop", action = "Index", en = "en" }
        );
            //  routes.MapRoute(
            //    name: "Trangchu",
            //    url: "trang-chu.html",
            //    defaults: new { controller = "Shop", action = "Index" }
            //);
            /***********************************************************************************/
            routes.MapRoute(
           name: "ForgetPassword",
           url: "quenmatkhau.html",
           defaults: new { controller = "Customer", action = "ForgetPassword" }
       );

            routes.MapRoute(
          name: "ResetPassword",
          url: "kichhoatmatkhaumoi/{email}/{pass}.html",
          defaults: new { controller = "Customer", action = "ResetPassword", email = UrlParameter.Optional, pass = UrlParameter.Optional }
      );
            routes.MapRoute(
             name: "RegisterCustomer",
             url: "dang-ky.html",
             defaults: new { controller = "Customer", action = "Register" }
         );

            routes.MapRoute(
          name: "ContactCustomer",
          url: "lien-he.html",
          defaults: new { controller = "Customer", action = "lienhe" }
      );
            routes.MapRoute(
          name: "LoginCustomer",
          url: "dang-nhap.html",
          defaults: new { controller = "Customer", action = "Login" }
        );
            routes.MapRoute(
       name: "InfCustomer",
       url: "thong-tin-tai-khoan.html",
       defaults: new { controller = "Customer", action = "Index" }
     );
            routes.MapRoute(
          name: "HistoryCartDetail",
          url: "chi-tiet-don-hang.html/{Id}",
          defaults: new { controller = "Customer", action = "HistoryCartDetail", Id = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "Cart",
            url: "gio-hang.html",
            defaults: new { controller = "Cart", action = "Index" }
            );
            routes.MapRoute(
           name: "CartChoosePay",
           url: "dat-hang.html",
           defaults: new { controller = "Cart", action = "ChoosePay" }
           );
            routes.MapRoute(
         name: "SosinhChoosePay",
         url: "dat-hang-do-so-sinh.html",
         defaults: new { controller = "Plan", action = "ChoosePay" }
         );
            routes.MapRoute(
            name: "CartAdd",
            url: "gio-hang.html/{id}/{cartCount}",
            defaults: new { controller = "Cart", action = "AddToCart", urlgroup = UrlParameter.Optional, id = UrlParameter.Optional, cartCount = UrlParameter.Optional }

            );
            routes.MapRoute(
         name: "Checkout",
         url: "xac-nhan.html",
         defaults: new { controller = "Checkout", action = "Index" }
         );
            routes.MapRoute(
          name: "CheckoutCofim",
          url: "xac-nhan-2.html",
          defaults: new { controller = "Checkout", action = "step2" }
          );
            //   routes.MapRoute(
            //name: "CheckoutSussce",
            //url: "xac-nhan-thanh-cong.html/{MaDH}",
            //defaults: new { controller = "Checkout", action = "Success", MaDH = UrlParameter.Optional }
            //);
            routes.MapRoute(
         name: "CheckoutSussce",
         url: "xac-nhan-thanh-cong.html",
         defaults: new { controller = "Checkout", action = "Success" }
         );
            routes.MapRoute(
     name: "CancelCart",
     url: "huy-gio-hang.html",
     defaults: new { controller = "Checkout", action = "CancelCart" }
     );
            routes.MapRoute(
           name: "Page",
           url: "bai-viet/{url}.html",
           defaults: new { controller = "Pages", action = "Index", url = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "PageTd",
           url: "tuyen-dung.html",
           defaults: new { controller = "Pages", action = "Indextd", url = UrlParameter.Optional }
           );
            routes.MapRoute(
        name: "CheckoutPlan",
        url: "xac-nhan-do-so-sinh.html",
        defaults: new { controller = "CheckoutPlan", action = "Index" }
        );
            routes.MapRoute(
          name: "CheckoutCofimplan",
          url: "xac-nhan-2-do-so-sinh.html",
          defaults: new { controller = "CheckoutPlan", action = "step2" }
          );
            routes.MapRoute(
         name: "CheckoutSusscePlan",
         url: "xac-nhan-thanh-cong-do-so-sinh.html/{MaDH}",
         defaults: new { controller = "CheckoutPlan", action = "Success", MaDH = UrlParameter.Optional }
         );
            routes.MapRoute(
        name: "CancelPlan",
        url: "huy-do-so-sinh.html",
        defaults: new { controller = "CheckoutPlan", action = "CancelCart" }
        );
            /**********Magazine************/
            routes.MapRoute(
              name: "Magazine",
              url: "cam-nang.html",
              defaults: new { controller = "Magazine", action = "Index", url = UrlParameter.Optional }
              );
            routes.MapRoute(
           name: "MagazineCollectionTags",
           url: "cam-nang/tags/{tag}",
           defaults: new { controller = "Magazine", action = "ListTag", tag = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "MagazineCollection",
           url: "cam-nang/{urlmenu}.html",
           defaults: new { controller = "Magazine", action = "Article", urlmenu = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "MagazineCollectionChild",
            url: "cam-nang/{urlmenu}/{urlgroup}.html",
            defaults: new { controller = "Magazine", action = "ArticleChild", urlgroup = UrlParameter.Optional, urlmenu = UrlParameter.Optional }
            );
            routes.MapRoute(
           name: "MagazineAricleDetail",
           url: "cam-nang/bai-viet/{id}/{url}.html",
           defaults: new { controller = "Magazine", action = "Detail", id = UrlParameter.Optional, url = UrlParameter.Optional }
           );
            routes.MapRoute(
         name: "MagazineAricleDetailUrlgroup",
         url: "cam-nang/{urlgroup}/{id}/{url}.html",
         defaults: new { controller = "Magazine", action = "Detail", id = UrlParameter.Optional, url = UrlParameter.Optional, urlgroup = UrlParameter.Optional }
         );

            /**********Game************/
            // routes.MapRoute(
            //name: "Game",
            //url: "game.html",
            //defaults: new { controller = "Game", action = "Index", url = UrlParameter.Optional }
            //);
            // routes.MapRoute(
            //  name: "GameCollection",
            //  url: "game/{urlmenu}.html",
            //  defaults: new { controller = "Game", action = "Article", urlmenu = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //  name: "GameCollectionChild",
            //  url: "game/{urlmenu}/{urlgroup}.html",
            //  defaults: new { controller = "Game", action = "ArticleChild", urlgroup = UrlParameter.Optional, urlmenu = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //name: "GameDetail",
            //url: "game/{urlgroup}/{id}/{url}.html",
            //defaults: new { controller = "Game", action = "Detail", urlgroup = UrlParameter.Optional, id = UrlParameter.Optional, url = UrlParameter.Optional }
            //);
            /****************New**************/
            routes.MapRoute(
             name: "News",
             url: "tintuc.html",
             defaults: new { controller = "News", action = "Index", url = UrlParameter.Optional }
             );
            routes.MapRoute(
            name: "NewsCollection",
            url: "tintuc/{urlmenu}.html",
            defaults: new { controller = "News", action = "Article", urlmenu = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "NewsCollectionChild",
            url: "tintuc/{urlmenu}/{urlgroup}.html",
            defaults: new { controller = "News", action = "ArticleChild", urlgroup = UrlParameter.Optional, urlmenu = UrlParameter.Optional }
            );
            routes.MapRoute(
           name: "NewsDetail",
           url: "tintuc/{urlgroup}/{id}/{url}.html",
           defaults: new { controller = "News", action = "Detail", urlgroup = UrlParameter.Optional, id = UrlParameter.Optional, url = UrlParameter.Optional }
           );
            /****************Animation**************/
            // routes.MapRoute(
            //  name: "Cartoon",
            //  url: "hoat-hinh.html",
            //  defaults: new { controller = "Cartoon", action = "Index", url = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //  name: "CartoonCollection",
            //  url: "hoat-hinh/{urlmenu}.html",
            //  defaults: new { controller = "Cartoon", action = "Article", urlmenu = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //  name: "CartoonCollectionChild",
            //  url: "hoat-hinh/{urlmenu}/{urlgroup}.html",
            //  defaults: new { controller = "Cartoon", action = "ArticleChild", urlgroup = UrlParameter.Optional, urlmenu = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //name: "CartoonDetail",
            //url: "hoat-hinh/{urlgroup}/{id}/{url}.html",
            //defaults: new { controller = "Cartoon", action = "Detail", urlgroup = UrlParameter.Optional, id = UrlParameter.Optional, url = UrlParameter.Optional }
            //);
            // /****************Music**************/
            // routes.MapRoute(
            //  name: "Music",
            //  url: "am-nhac.html",
            //  defaults: new { controller = "Music", action = "Index", url = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //  name: "MusicCollection",
            //  url: "am-nhac/{urlmenu}.html",
            //  defaults: new { controller = "Music", action = "Article", urlmenu = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //  name: "MusicCollectionChild",
            //  url: "am-nhac/{urlmenu}/{urlgroup}.html",
            //  defaults: new { controller = "Music", action = "ArticleChild", urlgroup = UrlParameter.Optional, urlmenu = UrlParameter.Optional }
            //  );
            // routes.MapRoute(
            //name: "MusicDetail",
            //url: "am-nhac/{urlgroup}/{id}/{url}.html",
            //defaults: new { controller = "Music", action = "Detail", urlgroup = UrlParameter.Optional, id = UrlParameter.Optional, url = UrlParameter.Optional }
            //);
            /****************Default**************/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Shop", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Babymart.Controllers" }
            );
        }
    }
}