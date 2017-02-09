using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.LangdingPage
{
    public interface ILangdingPage : IDisposable
    {
         List<Object> GetList();
         shop_loai GetDetail(int loai);
         void Update(shop_loai t);
    }
}