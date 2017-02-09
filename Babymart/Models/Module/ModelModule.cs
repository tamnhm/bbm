using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelModuleDetail
    {
        public int id { get; set; }
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
        public string[] Tags { get; set; }
        public string[] TagsProduct { get; set; }
        public Nullable<System.DateTime> createdate { get; set; }
        public Nullable<bool> hide { get; set; }
        //public virtual module_group module_group { get; set; }
        //public virtual module_menu module_menu { get; set; }
        //public virtual ICollection<module_detail_file> module_detail_file { get; set; }
    }
    public class ModelModuleGroup
    {
        public int id { get; set; }
        public string title { get; set; }

        public string title_us { get; set; }
        public string url { get; set; }
        public Nullable<int> idmenu { get; set; }
        public Nullable<int> showhome { get; set; }
        public string des { get; set; }
        public string keyword { get; set; }
        public List<ModelModuleDetail> module_detail { get; set; }
        //public virtual ICollection<module_detail> module_detail { get; set; }
        //public virtual module_menu module_menu { get; set; }
    }
    public class ModelModuleMenu
    {
        public int id { get; set; }
        public string tenloai { get; set; }
        public string tenloai_us { get; set; }
        public Nullable<int> typemodule { get; set; }
        public string url { get; set; }
        public string des { get; set; }
        public string keyword { get; set; }
        public List<ModelModuleGroup> module_group { get; set; }
        //public virtual ICollection<module_detail> module_detail { get; set; }
        //public virtual ICollection<module_group> module_group { get; set; }
    }
    public class ModelModuleGroupAd
    {
        public int id { get; set; }
        public string title { get; set; }
    }
    public class ModelModuleMenuAd
    {
        public int id { get; set; }
        public string tenloai { get; set; }
        public List<ModelModuleGroupAd> module_group { get; set; }
    }
}