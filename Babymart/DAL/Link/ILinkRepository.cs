using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Link
{
    public interface ILinkRepository : IDisposable
    {
        List<shop_danhmuccon> ListDanhmuccon(int iddanhmuc, int storeId = 1000);
        List<shop_danhmuc> Getdanhmuc(int loai = 0, int storeId = 1000);
        void InsertLink(shop_danhmuc dm);
        void UpdateLink(shop_danhmuc dm);
        void RemoveLink(int id);
        void InsertLinkCon(shop_danhmuccon dm);
        void UpdateLinkCon(shop_danhmuccon dm);
        List<shop_danhmuccon> Getdanhmuccon(int madanhmuccha, int storeId = 1000);
        void RemoveLinkCon(int id);
        List<shop_danhmuc> GetdanhmucgforAd(int storeId = 1000);
        List<shop_danhmuccon> GetdanhmucconAll(int madanhmuccha, int storeId = 1000);

    }
}