using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
using System.Data;
using AutoMapper;
namespace Babymart.DAL.Shipping
{
    public class ShippingRepository : IShippingRepository
    {
        private babymart_vnEntities _context;
        public ShippingRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
       
        public List<donhang_chuyenphat_tp> List_Tp()
        {
            var query = (from tp in _context.donhang_chuyenphat_tp
                         orderby tp.uutien
                         select tp
                        ).ToList();

            return query;
        }

        public List<donhang_chuyenphat_vung> List_Vung()
        {
            var query = (from vung in _context.donhang_chuyenphat_vung
                         
                         select vung
                         ).ToList();
            return query;
        }
        public List<donhang_gio_giaohang> List_Gio()
        {
            var query = (from vung in _context.donhang_gio_giaohang
                         select vung
                         ).ToList();
            return query;
        }
        public donhang_chuyenphat_tp GetTpById(int id)
        {
            var query = (from p in _context.donhang_chuyenphat_tp select p).Where(p => p.id == id).FirstOrDefault();
            return query;


        }

        public List<donhang_chuyenphat_vung> List_CP_Vung(string mavung)
        {
            var query = (from vung in _context.donhang_chuyenphat_vung
                         where vung.mavung.Equals(mavung)
                         select vung
                         ).ToList();
            return query;
        }
        public List<donhang_chuyenphat_tinh> List_CP_Tinh(int idtp)
        {
            var query = (from pro in _context.donhang_chuyenphat_tinh
                         where pro.idtp == idtp
                         select pro).ToList();
            return query;
        }
        public List<Object> List_CP_Vung(int mavung)
        {
            var query = (from pro in _context.donhang_chuyenphat_vung
                         // where pro.mavung == mavung
                         select pro).ToList<Object>();
            return query;
        }
        public int addcity(donhang_chuyenphat_tp tp)
        {
            var city = _context.donhang_chuyenphat_tp.Add(tp);
            _context.SaveChanges();
            return city.id;
        }
        public int Insercitys(donhang_chuyenphat_tp tp, List<donhang_chuyenphat_tinh> tinh)
        {
            var idtp = addcity(tp);
            if (tinh != null && tinh.Count > 0)
            {
                foreach (var item in tinh)
                {
                    item.idtp = idtp;
                    _context.donhang_chuyenphat_tinh.Add(item);
                }
                _context.SaveChanges();
            }

            return idtp;

        }

        public void UpdateCity(donhang_chuyenphat_tp tp)
        {
            _context.Entry(tp).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void RemoveCity(int idtp)
        {
            donhang_chuyenphat_tp item = _context.donhang_chuyenphat_tp.Find(idtp);
            var tinh = List_CP_Tinh(idtp);
            if (tinh != null && tinh.Count > 0)
            {
                foreach (var i in tinh)
                    RemoveTinh(i.id);
            }
            _context.donhang_chuyenphat_tp.Remove(item);
            _context.SaveChanges();
        }
        public void InsertTinh(int idtp, List<donhang_chuyenphat_tinh> tinh)
        {
            if (tinh.Count > 0)
            {
                foreach (var item in tinh)
                {
                    item.idtp = idtp;
                    _context.donhang_chuyenphat_tinh.Add(item);
                }
            }
            _context.SaveChanges();
        }
        public void UpdateTinh(donhang_chuyenphat_tinh tp)
        {
            _context.Entry(tp).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void RemoveTinh(int idtp)
        {
            donhang_chuyenphat_tinh item = _context.donhang_chuyenphat_tinh.Find(idtp);
            _context.donhang_chuyenphat_tinh.Remove(item);
            _context.SaveChanges();
        }
        public void RemoveVung(int id)
        {
            donhang_chuyenphat_vung item = _context.donhang_chuyenphat_vung.Find(id);
            _context.donhang_chuyenphat_vung.Remove(item);
            _context.SaveChanges();
        }
        public void AddVung(donhang_chuyenphat_vung vung)
        {
            _context.donhang_chuyenphat_vung.Add(vung);
            _context.SaveChanges();
        }

        //public void InsertQuan(quan q)
        //{
        //    _context.quans.Add(q);
        //    _context.SaveChanges();
        //}
        //public void UpdateQuan(quan q)
        //{
        //    _context.Entry(q).State = EntityState.Modified;
        //    _context.SaveChanges();
        //}
        //public void RemoveQuan(int idq)
        //{
        //    quan item = _context.quans.Find(idq);
        //    _context.quans.Remove(item);
        //    _context.SaveChanges();
        //}

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