using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelBanner
    {
        public int Id { get; set; }
        public string Banner { get; set; }
        public string Banner_us { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> Hide { get; set; }
        public string Link { get; set; }
        public string Link_us { get; set; }
    }
}