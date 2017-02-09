using AutoMapper;
using Babymart.DAL.Checkout;
using Babymart.DAL.Common;
using Babymart.DAL.Customer;
using Babymart.DAL.Product;
using Babymart.DAL.Shipping;
using Babymart.DAL.Sys_Mail;
using Babymart.Infractstructure;
using Babymart.Models;
using Babymart.Models.Module;
using Babymart.Models.Module.Enum;
using Babymart.ViewModels;
using Babymart.VnPay;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VNPAYMENT_NET_CS.Common;

namespace Babymart.Controllers
{
    public class CheckoutController : Controller
    {
        private babymart_vnEntities db = new babymart_vnEntities();
        private ICheckoutRepository _checkout;
        private IProductRepository _iproduct;
        private ICustomer _customer;
        private ICommonRepository _common;
        private ISys_MailRepository _imail;
        private IShippingRepository _shippingRepository;
        private string urlnt;
        private long iddhtc;
        private string emailkhach;
        private string tenkhach;
        private string noidungmail;
        private long madonhang;
        InitRq initRq;
        PaymentApi vnpay;

        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CheckoutController()
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
            if (IsHasCart() == false)
            {
                return View("/Views/Checkout/EmtyCart.cshtml");
            }

            return View();
        }

        public ActionResult step2()
        {
            if (IsHasCart() == false)
            {
                return View("/Views/Checkout/EmtyCart.cshtml");
            }
            return View();
        }

        //public ActionResult Success(string MaDH
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
                    ViewBag.Message = "Đặt hàng và thanh toán thành công <br/>";
                    ViewBag.Code = "1";
                }
                else
                {
                    ViewBag.Message = "<span style='font-size:20px;color:black'>Giao dịch không thành công .<br>Vui lòng kiểm tra và nhập lại thông tin chính xác hoặc <br> chọn phương thức thanh toán khác<br></span><span style='color:black;font-size: 20px;font-weight:normal'>Xin cảm ơn</span>";
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
                //_checkout.UpdateOrderContentVnPay(iddhtc, "00");
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
            UtilsBB.EmptyCart();
            return Redirect(UtilsBB.HasCookieLangEn() ? "/en" : "/");
        }

        [HttpPost]
        public ActionResult LoadCart()
        {
            var viewModel = new ShoppingCartViewModel
            {
                CartItemModel = UtilsBB.GetCartItem(),
                CartTotal = UtilsBB.GetTotal(),
                KgTotal = String.Format("{0:0.00}", UtilsBB.GetTotalKg())
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        private bool IsHasCart()
        {
            var set = UtilsBB.GetCartItem();
            if (set != null && set.Count > 0)
                return true;
            return false;
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
            //if (dh.ship == null)
            //{
            //    tongtien = (decimal)dh.tongtien + 0;
            //}
            //else
            //{
            //    tongtien = (decimal)dh.tongtien + (decimal)dh.ship;
            //}

            tongtien = (decimal)dh.tongtien + 0;
            string secretKey = ConfigurationManager.AppSettings["vnpay_secretkey"];
            initRq = new InitRq();
            initRq.Action = "INIT";
            initRq.Version = "1.0.1";
            initRq.TerminalId = ConfigurationManager.AppSettings["vnpay_tid"];
            initRq.OrderId = iddh.ToString();
            initRq.Amount = Math.Round(tongtien).ToString();
            initRq.OrderDesc = "Thanh toan don hang so " + iddh.ToString() + " tai babymart.vn";
            initRq.CurrCode = "VND";
            initRq.PaymentMethod = str_bankcode;
            initRq.Locale = "vi-VN";
            initRq.LocalDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            initRq.ClientIp = Utils.GetIpAddress();
            initRq.Signature = initRq.MakeSignature(secretKey);
            string data = JsonConvert.SerializeObject(initRq);
            vnpay = new PaymentApi();
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
            content = content.Replace("\n", "");
            var listpro = new List<shop_sanpham>();
            foreach (var item in UtilsBB.GetCartItem())
            {
                var pro = _iproduct.GetProductById((int)item.shop_bienthe.idsp);
                if (pro.countsale == null)
                {
                    pro.countsale = item.Count;
                }
                else
                {
                    pro.countsale += item.Count;
                }

                listpro.Add(pro);
            }
            _iproduct.UpdateListProduct(listpro);
            if (dh.pttt == (int)Phuongthucthanhtoan.martCard1)
            {
                this.PaymentNL(dh, kh, iddh);
                _checkout.UpdateOrderContentVnPay(iddh, "01");
                _checkout.UpdateOrderContent(iddh, content);
                //SendmailCheckout(kh.email, kh.hoten, content, iddh);
                //UtilsBB.EmptyCart();
                this.Session["iddhtc"] = iddh;
                this.Session["emailkhach"] = kh.email;
                this.Session["tenkhach"] = kh.hoten;
                this.Session["noidungmail"] = content;
                this.Session["madonhang"] = iddh;

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
                //UtilsBB.EmptyCart();
                //return Json("/xac-nhan-thanh-cong.html/" + iddh, JsonRequestBehavior.AllowGet);
                return Json("/xac-nhan-thanh-cong.html", JsonRequestBehavior.AllowGet);
            }
        }

        private void SendmailCheckout(string email, string hoten, string cart, long iddh)
        {
            var title = !UtilsBB.HasCookieLangEn() ? "Thông báo đặt hàng thành công" : "Notice Order Success";
            var titlebabymart = (!UtilsBB.HasCookieLangEn() ? "Đơn hàng số " : "Order ID ") + iddh + " : " + hoten + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
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
                content += "<div><span style=\"font-family: Arial; color: #000000;\"><strong>Thank you for shopping on <a href=\"https://babymart.vn\" target=\"_blank\">Babymart.vn</a></strong></span></div>";
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
            var viewModelCart = new ShoppingCartViewModel
            {
                CartItemModel = UtilsBB.GetCartItem(),
                CartTotal = UtilsBB.GetTotal(),
                KgTotal = String.Format("{0:0.00}", UtilsBB.GetTotalKg()),
                WHL = UtilsBB.GetTotalW_H_LCart()
            };
            bool isHasSaleoff = false;

            foreach (var item in viewModelCart.CartItemModel)
            {
                var bienthe = db.shop_bienthe.Find(item.ProductId);
                if (bienthe != null && bienthe.giasosanh > 0)
                {
                    isHasSaleoff = true;
                    break;
                }
            }
            return Json(new { city = city, vung = vung, tinhtra = tinhtra, cart = viewModelCart, isHasSaleoff = isHasSaleoff }, JsonRequestBehavior.AllowGet);
        }
    }
}