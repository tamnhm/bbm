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
    
    public partial class donhang
    {
        public donhang()
        {
            this.donhang_ct = new HashSet<donhang_ct>();
        }
    
        public long id { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> makh { get; set; }
        public Nullable<int> diemsp { get; set; }
        public Nullable<int> vanglai { get; set; }
        public string ghichu { get; set; }
        public string noidung { get; set; }
        public Nullable<int> ship { get; set; }
        public Nullable<long> tongtien { get; set; }
        public Nullable<int> idgiogiao { get; set; }
        public Nullable<System.DateTime> ngaydat { get; set; }
        public Nullable<int> pttt { get; set; }
        public Nullable<int> ptgh { get; set; }
        public Nullable<int> typeconfim { get; set; }
        public Nullable<bool> dagiao { get; set; }
        public Nullable<System.DateTime> ngaygiao { get; set; }
        public Nullable<int> datru_diem { get; set; }
        public Nullable<bool> dahuy { get; set; }
        public Nullable<System.DateTime> ngayhuy { get; set; }
        public string thongtinxedo { get; set; }
        public string tinhtrang { get; set; }
    
        public virtual ICollection<donhang_ct> donhang_ct { get; set; }
        public virtual khachhang khachhang { get; set; }
        public virtual khachhang_vanglai khachhang_vanglai { get; set; }
    }
}
