using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelDanhmuc
    {
        public int madanhmuc { get; set; }
        public string tendanhmuc { get; set; }
        public string tendanhmuc_us { get; set; }
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
        public List<ModeDanhmuccon> shop_danhmuccon { get; set; }
        public string noidung { get; set; }
        public string noidung_us { get; set; }
        public Nullable<int> sort { get; set; }
        public string background { get; set; }
        public string icon { get; set; }
        public string css { get; set; }

        public List<sys_tags_RefModel> ListTagsArticle { get; set; }
    }
    public class ModeDanhmuccon
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

        public string url_us { get; set; }
        public string color { get; set; }
        public Nullable<int> sort { get; set; }
        public string css { get; set; }
        public Nullable<int> ma_loai { get; set; }
        public Nullable<int> uutien { get; set; }
        public string grouplink { get; set; }

        public string grouplink_us { get; set; }
        public List<sys_tags_RefModel> ListTagsArticle { get; set; }
        public string metatitle_us { get; set; }
        public string metakeywords_us { get; set; }
        public string metadescription_us { get; set; }
    }
    public class ModelDanhmuc_Group
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string name_us { get; set; }
        public string url { get; set; }
        public string url_us { get; set; }
        public string background { get; set; }
        public string icon { get; set; }
        public Nullable<int> sort { get; set; }
        public Nullable<bool> hide { get; set; }
        public string noidung { get; set; }
        public string noidung_us { get; set; }
    }
}