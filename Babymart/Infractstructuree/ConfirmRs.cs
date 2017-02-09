namespace VNPAYMENT_NET_CS.Common
{
    /// <summary>
    /// Confirm response
    /// </summary>
    public class ConfirmRs
    {
        public string RspCode { get; set; }
        public string Message { get; set; } 
        public string TerminalId { get; set; }
        public string OrderId { get; set; }
        public string Localdate { get; set; }
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
                string signData = RspCode + "|" + Message + "|" + OrderId + "|" + secretKey;
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
                string signData = RspCode + "|" + Message + "|" + OrderId + "|" + secretKey;
                return Utils.Md5(signData).Equals(Signature);
            }
        }
        #endregion
    }
}