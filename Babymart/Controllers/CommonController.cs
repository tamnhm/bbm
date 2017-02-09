using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL.Shipping;
using Babymart.DAL.Common;
using Babymart.DAL.Link;
using Babymart.Models;
using Babymart.ViewModels;
using Babymart.DAL.Customer;
using Babymart.Models.Module.Enum;
using Babymart.DAL.Page;
using Babymart.DAL.Module;
using AutoMapper;
using Babymart.Models.Module;
using Babymart.DAL.Product;
using Babymart.Infractstructure;
using Babymart.Infractstructuree;
namespace Babymart.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/


        babymart_vnEntities db = new babymart_vnEntities();
        private ILinkRepository _iLinkRepository;
        private IModuleRepository _iModuleRepository;
        private ICommonRepository _iCommonRepository;
        private IShippingRepository _shippingRepository;
        private IPageRepository _ipageRepository;
        private IFiles _file;
        private IProductRepository _productRepository; 
        public CommonController()
        {
            this._iLinkRepository = new LinkRepository(new babymart_vnEntities());
            this._iCommonRepository = new CommonRepository(new babymart_vnEntities());
            this._shippingRepository = new ShippingRepository(new babymart_vnEntities());
            this._file = new Files(new babymart_vnEntities());
            this._ipageRepository = new PageRepository(new babymart_vnEntities());
            this._iModuleRepository = new ModuleRepository(new babymart_vnEntities());
            this._productRepository = new ProductRepository(new babymart_vnEntities());
        }

        public ActionResult Menu()
        { 
            var data = _iLinkRepository.Getdanhmuc(0, UtilsBB.GetStoreId());
            if (ControllerContext.DisplayMode.DisplayModeId == "Mobile")
            {
                return PartialView("~/Views/Shared/Partial/_Menu.Mobile.cshtml", data);
            }
            return PartialView("~/Views/Shared/Partial/_Menu.cshtml", data);
        }

        public ActionResult MenuModule(int idtype)
        {
            var data = _iCommonRepository.MenuModule(idtype, UtilsBB.GetStoreId());
            ViewBag.type = idtype;
            return PartialView("~/Views/Shared/Partial/_MenuModuel.cshtml", data);
        }
        public ActionResult Cart()
        {
            var viewModel = new ShoppingCartViewModel
            {
                CartItemModel = UtilsBB.GetCartItem(),
                CartTotal = UtilsBB.GetTotal()
            };
            if (ControllerContext.DisplayMode.DisplayModeId == "Mobile")
                return PartialView("~/Views/Shared/Partial/_Cart.Mobile.cshtml", viewModel);
            return PartialView("~/Views/Shared/Partial/_Cart.cshtml", viewModel);
        }
        [ChildActionOnly]
        public ActionResult Support_Partial()
        {
            var viewModel = _ipageRepository.ListpageByType(UtilsBB.GetStoreId());
            if (ControllerContext.DisplayMode.DisplayModeId == "Mobile")
                return PartialView("~/Views/Shared/Partial/_Hotro.Mobile.cshtml", viewModel);
            return PartialView("~/Views/Shared/Partial/_Hotro.cshtml", viewModel);
        }
        public ActionResult Support_Partial1()
        {
            var viewModel = _ipageRepository.ListpageByType1(UtilsBB.GetStoreId()); 
            return PartialView("~/Views/Shared/Partial/_Hotro1.cshtml", viewModel);
        }
        public ActionResult Footer()
        {
            var viewModel = _ipageRepository.Footer(UtilsBB.GetStoreId());
            return PartialView("~/Views/Shared/Partial/_Footer.cshtml", viewModel);
        }
        public JsonResult LoadCity()
        {
            var tp = _shippingRepository.List_Tp();
            return Json(tp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadTinh(int id)
        {
            var tinh = _shippingRepository.List_CP_Tinh(id);
            return Json(tinh, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadVung()
        {
            var tinh = _shippingRepository.List_Vung();
            return Json(tinh, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadGio()
        {
            var gio = _shippingRepository.List_Gio();
            return Json(gio, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RightColumn()
        {
            var data = _file.GetListByType((int)FilesType.BannerLeft, UtilsBB.GetStoreId());
            return PartialView("~/Views/Shared/Partial/_RightQC.cshtml", data);
        }
        public ActionResult GadgetsSupport()
        {
            var data = _ipageRepository.ListpageSYS("KM", UtilsBB.GetStoreId());
            return PartialView("~/Views/Shared/Partial/Gadgets/_GadgetsSupport.cshtml", data);
        }
        public ActionResult GadgetsMagazine()
        {
            var module = new List<ModelModuleDetail>();
            _iModuleRepository.ListMenu(5, UtilsBB.GetStoreId()).ForEach(x =>
            {
                foreach (var item in x.module_detail)
                {
                    module.Add(Mapper.Map<ModelModuleDetail>(item));
                }
            });
            var result = module.Where(o => o.hide == false).OrderByDescending(x => x.createdate).Take(6).ToList();
            return PartialView("~/Views/Shared/Partial/Gadgets/_GadgetsMagazine.cshtml", result);
        }
        public ActionResult GadgetsMagazine2()
        {
            var module = new List<ModelModuleDetail>();
            _iModuleRepository.ListMenu(5, UtilsBB.GetStoreId()).ForEach(x =>
            {
                foreach (var item in x.module_detail)
                {
                    module.Add(Mapper.Map<ModelModuleDetail>(item));
                }
            });
            var result = module.Where(o => o.hide == false).OrderByDescending(x => x.createdate).Take(6).ToList();
            return PartialView("~/Views/Shared/Partial/Gadgets/_GadgetMagazine2.cshtml", result);
        }
        public ActionResult GadgetsProduct()
        {
            var result = _productRepository.ProductTop("spgiamgia", 10, UtilsBB.GetStoreId());
            return PartialView("~/Views/Shared/Partial/Gadgets/_GadgetsProduct.cshtml", result);
        }
        public ActionResult ListDanhmucCon(int iddm)
        {
            var result = _iLinkRepository.ListDanhmuccon(iddm, UtilsBB.GetStoreId());
            return PartialView("~/Views/Shared/Partial/Gadgets/Shop/_ListDanhmuc.cshtml", result);
        }
        public JsonResult GetHttpCookieLang(string lang)
        {
            MyCookie co = new MyCookie("CookieLang");
            co.SetCookieForLang(lang);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
