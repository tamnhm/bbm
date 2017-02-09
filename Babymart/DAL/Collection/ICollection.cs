using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Collection
{
    public interface ICollection : IDisposable
    {
        void Add(shop_collection c);
        List<Object> GetListbyproid(int ?id);
        void Remove(int id);
    }
}