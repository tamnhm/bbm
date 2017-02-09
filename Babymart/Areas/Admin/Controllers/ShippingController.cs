using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL;
using Babymart.DAL.Shipping;
using Babymart.Models;
using Babymart.Models.Module.Enum;
using AutoMapper;
namespace Babymart.Areas.Admin.Controllers
{
    public class ShippingController : Controller
    {
        //
        // GET: /Admin/Shipping/
        private IShippingRepository _shippingRepository;
        babymart_vnEntities db = new babymart_vnEntities();
        public ShippingController()
        {
            this._shippingRepository = new ShippingRepository(new babymart_vnEntities());
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetCity()
        {
            var city = Mapper.Map<List<ModelShipping>>(_shippingRepository.List_Tp());

            return Json(city, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetTpById(int id)
        {
            return Json(Mapper.Map<ModelShipping>(_shippingRepository.GetTpById(id)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetVung(string mavung)
        {
            var vung = _shippingRepository.List_CP_Vung(mavung);

            return Json(vung, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Gettinhbytp(int idtp)
        {
            var tinh = _shippingRepository.List_CP_Tinh(idtp);
            return Json(Mapper.Map<List<ModelShippingTinh>>(tinh), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Gettinhtra(int idtp)
        {
            var tinh = db.donhang_chuyenphat_tp_tinhtra.Where(o => o.IpTp == idtp).ToList();
            return Json(tinh, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Themtinhtra(int idtp, int tinh)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var objtinh = db.donhang_chuyenphat_tinh.Find(tinh);
                var model = new donhang_chuyenphat_tp_tinhtra();
                model.IpTp = idtp;
                model.IdTinhtra = tinh;
                model.Tentinhtra = objtinh.tentinh;
                db.donhang_chuyenphat_tp_tinhtra.Add(model);
                db.SaveChanges();
                Messaging.messaging = "Thêm tỉnh trả thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Xoatinhtra(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {
                donhang_chuyenphat_tp_tinhtra item = db.donhang_chuyenphat_tp_tinhtra.Find(id);
                db.donhang_chuyenphat_tp_tinhtra.Remove(item);
                db.SaveChanges();
                Messaging.messaging = "Xóa tỉnh trả thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTp(ModelShipping model)
        {
            var Messaging = new RenderMessaging();

            try
            {
                var u = Mapper.Map<ModelShippingUpdate>(model);
                var tp = Mapper.Map<donhang_chuyenphat_tp>(u);
                tp.donhang_chuyenphat_tinh = null;
                tp.khachhang_vanglai = null;
                _shippingRepository.UpdateCity(tp);
                List<donhang_chuyenphat_tinh> tinhmoi = new List<donhang_chuyenphat_tinh>();
                var tinh = db.donhang_chuyenphat_tinh.Where(p => p.idtp == model.id).ToList();
                foreach (var item in model.donhang_chuyenphat_tinh)
                {
                    var index = tinh.FindIndex(p => p.id == item.id);
                    if (index < 0)
                    {
                        tinhmoi.Add(Mapper.Map<donhang_chuyenphat_tinh>(item));
                    }
                    else
                    {
                        _shippingRepository.UpdateTinh(Mapper.Map<donhang_chuyenphat_tinh>(item));
                    }
                }
                if (tinhmoi.Count > 0)
                {
                    _shippingRepository.InsertTinh(model.id, tinhmoi);
                }
                Messaging.messaging = "Cập nhật thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult insertTp(donhang_chuyenphat_tp dm, List<donhang_chuyenphat_tinh> tinh)
        {
            var Messaging = new RenderMessaging();
            try
            {
                Messaging.id = _shippingRepository.Insercitys(dm, tinh);
                Messaging.messaging = "Thêm thành phố" + dm.tentp + " thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveVung(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {
                _shippingRepository.RemoveVung(id);
                Messaging.messaging = "Xóa thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddVung(donhang_chuyenphat_vung model)
        {
            var Messaging = new RenderMessaging();
            try
            {
                if (model.ship == null || model.kilogram == null)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Dự liệu nhập không đúng !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
                _shippingRepository.AddVung(model);
                Messaging.messaging = "Thêm thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult updateTpho(donhang_chuyenphat_tp dm, List<donhang_chuyenphat_tinh> tinh)
        {
            bool checktinh = true;

            if (tinh != null && tinh.Count > 0)
                foreach (var item in tinh)
                {
                    if (item.tentinh == "" || item.tentinh == null)
                    {
                        checktinh = false;
                        break;
                    }
                }
            var Messaging = new RenderMessaging();
            if (checktinh)
            {
                try
                {
                    // _shippingRepository.UpdateCity(dm, tinh);
                    Messaging.model = _shippingRepository.List_CP_Tinh(dm.id);
                    Messaging.success = true;
                    if (dm.idtinhtra != null)
                        Messaging.id = (int)dm.idtinhtra;
                    Messaging.messaging = "Cập nhật " + dm.tentp + " thành công !";

                }
                catch
                {
                    Messaging.success = false;
                    Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
                }
            }
            else
            {
                Messaging.success = false;
                Messaging.messaging = "Tên tỉnh không được bỏ trống!";
            }



            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult removeTp(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _shippingRepository.RemoveCity(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa thành phố thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Vẫn còn tồn tại cách tỉnh của thành phố này ! ";
            }

            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult removeTinh(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _shippingRepository.RemoveTinh(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa tỉnh thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }

            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult updateTinh(donhang_chuyenphat_tinh t)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _shippingRepository.UpdateTinh(t);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật " + t.tentinh + " thành công !";
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
