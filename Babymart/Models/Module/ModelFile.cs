using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelFile
    {
        public int Id { get; set; }
        public string Files { get; set; }
        public string Files_us { get; set; }
        public int type { get; set; }
        public string link { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<int> sort { get; set; }
    }
}