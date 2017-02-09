using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.Models;
namespace Babymart.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/
         [Authorize]
        public ActionResult Index()
        {

            return RedirectToAction("Index", "Product");
        }

    }
}
