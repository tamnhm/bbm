namespace VNPAYMENT_NET_CS.Common
{
    /// <summary>
    /// Confirm request
    /// </summary>
    public class ConfirmRq
    {
        public string RspCode { get; set; }
        public string TerminalId { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string CurrCode { get; set; }
        public string VnpTranid { get; set; }
        public string PaymentMethod { get; set; }
        public string PayDate { get; set; }
        public string AdditionalInfo { get; set; }
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
                string signData = RspCode + "|" + TerminalId + "|" + OrderId + "|" + Amount + "|" + CurrCode + "|" +
                                  VnpTranid + "|" + PaymentMethod + "|" + PayDate + "|" +
                                  secretKey;
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
                string signData = RspCode + "|" + TerminalId + "|" + OrderId + "|" + Amount + "|" + CurrCode + "|" +
                                  VnpTranid + "|" + PaymentMethod + "|" + PayDate + "|" +
                                  secretKey;
                return Utils.Md5(signData).Equals(Signature);
            }
        }

        #endregion
    }
}