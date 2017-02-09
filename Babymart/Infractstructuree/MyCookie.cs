using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Infractstructuree
{
    public class MyCookie
    {
        public static string CookieName { get; set; }
        public MyCookie(string Name)
        {
            CookieName = Name;
        }
        public void SetCookieForLang(string nameLang)
        {
            var cookieHasit = this.GetCookie();
            if (cookieHasit != null)
            {
                if (!cookieHasit.Value.Equals(nameLang))
                {
                    this.CreateCookie(nameLang);
                }
            }
            else
            {
                this.CreateCookie(nameLang);
            }

           
        }
        public void CreateCookie(string value)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["CookieLang"] ?? new HttpCookie("CookieLang");
            myCookie.Value = value;
            myCookie.Expires = DateTime.Now.AddDays(365);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        public HttpCookie GetCookie()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (myCookie != null)
            {
                return myCookie;
            }
            return null;
        }
        public void RemoveGetHttpCookieLang()
        {
            var cookie = GetCookie();
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
    }
}