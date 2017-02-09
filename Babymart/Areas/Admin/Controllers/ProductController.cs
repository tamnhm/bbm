using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Babymart.DAL;
using Babymart.Models;
using System.Web.Script.Serialization;
using Babymart.DAL.Product;
using Babymart.DAL.Photo;
using Babymart.Models.Module.Enum;
using Babymart.Infractstructure;
using Babymart.DAL.Collection;
using AutoMapper;
using System.Data;
using Babymart.Models.Module;
using Babymart.Infractstructuree;
using Babymart.DAL.Tags;
namespace Babymart.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Admin/Product/
        babymart_vnEntities db = new babymart_vnEntities();
        private IProductRepository _productRepository;
        private ICollection _ICollection;
        private IPhotoRepository _imageRepository;
        private ITagsRepository _iTagsRepository;
        public ProductController()
        {
            this._productRepository = new ProductRepository(new babymart_vnEntities());
            this._imageRepository = new PhotoRepository(new babymart_vnEntities());
            this._ICollection = new Collection(new babymart_vnEntities());
            this._iTagsRepository = new TagsRepository(new babymart_vnEntities());
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List(int madanhmuc, int madanhmuccon, int thuonghieu, int site)
        {
            var db = _productRepository.GetListSP(madanhmuc, madanhmuccon, thuonghieu, site, UtilsBB.GetStoreId());
            var result = Mapper.Map<List<ModelCollection>>(db);
            var tagsRef = new List<sys_tags_RefModel>();
            foreach (var item in result)
            {
                var listTag = new List<string>();
                tagsRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref((int)Tags.TagsProduct, item.shop_sanpham.id));
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
                item.shop_sanpham.Tag = listTag.ToArray();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Detail(int id)
        {
            var product = _productRepository.GetProductById(id, UtilsBB.GetStoreId());
            var result = Mapper.Map<ModelProduct>(product);
            var tags = new List<sys_tags_SummaryModel>();
            var tagsRef = new List<sys_tags_RefModel>();

            tagsRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref((int)Tags.TagsProduct, id));
            tags = Mapper.Map<List<sys_tags_SummaryModel>>(_iTagsRepository.ListTagsSummary((int)Tags.TagsProduct));
            if (tagsRef.Count > 0 && tags.Count > 0)
            {
                foreach (var item in tagsRef)
                {
                    var obj = tags.Where(o => o.Id == item.RefTag).FirstOrDefault();
                    if (obj != null)
                        item.Tags = obj.Tags;
                }
            }
            var tagArticleSummary = Mapper.Map<List<sys_tags_SummaryModel>>(_iTagsRepository.ListTagsSummary((int)Tags.TagsModuleMagazine));
            var tagRefArticle = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTagsRefArticle(id, (int)Tags.TagsProduct, (int)TagsCollection.TagsProduct));
            foreach (var objRef in tagRefArticle)
            {
                var obj = tagArticleSummary.Where(o => o.Id == objRef.RefTag).FirstOrDefault();
                if (obj != null)
                    objRef.Tags = obj.Tags;
            }
            tagRefArticle = tagRefArticle.OrderBy(o => o.Tags).ToList();
            tagArticleSummary = tagArticleSummary.OrderBy(o => o.Tags).ToList();
            tags = tags.OrderBy(o => o.Tags).ToList();
            tagsRef = tagsRef.OrderBy(p => p.Tags).ToList();
            result.ListTagsArticle = Mapper.Map<List<sys_tags_RefModel>>(tagRefArticle);
            return Json(new
            {
                Data = result,
                ListTag = tags,
                Tag = tagsRef,
                TagArticle = tagArticleSummary,
            }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult AddNew(ModelProduct sp, shop_collection l)
        {
            var Messaging = new RenderMessaging();
            var sanpham = Mapper.Map<shop_sanpham>(sp);
            try
            {

                var check = _productRepository.CheckingByUrl(sp.spurl, sp.id);
                if (check == true)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Url này đã tồn tại !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {

                    _productRepository.InsertProduct(sanpham);
                    l.idsp = sanpham.id;
                    _ICollection.Add(l);
                    if (sanpham.shop_image != null)
                    {
                        foreach (var i in sanpham.shop_image)
                        {
                            i.RefId = sanpham.id;
                            _imageRepository.UpdateImh(i);
                        }
                    }
                    Messaging.success = true;
                    Messaging.id = sanpham.id;
                    Messaging.messaging = "Thêm sản phẩm thành công";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Messaging.success = false;
                    Messaging.messaging = "Dữ liệu nhập không đúng !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }


            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
                return Json(Messaging, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult Update(shop_sanpham sp, string[] tags, List<sys_tags_RefModel> tagRef)
        {
            var Messaging = new RenderMessaging();
            var item = db.shop_sanpham.Find(sp.id);

            List<ModelBienthe> bienthe = new List<ModelBienthe>();
            bienthe = Mapper.Map<List<ModelBienthe>>(item.shop_bienthe);

            var propeties = Mapper.Map<ModelPropetiesProduct>(sp);
            var result = Mapper.Map<shop_sanpham>(propeties);

            foreach (var i in sp.shop_bienthe)
            {
                if (String.IsNullOrEmpty(i.title) || i.gia == null || i.gia.GetType() == typeof(int?) || i.giasosanh.GetType() == typeof(int?))
                {
                    Messaging.success = false;
                    Messaging.messaging = "Tiêu đề hoặc giá không đúng !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }
            }
            var check = _productRepository.CheckingByUrl(sp.spurl, sp.id);
            if (check == true)
            {
                Messaging.success = false;
                Messaging.messaging = "Url này đã tồn tại !";
                return Json(Messaging, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (tags != null)
                    {
                        TagsCommon.ProcessTags(tags, sp.id, (int)Tags.TagsProduct);
                    }
                    var tagRefArticleHas = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTagsRefArticle(sp.id, (int)Tags.TagsProduct, (int)TagsCollection.TagsProduct));
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
                                    RefId = sp.id,
                                    RefTag = obj.RefTag,
                                    Type = (int)Tags.TagsProduct,
                                    TypeCollection = (int)TagsCollection.TagsProduct
                                };
                                listTagRef.Add(modelTagRef);
                            }
                        }
                        if (listTagRef.Count > 0)
                            _iTagsRepository.AddListagsRef(listTagRef);
                    }
                    _productRepository.UpdateProduct(result);
                    foreach (var i in sp.shop_bienthe)
                    {
                        var index = bienthe.FindIndex(p => p.id == i.id);
                        if (index < 0)
                        {
                            _productRepository.InsertBienthe(i);
                        }
                        else if (index >= 0)
                        {
                            _productRepository.UpdateBienthe(i);
                        }
                    }
                    Messaging.id = sp.id;
                    Messaging.success = true;
                    Messaging.messaging = "Cập nhật sản phẩm thành công";
                }
                else
                {
                    Messaging.success = false;
                    Messaging.messaging = "Dữ liệu nhập không đúng !";
                }
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult SetShowHome(int id)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _productRepository.SetShowHome(id);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetVisible(int id, bool visible)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _productRepository.SetVisible(id, visible);
                Messaging.success = true;
                Messaging.messaging = "Chuyển sang ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);



        }
        public JsonResult SetOut(int id, bool setout)
        {
            var Messaging = new RenderMessaging();

            try
            {
                _productRepository.Outset(id, setout);
                Messaging.success = true;
                Messaging.messaging = "Chuyển sang ";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Remove(int id)
        {

            var Messaging = new RenderMessaging();

            try
            {
                _productRepository.Remove(id);
                Messaging.success = true;
                Messaging.messaging = "Thêm sản phẩm thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult RemoveBienthe(int id)
        {
            var Messaging = new RenderMessaging();
            var bienthe = db.shop_bienthe.Find(id);
            if (bienthe == null)
            {
                Messaging.success = false;
                Messaging.messaging = "Có lỗi xảy ra, vui lòng thử lại!";
                return Json(Messaging, JsonRequestBehavior.AllowGet);
            }
            List<shop_bienthe> listbienthe = _productRepository.listBienthe(bienthe.idsp);
            if (listbienthe.Count == 1)
            {
                Messaging.success = false;
                Messaging.messaging = "Chỉ có 1 biến thể, không xóa được !";
            }

            else
            {
                try
                {
                    _productRepository.RemoveBienthe(id);
                    var list = _productRepository.listBienthe(bienthe.idsp);
                    if (list.Count == 1)
                    {
                        var tmp = list.FirstOrDefault();
                        tmp.title = "default";
                        _productRepository.UpdateBienthe(tmp);
                    }

                    Messaging.success = true;
                    Messaging.messaging = "Cập nhật khuyến mãi thafnhc công !";
                }
                catch
                {
                    Messaging.success = false;
                    Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
                }
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Saleoff(ModelProduct sp)
        {
            var item = db.shop_sanpham.Find(sp.id);
            var Messaging = new RenderMessaging();
            var product = new shop_sanpham();
            List<ModelBienthe> bienthe = new List<ModelBienthe>();
            product = Mapper.Map<shop_sanpham>(sp);
            bienthe = Mapper.Map<List<ModelBienthe>>(item.shop_bienthe);
            try
            {
                foreach (var i in sp.shop_bienthe)
                {
                    var index = bienthe.FindIndex(p => p.id == i.id);
                    if (index < 0)
                    {
                        if (String.IsNullOrEmpty(i.title) || i.gia == null)
                        {
                            Messaging.success = false;
                            Messaging.messaging = "Tiêu đề hoặc giá không đúng !";
                            return Json(Messaging, JsonRequestBehavior.AllowGet);
                        }
                        _productRepository.InsertBienthe(Mapper.Map<shop_bienthe>(i));
                    }
                    else if (index >= 0)
                    {
                        if (String.IsNullOrEmpty(i.title) || i.gia == null)
                        {
                            Messaging.success = false;
                            Messaging.messaging = "Tiêu đề hoặc giá không đúng !";
                            return Json(Messaging, JsonRequestBehavior.AllowGet);
                        }
                        _productRepository.UpdateBienthe(Mapper.Map<shop_bienthe>(i));
                    }
                }
                _productRepository.Saleoff(product);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật khuyến mãi thafnhc công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ResetSalloff()
        {
            var Messaging = new RenderMessaging();
            try
            {
                List<shop_bienthe> listBt = db.shop_bienthe.ToList();
                foreach (var item in listBt)
                {
                    if (item.giasosanh > 0)
                    {
                        item.gia = item.giasosanh;
                        item.giasosanh = 0;
                        item.shop_sanpham.ischecksaleoff = false;
                        item.shop_sanpham.ischeckgift = false;
                        item.shop_sanpham.gift = string.Empty;
                        item.shop_sanpham.timeend = null;
                        db.Entry(item).State = EntityState.Modified;
                    }

                }
                db.SaveChanges();
                Messaging.success = true;
                Messaging.messaging = "Đã xóa hết khuyến mãi sản phẩm !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            var Messaging = new RenderMessaging();
            try
            {
                //GetProductById
                //Remove
                // RemoveImg
                // RemoveBienthe
                //_productRepository.RemoveImg()
                var product = db.shop_sanpham.Find(id);
                if (product == null)
                {
                    Messaging.success = false;
                    Messaging.messaging = "Sản phẩm không tồn tại !";
                    return Json(Messaging, JsonRequestBehavior.AllowGet);
                }

                var listbienthe = db.shop_bienthe.Where(o => o.idsp == id).ToList();
                var listImg = db.shop_image.Where(o => o.RefId == id).ToList();
                var listCollection = db.shop_collection.Where(o => o.idsp == id).ToList();
                foreach (var item in listbienthe)
                {
                    db.shop_bienthe.Remove(item);
                }
                foreach (var item in listImg)
                {
                    db.shop_image.Remove(item);
                }
                foreach (var item in listCollection)
                {
                    db.shop_collection.Remove(item);
                }
                db.shop_sanpham.Remove(product);
                db.SaveChanges();
                Messaging.success = true;
                Messaging.messaging = "Đã xóa sản phẩm !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ResetProduct_Banner()
        {
            var Messaging = new RenderMessaging();
            try
            {
                var listBt = db.shop_sanpham.ToList();
                foreach (var item in listBt)
                {
                    if (item.showbanner == true)
                        item.showbanner = false;
                }
                db.SaveChanges();
                Messaging.success = true;
                Messaging.messaging = "Đã đưa sản phẩm ra khỏi banner !";
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
        public ActionResult Search_query(string query)
        {
            var db = _productRepository.Search_query(query);
            var result = Mapper.Map<List<ModelCollection>>(db);
            var tagsRef = new List<sys_tags_RefModel>();
            foreach (var item in result)
            {
                var listTag = new List<string>();
                tagsRef = Mapper.Map<List<sys_tags_RefModel>>(_iTagsRepository.ListTags_ref((int)Tags.TagsProduct, item.shop_sanpham.id));
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
                item.shop_sanpham.Tag = listTag.ToArray();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateAllSaleOffForMember(bool ischecksaleoff)
        {
            try
            {
                var result = db.shop_sanpham.ToList();
                foreach (var item in result)
                {
                    var ischeck = false;
                    foreach (var bienthe in item.shop_bienthe)
                    {
                        if (bienthe.giasosanh > 0)
                        {
                            ischeck = true;
                        }
                    }
                    if (ischeck == true)
                        item.ischecksaleoff = ischecksaleoff;
                }
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}