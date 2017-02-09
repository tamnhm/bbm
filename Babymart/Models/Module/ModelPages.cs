using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelPages
    {
        public int id { get; set; }
        public string codetype { get; set; }
        public string hinhanh { get; set; }
        public string tieude { get; set; }
        public string tieude_us { get; set; }
        public string titlemenu { get; set; }
        public string noidung { get; set; }
        public string noidung_us { get; set; }
        public Nullable<System.DateTime> ngayviet { get; set; }
        public Nullable<bool> showmenu { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<bool> islinkout { get; set; }
        public string url { get; set; }
        public Nullable<int> sort { get; set; }
        public string metatitle { get; set; }
        public string keywords { get; set; }
        public string description { get; set; }
    }
}