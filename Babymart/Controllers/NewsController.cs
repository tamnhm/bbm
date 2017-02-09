using Babymart.DAL.Common;
using Babymart.DAL.Module;
using Babymart.Infractstructure;
using Babymart.Models;
using Babymart.Models.Module.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Babymart.Controllers
{
    public class NewsController : Controller
    {
        private IModuleRepository _moduleRepository;
        private ICommonRepository _common;
        private int typemodule;
        public NewsController()
        {
            this._moduleRepository = new ModuleRepository(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
            typemodule = (int)ModuleType.News;
            ViewBag.typemodule = typemodule;

        }
        public ActionResult Index()
        {
            var data = _moduleRepository.Index(typemodule, UtilsBB.GetStoreId());
            foreach (var item in data)
            {
                foreach (var item2 in item.module_detail)
                {

                    if (item.module_detail.First() == item2)
                        item2.extract = _common.SplitString(item2.extract, 400);
                    else
                        item2.extract = _common.SplitString(item2.extract, 100);
                }
            }
            return View(data);
        }
        public ActionResult ArticleChild(string urlgroup, string urlmenu)
        {
            var data = _moduleRepository.ListArticleChild(urlmenu, urlgroup, typemodule, UtilsBB.GetStoreId());
            return View(data);
        }
        public ActionResult Article(string urlmenu)
        {
            var data = _moduleRepository.ListArticle(urlmenu, typemodule, UtilsBB.GetStoreId());
            return View(data);
        }
        public ActionResult Detail(string url, int id, string urlgroup)
        {
            var data = _moduleRepository.Article(url, id, urlgroup, typemodule, UtilsBB.GetStoreId());
            ViewData["ArticlesRelated"] = _common.ArticlesRelated(data.groupid, typemodule, 10, UtilsBB.GetStoreId());
            return View(data);
        }

    }
}
