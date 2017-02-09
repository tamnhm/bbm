using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.Models;
using Babymart.DAL.Collection;
namespace Babymart.Areas.Admin.Controllers
{
    public class CollectionController : Controller
    {
        //
        // GET: /Admin/Collection/
        private ICollection _ICollection;
        public CollectionController()
        {
            this._ICollection = new Collection(new babymart_vnEntities());
        }
         [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List(int id)
        {
            var p = _ICollection.GetListbyproid(id);
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Add(shop_collection c)
        {
            var Messaging = new RenderMessaging();

            try
            {
                bool check = IsCheckList(c.idsp, c.idloai, c.iddm, c.iddmc);
                if (check)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Collection này đã thêm vui lòng kiểm tra lại !";

                }
                else
                {
                    _ICollection.Add(c);
                    Messaging.success = true;
                }
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        public bool IsCheckList(int? id, int? idl, int? dm, int? dmc)
        {
            var list = _ICollection.GetListbyproid(id);
            bool check = false;
            foreach (shop_collection item in list)
            {
                if (item.idloai == idl && item.iddm == dm && item.iddmc == dmc)
                {
                    check = true;
                    break;
                }
            }
            return check;
        }
        [HttpPost]
        public JsonResult Remove(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _ICollection.Remove(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa collection thành công !";
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
