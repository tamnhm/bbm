using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
using Babymart.Infractstructure;
namespace Babymart.DAL.Product
{
    public class ProductRepository : IProductRepository
    {
        private babymart_vnEntities _context;
        string firststr;
        public ProductRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<shop_collection> GetListSP(int danhmuc = 0, int danhmuccon = 0, int thuonghieu = 0, int site = -1, int storeId = 1000)
        {
            if (danhmuc != 0 && danhmuccon == 0)
            {
                var query = from _col in _context.shop_collection
                            join _danhmuc in _context.shop_danhmuc on _col.iddm equals _danhmuc.madanhmuc
                            where _col.shop_sanpham.StoreId == storeId
                            select _col;
                query = query.Where(p => p.shop_danhmuccon.shop_danhmuc.madanhmuc == danhmuc);
                if (thuonghieu != 0)
                    query = query.Where(p => p.shop_sanpham.mahieu == thuonghieu);
                var a = query.ToList();
                return query.ToList();
            }
            else
            {
                var query = from _col in _context.shop_collection
                            join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                            where _col.shop_sanpham.StoreId == storeId
                            select _col;
                if (danhmuccon != 0)
                    query = query.Where(p => p.shop_danhmuccon.madanhmuccon == danhmuccon);
                if (thuonghieu != 0)
                    query = query.Where(p => p.shop_sanpham.mahieu == thuonghieu);
                return query.ToList();
            }


        }
        public List<shop_collection> Search_query(string stringquery)
        {
            var query = from _col in _context.shop_collection
                        join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                        where
                        _col.shop_sanpham.masp.Contains(stringquery)
                        || _col.shop_sanpham.tensp.Contains(stringquery)
                        || _col.shop_sanpham.tensp_us.Contains(stringquery)

                        select _col;
            return query.ToList();
        }
        public shop_sanpham GetProductById(int id, int storeId = 1000)
        {
            var query = (from p in _context.shop_sanpham where p.StoreId == storeId select p).Where(p => p.id == id).FirstOrDefault();
            return query;
        }
        public List<shop_sanpham> GetProduct(int storeId = 1000)
        {
            var query = (from p in _context.shop_sanpham where p.StoreId == storeId select p);
            return query.ToList(); 
        }
        public List<shop_collection> GetProByCollectionChild(string urldanhmuccha, string urldanhmuccon, int storeId = 1000)
        {
            var lang = UtilsBB.HasCookieLangEn();
            var query = from _col in _context.shop_collection
                        join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                        where _col.shop_sanpham.shop_bienthe.Count > 0 && _col.shop_sanpham.hide == false
                          && _col.shop_sanpham.StoreId == storeId
                        select _col;
            if (!string.IsNullOrEmpty(urldanhmuccha))
            {
                if (lang == true)
                {
                    query = query.Where(p => p.shop_danhmuccon.shop_danhmuc.url_us.Equals(urldanhmuccha));
                }
                else
                {
                    query = query.Where(p => p.shop_danhmuccon.shop_danhmuc.url.Equals(urldanhmuccha));
                }

            }

            else if (!string.IsNullOrEmpty(urldanhmuccon))
            {
                if (lang == true)
                {
                    query = query.Where(p => p.shop_danhmuccon.url_us.Equals(urldanhmuccon) && p.shop_sanpham.StoreId == storeId);
                }
                else
                {
                    query = query.Where(p => p.shop_danhmuccon.url.Equals(urldanhmuccon) && p.shop_sanpham.StoreId == storeId);
                }
            }
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();

        }

        public List<shop_collection> Relatedproduct(string nameproduct, int storeId = 1000)
        {
            nameproduct = nameproduct.TrimEnd();
            var Strqry = nameproduct.Split(' ');
            firststr = Strqry[0].ToString() + ' ';
            var query = _context.shop_collection.Where(r =>
                (Strqry.Any(qry => r.shop_sanpham.tensp.Contains(qry)))
                && r.shop_sanpham.shop_bienthe.Count > 0
                && r.shop_sanpham.hide == false
                && r.shop_sanpham.StoreId == storeId
                ).OrderByDescending(a => Strqry.All(qry => a.shop_sanpham.tensp.Contains(qry))).ThenByDescending(a => Strqry.All(qry => a.shop_sanpham.tensp_us.Contains(qry))).ThenBy(c => c.shop_sanpham.ischeckout == true).ThenByDescending(c => c.shop_sanpham.tensp.StartsWith(firststr)).ThenByDescending(b => Strqry.Count(qry => b.shop_sanpham.tensp.Contains(qry))).ThenByDescending(b => Strqry.Count(qry => b.shop_sanpham.tensp_us.Contains(qry))).Take(10);
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();

        }
        public List<shop_collection> GetProByCollectionChildbyListIdProduct(List<int> Ids, int id)
        {
            var query = from _col in _context.shop_collection
                        join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                        where Ids.Contains(_col.shop_sanpham.id)
                         && _col.shop_sanpham.shop_bienthe.Count > 0 && _col.shop_sanpham.hide == false
                        select _col;
            if (id != 0)
                query = query.Where(o => o.shop_sanpham.id != id);
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();

        }
        public List<shop_collection> GetProBySaleoff(int storeId = 1000)
        {
            var query = from _col in _context.shop_collection
                        join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                        where _col.shop_sanpham.shop_bienthe.Count > 0 && _col.shop_sanpham.hide == false
                        && (_col.shop_sanpham.shop_bienthe.FirstOrDefault().giasosanh > 0 || _col.shop_sanpham.showsptangkemvaomuckm ==true)
                        && _col.shop_sanpham.StoreId == storeId
                        select _col;
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.OrderBy(o => o.shop_sanpham.ischeckout).ThenBy(o => o.shop_sanpham.tensp).ThenBy(o => o.shop_sanpham.tensp_us).ToList();  
        }
        public List<shop_collection> GetProductOrther(int iddmc, int storeId = 1000)
        {
            var collection = _context.shop_collection.Where(o => o.idsp == iddmc).FirstOrDefault();
            var query = from _col in _context.shop_collection
                        join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                        where _col.shop_sanpham.shop_bienthe.Count > 0
                        && _col.shop_sanpham.hide == false
                        && _col.iddmc == collection.iddmc
                        && _col.shop_sanpham.StoreId == storeId
                        select _col;
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();
        }
        public List<shop_collection> GetProByBrand(string url, int storeId = 1000)
        {

            var query = from _col in _context.shop_collection
                        join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
                        where _col.shop_sanpham.shop_bienthe.Count > 0
                         && _col.shop_sanpham.hide == false
                         && _col.shop_sanpham.shop_thuonghieu.url.Contains(url)
                         && _col.shop_sanpham.StoreId == storeId
                        select _col;
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();
        }
        public List<shop_collection> Search(string nameproduct, int storeId = 1000)
        {
            if (nameproduct != null)
            {
                nameproduct = nameproduct.TrimEnd();
                var Strqry = nameproduct.Split(' '); 
                var query = _context.shop_collection.Where(r =>
                    (Strqry.Any(qry => r.shop_sanpham.tensp.Contains(qry))
                    || r.shop_sanpham.masp.Contains(nameproduct)
                    || r.shop_sanpham.shop_bienthe.Any(o => o.title.Contains(nameproduct))
                    || Strqry.Any(qry => r.shop_sanpham.tensp_us.Contains(qry))
                    || r.shop_sanpham.shop_bienthe.Any(o => o.title_us.Contains(nameproduct))
                    )
                    && r.shop_sanpham.shop_bienthe.Count > 0
                    && r.shop_sanpham.hide == false
                    && r.shop_sanpham.StoreId == storeId
                    ).OrderByDescending(a => Strqry.All(qry => a.shop_sanpham.tensp.Contains(qry))).ThenByDescending(a => Strqry.All(qry => a.shop_sanpham.tensp_us.Contains(qry))).ThenBy(c => c.shop_sanpham.ischeckout == true).ThenByDescending(b => Strqry.Count(qry => b.shop_sanpham.tensp.Contains(qry))).ThenByDescending(b => Strqry.Count(qry => b.shop_sanpham.tensp_us.Contains(qry))).Take(60); 
                if (UtilsBB.HasCookieLangEn() == true)
                    query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
                return query.ToList();
            }
            return null;

            //var query = from _col in _context.shop_collection
            //            join _danhmuccon in _context.shop_danhmuccon on _col.iddmc equals _danhmuccon.madanhmuccon
            //            where (_col.shop_sanpham.tensp.Contains(nameproduct)
            //            || _col.shop_sanpham.thongtin.Contains(nameproduct)
            //            || _col.shop_sanpham.spdescription.Contains(nameproduct)
            //            || _col.shop_sanpham.spkeywords.Contains(nameproduct)
            //            )
            //            && _col.shop_sanpham.shop_bienthe.Count > 0
            //            && _col.shop_sanpham.hide == false
            //            select _col;

        }
        public List<shop_thuonghieu> Thuonghieu(List<int> list_id)
        {

            var query = from _col in _context.shop_thuonghieu
                        where list_id.Contains(_col.mahieu)
                        select _col;
            return query.OrderBy(o => o.tenhieu).ToList();
        }
        public List<shop_danhmuccon> Danhmuccon(List<int> list_id, int storeId = 1000)
        {

            var query = from _col in _context.shop_danhmuccon
                        where list_id.Contains(_col.madanhmuccon)
                        && _col.StoreId == storeId
                        select _col;
            return query.ToList();
        }
        public shop_sanpham GetProductByUrl(string url, int storeId = 1000)
        {
            if (!UtilsBB.HasCookieLangEn())
            {
                var q = from product in _context.shop_sanpham
                        where product.spurl.Equals(url)
                        && product.hide == false
                        && product.StoreId == storeId
                        select product;
                var s = q.FirstOrDefault();
                return q.FirstOrDefault();
            }
            else
            {
                var q = from product in _context.shop_sanpham
                        where product.spurl_us.Equals(url)
                        && product.hide == false
                        && product.StoreId == storeId
                        select product;
                var s = q.FirstOrDefault();
                return q.FirstOrDefault();
            }

        }
        public List<shop_sanpham> ProductonHomepage(int storeId = 1000)
        {
            var query = from _col in _context.shop_sanpham
                        where _col.hide == false
                           && _col.StoreId == storeId
                        select _col;
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();

        }
        /********************************HOMEPAGE*************************************************/
        public List<shop_sanpham> ProductTop(string orderby, int take, int storeId = 1000)
        {
            var query = _context.shop_sanpham.Where(o => o.hide == false
                && o.shop_bienthe.Count > 0
                && o.StoreId == storeId
                && o.ischeckout == false
                );
            switch (orderby)
            {
                case "spmoi":
                    query = query.OrderByDescending(o => o.id);
                    if (UtilsBB.HasCookieLangEn() == true)
                        query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty).Take(take);
                    else
                        query = query.Take(take);
                    break;
                case "spbanchay":
                    query = query.OrderByDescending(o => o.countsale);
                    if (UtilsBB.HasCookieLangEn() == true)
                        query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty).Take(take);
                    else
                        query = query.Take(take);
                    break;
                case "spgiamgia":
                    query = query.Where(o => o.shop_bienthe.FirstOrDefault().giasosanh > 0 || o.showsptangkemvaomuckm == true).OrderByDescending(p => p.showsptangkemvaomuckm).ThenBy(o => ((o.shop_bienthe.FirstOrDefault().gia * 1.00) / (o.shop_bienthe.FirstOrDefault().giasosanh + 1)) * 100);
                    if (UtilsBB.HasCookieLangEn() == true)
                        query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty).Take(10); 
                    else
                        query = query.Where(o => (o.tensp ?? string.Empty) != string.Empty).Take(10); 
                    break;
                case "spnoibat":
                    query = query.Where(o => o.showhome == true);
                    if (UtilsBB.HasCookieLangEn() == true)
                        query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty);
                    break;
                case "spbanner":
                    query = query.Where(o => o.showbanner == true);
                    if (UtilsBB.HasCookieLangEn() == true)
                        query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty);
                    break;

                default:
                    if (UtilsBB.HasCookieLangEn() == true)
                        query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty);
                    query = query.Take(take);
                    break;
            }

            return query.ToList();

        }
        public List<shop_sanpham> ProductIndex(int iddm, int take, int storeId = 1000)
        {
            var query = (from a in _context.shop_sanpham
                         join b in _context.shop_collection
                            on a.id equals b.idsp
                         where b.iddm == iddm && a.StoreId == storeId && a.hide == false && a.showhome == true && a.ischeckout==false
                        orderby a.id descending
                         select a); 
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty).Take(take);
            else
                query = query.Where(o => (o.tensp ?? string.Empty) != string.Empty).Take(take);
                    
            return query.ToList();
        } 
        public List<shop_danhmuccon> CollectionIndex(int iddm, int storeId = 1000)
        {
            var query = from a in _context.shop_danhmuccon
                         join b in _context.shop_danhmuc
                            on a.madanhmuc equals b.madanhmuc
                        where b.madanhmuc == iddm && a.StoreId == storeId && a.hide == false
                        orderby a.tendanhmuccon ascending 
                         select a;
            return query.ToList();
        }
        public shop_danhmuc CollectionName(int iddm, int storeId = 1000)
        {
            var query = (from p in _context.shop_danhmuc where p.StoreId == storeId select p).Where(p => p.madanhmuc == iddm).FirstOrDefault(); 
            return query;
        }
        /********************************HOMEPAGE*************************************************/
        //Admin
        public void ResetSaleoffProduct()
        {
            var listpro = this.ProductTop("spgiamgia", 0, UtilsBB.GetStoreId());
            foreach (var item in listpro)
            {
                if (item.timeend != null && item.timeend <= DateTime.Now)
                {
                    try
                    {
                        foreach (var obj in item.shop_bienthe)
                        {
                            if (obj.giasosanh > 0)
                            {
                                obj.gia = obj.giasosanh;
                                obj.giasosanh = 0;
                            }
                        }
                        item.ischecksaleoff = false;
                        item.ischeckgift = false;
                        item.gift = string.Empty;
                        item.timeend = null;
                        _context.Entry(item).State = EntityState.Modified;
                    }
                    catch
                    {

                    }
                }
            }
            _context.SaveChanges();

        }
        public void RemoveImg(int id)
        {
            var item = _context.shop_image.Find(id);
            _context.shop_image.Remove(item);
            _context.SaveChanges();
        }
        public void InsertProduct(shop_sanpham sp)
        {
            sp.countsale = 0;
            sp.StoreId = UtilsBB.GetStoreId();
            _context.shop_sanpham.Add(sp);
            _context.SaveChanges();
        }
        public void UpdateProduct(shop_sanpham sp)
        {
            sp.StoreId = UtilsBB.GetStoreId();
            _context.Entry(sp).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void UpdateListProduct(List<shop_sanpham> sp)
        {
            foreach (var item in sp)
            {
                item.StoreId = UtilsBB.GetStoreId();
                _context.Entry(item).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }
        public void SetShowHome(int id)
        {
            var item = _context.shop_sanpham.Single(r => r.id == id);
            item.StoreId = UtilsBB.GetStoreId();
            if (item.showhome == true)
                item.showhome = false;
            else
                item.showhome = true;
            _context.SaveChanges();
        }

        public void SetVisible(int id, bool visible)
        {
            var item = _context.shop_sanpham.Single(r => r.id == id);
            item.StoreId = UtilsBB.GetStoreId();
            item.hide = visible;
            _context.SaveChanges();
        }
        public void Outset(int id, bool outset)
        {
            var item = _context.shop_sanpham.Single(r => r.id == id);
            item.ischeckout = outset;
            _context.SaveChanges();
        }
        public void Remove(int id)
        {
            shop_sanpham item = _context.shop_sanpham.Find(id);
            _context.shop_sanpham.Remove(item);
            _context.SaveChanges();
        }
        public List<shop_bienthe> listBienthe(int? idsp)
        {
            return _context.shop_bienthe.Where(p => p.idsp == idsp).ToList();
        }
        public void InsertBienthe(shop_bienthe bt)
        {
            _context.shop_bienthe.Add(bt);
            _context.SaveChanges();
        }
        public void UpdateBienthe(shop_bienthe bt)
        {
            _context.Entry(bt).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void RemoveBienthe(int id)
        {
            shop_bienthe item = _context.shop_bienthe.Find(id);
            item.isdelete = true;
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        //public void RemoveBienthe(int id)
        //{
        //    shop_bienthe item = _context.shop_bienthe.Find(id);
        //    _context.shop_bienthe.Remove(item);
        //    _context.SaveChanges();
        //}
        public void Saleoff(shop_sanpham sp)
        {
            shop_sanpham item = _context.shop_sanpham.Find(sp.id);
            if (sp.ischecksaleoff == true)
            {
                // item.gia = sp.gia;
                // item.giakm = sp.giakm;
                item.ischecksaleoff = true;
                item.timeend = sp.timeend;
                if (sp.ischeckgift == true)
                {
                    item.ischeckgift = true;
                    item.gift = sp.gift;
                }
            }
            else
            {
                //  item.gia = sp.gia;
                //  item.giakm = 0;
                item.ischecksaleoff = false;
                item.ischeckgift = false;
                item.gift = null;
                item.timeend = null;

            }
            _context.SaveChanges();
        }
        public shop_page Detail(string url)
        {
            return _context.shop_page.Where(p => p.url.Contains(url)).FirstOrDefault();
        }
        public List<shop_page> Listtd(string type)
        {
            return _context.shop_page.Where(p => p.codetype.Contains(type)).OrderByDescending(o=>o.ngayviet).ToList();
        }
        public List<shop_sanpham> GetlistProduct(int[] Ids)
        {
            var query = _context.shop_sanpham.Where(o => Ids.Contains(o.id));
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.tensp_us ?? string.Empty) != string.Empty);
            return query.ToList();
        }
        public List<shop_bienthe> GetListSpForPlan(int typePlan)
        {
            var query = _context.shop_bienthe.Where(o => o.shop_sanpham.plantype == typePlan
                && o.shop_sanpham.hide == false && o.isdelete == false);
            if (UtilsBB.HasCookieLangEn() == true)
                query = query.Where(o => (o.shop_sanpham.tensp_us ?? string.Empty) != string.Empty);
            return query.OrderBy(o => o.shop_sanpham.tensp).ToList();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //-----Checking
        public bool CheckingByUrl(string url, int igonId = 0)
        {

            if (igonId > 0)
            {
                var obj = _context.shop_sanpham.Where(p => p.spurl.Equals(url) && !p.id.Equals(igonId)).FirstOrDefault();
                if (obj != null)
                    return true;
                return false;
            }
            else
            {
                var obj = _context.shop_sanpham.Where(p => p.spurl.Equals(url)).FirstOrDefault();
                if (obj != null)
                    return true;
                return false;
            }

        }

    }

}