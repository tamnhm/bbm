using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.Models;
using Babymart.ViewModels;
using Babymart.DAL.Shipping;
using Babymart.DAL.Checkout;
using Babymart.Models.Module;
using AutoMapper;
using Babymart.DAL.Customer;
using Babymart.DAL.Sys_Mail;
using System.Text;
using Babymart.DAL.Common;
using Babymart.DAL.Product;
using Babymart.Infractstructure;
using System.Configuration;
using VNPAYMENT_NET_CS.Common;
using Babymart.VnPay;
using Newtonsoft.Json;
using log4net;
using Babymart.Models.Module.Enum;
namespace Babymart.Controllers
{
    public class CheckoutPlanController : Controller
    {
        babymart_vnEntities db = new babymart_vnEntities();
        private ICheckoutRepository _checkout;
        private IProductRepository _iproduct;
        private ICustomer _customer;
        private ICommonRepository _common;
        private ISys_MailRepository _imail;
        private IShippingRepository _shippingRepository;
        string urlnt;
        private long iddhtc;
        string emailkhach;
        string tenkhach;
        string noidungmail;
        long madonhang;
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CheckoutPlanController()
        {
            this._checkout = new CheckoutRepository(new babymart_vnEntities());
            this._customer = new Customer(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
            this._imail = new Sys_MailRepository(new babymart_vnEntities());
            this._iproduct = new ProductRepository(new babymart_vnEntities());
            this._shippingRepository = new ShippingRepository(new babymart_vnEntities());
        }
        public ActionResult Index()
        {
            if (this.Session["PlanCart"] == null)
            {
                return View("/Views/Checkout/EmtyCart.cshtml");
            }

            return View();
        }

        public ActionResult step2()
        {
            if (this.Session["PlanCart"] == null)
            {
                return View("/Views/Checkout/EmtyCart.cshtml");
            }
            return View();

        }
        public ActionResult Success()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["rspCode"]))
            {
                string RspCode = Request.QueryString["rspCode"];
                if (RspCode == "00")
                {
                    var objkh = this.Session["objkh"];
                    if (objkh != null)
                    {
                        _customer.Update(Mapper.Map<khachhang>(objkh));
                        this.Session["khachhang"] = null;
                        this.Session["khachhang"] = objkh;
                    }
                    ViewBag.Madh = this.Session["madonhang"];
                    var kh = (ModeCustomerPost)this.Session["khachhang"];
                    if (kh != null)
                    {
                        ViewBag.DiemKh = kh.diem;
                    }
                    iddhtc = long.Parse(this.Session["iddhtc"].ToString());
                    _checkout.UpdateOrderContentVnPay(iddhtc, "00");
                    emailkhach = this.Session["emailkhach"].ToString();
                    tenkhach = this.Session["tenkhach"].ToString();
                    noidungmail = this.Session["noidungmail"].ToString();
                    madonhang = long.Parse(this.Session["madonhang"].ToString());
                    SendmailCheckout(emailkhach, tenkhach, noidungmail, madonhang);
                    UtilsBB.EmptyCart();
                    this.Session["PlanCart"] = null;
                    this.Session["emailkhach"] = null;
                    this.Session["tenkhach"] = null;
                    this.Session["noidungmail"] = null;
                    this.Session["madonhang"] = null;
                    ViewBag.Message = "Đặt hàng và thanh toán thành công";
                    ViewBag.Code = "1";
                }
                else
                {
                    ViewBag.Message = ViewBag.Message = "<span style='font-size:20px;color:black'>Giao dịch không thành công, bạn đã nhập sai thông tin thẻ.<br>Vui lòng kiểm tra và nhập lại thông tin chính xác hoặc <br> chọn phương thức thanh toán khác<br></span><span style='color:black;font-size: 20px;font-weight:normal'>Xin cảm ơn</span>";
                    //_checkout.UpdateOrderContentVnPay(iddhtc, "01");
                    ViewBag.Code = "0";
                }
            }
            else
            {
                var objkh = this.Session["objkh"];
                if (objkh != null)
                {
                    _customer.Update(Mapper.Map<khachhang>(objkh));
                    this.Session["khachhang"] = null;
                    this.Session["khachhang"] = objkh;
                }
                ViewBag.Madh = this.Session["madonhang"];
                var kh = (ModeCustomerPost)this.Session["khachhang"];
                if (kh != null)
                {
                    ViewBag.DiemKh = kh.diem;
                }
                iddhtc = long.Parse(this.Session["iddhtc"].ToString());
                _checkout.UpdateOrderContentVnPay(iddhtc, "00");
                emailkhach = this.Session["emailkhach"].ToString();
                tenkhach = this.Session["tenkhach"].ToString();
                noidungmail = this.Session["noidungmail"].ToString();
                madonhang = long.Parse(this.Session["madonhang"].ToString());
                SendmailCheckout(emailkhach, tenkhach, noidungmail, madonhang);
                UtilsBB.EmptyCart();
                this.Session["PlanCart"] = null;
                this.Session["emailkhach"] = null;
                this.Session["tenkhach"] = null;
                this.Session["noidungmail"] = null;
                this.Session["madonhang"] = null;
                ViewBag.Message = "Đặt hàng thành công";
                ViewBag.Code = "1";
            }
            return View();
        }
        public ActionResult CancelCart()
        {
            this.Session["PlanCart"] = null;
            return Redirect(UtilsBB.HasCookieLangEn() ? "/en" : "/");
        }
        [HttpPost]
        public ActionResult LoadCart()
        {
            if (this.Session["PlanCart"] != null)
            {
                var model = (Babymart.Models.Module.PlanCartModel)Session["PlanCart"];
                if (model != null)
                    return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public string PaymentNL(ModelDonHang dh, ModeCustomerPost kh, float iddh)
        {
            string payment_method = dh.NLpayBankType;
            string str_bankcode;
            if (payment_method == "VISA")
            {
                str_bankcode = "VISA";
            }
            else
            {
                str_bankcode = dh.BankOnline;
            }

            decimal tongtien;
            tongtien = (decimal)dh.tongtien + 0;
            string secretKey = ConfigurationManager.AppSettings["vnpay_secretkey"];
            InitRq initRq = new InitRq();
            initRq.Action = "INIT";
            initRq.Version = "1.0.1";
            initRq.TerminalId = ConfigurationManager.AppSettings["vnpay_tid"];
            initRq.OrderId = iddh.ToString();
            //initRq.Amount = dh.tongtien.ToString();
            initRq.Amount = Math.Round(tongtien).ToString();
            initRq.OrderDesc = "Thanh toán đơn hàng số " + iddh.ToString() + " tại babymart.vn";
            initRq.CurrCode = "VND";
            initRq.PaymentMethod = str_bankcode;
            initRq.Locale = "vi-VN";
            initRq.LocalDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            initRq.ClientIp = Utils.GetIpAddress();
            initRq.Signature = initRq.MakeSignature(secretKey);
            string data = JsonConvert.SerializeObject(initRq);
            PaymentApi vnpay = new PaymentApi();
            log.InfoFormat("Begin call VNPAY Init transaction, data={0}", data);
            string vnpayReturn = vnpay.Execute(data);
            log.InfoFormat("End call VNPAY Init transaction, data={0}", data);
            log.InfoFormat("VNPAY return data={0}", vnpayReturn);
            InitRs initRs = JsonConvert.DeserializeObject<InitRs>(vnpayReturn);

            string merchantSign = initRs.MakeSignature(secretKey);
            urlnt = initRs.UrlRedirect;
            if (merchantSign.Equals(initRs.Signature))
            {
                log.InfoFormat("Check signature ok, redirect to:{0}", initRs.UrlRedirect);
                return urlnt;
            }
            else
            {
                //Display message
                log.InfoFormat("Invalid signature, Code={0},Msg={1}", initRs.RspCode, initRs.Message);
                return "Error VNPAY RETURN:" + vnpayReturn;
            }
        }
        [HttpPost]
        public ActionResult Checkout(ModelDonHang dh, ModeCustomerPost kh, string content)
        {

            if (kh.MaKH > 0)
            {
                var khachhang = db.khachhangs.Find(kh.MaKH);
                if (khachhang != null)
                {
                    khachhang.diem = (int.Parse(kh.diem) + int.Parse(kh.diemsp)).ToString();
                    khachhang.hoten = kh.hoten;
                    khachhang.dienthoai = kh.dienthoai;
                    khachhang.duong = kh.duong;
                    khachhang.idquan = kh.idquan;
                    khachhang.idtp = kh.idtp;
                    khachhang.email = kh.email;
                    var objkh = Mapper.Map<ModeCustomerPost>(khachhang);
                    this.Session["objkh"] = objkh;
                    //_customer.Update(Mapper.Map<khachhang>(objkh));
                    //this.Session["khachhang"] = null;
                    //this.Session["khachhang"] = objkh;
                }
            }
            else
            {
                dh.vanglai = _customer.AddVL(Mapper.Map<khachhang_vanglai>(kh));
                dh.makh = null;
            }

            dh.diemsp = int.Parse(kh.diemsp.ToString());
            var tmp = Mapper.Map<ModelDonHangPost>(dh);
            long iddh = _checkout.AddOrder(Mapper.Map<donhang>(tmp));
            List<donhang_ct> ctdh = new List<donhang_ct>();
            foreach (var item in dh.donhang_ct)
            {
                item.Sodh = iddh;
                ctdh.Add(Mapper.Map<donhang_ct>(item));
            }
            _checkout.AddOrder_CT(ctdh);
            // var cart = ShoppingCart.GetCart(this.HttpContext);
            content = content.Replace("\n", "");
            // var listpro = new List<shop_sanpham>();
            //foreach (var item in cart.GetCartItem())
            //{
            //    var pro = _iproduct.GetProductById((int)item.shop_bienthe.idsp);
            //    if (pro.countsale == null)
            //    {
            //        pro.countsale = item.Count;
            //    }
            //    else
            //    {
            //        pro.countsale += item.Count;
            //    }

            //    listpro.Add(pro);

            //}
            //_iproduct.UpdateListProduct(listpro);
            if (dh.pttt == (int)Phuongthucthanhtoan.martCard1)
            {
                this.PaymentNL(dh, kh, iddh);
                _checkout.UpdateOrderContentVnPay(iddh, "01");
                this.Session["iddhtc"] = iddh;
                this.Session["emailkhach"] = kh.email;
                this.Session["tenkhach"] = kh.hoten;
                this.Session["noidungmail"] = content;
                this.Session["madonhang"] = iddh;

                //SendmailCheckout(kh.email, kh.hoten, content, iddh);
                //this.Session["PlanCart"] = null;
                return Json(urlnt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _checkout.UpdateOrderContentVnPay(iddh, "01");
                this.Session["iddhtc"] = iddh;
                this.Session["emailkhach"] = kh.email;
                this.Session["tenkhach"] = kh.hoten;
                this.Session["noidungmail"] = content;
                this.Session["madonhang"] = iddh; 

                //SendmailCheckout(kh.email, kh.hoten, content, iddh);
                //this.Session["PlanCart"] = null;
                return Json("/xac-nhan-thanh-cong-do-so-sinh.html/", JsonRequestBehavior.AllowGet);
            }

        }
        private void SendmailCheckout(string email, string hoten, string cart, long iddh)
        {
            var title = !UtilsBB.HasCookieLangEn() ? "Thông báo đặt hàng thành công - Gói đồ sơ sinh" : "Notice Order Success - New baby checklist";
            var titlebabymart = (!UtilsBB.HasCookieLangEn() ? " Gói đồ sơ sinh - Đơn hàng số " : "New baby checklist - Order ID ") + iddh + " : " + hoten + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            //   var title = "Thông báo đặt hàng thành công - Gói đồ sơ sinh";
            // var titlebabymart = "Gói đồ sơ sinh - Đơn hàng số " + iddh + " : " + hoten + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var content = "<div style=\"line-height: 1.5;\">";


            if (UtilsBB.HasCookieLangEn() == false)
            {
                content += "<div><span style=\"font-family: Arial; color: #000000;\"> <strong>Chào bạn " + hoten + " !</strong></span></div>";
                content += "<div><span style=\"font-family: Arial; color: #000000;\"><strong> Cảm ơn bạn đ&atilde; đặt h&agrave;ng tại website&nbsp;<a href=\"https://babymart.vn\" target=\"_blank\">Babymart.vn</a></strong></span></div>";
                content += "<div><span style=\"font-family: Arial;  color: #000000;\"><strong>Dưới đ&acirc;y l&agrave; th&ocirc;ng tin đơn h&agrave;ng của bạn.</strong></span></div></div>";
                content += "<br/><br/>";
            }
            else
            {
                content += "<div><span style=\"font-family: Arial; color: #000000;\"> <strong>Dear  " + hoten + " !</strong></span></div>";
                content += "<div><span style=\"font-family: Arial; color: #000000;\"><strong> Thank you for shopping on <a href=\"https://babymart.vn\" target=\"_blank\">Babymart.vn</a></strong></span></div>";
                content += "<div><span style=\"font-family: Arial;  color: #000000;\"><strong>Order detail :</strong></span></div></div>";
                content += "<br/><br/>";
            }


            content += cart;
            _common.SendEmail(email, title, content);
            _common.SendEmail("babymart.vn@babymart.vn", titlebabymart, content);
            _checkout.UpdateOrderContent(iddh, content);





        }
        public ActionResult LoadComponese()
        {
            var city = Mapper.Map<List<ModelShipping>>(_shippingRepository.List_Tp());
            var vung = _shippingRepository.List_Vung();
            var tinhtra = db.donhang_chuyenphat_tp_tinhtra.ToList();
            var viewModelCart = new Babymart.Models.Module.PlanCartModel();
            if (this.Session["PlanCart"] != null)
            {
                viewModelCart = (Babymart.Models.Module.PlanCartModel)Session["PlanCart"];
            }
            return Json(new { city = city, vung = vung, tinhtra = tinhtra, cart = viewModelCart }, JsonRequestBehavior.AllowGet);
        }
    }
}
