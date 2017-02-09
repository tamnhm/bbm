namespace VNPAYMENT_NET_CS.Common
{
    /// <summary>
    /// Transaction Request
    /// </summary>
    public class InitRq
    {
        /// <summary>
        /// Action of transaction: INIT,DELIVERED,QUERY,REFUND
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// VNPAY API Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// vi-VN,en-US
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Terminal id
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// Transaction Id on merchant website
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Amount of transaction
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrCode { get; set; }

        /// <summary>
        /// Payment method, default: VNPAY
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Time on merchant website: yyyyMMddHHmmss
        /// </summary>
        public string LocalDate { get; set; }

        /// <summary>
        /// Order description
        /// </summary>
        public string OrderDesc { get; set; }

        /// <summary>
        /// IpAddress of customer
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// Transaction signature
        /// </summary>
        public string Signature { get; set; }

        #region Secure methods

        public string MakeSignature(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                return string.Empty;
            }
            else
            {
                string signData = TerminalId + "|" + OrderId + "|" + Amount + "|" + CurrCode + "|" +
                                  PaymentMethod + "|" + LocalDate + "|" +
                                  OrderDesc + "|" + ClientIp + "|" + secretKey;
                return Utils.Md5(signData);
            }
        }

        public bool IsValidSignature(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                return false;
            }
            else
            {
                string signData = TerminalId + "|" + OrderId + "|" + Amount + "|" + CurrCode + "|" +
                                  PaymentMethod + "|" + LocalDate + "|" +
                                  OrderDesc + "|" + ClientIp + "|" + secretKey;
                return Utils.Md5(signData).Equals(Signature);
            }
        }

        public string LogData
        {
            get
            {
                return "TerminalCode=" + TerminalId + ",OrderId=" + OrderId + ",Amount=" + Amount + ",CurrCode=" +
                       CurrCode + ",PaymentMethod=" +
                       PaymentMethod + ",LocalDate=" + LocalDate + ",OrderDesc=" +
                       OrderDesc + ",ClientIp=" + ClientIp;
            }
        }

        #endregion
    }
}
