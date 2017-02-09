using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
namespace Babymart.DAL.Collection
{
    public class Collection : ICollection
    {
        private babymart_vnEntities _context;
        public Collection(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
            _context.Configuration.ProxyCreationEnabled = false;
        }
        public void Add(shop_collection c)
        {
            _context.shop_collection.Add(c);
            _context.SaveChanges();
        }
        public List<Object> GetListbyproid(int ?id)
        {
            return (from p in _context.shop_collection where p.idsp == id select p).ToList<Object>();

        }
        public void Remove(int id)
        {
            var item = _context.shop_collection.Find(id);
            _context.shop_collection.Remove(item);
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