using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelDanhMucCon
    {
        public int madanhmuccon { get; set; }
        public string tendanhmuccon { get; set; }
        public string tendanhmuccon_us { get; set; }
        public Nullable<int> madanhmuc { get; set; }
        public Nullable<bool> hide { get; set; }
        public string noidung { get; set; }
        public string noidung_us { get; set; }
        public string hinhanh { get; set; }
        public string metatitle { get; set; }
        public string metakeywords { get; set; }
        public string metadescription { get; set; }
        public string url { get; set; }
        public string color { get; set; }
        public Nullable<int> sort { get; set; }
        public string css { get; set; }
        public Nullable<int> ma_loai { get; set; }
        public Nullable<int> uutien { get; set; }

        public virtual ICollection<shop_collection> shop_collection { get; set; }
    }
}