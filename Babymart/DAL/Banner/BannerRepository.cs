using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
using Babymart.Infractstructure;
namespace Babymart.DAL.Brand
{
    public class BannerRepository : IBannerRepository
    {
        private babymart_vnEntities _context;
        public BannerRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<sys_Banner> ListBanner(int type = 0)
        {
            var query = _context.sys_Banner.Where(o => o.Hide == false).OrderByDescending(o => o.Id).ToList();
            if (type > 0)
            {
                query = query.Where(o => o.Type == type).ToList();
            }
            return query;
        }
        public List<sys_Banner> ListBannerAll(int type = 0)
        {
            var query = _context.sys_Banner.OrderByDescending(o => o.Id).ToList();
            if (type > 0)
            {
                query = query.Where(o => o.Type == type).ToList();
            }
            return query;
        }
        public sys_Banner BannerDetail(int Id)
        {
            var query = _context.sys_Banner.Where(o => o.Id == Id).FirstOrDefault();
            return query;
        }
        public void SetVisible(int id, bool visible)
        {
            var item = _context.sys_Banner.Single(r => r.Id == id);
            item.Hide = visible;
            _context.SaveChanges();

        }

        public void Remove(int id)
        {
            var item = _context.sys_Banner.Find(id);
            _context.sys_Banner.Remove(item);
            _context.SaveChanges();
        }
        public int Insert(sys_Banner t)
        {
            _context.sys_Banner.Add(t);
            _context.SaveChanges();
            return t.Id;
        }
        public void Update(sys_Banner t)
        {
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