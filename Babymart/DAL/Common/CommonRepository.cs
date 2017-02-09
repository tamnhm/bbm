using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Babymart.Models;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using Babymart.DAL.Administrator;
namespace Babymart.DAL.Common
{
    public class CommonRepository : ICommonRepository
    {
        private babymart_vnEntities _context;
        public CommonRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }

        public List<shop_rightcol> ColumnRight(int StoreId = 1000)
        {
            return (_context.shop_rightcol.Where(o => o.StoreId == StoreId).ToList());
        }

        public List<module_menu> MenuModule(int type, int StoreId = 1000)
        {
            return (_context.module_menu.Where(p => p.typemodule == type && p.module_detail.Count > 0 && p.StoreId == StoreId).ToList());
        }
        public List<module_group> MenuModuleGroup(int idmenu, int StoreId = 1000)
        {
            return (_context.module_group.Where(p => p.idmenu == idmenu && p.module_detail.Count > 0 && p.StoreId == StoreId).ToList());
        }
        public List<module_detail> ArticlesRelated(int? idmenu, int typlemodule, int numberget, int StoreId = 1000)
        {
            var q = from _module in _context.module_detail
                    where _module.groupid == idmenu && _module.typlemodule == typlemodule
                     && _module.StoreId == StoreId
                    select _module;
            if (numberget != 0)
                return q.OrderByDescending(p=>p.createdate).Take(numberget).ToList();
            return q.ToList();
        }
        public string SplitString(string s, int length)
        {
            if (s != null && s.Length >= length)
            {

                s = Regex.Replace(s, "<.*?>", String.Empty);
                //s = HttpUtility.HtmlDecode(s);
                s = Regex.Replace(s, "&nbsp;", string.Empty);
                if (String.IsNullOrEmpty(s))
                    throw new ArgumentNullException(s);
                var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words[0].Length > length)
                    throw new ArgumentException("Từ đầu tiên dài hơn chuỗi cần cắt");
                var sb = new StringBuilder();
                foreach (var word in words)
                {
                    if ((sb + word).Length > length)
                        return string.Format("{0}...", sb.ToString().TrimEnd(' '));
                    sb.Append(word + " ");
                }
                return string.Format("{0}...", sb.ToString().TrimEnd(' '));

            }
            return s;

        }
        public string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        public void SendEmail(string address, string subject, string message)
        {
            var administrator = _context.sys_account_admin.Find(1);
            if (administrator != null)
            {
                var email = administrator.tendn;
                var loginInfo = new NetworkCredential(email, administrator.pass);

                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

            }

        }

        public string RandomString(int length)
        {
            Random _random = new Random(Environment.TickCount);
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}