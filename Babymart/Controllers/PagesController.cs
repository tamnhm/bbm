using Babymart.DAL.Product;
using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Babymart.Controllers
{
    public class PagesController : Controller
    {
        babymart_vnEntities db = new babymart_vnEntities();
        private IProductRepository _productRepository;
        public PagesController()
        {
            this._productRepository = new ProductRepository(new babymart_vnEntities());
        }
        public ActionResult Index(string url)
        {
            var data = _productRepository.Detail(url);
            if (data == null)
            {
                return Redirect("/404.html");
            }
            return View(data);
        }
        public ActionResult Indextd()
        {
            var data = _productRepository.Listtd("TD");
            if (data == null)
            {
                return Redirect("/404.html");
            }
            return View(data);
        }
    }
}
