using Babymart.Models.Module.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models
{
    public class ModelShipping
    {
        public int id { get; set; }
        public string tentp { get; set; }
        public string tentp_us { get; set; }
        public string mavung { get; set; }
        public string thoigian { get; set; }
        public Nullable<int> idtinhtra { get; set; }
        public List<ModelShippingTinh> donhang_chuyenphat_tinh { get; set; }

    }
    public class ModelShippingUpdate
    {
        public int id { get; set; }
        public string tentp { get; set; }
        public string tentp_us { get; set; }
        public string mavung { get; set; }
        public string thoigian { get; set; }
        public Nullable<int> idtinhtra { get; set; }

    }
    public class ModelShippingTinh
    {
        public int id { get; set; }
        public Nullable<int> idtp { get; set; }
        public string tentinh { get; set; }
        public string tentinh_us { get; set; }
        public Nullable<bool> vungsau { get; set; }
        public Nullable<decimal> ship { get; set; }

        public Nullable<decimal> Phivanchuyen { get; set; }
    }
}