using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Collection
{
    public interface IContentsSEO : IDisposable
    {
        List<sys_content> GetList(int StoreId = 1000);
        void UpdateSys_content(sys_content model);
    }
}