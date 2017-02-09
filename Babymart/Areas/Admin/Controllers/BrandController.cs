using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL;
using Babymart.DAL.Common;
using Babymart.DAL.Brand;
using Babymart.Models;
using AutoMapper;
using Babymart.Infractstructure;
namespace Babymart.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        //
        // GET: /Admin/Brand/
        private IBrandRepository _brandRepository;
        public BrandController()
        {
            this._brandRepository = new BrandRepository(new babymart_vnEntities());
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            var result = Mapper.Map<List<ModelThuongHieu>>(_brandRepository.GetList(UtilsBB.GetStoreId()));
            result = result.OrderBy(a => a.tenhieu).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
            //return Json(Mapper.Map<List<ModelThuongHieu>>(_brandRepository.GetList(UtilsBB.GetStoreId())), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Detail(int id)
        {
            return Json(Mapper.Map<ModelThuongHieu>(_brandRepository.GetBrandById(id, UtilsBB.GetStoreId())), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Remove(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _brandRepository.Remove(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa thương hiệu thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Thương hiệu này đã tồn tại sản phẩm !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetVisible(int id, bool visible)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _brandRepository.SetVisible(id, visible);
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
        public JsonResult SetShowHome(int id, bool showhome)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _brandRepository.SetShowHome(id, showhome);
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
        
        public JsonResult Insert(ModelThuongHieu th)
        {
            var Messaging = new RenderMessaging();

            try
            {

                Messaging.id = _brandRepository.Insert(Mapper.Map<shop_thuonghieu>(th));
                Messaging.success = true;
                Messaging.messaging = "Thêm danh mục " + th.tenhieu + " thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(ModelThuongHieu th)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _brandRepository.Update(Mapper.Map<shop_thuonghieu>(th));
                Messaging.id = th.mahieu;
                Messaging.success = true;
                Messaging.messaging = "Cập nhật " + th.tenhieu + " thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Plupload(int extra)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Uploads/" + file.FileName);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

       
    }

}
