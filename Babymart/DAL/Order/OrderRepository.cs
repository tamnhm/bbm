using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
using PagedList;
namespace Babymart.DAL.Order
{
    public class OrderRepository : IOrderRepository
    {
        private babymart_vnEntities _context;
        public OrderRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<donhang> Getlist()
        {
            return _context.donhangs.OrderByDescending(p => p.id).ToList();
        }
        public IPagedList<donhang> GetlistOrderPaging(int? page, int pageSize, int serach = 0)
        {
            if (serach > 0)
            {
                return _context.donhangs.Where(o => o.id.Equals(serach)).OrderByDescending(p => p.id).ToPagedList(page ?? 1, pageSize);
            }
            return _context.donhangs.OrderByDescending(p => p.id).ToPagedList(page ?? 1, pageSize);
        }
        public void Update(donhang dh)
        {
            _context.Entry(dh).State = EntityState.Modified;
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