using Babymart.DAL.Product;
using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.WebPages;
namespace Babymart
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigLogging();
            try
            {
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new Microsoft.Web.Mvc.FixedRazorViewEngine()); 
                AreaRegistration.RegisterAllAreas();
                var dislayModels = DisplayModeProvider.Instance.Modes;
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                BundleMobileConfig.RegisterBundles(BundleTable.Bundles); 
                AuthConfig.RegisterAuth();
                GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("")
                {
                    ContextCondition = (ctx =>
                    ctx.Request.UserAgent.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ctx.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    ctx.Request.UserAgent.IndexOf("mobile", StringComparison.OrdinalIgnoreCase) < 1)
                });

                DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Mobile")
                {
                    ContextCondition = (ctx =>
                        ctx.GetOverriddenBrowser().IsMobileDevice)
                });
            }
            catch
            {
            }

        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (babymart_vnEntities entities = new babymart_vnEntities())
                        {
                            admin user = entities.admins.SingleOrDefault(u => u.username == username);

                            roles = user.Roles;
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }
        public void ConfigLogging()
        {
            string sLogFile = HttpRuntime.AppDomainAppPath + "log4net.config";//Intergrated mode
            //string sLogFile = HttpContext.Current.Server.MapPath("~/")+ "log4net.config"; //iis classic mode
            if ((System.IO.File.Exists(sLogFile)))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(sLogFile));
            }
        }
        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode == 404)
            {
                if ((!Request.RawUrl.Contains("style")) && (!Request.RawUrl.Contains("images")))
                {
                    Response.Clear();
                    if (Response.StatusCode == 404)
                    {
                        Response.Redirect("/404.html");
                    }
                }
            }
        } 
    }
}