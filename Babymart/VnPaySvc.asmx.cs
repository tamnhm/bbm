using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Configuration;
using VNPAYMENT_NET_CS.Common;
using Babymart.Models;
using Babymart.DAL.Checkout;

namespace Babymart
{
    /// <summary>
    /// Summary description for VnPaySvc
    /// </summary>
    [WebService(Namespace = "http://vnpayment.vn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VnPaySvc : System.Web.Services.WebService
    {
        private ICheckoutRepository _checkout;
        babymart_vnEntities db = new babymart_vnEntities();
        public static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello From Merchant:" + DateTime.Now;
        }

        //[WebMethod]
        //public string Execute(string data)
        //{
        //    this._checkout = new CheckoutRepository(new babymart_vnEntities());
        //    log.InfoFormat("Begin VNPAY confirm, data={0}", data);
        //    ConfirmRs cfRs = new ConfirmRs();

        //    try
        //    {
        //        //Return VNPAY
        //        //00    Merchant Ghi nhan thanh cong (Success)
        //        //01	Không tìm thấy GD (Transaction Not found)
        //        //02	Giao dịch đã được confirm (Transaction Already Confirmed)
        //        //08	Hệ thống bảo trì hoặc kết nối webservices bị timeout
        //        //97	Chữ ký không hợp lệ (Invalid signature)
        //        ConfirmRq rq = JsonConvert.DeserializeObject<ConfirmRq>(data);
        //        log.InfoFormat("Vnpay Confirm: OrderId={0},VnpayTranId={1},Code={2},AddInfo={3}", rq.OrderId,
        //            rq.VnpTranid, rq.RspCode, rq.AdditionalInfo);



        //        string vnpayData = rq.RspCode + "|" +
        //                           rq.TerminalId + "|" +
        //                           rq.OrderId + "|" +
        //                           rq.Amount + "|" +
        //                           rq.CurrCode + "|" +
        //                           rq.VnpTranid + "|" +
        //                           rq.PaymentMethod + "|" +
        //                           rq.PayDate + "|" +
        //                           rq.AdditionalInfo + "|" + ConfigurationManager.AppSettings["vnpay_secretkey"];
        //        string merchantSign = Utils.Md5(vnpayData);

        //        int orderid = int.Parse(rq.OrderId);
        //        int ordercount = db.donhangs.Where(o => o.id == orderid).Count();
        //        donhang dh = db.donhangs.Where(o => o.id == orderid).FirstOrDefault();

        //        if (ConfigurationManager.AppSettings["SysMaintenance"] == "0")
        //        {

        //            if (ordercount > 0)
        //            {

        //                if (rq.Signature != merchantSign)
        //                {
        //                    cfRs.RspCode = "97";
        //                    cfRs.Message = "Sai chu ky";
        //                    cfRs.TerminalId = rq.TerminalId;
        //                    cfRs.OrderId = rq.OrderId;
        //                    cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        //                    string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
        //                                                 cfRs.OrderId +
        //                                                 "|" + cfRs.Localdate + "|" +
        //                                                 ConfigurationManager.AppSettings["vnpay_secretkey"];

        //                    cfRs.Signature = Utils.Md5(signReturnVnpayData);
        //                }
        //                else
        //                {
        //                    if (dh.tinhtrang == "00")
        //                    {
        //                        cfRs.RspCode = "00";
        //                        cfRs.Message = "Giao dich thanh cong";
        //                        cfRs.TerminalId = rq.TerminalId;
        //                        cfRs.OrderId = rq.OrderId;
        //                        cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        //                        string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
        //                                                     cfRs.OrderId +
        //                                                     "|" + cfRs.Localdate + "|" +
        //                                                     ConfigurationManager.AppSettings["vnpay_secretkey"];
        //                        cfRs.Signature = Utils.Md5(signReturnVnpayData);
        //                        _checkout.UpdateOrderContentVnPay(iddh, "02");
        //                    }
        //                    else
        //                    {
        //                        cfRs.RspCode = "02";
        //                        cfRs.Message = "giao dich ton tai";
        //                        cfRs.TerminalId = rq.TerminalId;
        //                        cfRs.OrderId = rq.OrderId;
        //                        cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        //                        string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
        //                                                     cfRs.OrderId +
        //                                                     "|" + cfRs.Localdate + "|" +
        //                                                     ConfigurationManager.AppSettings["vnpay_secretkey"];
        //                        cfRs.Signature = Utils.Md5(signReturnVnpayData);
        //                    }
        //                }
        //            }
        //            if (ordercount == 0 || dh.tinhtrang == "01")
        //            {
        //                cfRs.RspCode = "01";
        //                cfRs.Message = "Không tìm thấy GD";
        //                cfRs.TerminalId = rq.TerminalId;
        //                cfRs.OrderId = rq.OrderId;
        //                cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        //                string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
        //                                             cfRs.OrderId +
        //                                             "|" + cfRs.Localdate + "|" +
        //                                             ConfigurationManager.AppSettings["vnpay_secretkey"];
        //                cfRs.Signature = Utils.Md5(signReturnVnpayData);

        //            }

        //        }
        //        else
        //        {
        //            cfRs.RspCode = "08";
        //            cfRs.Message = "Hệ thống đang bảo trì";
        //            cfRs.TerminalId = rq.TerminalId;
        //            cfRs.OrderId = rq.OrderId;
        //            cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        //            string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
        //                                         cfRs.OrderId +
        //                                         "|" + cfRs.Localdate + "|" +
        //                                         ConfigurationManager.AppSettings["vnpay_secretkey"];
        //            cfRs.Signature = Utils.Md5(signReturnVnpayData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cfRs.RspCode = "99";
        //        cfRs.Message = ex.Message;
        //        log.FatalFormat("Exception:{0}", ex);
        //    }


        //    string returnVnpay = JsonConvert.SerializeObject(cfRs);
        //    log.InfoFormat("returnVnpay: {0}", returnVnpay);
        //    return returnVnpay;
        //}

        [WebMethod]
        public string Execute(string data)
        {
            this._checkout = new CheckoutRepository(new babymart_vnEntities());
            log.InfoFormat("Begin VNPAY confirm, data={0}", data);
            ConfirmRs cfRs = new ConfirmRs();

            try
            {
                //Return VNPAY
                //00    Merchant Ghi nhan thanh cong (Success)
                //01	Không tìm thấy GD (Transaction Not found)
                //02	Giao dịch đã được confirm (Transaction Already Confirmed)
                //08	Hệ thống bảo trì hoặc kết nối webservices bị timeout
                //97	Chữ ký không hợp lệ (Invalid signature)
                ConfirmRq rq = JsonConvert.DeserializeObject<ConfirmRq>(data);
                log.InfoFormat("Vnpay Confirm: OrderId={0},VnpayTranId={1},Code={2},AddInfo={3}", rq.OrderId,
                    rq.VnpTranid, rq.RspCode, rq.AdditionalInfo);
                int ordercount = 0;


                string vnpayData = rq.RspCode + "|" +
                                   rq.TerminalId + "|" +
                                   rq.OrderId + "|" +
                                   rq.Amount + "|" +
                                   rq.CurrCode + "|" +
                                   rq.VnpTranid + "|" +
                                   rq.PaymentMethod + "|" +
                                   rq.PayDate + "|" +
                                   rq.AdditionalInfo + "|" + ConfigurationManager.AppSettings["vnpay_secretkey"];
                string merchantSign = Utils.Md5(vnpayData);

                int orderid = int.Parse(rq.OrderId);
                ordercount = db.donhangs.Where(o => o.id == orderid).Count();
                donhang dh = db.donhangs.Where(o => o.id == orderid).FirstOrDefault();

                if (ConfigurationManager.AppSettings["SysMaintenance"] == "0")
                {
                    if (rq.Signature != merchantSign)
                    {
                        cfRs.RspCode = "97";
                        cfRs.Message = "Sai chu ky";
                        cfRs.TerminalId = rq.TerminalId;
                        cfRs.OrderId = rq.OrderId;
                        cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
                                                     cfRs.OrderId +
                                                     "|" + cfRs.Localdate + "|" +
                                                     ConfigurationManager.AppSettings["vnpay_secretkey"];

                        cfRs.Signature = Utils.Md5(signReturnVnpayData);
                    }
                    else
                    {
                        if(ordercount==0)
                        {
                            cfRs.RspCode = "01";
                            cfRs.Message = "Khong tim thay GD";
                            cfRs.TerminalId = rq.TerminalId;
                            cfRs.OrderId = rq.OrderId;
                            cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                            string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
                                                         cfRs.OrderId +
                                                         "|" + cfRs.Localdate + "|" +
                                                         ConfigurationManager.AppSettings["vnpay_secretkey"];

                            cfRs.Signature = Utils.Md5(signReturnVnpayData);
                        }
                        else
                        {
                            if(dh.tinhtrang=="00")
                            {
                                cfRs.RspCode = "02";
                                cfRs.Message = "GD da duoc confirm";
                                cfRs.TerminalId = rq.TerminalId;
                                cfRs.OrderId = rq.OrderId;
                                cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                                string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
                                                             cfRs.OrderId +
                                                             "|" + cfRs.Localdate + "|" +
                                                             ConfigurationManager.AppSettings["vnpay_secretkey"];

                                cfRs.Signature = Utils.Md5(signReturnVnpayData);
                            }
                            else
                            {
                            cfRs.RspCode = "00";
                            cfRs.Message = "Ghi nhan GD thanh cong";
                            cfRs.TerminalId = rq.TerminalId;
                            cfRs.OrderId = rq.OrderId;
                            cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                            string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
                                                         cfRs.OrderId +
                                                         "|" + cfRs.Localdate + "|" +
                                                         ConfigurationManager.AppSettings["vnpay_secretkey"];

                            cfRs.Signature = Utils.Md5(signReturnVnpayData);
                            _checkout.UpdateOrderContentVnPay(orderid, "00");
                            }
                        }
                        
                    }
                }
                else
                {
                    cfRs.RspCode = "08";
                    cfRs.Message = "He thong dang bao tri";
                    cfRs.TerminalId = rq.TerminalId;
                    cfRs.OrderId = rq.OrderId;
                    cfRs.Localdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string signReturnVnpayData = cfRs.RspCode + "|" + cfRs.Message + "|" + cfRs.TerminalId + "|" +
                                                 cfRs.OrderId +
                                                 "|" + cfRs.Localdate + "|" +
                                                 ConfigurationManager.AppSettings["vnpay_secretkey"];
                    cfRs.Signature = Utils.Md5(signReturnVnpayData);
                }
            }
            catch (Exception ex)
            {
                cfRs.RspCode = "99";
                cfRs.Message = ex.Message;
                log.FatalFormat("Exception:{0}", ex);
            }


            string returnVnpay = JsonConvert.SerializeObject(cfRs);
            log.InfoFormat("returnVnpay: {0}", returnVnpay);
            return returnVnpay;
        }
    }
}

