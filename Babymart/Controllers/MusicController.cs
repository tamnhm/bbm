using Babymart.DAL.Common;
using Babymart.DAL.Module;
using Babymart.Models;
using Babymart.Models.Module.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Babymart.Infractstructure;
namespace Babymart.Controllers
{
    public class MusicController : Controller
    {
        private IModuleRepository _moduleRepository;
        private ICommonRepository _common;
        private int typemodule;
        public MusicController()
        {
            this._moduleRepository = new ModuleRepository(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
            typemodule = (int)ModuleType.Music;
            ViewBag.typemodule = typemodule;

        }
        public ActionResult Index()
        {
            var data = _moduleRepository.Index(typemodule, UtilsBB.GetStoreId());
            return View(data);
        }
       
        public ActionResult ArticleChild(string urlgroup, string urlmenu, int? page)
        {
            ViewBag.Currentgroup = urlgroup;
            ViewBag.Currentmenu = urlmenu;

            var data = _moduleRepository.ListArticleChild(urlmenu, urlgroup, typemodule, UtilsBB.GetStoreId());

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));

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
