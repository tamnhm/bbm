using Babymart.DAL.Product;
using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Babymart.Models.Module;
using Babymart.DAL.Plan;
using Babymart.Models.Module.Plan;
using Babymart.Infractstructuree;
using Babymart.DAL.Module;
using Babymart.Models.Module.Enum;
using Babymart.DAL.Common;
using Babymart.Infractstructure;
namespace Babymart.Controllers
{
    public class PlanController : Controller
    {
        //
        // GET: /Plan/
        private IProductRepository _productRepository;
        private IModuleRepository _moduleRepository;
        private IPlanRepository _plan;
        private ICommonRepository _common;
        babymart_vnEntities _context = new babymart_vnEntities();
        public PlanController()
        {
            this._productRepository = new ProductRepository(new babymart_vnEntities());
            this._plan = new PlanRepository(new babymart_vnEntities());
            this._moduleRepository = new ModuleRepository(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
        }

        public ActionResult Index(string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var str = ">>Gói đồ sơ sinh";
            ViewBag.Tags = str;
            var listId = TagsCommon.ListRefIdTagBy((int)Tags.TagsModuleMagazine, str);
            if (listId != null && listId.Count > 0)
            {
                var listArticalbyTag = _moduleRepository.GetListArticlebyIds(listId.ToArray());
                listArticalbyTag = listArticalbyTag.OrderByDescending(o => o.createdate).ToList();
                foreach (var obj in listArticalbyTag)
                {
                    obj.extract = _common.SplitString(obj.extract, 75);
                }
                ViewData["ArticlesTags"] = Mapper.Map<List<ModelModuleDetail>>(listArticalbyTag);
            }
            return View();
        }
        public JsonResult GetListSpForPlan(int type)
        {
            var result = _productRepository.GetListSpForPlan(type);
            var sp = Mapper.Map<List<PlanBienthe>>(result);
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetlistShop_plan_saleoff()
        {
            var result = _plan.GetlistShop_plan_saleoff().Where(o => o.Visible == false).ToList();
            var sp = Mapper.Map<List<Plan_SaleoffModel>>(result);
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetlistShop_plan_type()
        {
            var result = Mapper.Map<List<Plan_type>>(_plan.GetlistShop_plan_type());
            if (UtilsBB.HasCookieLangEn() == true)
            {
                foreach (var item in result)
                {
                    item.Type = !string.IsNullOrEmpty(item.Type_us) ? item.Type_us : item.Type;
                }
            }

            var resultsaleoff = Mapper.Map<List<Plan_SaleoffModel>>(_plan.GetlistShop_plan_saleoff());
            return Json(new { plantype = result, saleoff = resultsaleoff }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetlistProduct(string ids)
        {
            int[] ia = ids.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var result = _productRepository.GetlistProduct(ia);
            var lstPro = new List<PlanGift>();
            foreach (var item in result)
            {
                lstPro.Add(new PlanGift
                {
                    img = item.shop_image.FirstOrDefault().url,
                    tensp = !UtilsBB.HasCookieLangEn() ? item.tensp : item.tensp_us,
                    url = item.spurl,
                    url_us = item.spurl_us
                });
            }
            return Json(lstPro, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddCart(PlanCartModel model)
        {
            var tmp = new List<PlanModel>();
            double totalKg = 0;
            foreach (var item in model.PlanModel)
            {
                if (item.IdProdut > 0 && item.Count > 0)
                {
                    totalKg += item.kg * item.Count;
                    tmp.Add(item);
                }
            }
            model.PlanModel = tmp;
            model.KgTotal = String.Format("{0:0.00}", totalKg);
            this.Session["PlanCart"] = model;

            string urlReturn = string.Empty;
            var kh = this.Session["khachhang"];
            if (kh != null)
            {
                urlReturn = UtilsBB.IsMobileDevice() ? "xac-nhan-2-do-so-sinh.html" : "/xac-nhan-do-so-sinh.html";
            }
            else
            {
                urlReturn = "/dat-hang-do-so-sinh.html";
            }
            return Json(urlReturn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChoosePay()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CancelPlan()
        {
            this.Session["PlanCart"] = null;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
