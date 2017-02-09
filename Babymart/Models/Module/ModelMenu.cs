using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models
{
    public class ModelMenu
    {
        public string tendanhmuc { get; set; }
        public string tendanhmuc_us { get; set; }
        public int madanhmuc { get; set; }
        public IList<shop_danhmuccon> danhmuccon { get; set; }
    }
}