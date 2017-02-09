using Babymart.Infractstructure;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Module
{
    public class ModuleRepository : IModuleRepository
    {
        private babymart_vnEntities _context;
        public ModuleRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<module_group> Index(int typlemodule, int storeId = 1000)
        {

            List<module_group> db = (from _module in _context.module_group
                                     where _module.module_menu.typemodule == typlemodule
                                      && _module.module_detail.Count > 0
                                     select _module).ToList();
            return db;
        }
        public List<module_detail> GetListArticlebyIds(int[] Ids)
        {
            return _context.module_detail.Where(x => Ids.Contains(x.id)).OrderByDescending(o=>o.createdate).Take(10).ToList();
        }
        public List<module_group> ListArticle(string urlmenu, int typlemodule, int storeId = 1000)
        {

            var db = (from _module in _context.module_group
                      where _module.module_menu.url.Contains(urlmenu) && _module.module_menu.typemodule == typlemodule
                      select _module);
            return db.ToList();
        }

        public List<module_detail> ListArticleChild(string urlmenu, string urlgroup, int typlemodule, int storeId = 1000)
        {
            //_module.module_group.module_menu.url.Contains(urlmenu) &&
            var db = (from _module in _context.module_detail
                      where _module.module_group.url.Contains(urlgroup) && _module.typlemodule == typlemodule
                      && _module.hide == false
                      && _module.StoreId == storeId
                      orderby _module.title
                      select _module);
            return db.ToList();
        }
        public module_detail Article(string url, int id, string urlgroup, int typlemodule, int storeId = 1000)
        {
            return (from _module in _context.module_detail
                    where _module.id == id && _module.url.Contains(url)
                    && _module.typlemodule == typlemodule
                    && _module.hide == false
                    && _module.StoreId == storeId
                    select _module).FirstOrDefault();
        }

        /**************************Admin*****************************/
        public module_detail GetArticle(int id, int storeId = 1000)
        {
            var db = (from _module in _context.module_detail
                      where _module.id == id && _module.StoreId == storeId
                      select _module).FirstOrDefault();
            return db;
        }
        public List<module_detail> ListModuleArticle(int groupid, int storeId = 1000)
        {
            var db = (from _module in _context.module_detail
                      where _module.groupid == groupid
                      && _module.StoreId == storeId
                      select _module);
            return db.ToList();
        }
        public List<module_group> ListGroup(int idmenu = 0, int storeId = 1000)
        {
            var db = (from _module in _context.module_group
                      where _module.StoreId == storeId
                      select _module);
            if (idmenu != 0)
                db = db.Where(p => p.idmenu == idmenu);
            return db.ToList();
        }
        public List<module_menu> ListMenu(int typemodule = 0, int storeId = 1000)
        {
            var db = _context.module_menu.Where(o => o.StoreId == storeId).ToList();
            if (typemodule != 0)
                db = db.Where(p => p.typemodule == typemodule).ToList();

            return db;
        }
        public void InsertArticle(module_detail model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.module_detail.Add(model);
            _context.SaveChanges();
        }
        public void UpdateArticle(module_detail model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteArticle(int id)
        {
            module_detail item = _context.module_detail.Find(id);
            _context.module_detail.Remove(item);
            _context.SaveChanges();
        }
        /**********************************/
        public void InsertGroup(module_group model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.module_group.Add(model);
            _context.SaveChanges();
        }
        public void UpdateGroup(module_group model)
        {
            var data = _context.module_group.Find(model.id);
            data.title = model.title;
            data.url = model.url;
            data.showhome = model.showhome;
            data.des = model.des;
            data.keyword = model.keyword;
            data.StoreId = UtilsBB.GetStoreId();
            data.title_us = model.title_us;
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteGroup(int id)
        {
            module_group item = _context.module_group.Find(id);
            _context.module_group.Remove(item);
            _context.SaveChanges();
        }

        /*************************************************/
        public int InsertMenu(module_menu model)
        {
            model.StoreId = UtilsBB.GetStoreId();
            _context.module_menu.Add(model);
            _context.SaveChanges();
            return model.id;
        }
        public void UpdateMenu(module_menu model)
        {
            var data = _context.module_menu.Find(model.id);
            data.tenloai = model.tenloai;
            data.url = model.url;
            data.des = model.des;
            data.keyword = model.keyword;
            data.StoreId = UtilsBB.GetStoreId();
            data.tenloai_us = model.tenloai_us;
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteMenu(int id)
        {
            module_menu item = _context.module_menu.Find(id);
            _context.module_menu.Remove(item);
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
        public List<module_detail> Search_query(string stringquery)
        {
            var query = _context.module_detail.Where(o => o.title.Contains(stringquery));
            return query.ToList();
        }
    }
}