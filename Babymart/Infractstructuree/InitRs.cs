namespace VNPAYMENT_NET_CS.Common
{
    /// <summary>
    /// Transaction Response
    /// </summary>
  public  class InitRs
    {
      public string RspCode { get; set; }
      public string Message { get; set; }
      public string UrlRedirect { get; set; }
      public string Signature { get; set; }
      //Transaction info
      public string TerminalId { get; set; }
      public string OrderId { get; set; }
      public string Amount { get; set; }
      public string CurrCode { get; set; }
      public string VnpTranid { get; set; }
      public string PaymentMethod { get; set; }
      public string LocalDate { get; set; }
      public string CreatedDate { get; set; }
      public string AdditionalInfo { get; set; }
      public string Status { get; set; }

        public InitRs(string code,string msg)
        {
            RspCode = code;
            Message = msg;
        }
        #region Secure methods
        public string MakeSignature(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                return string.Empty;
            }
            else
            {
                string signData = RspCode + "|" + Message + "|" + UrlRedirect + "|" + secretKey;
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
                string signData = RspCode + "|" + Message + "|" + UrlRedirect + "|" + secretKey;
                return Utils.Md5(signData).Equals(Signature);
            }
        }
        #endregion
    }
}
