using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Module
{
    public interface IModuleRepository : IDisposable
    {
        List<module_group> Index(int typlemodule, int storeId = 1000);
        List<module_detail> ListArticleChild(string urlmenu, string urlgroup, int typlemodule, int storeId = 1000);
        List<module_group> ListArticle(string urlmenu, int typlemodule, int storeId = 1000);
        module_detail Article(string url, int id, string urlgroup, int typlemodule, int storeId = 1000);
        List<module_detail> ListModuleArticle(int groupid, int storeId = 1000);
        List<module_group> ListGroup(int idmenu = 0, int storeId = 1000);
        List<module_menu> ListMenu(int typemodule = 0, int storeId = 1000);
        void InsertArticle(module_detail model);
        void UpdateArticle(module_detail model);
        void DeleteArticle(int id);
        void InsertGroup(module_group model);
        void UpdateGroup(module_group model);
        void DeleteGroup(int id);
        int InsertMenu(module_menu model);
        void UpdateMenu(module_menu model);
        void DeleteMenu(int id);
        module_detail GetArticle(int id, int storeId = 1000);
        List<module_detail> GetListArticlebyIds(int[] Ids);
        List<module_detail> Search_query(string stringquery);
    }
}