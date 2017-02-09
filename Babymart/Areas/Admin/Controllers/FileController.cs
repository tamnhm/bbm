using AutoMapper;
using Babymart.DAL.Common;
using Babymart.DAL.Customer;
using Babymart.Infractstructure;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Babymart.Areas.Admin.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /Admin/File/
        private IFiles _files;
        public FileController()
        {
            this._files = new Files(new babymart_vnEntities());
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetList()
        {
            var files = Mapper.Map<List<ModelFile>>(_files.GetList());
            return Json(files, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddFile(ModelFile file)
        {
            var Messaging = new RenderMessaging();

            try
            {
                var f = Mapper.Map<sys_file>(file);
                _files.Add(f);
                Messaging.success = true;
                Messaging.messaging = "Thêm file thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateFile(ModelFile file)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _files.Update(Mapper.Map<sys_file>(file));
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveFile(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var file = _files.GetFile(id, UtilsBB.GetStoreId());
                if (file == null)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Không xóa file này được !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
                var path = string.Empty;
                if (file.type == 10)
                    path = "~/" + file.Files;
                else
                    path = "~/Images/hinhdulieu/upload/" + file.Files;
                string fullPath = Request.MapPath(path);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    _files.Remove(id);
                    Messaging.success = true;
                }
                else
                {
                    Messaging.success = false;
                    Messaging.messaging = "Không xóa file này được !";
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
