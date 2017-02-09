using AutoMapper;
using Babymart.DAL.Plan;
using Babymart.Models;
using Babymart.Models.Module;
using Babymart.Models.Module.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Babymart.Areas.Admin.Controllers
{
    public class PlansController : Controller
    {
        //
        // GET: /Admin/Plan/
        babymart_vnEntities db = new babymart_vnEntities();
        private IPlanRepository _iplanRepository;
        //
        // GET: /Admin/Module/
        public PlansController()
        {
            this._iplanRepository = new PlanRepository(new babymart_vnEntities());

        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetlistShop_plan_saleoff()
        {
            var result = _iplanRepository.GetlistShop_plan_saleoff();
            var sp = Mapper.Map<List<Plan_SaleoffModel>>(result);
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetlistShop_plan_type()
        {
            var result = Mapper.Map<List<Plan_type>>(_iplanRepository.GetlistShop_plan_type().OrderBy(o=>o.Id).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertPlanType(Plan_type model)
        {
            var Messaging = new RenderMessaging();

            try
            {
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
                Messaging.id = _iplanRepository.AddTypPlan(Mapper.Map<shop_plan_type>(model));
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdatePlanType(Plan_type model)
        {
            var Messaging = new RenderMessaging();

            try
            {

                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
                Messaging.id = _iplanRepository.UpdateTypPlan(Mapper.Map<shop_plan_type>(model));
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePlanType(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _iplanRepository.DeleteTypPlan(id);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
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
