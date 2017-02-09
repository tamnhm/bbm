using Babymart.DAL.Common;
using Babymart.DAL.Module;
using Babymart.Models;
using Babymart.Models.Module.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Babymart.Models.Module;
using PagedList;
using Babymart.Infractstructure;
using Babymart.DAL.Tags;
using Babymart.Infractstructuree;
using Babymart.DAL.Product;
namespace Babymart.Controllers
{
    public class MagazineController : Controller
    {
        private IModuleRepository _moduleRepository;
        private ICommonRepository _common;
        private ITagsRepository _tags;
        private int typemodule;
        private babymart_vnEntities _context;
        private ITagsRepository _iTagsRepository;

        private IProductRepository _iProductRepository;
        public MagazineController()
        {
            this._moduleRepository = new ModuleRepository(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
            this._tags = new TagsRepository(new babymart_vnEntities());
            this._context = new babymart_vnEntities();
            typemodule = (int)ModuleType.Magazine;
            ViewBag.typemodule = typemodule;
            this._iTagsRepository = new TagsRepository(new babymart_vnEntities());
            this._iProductRepository = new ProductRepository(new babymart_vnEntities());

        }
        public ActionResult Index()
        {
            var data = _moduleRepository.Index(typemodule, UtilsBB.GetStoreId());
            return View(data);
        }
        public ActionResult Article(string urlmenu)
        {
            var data = _moduleRepository.ListArticle(urlmenu, typemodule, UtilsBB.GetStoreId());
            return View(data);
        }
        public ActionResult ArticleChild(string urlgroup, string urlmenu, int? page)
        {
            ViewBag.Currentgroup = urlgroup;
            ViewBag.Currentmenu = urlmenu;

            var data = _moduleRepository.ListArticleChild(urlmenu, urlgroup, typemodule, UtilsBB.GetStoreId()).OrderByDescending(p => p.createdate).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ArticleChild2(string urlgroup, string urlmenu)
        {
            var data = _moduleRepository.ListArticleChild(urlmenu, urlgroup, typemodule, UtilsBB.GetStoreId());
            return View(data);
        }

        public ActionResult Detail(string url, int id, string urlgroup)
        {
            var data = _moduleRepository.Article(url, id, urlgroup, typemodule, UtilsBB.GetStoreId()); 
            if (data==null)
            {
                return Redirect("/404.html");
            }
            if (data != null && data.groupid != null)
            {
                ViewData["ArticlesRelated"] = _common.ArticlesRelated(data.groupid, typemodule, 10, UtilsBB.GetStoreId());
                var listTagsRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref((int)Tags.TagsModuleMagazine, id));
                List<string> listTagStr = new List<string>();
                if (listTagsRef != null && listTagsRef.Count > 0)
                {
                    var ids = new List<int>();
                    foreach (var item in listTagsRef)
                    {
                        var listTag = _iTagsRepository.ListTagsRefbyRefTag(item.RefTag.Value);
                        foreach (var item2 in listTag)
                        {
                            ids.Add(item2.RefId.Value);
                            listTagStr.Add(item2.sys_tags_Summary.Tags);
                        }
                    }
                    listTagStr = listTagStr.Distinct().ToList();
                    ViewBag.TagsList = listTagStr;
                    int[] listIdsProduct = TagsCommon.listIdArticlebyTag(id, (int)TagsCollection.TagsProduct_Magazine, (int)Tags.TagsProduct);
                    var listproduct = _iProductRepository.GetlistProduct(listIdsProduct);
                    ViewData["ListProduct"] = Mapper.Map<List<ModelProduct>>(listproduct);
                    ViewData["ListTags"] = _moduleRepository.GetListArticlebyIds(ids.ToArray());
                    ViewData["ArticlesTags"] = _moduleRepository.GetListArticlebyIds(ids.ToArray());
                }

            }
            return View(data);
        }

        public ActionResult ListTag(string tag)
        {
            ViewBag.Tags = tag;
            var listIdPro = TagsCommon.ListRefIdTagBy((int)Tags.TagsModuleMagazine, tag.Trim());
            if (listIdPro != null && listIdPro.Count > 0)
            {
                var pro = _moduleRepository.GetListArticlebyIds(listIdPro.ToArray());
                if (pro != null && pro.Count > 0)
                {
                    return View(pro);
                }
                return View();
            }
            return View();
        }

    }
}
