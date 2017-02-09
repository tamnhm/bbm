using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Babymart.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Admin/Account/
        babymart_vnEntities entities = new babymart_vnEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(ModelAcc model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var adminStrator = IsValidAccount(model);
                if (adminStrator != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    this.Session["AdminStrator"] = adminStrator;
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                     && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }
        public admin IsValidAccount(ModelAcc model)
        {
            try
            {
                var admin = entities.admins.First(user => user.username == model.UserName);
                if (admin != null)
                {
                    if (admin.password == model.Password)
                    {
                        return admin;
                    }
                }
            }
            catch
            {
                return null;

            }

            return null;
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

    }
}
