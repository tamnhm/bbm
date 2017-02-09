using Babymart.DAL.Module;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Babymart.Infractstructure;
using Babymart.DAL.Tags;
using Babymart.Models.Module.Enum;
using Babymart.Infractstructuree;
namespace Babymart.Areas.Admin.Controllers
{
    public class ModuleController : Controller
    {
        babymart_vnEntities db = new babymart_vnEntities();
        private IModuleRepository _iModuleRepository;
        private ITagsRepository _iTagsRepository;
        //
        // GET: /Admin/Module/
        public ModuleController()
        {
            this._iModuleRepository = new ModuleRepository(new babymart_vnEntities());
            this._iTagsRepository = new TagsRepository(new babymart_vnEntities());

        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetArticle(int id)
        {
            var model = Mapper.Map<ModelModuleDetail>(_iModuleRepository.GetArticle(id, UtilsBB.GetStoreId()));
            var tags = new List<sys_tags_SummaryModel>();
            var tagsRef = new List<sys_tags_RefModel>();
            tagsRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref((int)Tags.TagsModuleMagazine, id));
            tags = TagsCommon.Gettags_Summary((int)Tags.TagsModuleMagazine);
            if (tagsRef.Count > 0 && tags.Count > 0)
            {
                foreach (var item in tagsRef)
                {
                    var obj = tags.Where(o => o.Id == item.RefTag).FirstOrDefault();
                    if (obj != null)
                        item.Tags = obj.Tags;
                }
            }
            tagsRef = tagsRef.OrderBy(o => o.Tags).ToList();
            tags = tags.OrderBy(o => o.Tags).ToList();

            var tagsProductRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref_RroductAmazing((int)TagsCollection.TagsProduct_Magazine, id));
            var tagsProduct = Mapper.Map<List<sys_tags_SummaryModel>>(_iTagsRepository.ListTagsSummary((int)Tags.TagsProduct));
            if (tagsProductRef.Count > 0 && tagsProduct.Count > 0)
            {
                foreach (var item in tagsProductRef)
                {
                    var obj = tagsProduct.Where(o => o.Id == item.RefTag).FirstOrDefault();
                    if (obj != null)
                        item.Tags = obj.Tags;
                }
            }




            return Json(new
            {
                Data = model,
                ListTag = Mapper.Map<List<sys_tags_SummaryModel>>(tags.OrderBy(o => o.Tags).ToList()),
                Tag = tagsRef.OrderBy(o => o.Tags).ToList(),
                TagsProductRef = tagsProductRef.OrderBy(o => o.Tags).ToList(),
                TagsProduct = tagsProduct.OrderBy(o => o.Tags).ToList(),
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Getlist(int groupid)
        {
            var result = Mapper.Map<List<ModelModuleDetail>>(_iModuleRepository.ListModuleArticle(groupid, UtilsBB.GetStoreId()));
            var tagsRef = new List<sys_tags_RefModel>();
            foreach (var item in result)
            {
                var listTag = new List<string>();
                tagsRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref((int)Tags.TagsModuleMagazine, item.id));
                if (tagsRef != null && tagsRef.Count > 0)
                {
                    foreach (var obj in tagsRef)
                    {
                        var tmpTag = _iTagsRepository.GetbyIdtags_Summary(obj.RefTag.Value);
                        if (tmpTag != null)
                            listTag.Add(tmpTag.Tags);
                    }
                }
                listTag = listTag.OrderBy(o => o).ToList();
                item.Tags = listTag.ToArray();
            }
            return UtilsBB.JsonMaxValue(result);
        }
        [HttpPost]
        public ActionResult InsertArticle(ModelModuleDetail model)
        {
            var Messaging = new RenderMessaging();

            try
            {
                model.createdate = DateTime.Now;
                var newdata = Mapper.Map<module_detail>(model);
                _iModuleRepository.InsertArticle(newdata);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
                Messaging.id = newdata.id;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InvisibleArticle(int id)
        {
            var Messaging = new RenderMessaging();
            var data = Mapper.Map<ModelModuleDetail>(db.module_detail.Find(id));
            if (data.hide == true)
                data.hide = false;
            else data.hide = true;
            try
            {
                _iModuleRepository.UpdateArticle(Mapper.Map<module_detail>(data));
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateArticle(ModelModuleDetail model)
        {


            var Messaging = new RenderMessaging();
            try
            {
                if (model.Tags != null)
                    TagsCommon.ProcessTags(model.Tags, model.id, (int)Tags.TagsModuleMagazine);
                if (model.TagsProduct != null)
                    TagsCommon.ProcessTags(model.TagsProduct, model.id, (int)Tags.TagsProduct, (int)TagsCollection.TagsProduct_Magazine);
                _iModuleRepository.UpdateArticle(Mapper.Map<module_detail>(model));
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
                Messaging.id = model.id;
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteArticle(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _iModuleRepository.DeleteArticle(id);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }

            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }

        /**********************************/
        [HttpPost]
        public ActionResult ListGroup(int idmenu)
        {
            var data = Mapper.Map<List<ModelModuleGroup>>(_iModuleRepository.ListGroup(idmenu, UtilsBB.GetStoreId()));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGroup(List<ModelModuleGroup> model)
        {
            var Messaging = new RenderMessaging();
            foreach (var i in model)
            {
                if (String.IsNullOrEmpty(i.title) || String.IsNullOrEmpty(i.url))
                {
                    Messaging.success = false;
                    Messaging.messaging = "Dữ liệu không hợp lệ!";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                var list = _iModuleRepository.ListGroup(0, UtilsBB.GetStoreId());
                foreach (var item in model)
                {
                    var index = list.FindIndex(p => p.id == item.id);
                    if (index >= 0)
                    {
                        _iModuleRepository.UpdateGroup(Mapper.Map<module_group>(item));
                    }
                    else
                    {
                        _iModuleRepository.InsertGroup(Mapper.Map<module_group>(item));
                    }

                }
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveTag(int IdTags)
        {
            try
            {
                var result = TagsCommon.RemoveTag(IdTags);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteGroup(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _iModuleRepository.DeleteGroup(id);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }

            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }

        /*************************************************/
        [HttpPost]
        public ActionResult ListMenu(int typemodule = 0)
        {
            var data = _iModuleRepository.ListMenu(typemodule, UtilsBB.GetStoreId());

            var result = Mapper.Map<List<ModelModuleMenuAd>>(data);


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateMenu(List<ModelModuleMenu> model)
        {
            var Messaging = new RenderMessaging();
            foreach (var i in model)
            {
                if (String.IsNullOrEmpty(i.tenloai) || String.IsNullOrEmpty(i.url))
                {
                    Messaging.success = false;
                    Messaging.messaging = "Dữ liệu không hợp lệ!";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                var list = _iModuleRepository.ListMenu(0, UtilsBB.GetStoreId());
                foreach (var item in model)
                {
                    var index = list.FindIndex(p => p.id == item.id);
                    if (index >= 0)
                    {
                        _iModuleRepository.UpdateMenu(Mapper.Map<module_menu>(item));
                    }
                    else
                    {
                        var group = new module_group();
                        group.title = "default";
                        group.url = "default";
                        group.idmenu = _iModuleRepository.InsertMenu(Mapper.Map<module_menu>(item));
                        _iModuleRepository.InsertGroup(group);
                    }

                }
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteMenu(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _iModuleRepository.DeleteMenu(id);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công  ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Search_query(string query)
        {
            var db = _iModuleRepository.Search_query(query);
            var result = Mapper.Map<List<ModelModuleDetail>>(db);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
