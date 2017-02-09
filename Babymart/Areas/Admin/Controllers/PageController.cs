using Babymart.DAL.Page;
using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Babymart.Models.Module;
using Babymart.Infractstructure;
namespace Babymart.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        //
        // GET: /Admin/Page/
        babymart_vnEntities db = new babymart_vnEntities();
        private IPageRepository _ipageRepository;
        //
        // GET: /Admin/Module/
        public PageController()
        {
            this._ipageRepository = new PageRepository(new babymart_vnEntities());

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Getlist()
        {
            return Json(Mapper.Map<List<ModelPages>>(_ipageRepository.ListpageAll(UtilsBB.GetStoreId())), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetail(int id)
        {
            return Json(Mapper.Map<ModelPages>(_ipageRepository.pageSYS(id, UtilsBB.GetStoreId())), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertPage(ModelPages model)
        {
            var Messaging = new RenderMessaging();

            try
            {
                model.ngayviet = DateTime.Now;
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
                Messaging.id = _ipageRepository.InsertArticle(Mapper.Map<shop_page>(model));
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateArticle(ModelPages model)
        {
            var Messaging = new RenderMessaging();

            try
            {

                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
                Messaging.id = _ipageRepository.UpdateArticle(Mapper.Map<shop_page>(model));
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteArticle(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                var footer = _ipageRepository.Footer(UtilsBB.GetStoreId());
                if (footer.id == id)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Footer không được quyền xóa !";
                }
                else
                {
                    _ipageRepository.DeleteArticle(id);
                    Messaging.success = true;
                    Messaging.messaging = "Cập nhật thành công  ";

                }

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
