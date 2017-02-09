using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Babymart.Models;
using PagedList;
namespace Babymart.DAL.Order
{
    public interface IOrderRepository : IDisposable
    {
        List<donhang> Getlist();
        void Update(donhang dh);
        IPagedList<donhang> GetlistOrderPaging(int? page, int pageSize, int serach = 0);
    }
}