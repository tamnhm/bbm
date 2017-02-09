namespace VNPAYMENT_NET_CS.Common
{
    public class ResponseObject
    {
        public string Action { get; set; }
        public string RspCode { get; set; }
        public string Message { get; set; }

        public string TerminalId { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string CurrCode { get; set; }
        public string VnpTranId { get; set; }
        public string PaymentMethod { get; set; }
        public string LocalDate { get; set; }
        public string CreatedDate { get; set; }
        public string PayDate { get; set; }
        public string AdditionalInfo { get; set; }
        public string Status { get; set; }
        public string Signature { get; set; }

        #region Construct

        public ResponseObject()
        {
            
        }
        public ResponseObject(string code, string msg)
        {
            this.RspCode = code;
            this.Message = msg;
        }
        #endregion
        #region Secure methods

        public string MakeSignature(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                return string.Empty;
            }
            else
            {
                string signData = Action + "|" + RspCode + "|" + TerminalId + "|" + OrderId + "|"   + secretKey;
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
                string signData = Action + "|" + RspCode + "|" + TerminalId + "|" + OrderId + "|" + secretKey;
                return Utils.Md5(signData).Equals(Signature);
            }
        }
        #endregion
    }
}