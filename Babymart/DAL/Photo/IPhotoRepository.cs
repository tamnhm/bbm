using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Babymart.Models;
namespace Babymart.DAL.Photo
{
    public interface IPhotoRepository : IDisposable
    {
        shop_image InsertImg(shop_image img);
        void RemoveImg(int id);
        void UpdateImh(shop_image t);

    }
}