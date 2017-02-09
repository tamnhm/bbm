using Babymart.DAL.Collection;
using Babymart.Infractstructure;
using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Babymart.Areas.Admin.Controllers
{
    public class ContentsMetaController : Controller
    {
        //
        // GET: /Admin/ContentsMEta/
        private IContentsSEO _IContentsSEO;
        public ContentsMetaController()
        {
            this._IContentsSEO = new ContentsSEO(new babymart_vnEntities());
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetList()
        {
            return Json(_IContentsSEO.GetList(UtilsBB.GetStoreId()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateContentsMeta(sys_content data)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _IContentsSEO.UpdateSys_content(data);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công !";

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
