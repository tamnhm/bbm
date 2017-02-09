using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Checkout
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private babymart_vnEntities _context;
        public CheckoutRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public long AddOrder(donhang model)
        {
            _context.donhangs.Add(model);
            _context.SaveChanges();
            return model.id;
        }

        public void AddOrder_CT(List<donhang_ct> model)
        {
            foreach (var item in model)
                _context.donhang_ct.Add(item);
            _context.SaveChanges();
        }

        public void UpdateOrderContent(long Id, string contents)
        {
            donhang item = _context.donhangs.Find(Id);
            item.noidung = contents;
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void UpdateOrderContentVnPay(long Id, string rspcode_vnp)
        {
            donhang item = _context.donhangs.Find(Id);
            item.tinhtrang = rspcode_vnp;
            _context.Entry(item).State = EntityState.Modified;
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