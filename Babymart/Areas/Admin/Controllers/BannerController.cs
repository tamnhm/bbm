using AutoMapper;
using Babymart.DAL.Brand;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Babymart.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        //
        // GET: /Admin/Banner/
        private IBannerRepository _bannerRepository;
        public BannerController()
        {
            this._bannerRepository = new BannerRepository(new babymart_vnEntities());
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ListBanner(int type)
        {
            var result = Mapper.Map<List<ModelBanner>>(_bannerRepository.ListBannerAll(type));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Detail(int id)
        {
            return Json(Mapper.Map<ModelBanner>(_bannerRepository.BannerDetail(id)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Remove(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _bannerRepository.Remove(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa Banner thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Banner này đã tồn tại sản phẩm !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult Insert(ModelBanner th)
        {
            var Messaging = new RenderMessaging();

            try
            {

                Messaging.id = _bannerRepository.Insert(Mapper.Map<sys_Banner>(th));
                Messaging.success = true;
                Messaging.messaging = "Thêm banner thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(ModelBanner th)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _bannerRepository.Update(Mapper.Map<sys_Banner>(th));
                Messaging.success = true;
                Messaging.messaging = "Cập nhật banner thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }


    }
}
