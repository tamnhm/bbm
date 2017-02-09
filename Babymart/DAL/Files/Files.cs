using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;

using System.Text;
using Babymart.DAL.Common;
using System.Data;
using Babymart.Infractstructure;
namespace Babymart.DAL.Customer
{
    public class Files : IFiles
    {
        private babymart_vnEntities _context;
        private ICommonRepository _common;

        public Files(babymart_vnEntities _db)
        {
            this._context = _db;
            this._common = new CommonRepository(new babymart_vnEntities());
        }
        public sys_file GetFile(int id, int storeId = 1000)
        {

            return _context.sys_file.Where(o => o.StoreId == storeId && o.Id == id).FirstOrDefault();

        }
        public List<sys_file> GetList()
        {
            return (_context.sys_file.OrderByDescending(p => p.Id).ToList());
        }
        public List<sys_file> GetListByType(int type, int storeId = 1000)
        {
            return (_context.sys_file.Where(o => o.type == type && o.hide == false && o.StoreId == storeId).OrderByDescending(p => p.sort).ToList());

        }
        public void Add(sys_file file)
        {
            file.StoreId = UtilsBB.GetStoreId();
            _context.sys_file.Add(file);
            _context.SaveChanges();
        }
        public void Update(sys_file file)
        {
            file.StoreId = UtilsBB.GetStoreId();
            _context.Entry(file).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Remove(int id)
        {
            var item = _context.sys_file.Find(id);
            _context.sys_file.Remove(item);
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