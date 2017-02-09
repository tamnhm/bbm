using Babymart.Infractstructure;
using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Page
{

    public class PageRepository : IPageRepository
    {
        private babymart_vnEntities _context;
        public PageRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public shop_page pageSYS(int id, int storeId = 1000)
        {
            return _context.shop_page.Where(o => o.StoreId == storeId && o.id == id).FirstOrDefault();
        }
        public List<shop_page> ListpageAll(int storeId = 1000)
        {
            return _context.shop_page.Where(o => o.StoreId == storeId).ToList();
        }
        public List<shop_page> ListpageSYS(string type = "", int storeId = 1000)
        {
            if (!String.IsNullOrEmpty(type))
                return _context.shop_page.Where(
                    o => o.hide == false
                    && o.codetype.Equals(type)
                    && o.StoreId == storeId
                    ).ToList();
            return _context.shop_page.Where(o => o.hide == false && o.StoreId == storeId).ToList();
        }
        public List<shop_page> ListpageByType(int storeId = 1000)
        {
            return _context.shop_page.Where(o => o.hide == false && o.showmenu == true 
                && o.codetype.Contains("HH")
                  && o.StoreId == storeId
                ).OrderBy(o => o.sort).ToList();
        }
        public List<shop_page> ListpageByType1(int storeId = 1000)
        {
            return _context.shop_page.Where(o => o.hide == false
                  && o.StoreId == storeId
                ).OrderBy(o => o.sort).ToList();
        }
        public shop_page Footer(int storeId = 1000)
        {
            return _context.shop_page.Where(o => o.tieude.Equals("Footer") && o.StoreId == storeId).FirstOrDefault();
        }
        public int InsertArticle(shop_page model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.shop_page.Add(model);
            _context.SaveChanges();
            return model.id;
        }
        public int UpdateArticle(shop_page model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            return model.id;
        }
        public void DeleteArticle(int id)
        {
            shop_page item = _context.shop_page.Find(id);
            _context.shop_page.Remove(item);
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