using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
using System.Data;
namespace Babymart.DAL.LangdingPage
{
    public class LangdingPage : ILangdingPage
    {
        private babymart_vnEntities _context;
        public LangdingPage(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
            _context.Configuration.ProxyCreationEnabled = false;
        }
        public List<Object> GetList()
        {
            return (from p in _context.shop_loai select new { p.maloai, p.tenloai }).ToList<Object>();

        }
        public shop_loai GetDetail(int loai)
        {
            return (from p in _context.shop_loai where p.maloai == loai select p).FirstOrDefault();
        }
        public void Update(shop_loai t)
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