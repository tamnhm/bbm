using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
namespace Babymart.DAL.Administrator
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private babymart_vnEntities _context;
        public AdministratorRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }

        public sys_account_admin GetAdmin(int id)
        {
            var query = (from p in _context.sys_account_admin select p).Where(p => p.ma == id).FirstOrDefault();
            return query;
        }
        public List<admin_role> AdminRole(int AdminId)
        {
            return (from p in _context.admin_role select p).Where(p => p.AdminId == AdminId).ToList();
        }

        public admin Administrator(int AdminId)
        {
            var query = (from p in _context.admins select p).Where(p => p.id == AdminId).FirstOrDefault();
            return query;
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