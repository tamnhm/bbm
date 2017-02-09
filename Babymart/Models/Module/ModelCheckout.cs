using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModeDonHangCT
    {
        public long Id { get; set; }
        public long Sodh { get; set; }
        public long IdPro { get; set; }
        public int Soluong { get; set; }
        public Nullable<int> Dongia { get; set; }
        public Nullable<int> DongiaSS { get; set; }
        public ModelBienthe shop_bienthe { get; set; }
    }
    public class ModelDonHang
    {
        public long id { get; set; }
        public Nullable<int> makh { get; set; }
        public Nullable<int> vanglai { get; set; }
        public int? diemsp { get; set; }
        public string ghichu { get; set; }
        public Nullable<decimal> ship { get; set; }
        public Nullable<decimal> tongtien { get; set; }
        public Nullable<decimal> tiensp { get; set; }
        public Nullable<int> idgiogiao { get; set; }
        public Nullable<int> typeconfim { get; set; }
        public Nullable<System.DateTime> ngaygiao { get; set; }
        public Nullable<System.DateTime> ngaydat { get; set; }
        public Nullable<int> pttt { get; set; }
        public Nullable<int> ptgh { get; set; }
        public Nullable<bool> dagiao { get; set; }
        public string thongtinxedo { get; set; }
        public List<ModeDonHangCT> donhang_ct { get; set; }
        public int? datru_diem { get; set; }
        public string NLpayBankType { get; set; }
        public string BankOnline { get; set; }

    }
    public class ModelDonHangPost
    {
        public long id { get; set; }
        public Nullable<int> makh { get; set; }
        public Nullable<int> vanglai { get; set; }
        public string ghichu { get; set; }
        public int? diemsp { get; set; }
        public Nullable<decimal> ship { get; set; }
        public Nullable<decimal> tongtien { get; set; }
        public Nullable<int> idgiogiao { get; set; }
        public Nullable<int> typeconfim { get; set; }
        public Nullable<System.DateTime> ngaygiao { get; set; }
        public Nullable<System.DateTime> ngaydat { get; set; }
        public Nullable<int> pttt { get; set; }
        public Nullable<int> ptgh { get; set; }
        public Nullable<bool> dagiao { get; set; }
        public string thongtinxedo { get; set; }
        public int? datru_diem { get; set; }
        public string NLpayBankType { get; set; }
        public string BankOnline { get; set; }
    }
}