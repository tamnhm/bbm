using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelOrder
    {
        public string tendn { get; set; }
        public string diem { get; set; }
        public string dienthoai { get; set; }
        public string email { get; set; }
        public long id { get; set; }

        public int? diemsp { get; set; }
        public Nullable<int> makh { get; set; }
        public Nullable<int> vanglai { get; set; }
        public string ghichu { get; set; }
        public Nullable<int> ship { get; set; }
        public Nullable<long> tongtien { get; set; }
        public Nullable<int> idgiogiao { get; set; }
        public Nullable<System.DateTime> ngaygiao { get; set; }
        public Nullable<System.DateTime> ngaydat { get; set; }
        public Nullable<int> pttt { get; set; }
        public Nullable<int> ptgh { get; set; }
        public Nullable<bool> dagiao { get; set; }
        public string thongtinxedo { get; set; }
        public string noidung { get; set; }
        public Nullable<int> typeconfim { get; set; }
        public List<ModelDonhang_ct> donhang_ct { get; set; }

        public Nullable<bool> dahuy { get; set; }
        public Nullable<System.DateTime> ngayhuy { get; set; }
    }
    public class ModelDonhang_ct
    {
        public long SoDH { get; set; }
        public int ID { get; set; }
        public Nullable<int> bienthe { get; set; }
        public Nullable<int> Soluong { get; set; }
        public int Dongia { get; set; }
        public ModelBienthe shop_bienthe { get; set; }
    }
    public class ModelOrderUpdate
    {
        public string tendn { get; set; }
        public string diem { get; set; }
        public string dienthoai { get; set; }
        public string email { get; set; }
        public long id { get; set; }

        public Nullable<int> makh { get; set; }
        public Nullable<int> vanglai { get; set; }
        public string ghichu { get; set; }
        public Nullable<int> ship { get; set; }
        public Nullable<long> tongtien { get; set; }
        public Nullable<int> idgiogiao { get; set; }
        public Nullable<System.DateTime> ngaygiao { get; set; }
        public Nullable<System.DateTime> ngaydat { get; set; }
        public Nullable<int> pttt { get; set; }
        public Nullable<int> ptgh { get; set; }
        public Nullable<bool> dagiao { get; set; }
        public string thongtinxedo { get; set; }
        public string noidung { get; set; }
    }
}