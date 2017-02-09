using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Plan
{
    public class PlanRepository : IPlanRepository
    {
        private babymart_vnEntities _context;
        public PlanRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<shop_plan_saleoff> GetlistShop_plan_saleoff()
        {
            return _context.shop_plan_saleoff.ToList();
        }
        public List<shop_plan_type> GetlistShop_plan_type()
        {
            return _context.shop_plan_type.OrderBy(o=>o.Type).ToList();
        }
        public int AddTypPlan(shop_plan_type model)
        {
            _context.shop_plan_type.Add(model);
            _context.SaveChanges();
            return model.Id;
        }
        public int UpdateTypPlan(shop_plan_type model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            return model.Id;
        }
        public void DeleteTypPlan(int id)
        {
            shop_plan_type item = _context.shop_plan_type.Find(id);
            _context.shop_plan_type.Remove(item);
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