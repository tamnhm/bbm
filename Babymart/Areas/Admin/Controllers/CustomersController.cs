using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL.Customer;
using Babymart.DAL.Common;
using Babymart.Models;
using AutoMapper;
using Babymart.Models.Module;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
namespace Babymart.Areas.Admin.Controllers
{
    public class CustomersController : Controller
    {
        //
        // GET: /Admin/Customer/
        private ICustomer _customer;
        private ICommonRepository _common;
        public CustomersController()
        {
            this._customer = new Customer(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetList()
        {
            var kh = Mapper.Map<List<ModeCustomer>>(_customer.GetList());
            return Json(kh, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetList(int? page, int pageSize, string search)
        {
            long count = 0;
            List<ModeCustomer> model = new List<ModeCustomer>();
            model = Mapper.Map<List<ModeCustomer>>(_customer.GetlistCustomerPaging(page, pageSize, out count, search));
            //foreach (var item in model)
            //{
            //    string passlDecry = _common.Decryptdata(item.matkhau);
            //    item.matkhau = passlDecry;
            //}
            float tmp = (float)count / pageSize;
            var maxpage = Math.Ceiling(tmp);
            return Json(new { data = model, maxpage = maxpage, count = count }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(ModeCustomerPost kh)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _customer.UpdateThongTin(Mapper.Map<khachhang>(kh));
                Messaging.success = true;
                Messaging.messaging = "Cập nhật khách hàng thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Add(ModeCustomer kh)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _customer.Add(Mapper.Map<khachhang>(kh));
                Messaging.success = true;
                Messaging.messaging = "Thêm khách hàng thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPassword(int Id)
        {
            var Messaging = new RenderMessaging();
            if (Id <= 0)
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
                return Json(Messaging, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cus = _customer.GetDetail(Id);
                if (cus == null)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string passrandom = _common.RandomString(10);
                    string emailEncry = _common.Encryptdata(cus.email);
                    string passlEncry = _common.Encryptdata(passrandom);
                    string url = "https://babymart.vn/kichhoatmatkhaumoi/" + emailEncry + "/" + passlEncry + ".html";
                    string content = "<span style='font-size: 9pt;'>Chào <strong>" + cus.email + "</strong></span><span style='font-size: 9pt;'> ,bạn đã gởi yêu cầu tạo mật khẩu mới .</span><br />";
                    content += " <span style='font-size: 9pt;'>Mật khẩu mới của bạn : <strong>" + passrandom + "</strong></span><br />";
                    content += "<strong><span style='font-size: 9pt; color: #ff0000;'>(Lưu ý, nếu bạn không có yêu cầu thay đổi mật khẩu mới, xin đừng click vào đường dẫn này)</span></strong><br />";
                    content += "<span style='font-size: 9pt;'>Vui lòng click vào đường dẫn kích hoạt mật khẩu mới của bạn ở dưới đây : <a href='" + url + "'>Kích hoạt mật khẩu mới</a></span><br />";
                    content += "<span style='font-size: 9pt;'>Xin cảm ơn !</span><br /><br /><br />";
                    content += "<div style='text-align:center;margin-top:20px'>Website Babymart.vn - Nơi bố mẹ gửi trọn niềm tin.<br /> 325 Trương Vĩnh Ký, P. Tân Thành, Q.Tân Phú - Tp. HCM<br />ĐT: 08.3812.3813, Đường dây nóng: 0918.644.643 - 0909.324.824<br />Email: babymart.vn@babymart.vn - Website : <a href='babymart.vn' target='_blank'>babymart.vn</a><br /></div>";
                    _common.SendEmail(cus.email, "Babymrt.vn - Thay đổi mật khẩu", content);
                    ViewBag.message = "Đã gởi mật khẩu mới vào Email của bạn, vui lòng kiểm tra Email để xác thực. Xin cảm ơn!";
                    Messaging.success = true;
                    Messaging.messaging = "Đã gởi mail tạo mật khẩu mới đến khách hàng thành công !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public ActionResult ExportInventory()
        { 
            GridView gv = new GridView();
            gv.DataSource = _customer.GetListCustoExcel();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=danh-sach-khach-hang-babymart.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("StudentDetails");
        }

    }
}
