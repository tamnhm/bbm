using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;

using System.Text;
using Babymart.DAL.Common;
using System.Data;
using PagedList;
using Babymart.Infractstructuree;
namespace Babymart.DAL.Customer
{
    public class Customer : ICustomer
    {
        private babymart_vnEntities _context;
        private ICommonRepository _common;

        public Customer(babymart_vnEntities _db)
        {
            this._context = _db;
            this._common = new CommonRepository(new babymart_vnEntities());
        }

        public List<khachhang> GetList()
        {
            return (_context.khachhangs.OrderByDescending(a => a.MaKH).ToList());
        }
        public List<khachhangexcel> GetListCustoExcel()
        {
            var query2 = from c in _context.khachhangs
                         orderby c.MaKH descending
                         select new khachhangexcel()
                         {
                             MaKH = c.MaKH,
                             hoten = c.hoten,
                             duong = c.duong,
                             dienthoai = c.dienthoai,
                             email = c.email
                         };
            return query2.ToList(); 
        }
        public IPagedList<khachhang> GetlistCustomerPaging(int? page, int pageSize, out long count, string serach = null)
        {
            count = _context.khachhangs.Count();
            if (serach != null)
            {
                return _context.khachhangs.Where(o =>
                    o.email.Contains(serach)
                    || o.dienthoai.Contains(serach)
                    || o.tendn.Contains(serach)).OrderByDescending(p => p.MaKH).ToPagedList(page ?? 1, pageSize);
            }
            return _context.khachhangs.OrderByDescending(p => p.MaKH).ToPagedList(page ?? 1, pageSize);
        }
        public khachhang GetDetail(int makh)
        {
            return (from c in _context.khachhangs where c.MaKH == makh select c).FirstOrDefault();
        }
        public khachhang GetCustomerByEmail(string email)
        {
            return (from c in _context.khachhangs where c.email.Equals(email) select c).FirstOrDefault();
        }
        public khachhang GetCustomerByUserName(string username)
        {
            return (from c in _context.khachhangs where c.tendn.Equals(username) select c).FirstOrDefault();
        }
        public List<donhang_ct> GetDetailCart(long id)
        {
            return (from c in _context.donhang_ct where c.Sodh.Equals(id) select c).ToList();
        }
        public void Update(khachhang kh)
        {
            _context.Entry(kh).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void UpdateThongTin(khachhang kh)
        {
            //kh.matkhau = _common.Encryptdata(kh.matkhau);
            _context.Entry(kh).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Add(khachhang kh)
        {
            _context.khachhangs.Add(kh);
            _context.SaveChanges();
        }
        public long InsertForFacebook(khachhang entity)
        {
            var user = _context.khachhangs.SingleOrDefault(x => x.email == entity.email);
            if (user == null)
            {
                entity.ngaydangky= DateTime.Now;
                _context.khachhangs.Add(entity);
                _context.SaveChanges();
                return entity.MaKH;
            }
            else
            {
                return user.MaKH;
            }
        }
        public int AddVL(khachhang_vanglai kh)
        {
            _context.khachhang_vanglai.Add(kh);
            _context.SaveChanges();
            return kh.Id;
        }

        public int Register(khachhang kh)
        {
            kh.matkhau = _common.Encryptdata(kh.matkhau);
            kh.ngaydangky = DateTime.Now;
            _context.khachhangs.Add(kh);
            _context.SaveChanges();
            return kh.MaKH;
        }
        public List<donhang_chuyenphat_tp> ListCity()
        {
            var city = (from c in _context.donhang_chuyenphat_tp orderby c.uutien select c).ToList();
            return city;
        }
        public List<donhang_chuyenphat_tinh> ListTinh(int? tp)
        {
            var city = (from c in _context.donhang_chuyenphat_tinh orderby c.tentinh where c.idtp == tp select c).ToList();
            return city;
        }
        public khachhang Login(string tendn, string matkhau)
        {
            var cus = (from c in _context.khachhangs
                       where c.tendn.Equals(tendn)
                           || c.dienthoai.Equals(tendn)
                           || c.email.Equals(tendn)
                       select c).FirstOrDefault();
            if (cus != null)
            {
                var pass = _common.Decryptdata(cus.matkhau);
                if (matkhau == pass)
                {
                    return cus;
                }
            }
            return null;
        }
        public khachhang Loginfacebook(string tendn)
        {
            var cus = (from c in _context.khachhangs
                       where c.tendn.Equals(tendn)
                           || c.dienthoai.Equals(tendn)
                           || c.email.Equals(tendn)
                       select c).FirstOrDefault();
            if (cus != null)
            {
                return cus;
            }
            return null;
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