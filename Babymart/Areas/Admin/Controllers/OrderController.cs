using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL;
using Babymart.Models;
using Babymart.DAL.Order;
using Babymart.Models.Module;
using AutoMapper;
using PagedList;
namespace Babymart.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        babymart_vnEntities db = new babymart_vnEntities();
        private IOrderRepository _iOrderRepository;
        public OrderController()
        {
            this._iOrderRepository = new OrderRepository(new babymart_vnEntities());

        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Getlist()
        {
            List<ModelOrder> model = new List<ModelOrder>();
            model = Mapper.Map<List<ModelOrder>>(_iOrderRepository.Getlist().OrderByDescending(o => o.id).ToList());
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Giaohang(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var donhang = db.donhangs.Where(i => i.id == id).Single();
                donhang.dagiao = true;
                donhang.ngaygiao = DateTime.Now;
                donhang.dahuy = false;
                donhang.ngayhuy = null;
                db.Entry(donhang).State = System.Data.EntityState.Modified;


                db.SaveChanges();
                Messaging.success = true;
                Messaging.messaging = "Đã cập nhật sang trạng thái đã giao hàng";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var donhang = db.donhangs.Where(i => i.id == id).Single();
                //if (donhang.makh > 0)
                //{
                //    var kh = db.khachhangs.Find(donhang.makh);
                //    if (kh != null)
                //    {
                //        var diem = int.Parse(kh.diem);

                //        if (donhang.datru_diem != null && donhang.datru_diem.Value >= 1000)
                //        {
                //            diem = diem + donhang.datru_diem.Value;
                //        }
                //        if (donhang.diemsp != null
                //         && donhang.diemsp.Value > 0
                //         && diem >= donhang.diemsp.Value)
                //        {
                //            diem = diem - donhang.diemsp.Value;
                //        }
                //        kh.diem = diem.ToString();
                //        db.Entry(kh).State = System.Data.EntityState.Modified;
                //    }
                //}
                donhang.dahuy = true;
                donhang.ngayhuy = DateTime.Now;
                donhang.dagiao = false;
                donhang.ngaygiao = null;
                db.Entry(donhang).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                Messaging.success = true;
                Messaging.messaging = "Đơn hàng đã hủy";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdateNote(int id, string ghichu)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var donhang = db.donhangs.Find(id);
                donhang.ghichu = ghichu;
                var dh = Mapper.Map<ModelOrderUpdate>(donhang);
                _iOrderRepository.Update(Mapper.Map<donhang>(dh));
                Messaging.success = true;
                Messaging.messaging = "Đã cập nhật ghi chú đơn hàng";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetlistOrder(int? page, int pageSize, int search)
        {
            int count = db.donhangs.Count();
            float tmp = (float)count / pageSize;
            var maxpage = Math.Ceiling(tmp);
            List<ModelOrder> model = new List<ModelOrder>();
            model = Mapper.Map<List<ModelOrder>>(_iOrderRepository.GetlistOrderPaging(page, pageSize, search));
            return Json(new { data = model, maxpage = maxpage, count = count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Indonhang(int id)
        {
            var donhang = db.donhangs.Where(i => i.id == id).Single();
            return View(donhang);
        }
    }
}
