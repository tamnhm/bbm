namespace VNPAYMENT_NET_CS.Common
{
    public class RequestObject
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
        /// Order Id on merchant website
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Transaction in VNPAY
        /// </summary>
        public string VnPayTranId { get; set; }
        /// <summary>
        /// Time on merchant website: yyyyMMddHHmmss
        /// </summary>
        public string LocalDate { get; set; }

        /// <summary>
        /// Request description
        /// </summary>
        public string RequestDesc { get; set; }

        /// <summary>
        /// IpAddress of customer
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
                string signData =Action + "|" + TerminalId + "|" + OrderId + "|"  + LocalDate + "|" +
                                  RequestDesc + "|"   + secretKey;
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
                string signData = Action + "|" + TerminalId + "|" + OrderId + "|" + LocalDate + "|" +
                                 RequestDesc + "|" + secretKey;
                return Utils.Md5(signData).Equals(Signature);
            }
        }
        #endregion
    }
}