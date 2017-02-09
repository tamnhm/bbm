using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
using Babymart.Infractstructure;
namespace Babymart.DAL.Brand
{
    public class BrandRepository : IBrandRepository
    {
        private babymart_vnEntities _context;
        public BrandRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<shop_thuonghieu> TopBrand(int count, int StoreId = 1000)
        {
            var query = _context.shop_thuonghieu.Where(o => o.showhome == true && o.hide == false && o.StoreId == StoreId).OrderByDescending(o => o.sort_metro).Take(count).ToList();
            return query;
        }

        public List<Object> GetList(int StoreId = 1000)
        {
            return _context.shop_thuonghieu.Where(o => o.StoreId == StoreId).ToList<Object>();
            //   var query = (from p in _context.shop_thuonghieu orderby p.mahieu descending select p).ToList<Object>();
            //  return query;
        }
        public shop_thuonghieu GetBrandById(int id, int StoreId = 1000)
        {
            var query = (from p in _context.shop_thuonghieu select p).Where(p => p.mahieu == id && p.StoreId == StoreId).FirstOrDefault();
            return query;
        }


        public void SetVisible(int id, bool visible)
        {
            var item = _context.shop_thuonghieu.Single(r => r.mahieu == id);
            item.StoreId = UtilsBB.GetStoreId();
            item.hide = visible;
            _context.SaveChanges();

        }
        public void SetShowHome(int id, bool showhome)
        {
            var item = _context.shop_thuonghieu.Single(r => r.mahieu == id);
            item.showhome = showhome;
            item.sort_metro = 0;
            item.StoreId = UtilsBB.GetStoreId();
            _context.SaveChanges();

        }

        public void Remove(int id)
        {
            var item = _context.shop_thuonghieu.Find(id);
            _context.shop_thuonghieu.Remove(item);
            _context.SaveChanges();


        }
        public int Insert(shop_thuonghieu t)
        {
            t.StoreId = UtilsBB.GetStoreId();
            _context.shop_thuonghieu.Add(t);
            _context.SaveChanges();
            return t.mahieu;
        }
        public void Update(shop_thuonghieu t)
        {
            t.StoreId = UtilsBB.GetStoreId();
            _context.Entry(t).State = EntityState.Modified;          
            _context.SaveChanges();
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
    }
}