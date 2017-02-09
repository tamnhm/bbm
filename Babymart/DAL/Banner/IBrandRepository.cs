using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Brand
{
    public interface IBannerRepository : IDisposable
    {
        List<sys_Banner> ListBanner(int type = 0);
        List<sys_Banner> ListBannerAll(int type = 0);
        void SetVisible(int id, bool visible);
        void Remove(int id);
        int Insert(sys_Banner t);
        sys_Banner BannerDetail(int Id);
        void Update(sys_Banner t);   
    }
}