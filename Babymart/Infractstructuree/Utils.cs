using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using log4net;

namespace VNPAYMENT_NET_CS.Common
{
   public class Utils
    {
       public static readonly ILog log =
         LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       public static string Md5(string strInput)
       {
           var algorithmType = default(HashAlgorithm);
           byte[] valueByteArr = Encoding.ASCII.GetBytes(strInput);
           byte[] hashArray = null;
           // Encrypt Input string 
           algorithmType = new MD5CryptoServiceProvider();
           hashArray = algorithmType.ComputeHash(valueByteArr);
           //Convert byte hash to HEX
           var sb = new StringBuilder();
           foreach (byte b in hashArray)
           {
               sb.AppendFormat("{0:x2}", b);
           }
           return sb.ToString();
       }
       public static string ToAscii(string sInput)
       {

           var sContent = new StringBuilder();
           string sTemp = null;
           //sTemp = HttpContext.Current.Server.HtmlDecode(sInput);
           //sTemp = sTemp.Replace("&", " ")
           sTemp = System.Text.RegularExpressions.Regex.Replace(sTemp, "Đ|&#273;", "D");
           sTemp = System.Text.RegularExpressions.Regex.Replace(sTemp, "đ|&#272;", "d");
           // sTemp = System.Text.RegularExpressions.Regex.Replace(sTemp, "-", " ")
           //	// normalize the Unicode
           sTemp = sTemp.Normalize(NormalizationForm.FormKD);
           foreach (char s in sTemp)
           {
               if (char.IsWhiteSpace(s))
               {
                   sContent.Append(" ");
               }
               else if ((char.GetUnicodeCategory(s) != UnicodeCategory.NonSpacingMark) && !(char.IsPunctuation(s)) && !(char.IsSymbol(s)))
               {
                   sContent.Append(s);
               }
           }

           return sContent.ToString();

       }
       public static string GetIpAddress()
       {
           string ipAddress;
           try
           {
               HttpRequest currentRequest = HttpContext.Current.Request;
               ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

               if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
                   ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];
           }
           catch (Exception ex)
           {
               ipAddress = "Invalid IP:" + ex.Message;
           }

           return ipAddress;
       }
    
    }
}
