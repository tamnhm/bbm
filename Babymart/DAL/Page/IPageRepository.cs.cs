using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Page
{
    public interface IPageRepository : IDisposable
    {
        shop_page pageSYS(int id, int storeId = 1000);
        List<shop_page> ListpageAll(int storeId = 1000);
        List<shop_page> ListpageSYS(string type = "", int storeId = 1000);
        List<shop_page> ListpageByType(int storeId = 1000);
        List<shop_page> ListpageByType1(int storeId = 1000);
        int InsertArticle(shop_page model);
        int UpdateArticle(shop_page model);
        void DeleteArticle(int id);
        shop_page Footer(int storeId = 1000);
    }
}