using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
namespace Babymart.DAL.Photo
{
    public class PhotoRepository : IPhotoRepository
    {
        private babymart_vnEntities _context;
        public PhotoRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
            _context.Configuration.ProxyCreationEnabled = false;
        }

        public shop_image InsertImg(shop_image img)
        {
            _context.shop_image.Add(img);
            _context.SaveChanges();
            return img;
        }
        public void RemoveImg(int id)
        {
            var item = _context.shop_image.Find(id);
            _context.shop_image.Remove(item);
            _context.SaveChanges();
        }
        public void UpdateImh(shop_image t)
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