using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL;
using Babymart.DAL.Customer;
using Babymart.DAL.Common;
using Babymart.Models;
using System.Text;
using AutoMapper;
using Babymart.Models.Module;
using Babymart.DAL.Sys_Mail;
using Babymart.DAL.Shipping;
using Babymart.Infractstructure;
using Facebook;
using System.Configuration;
namespace Babymart.Controllers
{
    public class CustomerController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uribuilder = new UriBuilder(Request.Url);
                uribuilder.Query = null;
                uribuilder.Fragment = null;
                uribuilder.Path = Url.Action("FacebookCallback");
                return uribuilder.Uri;
            }
        }
        //
        // GET: /Customer/
        private ICustomer _ICustomer;
        private ICommonRepository _common;
        private ISys_MailRepository _imail;
        private IShippingRepository _shippingRepository;
        babymart_vnEntities db = new babymart_vnEntities();
        public CustomerController()
        {
            this._ICustomer = new Customer(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
            this._imail = new Sys_MailRepository(new babymart_vnEntities());
            this._shippingRepository = new ShippingRepository(new babymart_vnEntities());
        }
        public ActionResult HistoryCartDetail(long Id)
        {
            var kh = (ModeCustomerPost)this.Session["khachhang"];
            var cart = _ICustomer.GetDetailCart(Id);
            if (cart.Count > 0)
            {
                var makh = cart.FirstOrDefault().donhang.makh;
                if (kh == null)
                {
                    return RedirectToAction("Login");
                }
                if (kh.MaKH != makh)
                {
                    this.Session["khachhang"] = null;
                    return RedirectToAction("Logout");
                }
                return View(cart);
            }
            return RedirectToAction("HistoryCart");
        }
        public ActionResult HistoryCart()
        {
            var kh = (ModeCustomerPost)this.Session["khachhang"];
            if (kh == null)
            {
                return RedirectToAction("Login");
            }
            return View(Mapper.Map<ModeCustomer>(db.khachhangs.Find(kh.MaKH)));
        }
        public ActionResult Index()
        {

            if (this.Session["khachhang"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var sessionKhachhang = (ModeCustomerPost)this.Session["khachhang"];
                var d = db.khachhangs.Find(sessionKhachhang.MaKH);
                var khachhang = Mapper.Map<ModeCustomer>(d);
                ViewData["Thanhpho"] = _ICustomer.ListCity();
                ViewData["Tinhthanh"] = _ICustomer.ListTinh(d.idtp);
                return View(khachhang);
            }
        }
        public ActionResult lienhe()
        {
            return View();
        }
        public ActionResult SubmitLienhe(ModelContact model)
        {
            if (ModelState.IsValid)
            {
                db.khachhang_lienhe.Add(Mapper.Map<khachhang_lienhe>(model));
                db.SaveChanges();
                var hoten = model.Name;
                var phone = model.Phone;
                var noidung = model.Contents;
                var title = !UtilsBB.HasCookieLangEn() ? "Mail liên hệ từ babymart.vn" : "Contact mail from babymart.vn";
                var email = model.Mail;
                var content = "<div style=\"line-height: 1.5;\">";
                if (UtilsBB.HasCookieLangEn() == false)
                {
                    content += "<div><span style=\"font-family: Arial; color: #000000;\"> Chào bạn <strong>" + hoten + " (" + phone + ") </strong>!</span></div><br/>";
                    content += "<div><span style=\"font-family: Arial; color: #000000;\">Cảm ơn bạn đã truy cập <strong>babymart.vn.</strong></span></div><br/>";
                    content += "<div><span style=\"font-family: Arial;  color: #000000;\">Bạn đã gửi mail cho babymart.vn với nội dung</span>:<strong> " + noidung + "</strong></div></div><br/>";
                    content += "<div><span style=\"font-family: Arial; color: #000000;\">Babymart.vn sẽ liên lạc với bạn sớm nhất.</span></div><br/>";
                    content += "<div>Trân trọng!</div>";
                    content += "<br/><br/>";
                    content += "<div style=\"text-align:center;margin-top:20px;\"BABYMART - Nơi bố mẹ gửi trọn niềm tin.<br>325 Trương Vĩnh Ký, P. Tân Thành, Q.Tân Phú - Tp. Hồ Chí Minh<br>ĐT: 08.6672.0909 - 08.3812.3813, Đường dây nóng: 0918.644.643 - 0909.324.824<br>Email: babymart.vn@babymart.vn - Website : <a href=\"babymart.vn\" rel=\"nofollow\">babymart.vn</a><br></div>";
                }
                else
                {
                    content += "<div><span style=\"font-family: Arial; color: #000000;\">Dear <strong>" + hoten + " (" + phone + ")</strong>!</span></div><br/>";
                    content += "<div><span style=\"font-family: Arial; color: #000000;\">Thank you for visiting <strong>babymart.vn.</strong></span></div><br/>";
                    content += "<div><span style=\"font-family: Arial;  color: #000000;\">You have sent mail to babymart.vn with the conten</span>:<strong> " + noidung + "</strong></div></div><br/>";
                    content += "<div><span style=\"font-family: Arial; color: #000000;\">Babymart.vn will contact you soon.</span></div><br/>";
                    content += "<div>Sincerely!</div>";
                    content += "<br/><br/>";
                    content += "<div style=\"text-align:center;margin-top:20px;\"BABYMART - Where parents fully trust.<br>325 Truong Vinh Ky St, Tan Thanh ward, Tan Phu Dist, Ho Chi Minh City<br>Tel: 84.8.6672.0909 - 84.8.3812.3813, Hotline: 0918.644.643 - 0909.324.824<br>Email: babymart.vn@babymart.vn - Website : <a href=\"babymart.vn\" rel=\"nofollow\">babymart.vn</a><br></div>";
                }
                _common.SendEmail(email, title, content);
                _common.SendEmail("babymart.vn@babymart.vn", title, content);
                return Json(true);
            }

            return Json(false);
        }
        public ActionResult LoadQuaHuyen(int id)
        {
            var quan = db.donhang_chuyenphat_tinh.Where(o => o.idtp == id).Select(x => new { tentinh = x.tentinh, id = x.id }).ToArray();
            return Json(quan, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(ModeCustomer kh)
        {
            if (ModelState.IsValid)
            {

                var khang = _ICustomer.GetDetail(kh.MaKH);
                khang.idquan = kh.idquan;
                khang.idtp = kh.idtp;
                khang.idquan = kh.idquan;
                khang.hoten = kh.hoten;
                khang.dienthoai = kh.dienthoai;
                khang.email = kh.email;
                khang.duong = kh.duong;
                khang.konhanmail = Convert.ToBoolean(kh.konhanmail);
                this.Session["khachhang"] = Mapper.Map<ModeCustomerPost>(khang);
                try
                {
                    _ICustomer.Update(khang);
                    return Json("Cập nhật thành công");
                }
                catch
                {
                    return Json("Do sự cố mạng, vui lòng thử lại. Chúng tôi rất tiếc về lỗi này!");
                }
            }
            else
            {
                var response = new
                {
                    isValid = ModelState.IsValid,
                    errors = ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(ms => ms.ErrorMessage)
                };
                return Json(response);
            }
        }
        [HttpPost]
        public ActionResult UpdatePassword(FormCollection f)
        {
            var Messaging = new RenderMessaging();
            if (this.Session["khachhang"] == null)
            {
                return RedirectToAction("Login");
            }
            try
            {
                //this.Session["khachhang"] = Mapper.Map<ModeCustomerPost>(_ICustomer.GetDetail(193));
                var khachang = (ModeCustomerPost)this.Session["khachhang"];
                var passcurrent = f["_passCurrent"];
                var passDecr = _common.Encryptdata(passcurrent.ToString());
                var newpass = f["_passNew"];
                var newpassRe = f["_passNewRe"];
                var tmpkh = _ICustomer.GetDetail(khachang.MaKH);
                if (!newpass.Equals(newpassRe))
                {
                    var response = new
                    {
                        errors = new[] { "Mật khẩu nhập lại không khớp với mật khẩu mới !" }
                    };
                    return Json(response);

                }
                if (!tmpkh.matkhau.Equals(passDecr))
                {
                    var response = new
                    {
                        errors = new[] { "Mật khẩu hiện tại nhập không đúng !" }
                    };
                    return Json(response);
                }
                tmpkh.matkhau = _common.Encryptdata(newpassRe.ToString());
                _ICustomer.Update(tmpkh);
                return Json("Cập nhật mật khẩu thành công.");

            }
            catch
            {
                var response = new
                    {
                        errors = new[] { "Do sự cố mạng, vui lòng thử lại !" }
                    };

                return Json(response);
            }

        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Loginfacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["fbappid"],
                client_secret = ConfigurationManager.AppSettings["fbapppass"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["fbappid"],
                client_secret = ConfigurationManager.AppSettings["fbapppass"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new khachhang();
                user.email = email;
                user.tendn = email;
                user.hoten = firstname + " " + middlename + " " + lastname;
                var resultInsert = _ICustomer.InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var u = _ICustomer.Loginfacebook(user.tendn);
                    var k = Mapper.Map<ModeCustomerPost>(u);

                    if (k != null)
                    {
                        this.Session["khachhang"] = k;
                    }
                }
            }
            return View("_thongbao");
        }
        public ActionResult Logout(string returnUrl)
        {
            this.Session["khachhang"] = null;
            if (Request.Cookies["Cookie_customer"] != null)
            {
                var c = new HttpCookie("Cookie_customer");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        public JsonResult Register(khachhang kh)
        {
            var Messaging = new RenderMessaging();

            try
            {
                var cusExitByEmail = _ICustomer.GetCustomerByEmail(kh.email);
                var cusExitByName = _ICustomer.GetCustomerByUserName(kh.tendn);
                if (cusExitByEmail != null)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Email đã tồn tại,vui lòng chọn Email khác";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
                if (cusExitByName != null)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Tên đăng nhập đã tồn tại,vui lòng chọn Tên  khác";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }

                _ICustomer.Register(kh);
                //this.Session["khachhang"] = Mapper.Map<ModeCustomerPost>(kh);
                SendmailRegister(kh);
                Messaging.success = true;

            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        private void SendmailRegister(khachhang kh)
        {
            var mail = _imail.Getmail("register");
            var r = mail.contents;
            StringBuilder str = new StringBuilder(r);
            str.Replace("{{hoten}}", kh.hoten);
            str.Replace("{{tendn}}", kh.tendn);
            var x = str.ToString();
            _common.SendEmail(kh.email, mail.titile, x);
        }

        [HttpPost]
        public ActionResult Login(khachhang kh, FormCollection f)
        {
            if (!string.IsNullOrEmpty(kh.tendn) && !string.IsNullOrEmpty(kh.matkhau))
            {
                try
                {
                    var u = _ICustomer.Login(kh.tendn, kh.matkhau);
                    var k = Mapper.Map<ModeCustomerPost>(u);

                    if (k != null)
                    {
                        if (f["remember_acc"] != null)
                        {
                            HttpCookie cookie = new HttpCookie("Cookie_customer");
                            cookie.Values["Cookie_customer_tendn"] = kh.tendn;
                            cookie.Values["Cookie_customer_matkhau"] = kh.matkhau;
                            this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        }
                        this.Session["khachhang"] = k;
                        return View("_thongbao");
                    }
                    else
                    {
                        ViewBag.Erro = "Tên đăng nhập và mật khẩu không đúng";
                        return View();

                    }
                }
                catch
                {
                    ViewBag.Erro = "Tên đăng nhập và mật khẩu không đúng";
                    return View();
                }
            }
            else
            {
                ViewBag.Erro = "Tên đăng nhập và mật khẩu không được rỗng";
                return View();
            }

        }
        [HttpPost]
        public JsonResult Gettinhbytp(int idtp)
        {
            var tinh = _shippingRepository.List_CP_Tinh(idtp);
            return Json(Mapper.Map<List<ModelShippingTinh>>(tinh), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(FormCollection f)
        {
            var email = f["Email"];
            if (!string.IsNullOrEmpty(email))
            {
                var cus = _ICustomer.GetCustomerByEmail(email);
                if (cus == null)
                {
                    ViewBag.message = "Email này không tồn tại, quý khách vui lòng nhập lại, hoặc <a href='/dang-ky.html'>đăng ký tài khoản mới </a>!";
                }
                else
                {
                    string pass = _common.RandomString(10);
                    string emailEncry = _common.Encryptdata(email);
                    string passlEncry = _common.Encryptdata(pass);
                    string url = "https://babymart.vn/kichhoatmatkhaumoi/" + emailEncry + "/" + passlEncry + ".html";
                    string content = "<span style='font-size: 9pt;'>Chào <strong>" + email + "</strong></span><span style='font-size: 9pt;'> ,bạn đã gởi yêu cầu tạo mật khẩu mới .</span><br />";
                    content += " <span style='font-size: 9pt;'>Tên đăng nhập của bạn : <strong>" + cus.tendn + "</strong></span><br />";
                    content += " <span style='font-size: 9pt;'>Mật khẩu mới của bạn : <strong>" + pass + "</strong></span><br />";
                    content += "<strong><span style='font-size: 9pt; color: #ff0000;'>(Lưu ý, nếu bạn không có yêu cầu thay đổi mật khẩu mới, xin đừng click vào đường dẫn này)</span></strong><br />";
                    content += "<span style='font-size: 9pt;'>Vui lòng click vào đường dẫn kích hoạt mật khẩu mới của bạn ở dưới đây : <a href='" + url + "'>Kích hoạt mật khẩu mới</a></span><br />";
                    content += "<span style='font-size: 9pt;'>Xin cảm ơn !</span><br /><br /><br />";
                    content += "<div style='text-align:center;margin-top:20px'>Website Babymart.vn - Nơi bố mẹ gửi trọn niềm tin.<br /> 325 Trương Vĩnh Ký, P. Tân Thành, Q.Tân Phú - Tp. HCM<br />ĐT: 08.6672.0909 - 08.3812.3813, Đường dây nóng: 0918.644.643 - 0909.324.824<br />Email: babymart.vn@babymart.vn - Website : <a href='babymart.vn' target='_blank'>babymart.vn</a><br /></div>";
                    _common.SendEmail(email, "Babymart.vn - Thay đổi mật khẩu", content);
                    ViewBag.message = "Đã gởi mật khẩu mới vào Email của bạn, vui lòng kiểm tra Email để xác thực. Xin cảm ơn!";

                }
            }

            return View("~/Views/Customer/Login.cshtml");
        }
        public ActionResult ResetPassword(string email, string pass)
        {
            try
            {
                string emailDecry = _common.Decryptdata(email);
                string passlDecry = _common.Decryptdata(pass);
                var cus = _ICustomer.GetCustomerByEmail(emailDecry);
                if (cus == null)
                {
                    ViewBag.message = "Thành thật xin lỗi quý khách, vui lòng thử lại thay đổi mật khẩu <a href='/quenmatkhau.html'>Thay đổi mật khẩu</a>";
                }
                else
                {
                    cus.matkhau = pass;
                    _ICustomer.Update(cus);
                    ViewBag.message = "<h4 style='padding-top:20px'>" + cus.email + " đã thay đổi mật khẩu thành công !</h4> <p style='padding-top:10px'> Bạn vui lòng đăng nhập với mật khẩu chúng tôi đã gởi qua mail và chỉnh sửa lại mật khẩu ở trang thông tin tài khoản.</p>";
                }
            }
            catch
            {
                ViewBag.message = "<h4 style='padding-top:20px'>Thành thật xin lỗi quý khách, vui lòng thử lại thay đổi mật khẩu <a href='/quenmatkhau.html' style='color: red'>Thay đổi mật khẩu</a></h4>";

            }
            return View();
        }
    }
}
