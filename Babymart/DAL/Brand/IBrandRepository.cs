using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Brand
{
    public interface IBrandRepository : IDisposable
    {
        List<Object> GetList(int StoreId = 1000);
        shop_thuonghieu GetBrandById(int id, int StoreId = 1000);
        void SetVisible(int id, bool visible);
        void SetShowHome(int id, bool showhome);
        void Remove(int id);
        int Insert(shop_thuonghieu t);
        void Update(shop_thuonghieu t);
        List<shop_thuonghieu> TopBrand(int count, int StoreId = 1000);
    }
}