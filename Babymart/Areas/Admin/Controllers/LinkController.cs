using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL;
using Babymart.DAL.Link;
using Babymart.Models;
using AutoMapper;
using Babymart.Models.Module;
using Babymart.Infractstructure;
using Babymart.DAL.Tags;
using Babymart.Models.Module.Enum;
namespace Babymart.Areas.Admin.Controllers
{
    public class LinkController : Controller
    {
        //
        // GET: /Admin/Link/
        private ILinkRepository _linkRepository;
        private ITagsRepository _iTagsRepository;
        babymart_vnEntities db = new babymart_vnEntities();
        public LinkController()
        {
            this._linkRepository = new LinkRepository(new babymart_vnEntities());
            this._iTagsRepository = new TagsRepository(new babymart_vnEntities());
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();

        }

        public JsonResult Getdanhmuc()
        {
            var result = Mapper.Map<List<ModelDanhmuc>>(_linkRepository.GetdanhmucgforAd(UtilsBB.GetStoreId()));
            var tagArticleSummary = _iTagsRepository.ListTagsSummary((int)Tags.TagsModuleMagazine);
            tagArticleSummary = tagArticleSummary.OrderBy(o => o.Tags).ToList();
            foreach (var item in result)
            {
                var tagRefArticle = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTagsRefArticle(item.madanhmuc, (int)Tags.TagsProduct, (int)TagsCollection.TagsListCategories));
                foreach (var objRef in tagRefArticle)
                {
                    var obj = tagArticleSummary.Where(o => o.Id == objRef.RefTag).FirstOrDefault();
                    if (obj != null)
                        objRef.Tags = obj.Tags;
                }
                item.ListTagsArticle = Mapper.Map<List<sys_tags_RefModel>>(tagRefArticle);

                foreach (var itemLinkcon in item.shop_danhmuccon)
                {
                    var tagRefArticleLinkcon = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTagsRefArticle(itemLinkcon.madanhmuccon, (int)Tags.TagsProduct, (int)TagsCollection.TagsCategories));
                    foreach (var objRef in tagRefArticleLinkcon)
                    {
                        var obj = tagArticleSummary.Where(o => o.Id == objRef.RefTag).FirstOrDefault();
                        if (obj != null)
                            objRef.Tags = obj.Tags;
                    }
                    tagRefArticleLinkcon = tagRefArticleLinkcon.OrderBy(o => o.Tags).ToList();
                    itemLinkcon.ListTagsArticle = Mapper.Map<List<sys_tags_RefModel>>(tagRefArticleLinkcon);
                }

            }
            return Json(new
            {
                Data = result,
                TagArticle = Mapper.Map<List<sys_tags_SummaryModel>>(tagArticleSummary),
            }, JsonRequestBehavior.AllowGet);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getdanhmuccon(int id)
        {
            var link = _linkRepository.GetdanhmucconAll(id, UtilsBB.GetStoreId());
            List<ModelDanhMucCon> danhmuccon = new List<ModelDanhMucCon>();
            foreach (var item in link)
            {
                var obj = new ModelDanhMucCon();
                obj.tendanhmuccon = item.tendanhmuccon;
                obj.madanhmuccon = item.madanhmuccon;
                danhmuccon.Add(obj);
            }

            return Json(danhmuccon, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult insertLink(shop_danhmuc dm)
        {
            var Messaging = new RenderMessaging();
            try
            {
                _linkRepository.InsertLink(dm);
                Messaging.messaging = "Thêm danh mục " + dm.tendanhmuc + " thành công";
                Messaging.success = true;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult updateLink(shop_danhmuc dm, List<sys_tags_RefModel> tagRef)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var tagRefArticleHas = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTagsRefArticle(dm.madanhmuc, (int)Tags.TagsProduct, (int)TagsCollection.TagsListCategories));
                if (tagRef != null && tagRef.Count > 0)
                {
                    var listTagRef = new List<sys_tags_Ref>();
                    foreach (var obj in tagRef)
                    {
                        var hasRef = tagRefArticleHas.Where(o => o.Id == obj.Id).FirstOrDefault();
                        if (hasRef == null)
                        {
                            var modelTagRef = new sys_tags_Ref
                            {
                                RefId = dm.madanhmuc,
                                RefTag = obj.RefTag,
                                Type = (int)Tags.TagsProduct,
                                TypeCollection = (int)TagsCollection.TagsListCategories
                            };
                            listTagRef.Add(modelTagRef);
                        }
                    }
                    if (listTagRef.Count > 0)
                        _iTagsRepository.AddListagsRef(listTagRef);
                }
                _linkRepository.UpdateLink(dm);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật " + dm.tendanhmuc + " thành công !";

            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult removeLink(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _linkRepository.RemoveLink(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa danh mục thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Lỗi : Danh mục này đã tồn tại link!";
            }

            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult insertLinkCon(shop_danhmuccon dm)
        {
            var Messaging = new RenderMessaging();

            try
            {

                _linkRepository.InsertLinkCon(dm);
                Messaging.success = true;
                Messaging.messaging = "Thêm link " + dm.tendanhmuccon + " thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult updateLinkCon(shop_danhmuccon dm, List<sys_tags_RefModel> tagRef)
        {
            var Messaging = new RenderMessaging();
            try
            {
                var tagRefArticleHas = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTagsRefArticle(dm.madanhmuccon, (int)Tags.TagsProduct, (int)TagsCollection.TagsCategories));
                if (tagRef != null && tagRef.Count > 0)
                {
                    var listTagRef = new List<sys_tags_Ref>();
                    foreach (var obj in tagRef)
                    {
                        var hasRef = tagRefArticleHas.Where(o => o.Id == obj.Id).FirstOrDefault();
                        if (hasRef == null)
                        {
                            var modelTagRef = new sys_tags_Ref
                            {
                                RefId = dm.madanhmuccon,
                                RefTag = obj.RefTag,
                                Type = (int)Tags.TagsProduct,
                                TypeCollection = (int)TagsCollection.TagsCategories
                            };
                            listTagRef.Add(modelTagRef);
                        }
                    }
                    if (listTagRef.Count > 0)
                        _iTagsRepository.AddListagsRef(listTagRef);
                }
                _linkRepository.UpdateLinkCon(dm);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật link " + dm.tendanhmuccon + " thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveLinkCon(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {

                _linkRepository.RemoveLinkCon(id);
                Messaging.success = true;
                Messaging.messaging = "Xóa link thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Lỗi : Link này còn tồn tại sản phẩm";
            }

            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }


    }
}
