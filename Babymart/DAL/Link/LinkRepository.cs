using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
using System.Data;
using Babymart.Infractstructure;
namespace Babymart.DAL.Link
{
    public class LinkRepository : ILinkRepository
    {
        private babymart_vnEntities _context;
        public LinkRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
            //   _context.Configuration.ProxyCreationEnabled = false;
        }
        public List<shop_danhmuccon> ListDanhmuccon(int iddanhmuc, int storeId = 1000)
        {
            var result = _context.shop_danhmuccon.Where(o => o.shop_danhmuc.madanhmuc == iddanhmuc

                ).ToList();
            return result;
        }
        public List<shop_danhmuc> Getdanhmuc(int loai = 0, int storeId = 1000)
        {
            if (loai != 0)
            {
                return _context.shop_danhmuc.Where(p => p.maloai == loai && p.hide == false && p.StoreId == storeId).OrderByDescending(o => o.sort).ThenBy(o => o.tendanhmuc).ToList();
            }
            else
            {
                return _context.shop_danhmuc.Where(p => p.hide == false && p.StoreId == storeId).OrderByDescending(o => o.sort).ThenBy(o => o.tendanhmuc).ToList();
            }


        }
        public List<shop_danhmuc> GetdanhmucgforAd(int storeId = 1000)
        {
            return _context.shop_danhmuc.Where(p => p.StoreId == storeId).ToList();
        }
        public List<shop_danhmuccon> Getdanhmuccon(int madanhmuccha, int storeId = 1000)
        {
            var query = (from p in _context.shop_danhmuccon select p).Where(p => p.madanhmuc == madanhmuccha && p.hide == false && p.StoreId == storeId).ToList();

            return query;
        }
        public List<shop_danhmuccon> GetdanhmucconAll(int madanhmuccha, int storeId = 1000)
        {
            var query = (from p in _context.shop_danhmuccon select p).Where(p => p.madanhmuc == madanhmuccha && p.StoreId == storeId).ToList();

            return query;
        }
        public void InsertLink(shop_danhmuc dm)
        {
            dm.hide = false;
            dm.StoreId = UtilsBB.GetStoreId();
            _context.shop_danhmuc.Add(dm);
            _context.SaveChanges();
        }
        public void UpdateLink(shop_danhmuc dm)
        {
            dm.StoreId = UtilsBB.GetStoreId();
            _context.Entry(dm).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void RemoveLink(int id)
        {
            shop_danhmuc item = _context.shop_danhmuc.Find(id);
            _context.shop_danhmuc.Remove(item);
            _context.SaveChanges();
        }
        public void InsertLinkCon(shop_danhmuccon dm)
        {
            dm.hide = false;
            dm.StoreId = UtilsBB.GetStoreId();
            _context.shop_danhmuccon.Add(dm);
            _context.SaveChanges();
        }
        public void UpdateLinkCon(shop_danhmuccon dm)
        {
            dm.StoreId = UtilsBB.GetStoreId();
            _context.Entry(dm).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void RemoveLinkCon(int id)
        {
            shop_danhmuccon item = _context.shop_danhmuccon.Find(id);
            _context.shop_danhmuccon.Remove(item);
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