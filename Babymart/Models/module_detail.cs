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
    
    public partial class module_detail
    {
        public int id { get; set; }
        public Nullable<int> StoreId { get; set; }
        public string title { get; set; }
        public string title_us { get; set; }
        public Nullable<int> groupid { get; set; }
        public Nullable<int> typlemodule { get; set; }
        public Nullable<int> menuid { get; set; }
        public string extract { get; set; }
        public string extract_us { get; set; }
        public string contents { get; set; }
        public string contents_us { get; set; }
        public string img { get; set; }
        public string description { get; set; }
        public string keyword { get; set; }
        public string url { get; set; }
        public Nullable<System.DateTime> createdate { get; set; }
        public Nullable<bool> hide { get; set; }
    
        public virtual module_group module_group { get; set; }
        public virtual module_menu module_menu { get; set; }
    }
}
