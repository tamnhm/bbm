using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Babymart.Models;
namespace Babymart.DAL.Sys_Mail
{
    public class Sys_MailRepository : ISys_MailRepository
    {
        private babymart_vnEntities _context;
        public Sys_MailRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public sys_mail Getmail(string type)
        {
            var query = (from p in _context.sys_mail select p).Where(p => p.type.Equals(type)).FirstOrDefault();
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