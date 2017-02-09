using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Customer
{
    public interface IFiles : IDisposable
    {
        sys_file GetFile(int id, int storeId = 1000);
        List<sys_file> GetList();
        void Add(sys_file file);
        void Remove(int id);
        void Update(sys_file file);
        List<sys_file> GetListByType(int type, int storeId = 1000);

    }
}