//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Babymart.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class shop_danhmuc
    {
        public shop_danhmuc()
        {
            this.shop_danhmuccon = new HashSet<shop_danhmuccon>();
        }
    
        public int madanhmuc { get; set; }
        public Nullable<int> groupId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public string tendanhmuc { get; set; }
        public string tendanhmuc_us { get; set; }
        public string noidung { get; set; }
        public string noidung_us { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<int> maloai { get; set; }
        public string meta_keyword { get; set; }
        public string meta_description { get; set; }
        public string meta_title { get; set; }
        public string meta_keyword_us { get; set; }
        public string meta_description_us { get; set; }
        public string meta_title_us { get; set; }
        public string url { get; set; }
        public string url_us { get; set; }
        public string background { get; set; }
        public int sort { get; set; }
        public string icon { get; set; }
        public string css { get; set; }
    
        public virtual shop_loai shop_loai { get; set; }
        public virtual ICollection<shop_danhmuccon> shop_danhmuccon { get; set; }
    }
}
