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
    
    public partial class khachhang
    {
        public khachhang()
        {
            this.donhangs = new HashSet<donhang>();
        }
    
        public int MaKH { get; set; }
        public Nullable<int> idtp { get; set; }
        public Nullable<int> idquan { get; set; }
        public string hoten { get; set; }
        public string duong { get; set; }
        public string dienthoai { get; set; }
        public string email { get; set; }
        public string tendn { get; set; }
        public string matkhau { get; set; }
        public string diem { get; set; }
        public bool konhanmail { get; set; }
        public Nullable<System.DateTime> ngaydangky { get; set; }
    
        public virtual ICollection<donhang> donhangs { get; set; }
    }
}
