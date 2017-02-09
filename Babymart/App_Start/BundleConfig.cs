using System.Web;
using System.Web.Optimization;

namespace Babymart
{
    public class BundleConfig
    {

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region System
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/libs/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/libs/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libs/jquery.unobtrusive*",
                        "~/Scripts/libs/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/libs/modernizr-*"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            #endregion
            #region Me

            bundles.Add(new ScriptBundle("~/bundles/commonjs_ko").Include(
                        "~/Scripts/libs/knockout-{version}.js",
                        "~/Scripts/libs/knockout.mapping-latest.js",
                        "~/Scripts/libs/knockout.validation.js",
                        "~/Scripts/libs/knockout.dirtyFlag.js",
                         "~/Scripts/me/CommonUtils.js",
                         "~/Scripts/me/custom/toastr.js",
                        "~/Scripts/me/sammy-{version}.js",
                        "~/Scripts/me/sammy.title.js",
                          "~/Scripts/me/ko.BHN.js",
                          "~/Scripts/module/common/m.image.js",
                          "~/Scripts/libs/jquery_globalize/globalize.js",
                          "~/Scripts/libs/jquery_globalize/cultures/globalize.culture.vi-VN.js",
                          "~/Scripts/libs/jquery_globalize/cultures/globalize.culture.en-US.js"
                      ));

            bundles.Add(new ScriptBundle("~/module/product").Include(
                        "~/Scripts/module/product/mproduct.js",
                         "~/Scripts/module/product/mvproduct.js",
                         "~/Scripts/module/product/mvedit.js",
                         "~/Scripts/module/product/mvlist.js",
                         "~/Scripts/module/product/mvadd.js",
                         "~/Scripts/module/product/rproduct.js",
                           "~/Scripts/module/link/mlink.js"

                         ));
            bundles.Add(new ScriptBundle("~/module/link").Include(
                      "~/Scripts/module/link/mlink.js",
                         "~/Scripts/module/link/mvlist.js",
                           "~/Scripts/module/link/mvlink.js"
                       ));
            bundles.Add(new ScriptBundle("~/module/brand").Include(
                    "~/Scripts/module/brand/mbrand.js",
                      "~/Scripts/module/brand/mvbrand.js",
                       "~/Scripts/module/brand/mvlist.js",
                         "~/Scripts/module/brand/mvedit.js"
                     ));
            bundles.Add(new ScriptBundle("~/module/langdingpage").Include(
                    "~/Scripts/module/langdingpage/mlangdingpage.js",
                      "~/Scripts/module/langdingpage/mvlangdingpage.js"

                     ));
            bundles.Add(new ScriptBundle("~/module/shipping").Include(
                    "~/Scripts/module/shipping/mshipping.js",
                      "~/Scripts/module/shipping/mvshipping.js"

                     ));
            bundles.Add(new ScriptBundle("~/module/customer").Include(
                   "~/Scripts/module/order/morder.js",
                    "~/Scripts/module/product/mproduct.js",
                   "~/Scripts/module/customer/mcustomer.js",
                   "~/Scripts/module/shipping/mshipping.js",
                   "~/Scripts/module/customer/mvcustomer.js",
                   "~/Scripts/module/customer/mvedit.js",
                   "~/Scripts/module/customer/mvlist.js"

                    ));
            bundles.Add(new ScriptBundle("~/module/cart").Include(
                   "~/Scripts/module/cart/mcart.js",
                   "~/Scripts/module/cart/mvcart.js"

                    ));
            bundles.Add(new ScriptBundle("~/module/order").Include(
                  "~/Scripts/module/order/morder.js",
                  "~/Scripts/module/product/mproduct.js",
                  "~/Scripts/module/order/mvlist.js",
                  "~/Scripts/module/order/mvorder.js"

                   ));
            bundles.Add(new ScriptBundle("~/module/module").Include(
                 "~/Scripts/module/module/m.module.js",
                 "~/Scripts/module/module/mv.edit.js",
                 "~/Scripts/module/module/mv.add.js",
                 "~/Scripts/module/module/mv.list.js",
                 "~/Scripts/module/module/mv.module.js"

                  ));
            bundles.Add(new ScriptBundle("~/module/pages").Include(
                "~/Scripts/module/pages/m.page.js",
                "~/Scripts/module/pages/mv.edit.js",
                "~/Scripts/module/pages/mv.list.js",
                "~/Scripts/module/pages/mv.page.js"

                 ));
            bundles.Add(new ScriptBundle("~/module/file").Include(
                "~/Scripts/module/file/m.file.js",
                "~/Scripts/module/file/mv.file.js"

                 ));
            bundles.Add(new ScriptBundle("~/module/banner").Include(
               "~/Scripts/module/banner/m.banner.js",
               "~/Scripts/module/banner/mv.banner.js"

                ));

            bundles.Add(new ScriptBundle("~/module/contents").Include(
             "~/Scripts/module/contents/m.contents.js",
             "~/Scripts/module/contents/mv.contents.js"

              ));
            bundles.Add(new ScriptBundle("~/module/checkout_plan").Include(
            "~/Scripts/module/checkoutplan/m.checkoutPlan.js",
            "~/Scripts/module/checkoutplan/mv.checkoutPlan_step1.js",
            "~/Scripts/module/checkoutplan/mv.checkoutPlan.js"
             ));
            bundles.Add(new ScriptBundle("~/module/checkout").Include(
              "~/Scripts/module/checkout/mcheckout.js",
               "~/Scripts/module/checkout/mvcheckout_step1.js",
              "~/Scripts/module/checkout/mvcheckout.js"
              ));
            #endregion

        }
    }
}