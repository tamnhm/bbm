using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL.LangdingPage;
using Babymart.Models;
namespace Babymart.Areas.Admin.Controllers
{
    public class LandingpageController : Controller
    {
        //
        // GET: /Admin/Landingpage/
        private ILangdingPage _langdingpage;
        public LandingpageController()
        {
            this._langdingpage = new LangdingPage(new babymart_vnEntities());
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get()
        {
            return Json(_langdingpage.GetList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetail(int maloai)
        {
            return Json(_langdingpage.GetDetail(maloai), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(shop_loai l)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _langdingpage.Update(l);
                Messaging.success = true;
                Messaging.id = l.maloai;
                Messaging.messaging = "Cập nhật Langdingpage thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Lỗi không xác đinh !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
    }
}
