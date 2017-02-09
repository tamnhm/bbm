using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelSessionCart
    {
        public long ProductId { get; set; }
        public int Count { get; set; }
    }
    public class ModelCart
    {
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public ModelBienthe shop_bienthe { get; set; }
    }
}