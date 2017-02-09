using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
using Babymart.Infractstructure;
namespace Babymart.DAL.Collection
{
    public class ContentsSEO : IContentsSEO
    {
        private babymart_vnEntities _context;
        public ContentsSEO(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<sys_content> GetList(int StoreId = 1000)
        {
            return _context.sys_content.Where(p => p.StoreId == StoreId && p.hide == false).OrderBy(p => p.Type).ToList();
        }

        public void UpdateSys_content(sys_content model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.Entry(model).State = EntityState.Modified;
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