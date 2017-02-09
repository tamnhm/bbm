using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Babymart.Models;
using Babymart.DAL.Product;
using Babymart.DAL.Brand;
using Babymart.Models.Module;
using AutoMapper;
using Babymart.DAL.Page;
using Babymart.DAL.Common;
using Babymart.DAL.Customer;
using Babymart.Models.Module.Enum;
using Babymart.Infractstructure;
using Babymart.Infractstructuree;
using Babymart.DAL.Tags;
using Babymart.DAL.Module;
namespace Babymart.Controllers
{


    public class ShopController : Controller
    {
        // GET: /Admin/Product/
        babymart_vnEntities db = new babymart_vnEntities();
        private IProductRepository _productRepository;
        private IBrandRepository _brandRepository;
        private IPageRepository _pageRepository;
        private ICommonRepository _common;
        private IModuleRepository _moduleRepository;
        private IBannerRepository _banner;
        private ITagsRepository _rpTags;
        public ShopController()
        {
            this._moduleRepository = new ModuleRepository(new babymart_vnEntities());
            this._productRepository = new ProductRepository(new babymart_vnEntities());
            this._brandRepository = new BrandRepository(new babymart_vnEntities());
            this._pageRepository = new PageRepository(new babymart_vnEntities());
            this._common = new CommonRepository(new babymart_vnEntities());
            this._banner = new BannerRepository(new babymart_vnEntities());
            this._rpTags = new TagsRepository(new babymart_vnEntities());
            _productRepository.ResetSaleoffProduct();
        }
        public ActionResult Index(string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            ViewData["Thuongthieu"] = _brandRepository.TopBrand(16, UtilsBB.GetStoreId());
            ViewData["Banner"] = _banner.ListBanner((int)FilesType.BannerHome);
            ViewData["Spmoi"] = _productRepository.ProductTop("spmoi", 8, UtilsBB.GetStoreId()).ToList();
            ViewData["Spbanchay"] = _productRepository.ProductTop("spbanchay", 8, UtilsBB.GetStoreId());
            ViewData["Spgiamgia"] = _productRepository.ProductTop("spgiamgia", 10, UtilsBB.GetStoreId()).Take(10).ToList(); ;
            ViewData["Spnoibat"] = _productRepository.ProductTop("spnoibat", 0, UtilsBB.GetStoreId());
            ViewData["Spbanner"] = _productRepository.ProductTop("spbanner", 0, UtilsBB.GetStoreId());
            ViewData["Spbimta"] = _productRepository.ProductIndex(12, 8, UtilsBB.GetStoreId());
            ViewData["Dmbimta"] = _productRepository.CollectionIndex(12, UtilsBB.GetStoreId());
            ViewData["bimta"] = _productRepository.CollectionName(12, UtilsBB.GetStoreId());
            ViewData["Spbinhsua"] = _productRepository.ProductIndex(8, 8, UtilsBB.GetStoreId());
            ViewData["Dmbinhsua"] = _productRepository.CollectionIndex(8, UtilsBB.GetStoreId());
            ViewData["binhsua"] = _productRepository.CollectionName(8, UtilsBB.GetStoreId());
            ViewData["Spbekhoe"] = _productRepository.ProductIndex(17, 8, UtilsBB.GetStoreId());
            ViewData["Dmbekhoe"] = _productRepository.CollectionIndex(17, UtilsBB.GetStoreId());
            ViewData["bekhoe"] = _productRepository.CollectionName(17, UtilsBB.GetStoreId());
            ViewData["Spanuong"] = _productRepository.ProductIndex(9, 8, UtilsBB.GetStoreId());
            ViewData["Dmanuong"] = _productRepository.CollectionIndex(9, UtilsBB.GetStoreId());
            ViewData["anuong"] = _productRepository.CollectionName(9, UtilsBB.GetStoreId());
            ViewData["Spbengu"] = _productRepository.ProductIndex(15, 8, UtilsBB.GetStoreId());
            ViewData["Dmbengu"] = _productRepository.CollectionIndex(15, UtilsBB.GetStoreId());
            ViewData["bengu"] = _productRepository.CollectionName(15, UtilsBB.GetStoreId());
            ViewData["Spvuichoi"] = _productRepository.ProductIndex(16, 8, UtilsBB.GetStoreId());
            ViewData["Dmvuichoi"] = _productRepository.CollectionIndex(16, UtilsBB.GetStoreId());
            ViewData["vuichoi"] = _productRepository.CollectionName(16, UtilsBB.GetStoreId());
            ViewData["Spdinhduong"] = _productRepository.ProductIndex(23, 8, UtilsBB.GetStoreId());
            ViewData["Dmdinhduong"] = _productRepository.CollectionIndex(23, UtilsBB.GetStoreId());
            ViewData["dinhduong"] = _productRepository.CollectionName(23, UtilsBB.GetStoreId());
            ViewData["Spchome"] = _productRepository.ProductIndex(20, 8, UtilsBB.GetStoreId());
            ViewData["Dmchome"] = _productRepository.CollectionIndex(20, UtilsBB.GetStoreId());
            ViewData["chome"] = _productRepository.CollectionName(20, UtilsBB.GetStoreId());
            ViewData["Spquanao"] = _productRepository.ProductIndex(13, 8, UtilsBB.GetStoreId());
            ViewData["Dmquanao"] = _productRepository.CollectionIndex(13, UtilsBB.GetStoreId());
            ViewData["quanao"] = _productRepository.CollectionName(13, UtilsBB.GetStoreId());
            ViewData["Spvesinh"] = _productRepository.ProductIndex(25, 8, UtilsBB.GetStoreId());
            ViewData["Dmvesinh"] = _productRepository.CollectionIndex(25, UtilsBB.GetStoreId());
            ViewData["vesinh"] = _productRepository.CollectionName(25, UtilsBB.GetStoreId());
            ViewData["Spxe"] = _productRepository.ProductIndex(19, 8, UtilsBB.GetStoreId());
            ViewData["Dmxe"] = _productRepository.CollectionIndex(19, UtilsBB.GetStoreId());
            ViewData["xe"] = _productRepository.CollectionName(19, UtilsBB.GetStoreId());
            ViewData["Spbimta1"] = _productRepository.ProductIndex(12, 4, UtilsBB.GetStoreId());
            ViewData["Spbinhsua1"] = _productRepository.ProductIndex(8, 4, UtilsBB.GetStoreId());
            ViewData["Spbekhoe1"] = _productRepository.ProductIndex(17, 4, UtilsBB.GetStoreId());
            ViewData["Spanuong1"] = _productRepository.ProductIndex(9, 4, UtilsBB.GetStoreId());
            ViewData["Spbengu1"] = _productRepository.ProductIndex(15, 4, UtilsBB.GetStoreId());
            ViewData["Spvuichoi1"] = _productRepository.ProductIndex(16, 4, UtilsBB.GetStoreId());
            ViewData["Spdinhduong1"] = _productRepository.ProductIndex(23, 4, UtilsBB.GetStoreId());
            ViewData["Spchome1"] = _productRepository.ProductIndex(20, 4, UtilsBB.GetStoreId());
            ViewData["Spquanao1"] = _productRepository.ProductIndex(13, 4, UtilsBB.GetStoreId());
            ViewData["Spvesinh1"] = _productRepository.ProductIndex(25, 4, UtilsBB.GetStoreId());
            ViewData["Spxe1"] = _productRepository.ProductIndex(19, 4, UtilsBB.GetStoreId());
            ViewData["Spgiamgia1"] = _productRepository.ProductTop("spgiamgia", 6, UtilsBB.GetStoreId()).Take(6).ToList(); ;
            TempData["IndexMLoadMore"] = 1;

            var str = ">>Home";
            ViewBag.Tags = str;
            var listId = TagsCommon.ListRefIdTagBy((int)Tags.TagsModuleMagazine, str);
            if (listId != null && listId.Count > 0)
            {
                var listArticalbyTag = _moduleRepository.GetListArticlebyIds(listId.ToArray());
                listArticalbyTag = listArticalbyTag.OrderByDescending(o => o.createdate).Take(2).ToList();
                foreach (var obj in listArticalbyTag)
                {
                    obj.extract = _common.SplitString(obj.extract, 75);
                }
                ViewData["ArticlesTags"] = Mapper.Map<List<ModelModuleDetail>>(listArticalbyTag);
            }
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult CollectionProduct(string urlcollection, string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var pro = _productRepository.GetProByCollectionChild(null, urlcollection, UtilsBB.GetStoreId()).OrderBy(o => o.shop_sanpham.ischeckout).ThenBy(o => o.shop_sanpham.tensp).ThenBy(o => o.shop_sanpham.tensp_us).ToList();
            if (pro != null && pro.Count > 0)
            {
                List<int> listId = new List<int>();
                foreach (var item in pro)
                {
                    if (item.shop_sanpham.shop_thuonghieu != null)
                        listId.Add(item.shop_sanpham.shop_thuonghieu.mahieu);
                }
                var listArticalbyTag = new List<ModelModuleDetail>();
                var madanhmuccon = pro.FirstOrDefault().shop_danhmuccon.madanhmuccon;
                int[] listIds = TagsCommon.listIdArticlebyTag(madanhmuccon, (int)TagsCollection.TagsCategories, (int)Tags.TagsModuleMagazine);
                if (listIds.Length > 0)
                {
                    listArticalbyTag = Mapper.Map<List<ModelModuleDetail>>(_moduleRepository.GetListArticlebyIds(listIds.ToArray()));
                    listArticalbyTag = listArticalbyTag.OrderByDescending(o => o.createdate).Take(6).ToList();
                }
                foreach (var obj in listArticalbyTag)
                {
                    obj.title = _common.SplitString(obj.title, 45);
                    obj.extract = _common.SplitString(obj.extract, 60);
                }
                ViewData["ListArticleTags"] = listArticalbyTag;
                ViewData["thuonghieu"] = _productRepository.Thuonghieu(listId);
                Session["dataproduct"] = pro;
                return View(pro);
            }
            return Redirect("/404.html");
        }
        public ActionResult CollectionParentProduct(string urlcollection, string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var pro = _productRepository.GetProByCollectionChild(urlcollection, null, UtilsBB.GetStoreId()).OrderBy(o => o.shop_sanpham.ischeckout).ThenBy(o => o.shop_sanpham.tensp).ThenBy(o => o.shop_sanpham.tensp_us).ToList();
            if (pro != null && pro.Count > 0)
            {
                List<int> listId = new List<int>();
                foreach (var item in pro)
                {
                    if (item.shop_sanpham.shop_thuonghieu != null)
                        listId.Add(item.shop_sanpham.shop_thuonghieu.mahieu);
                }
                var listArticalbyTag = new List<ModelModuleDetail>();

                var madanhmuc = pro.FirstOrDefault().shop_danhmuccon.shop_danhmuc.madanhmuc;
                int[] listIds = TagsCommon.listIdArticlebyTag(madanhmuc, (int)TagsCollection.TagsListCategories, (int)Tags.TagsModuleMagazine);
                if (listIds.Length > 0)
                {
                    listArticalbyTag = Mapper.Map<List<ModelModuleDetail>>(_moduleRepository.GetListArticlebyIds(listIds.ToArray()));
                    listArticalbyTag = listArticalbyTag.OrderByDescending(o => o.createdate).Take(6).ToList();
                }
                foreach (var obj in listArticalbyTag)
                {
                    obj.title = _common.SplitString(obj.title, 45);
                    obj.extract = _common.SplitString(obj.extract, 60);
                }
                ViewData["ListArticleTags"] = listArticalbyTag;
                ViewData["thuonghieu"] = _productRepository.Thuonghieu(listId);
                Session["dataproduct"] = pro;
                return View(pro);
            }
            return Redirect("/404.html");
        }
        public ActionResult ProductSaleoff(string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var pro = _productRepository.GetProBySaleoff(UtilsBB.GetStoreId());
            if (pro != null && pro.Count > 0)
            {
                List<int> listId = new List<int>();
                List<int> listIdCollection = new List<int>();
                foreach (var item in pro)
                {
                    if (item.shop_sanpham.shop_thuonghieu != null)
                        listId.Add(item.shop_sanpham.shop_thuonghieu.mahieu);
                    if (item.shop_danhmuccon != null)
                        listIdCollection.Add(item.shop_danhmuccon.madanhmuccon);
                }

                ViewData["thuonghieu"] = _productRepository.Thuonghieu(listId);
                ViewData["danhmuccon"] = _productRepository.Danhmuccon(listIdCollection, UtilsBB.GetStoreId());
                Session["dataproduct"] = pro;
                return View(pro);

            }
            return Redirect("/san-pham/het-han-khuyen-mai.html");
        }
        public ActionResult ProductNotSaleoff()
        {
            return View();
        }
        public ActionResult GetProByBrand(string url, string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var pro = _productRepository.GetProByBrand(url, UtilsBB.GetStoreId()).OrderBy(o => o.shop_sanpham.ischeckout).ToList();
            if (pro != null)
            {
                List<int> listId = new List<int>();
                foreach (var item in pro)
                {
                    listId.Add(item.shop_sanpham.shop_thuonghieu.mahieu);
                }

                ViewData["thuonghieu"] = _productRepository.Thuonghieu(listId);
                Session["dataproduct"] = pro;
            }
            return View(pro);
        }
        [HttpPost]
        public ActionResult SearchPost(string search)
        {
            if (search != null)
            {
                TempData["key"] = search;
                var model = _productRepository.Search(search, UtilsBB.GetStoreId()).ToList();
                TempData["list"] = model;
                return RedirectToAction("Search");
            }
            return null;
        }
        public ActionResult Search()
        {
            ViewData["keysearch"] = TempData["key"];
            var pro = TempData["list"] as List<shop_collection>;
            if (pro != null && pro.Count > 0)
            {
                List<int> listIdBrand = new List<int>();//2 sp co 2 thuong hieu khac nhau
                List<int> listIdCollection = new List<int>();
                foreach (var item in pro)
                {
                    if (item.shop_sanpham.shop_thuonghieu != null)
                        listIdBrand.Add(item.shop_sanpham.shop_thuonghieu.mahieu);
                    if (item.shop_danhmuccon != null)
                        listIdCollection.Add(item.shop_danhmuccon.madanhmuccon);
                }

                ViewData["thuonghieu"] = _productRepository.Thuonghieu(listIdBrand);
                ViewData["danhmuccon"] = _productRepository.Danhmuccon(listIdCollection, UtilsBB.GetStoreId());

                Session["dataproduct"] = pro;
                return View(pro);

            }
            return View();
        }
        public ActionResult AjaxSort(string sort)
        {
            Session["sort"] = sort;
            List<shop_collection> data = (List<shop_collection>)Session["dataproduct"];
            var pro = GetAJaxData(data);
            return PartialView("~/Views/Shared/Partial/Gadgets/Shop/_listProduct.cshtml", pro);
        }
        public ActionResult AjaxFilter(string[] filter)
        {
            Session["filter"] = filter;
            List<shop_collection> data = (List<shop_collection>)Session["dataproduct"];
            var pro = GetAJaxData(data);
            return PartialView("~/Views/Shared/Partial/Gadgets/Shop/_listProduct.cshtml", pro);

        }
        public List<shop_collection> GetAJaxData(List<shop_collection> data)
        {
            var pro = new List<shop_collection>();
            var sort = Session["sort"];
            var filter = Session["filter"];
            try
            {
                if (filter != null)
                {
                    string[] arr = filter as string[];
                    // var idbrand = arr.Select(s => (int).Parse(s)).ToList();
                    var idbrand = arr.Select(s => { int i; return (int.TryParse(s, out i)) ? i : (int?)null; }).ToList();
                    pro = data.Where(p => (idbrand.Contains(p.shop_sanpham.mahieu))).ToList();
                }
                else
                {
                    pro = data;
                }


                if (sort != null)
                {
                    switch (sort.ToString())
                    {
                        case "NameProduct":
                            pro = pro.OrderBy(p => p.shop_sanpham.tensp).ToList();
                            break;
                        case "NewProduct":
                            pro = pro.OrderByDescending(p => p.shop_sanpham.id).ToList();
                            break;
                        case "SaleProduct":
                            pro = pro.OrderByDescending(p => p.shop_sanpham.shop_bienthe.FirstOrDefault().giasosanh).ToList();
                            break;
                        case "PriceProduct":
                            pro = pro.OrderBy(p => p.shop_sanpham.shop_bienthe.FirstOrDefault().gia).ToList();
                            break;
                        case "PriceDescProduct":
                            pro = pro.OrderByDescending(p => p.shop_sanpham.shop_bienthe.FirstOrDefault().gia).ToList();
                            break;
                        default:
                            pro = pro.OrderBy(p => p.shop_sanpham.tensp).ToList();
                            break;

                    }
                }
            }
            catch { }

            return pro;
        }

        [HttpPost]
        public ActionResult CollectionParentProduct(FormCollection formData)
        {
            return View();
        }
        public ActionResult DeatialProduct(string urlproduct, string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var pro = _productRepository.GetProductByUrl(urlproduct, UtilsBB.GetStoreId());

            if (TempData["ErorMessage"] != null)
                ViewBag.Eror = TempData["ErorMessage"].ToString();
            var listArticalbyTag = new List<ModelModuleDetail>();
            var sp = Mapper.Map<ModelProduct>(pro);
            if (sp != null && sp.id > 0)
            {
                List<shop_collection> pro_orther = new List<shop_collection>();
                List<sys_tags_SummaryModel> pro_ortherbyTags = new List<sys_tags_SummaryModel>();
                if (pro.shop_collection.FirstOrDefault() != null && pro.shop_collection.First().shop_danhmuccon != null)
                {
                    //pro_orther = _productRepository.GetProByCollectionChild(null, UtilsBB.HasCookieLangEn() ? pro.shop_collection.First().shop_danhmuccon.url_us : pro.shop_collection.First().shop_danhmuccon.url, UtilsBB.GetStoreId()).Where(o => o.shop_sanpham.id != pro.id).OrderBy(o => o.shop_sanpham.ischeckout).ThenBy(o => o.shop_sanpham.tensp).ThenBy(o => o.shop_sanpham.tensp_us).ToList();
                    pro_orther = _productRepository.Relatedproduct(UtilsBB.HasCookieLangEn() ? pro.shop_collection.First().shop_sanpham.tensp_us : pro.shop_collection.First().shop_sanpham.tensp);
                    var lstTag = TagsCommon.Gettags_SummarybyTagRef((int)Tags.TagsProduct, pro.id);
                    if (lstTag != null && lstTag.Count > 0)
                    {
                        pro_ortherbyTags = lstTag;
                    }
                    int[] listIds = TagsCommon.listIdArticlebyTag(sp.id, (int)TagsCollection.TagsProduct, (int)Tags.TagsModuleMagazine);
                    if (listIds.Length > 0)
                    {
                        listArticalbyTag = Mapper.Map<List<ModelModuleDetail>>(_moduleRepository.GetListArticlebyIds(listIds.ToArray()));
                        listArticalbyTag = listArticalbyTag.OrderByDescending(o => o.createdate).Take(6).ToList();
                    }
                    foreach (var obj in listArticalbyTag)
                    {
                        obj.extract = _common.SplitString(obj.extract, 100);
                    }
                    sp.shop_bienthe = sp.shop_bienthe.Where(o => o.isdelete == false).ToList();

                    var views = Request.Cookies["views"];
                    // Nếu chưa có cookie cũ -> tạo mới
                    if (views == null)
                    {
                        views = new HttpCookie("views");
                    }
                    // Bổ sung mặt hàng đã xem vào cookie
                    views.Values[sp.id.ToString()] = sp.id.ToString();
                    // Đặt thời hạn tồn tại của cookie
                    views.Expires = DateTime.Now.AddDays(5);
                    // Gửi cookie về client để lưu lại
                    Response.Cookies.Add(views);
                    // Lấy List<int> chứa mã hàng đã xem từ cookie
                    var keys = views.Values
                        .AllKeys.Select(k => int.Parse(k)).ToList();
                    // Truy vấn háng đãn xem
                    ViewData["Spdaxem"] = db.shop_collection
                        .Where(p => keys.Contains(p.shop_sanpham.id)).OrderBy(o => o.shop_sanpham.ischeckout).ThenBy(o => o.shop_sanpham.tensp).ThenBy(o => o.shop_sanpham.tensp_us).ToList();
                }
                ViewData["ListArticleTags"] = listArticalbyTag;
                ViewData["Tags"] = pro_ortherbyTags;
                ViewData["Spkhac"] = pro_orther;
                ViewData["PTGH"] = _pageRepository.pageSYS(2);
                ViewData["PTTT"] = _pageRepository.pageSYS(10);
            }
            else
            {
                return Redirect(UtilsBB.HasCookieLangEn() ? "/en" : "/");
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult DeatialProduct(ModelProduct bt)
        {
            var flag = false;
            foreach (var item in bt.shop_bienthe)
            {
                if (item.soluong > 0)
                {
                    flag = true;
                    AddToCart(item.id, item.soluong);
                }
            }
            if (flag == false)
            {
                TempData["ErorMessage"] = "Bạn chưa chọn số lượng sản phẩm cần mua.";
                return RedirectToAction("DeatialProduct", "Shop");
            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
        }

        private void AddToCart(long id, int cartCount)
        {
            // Retrieve the album from the database
            UtilsBB.AddToCart((int)id, cartCount);

        }
        public ActionResult ProductbyTags(string tag, string en = null)
        {
            MyCookie cookieLag = new MyCookie("CookieLang");
            cookieLag.SetCookieForLang(string.IsNullOrEmpty(en) ? "vn" : "en");
            var listIdPro = TagsCommon.ListRefIdTagBy((int)Tags.TagsProduct, tag.Trim());
            ViewData["keyTags"] = tag;
            if (listIdPro != null && listIdPro.Count > 0)
            {
                var pro = _productRepository.GetProByCollectionChildbyListIdProduct(listIdPro).OrderBy(o => o.shop_sanpham.ischeckout).ToList();
                if (pro != null && pro.Count > 0)
                {
                    List<int> listId = new List<int>();
                    List<int> listIdCollection = new List<int>();
                    foreach (var item in pro)
                    {
                        if (item.shop_sanpham.shop_thuonghieu != null)
                            listId.Add(item.shop_sanpham.shop_thuonghieu.mahieu);
                        if (item.shop_danhmuccon != null)
                            listIdCollection.Add(item.shop_danhmuccon.madanhmuccon);
                    }
                    ViewData["danhmuccon"] = _productRepository.Danhmuccon(listIdCollection, UtilsBB.GetStoreId());
                    ViewData["thuonghieu"] = _productRepository.Thuonghieu(listId);
                    Session["dataproduct"] = pro;
                    return View(pro);
                }
                return View();
            }
            return View();
        }

        public ActionResult AjaxLoadMore()
        {
            var index = 1;
            if (TempData["IndexMLoadMore"] != null)
            {
                var indexMore = (int)TempData["IndexMLoadMore"];
                index = indexMore + 1;
                TempData["IndexMLoadMore"] = index;
            }
            var pro = _productRepository.ProductTop("spgiamgia", 0, UtilsBB.GetStoreId());
            pro = pro.Take(12 * index).ToList();
            return PartialView("~/Views/Shared/Partial/Gadgets/Shop/_listProductHome_List.cshtml", pro);
        }
        public JsonResult GetCustomers(string term)
        {
            var lang = UtilsBB.HasCookieLangEn();
            if (lang == false)
            {
                var customers = from sp in db.shop_sanpham.Where(c => c.tensp.Contains(term) && c.hide == false)
                                select sp.tensp;
                customers = customers.Distinct();
                return Json(customers, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var customers = from sp in db.shop_sanpham.Where(c => c.tensp_us.Contains(term) && c.hide == false)
                                select sp.tensp_us;
                customers = customers.Distinct();
                return Json(customers, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
