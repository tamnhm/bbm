using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Babymart.Models.Module
{
    public class ModeCustomer
    {
        public int MaKH { get; set; }
        public Nullable<int> idtp { get; set; }
        public Nullable<int> idquan { get; set; }
        [Required(ErrorMessage = "Tên không được bỏ trống .")]
        public string hoten { get; set; }
        [Required(ErrorMessage = "Tên đường được bỏ trống .")]
        public string duong { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống .")]
        public string dienthoai { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống .")]
        public string email { get; set; }
        public string tendn { get; set; }
        public string matkhau { get; set; }
        public string diem { get; set; }

        public bool konhanmail { get; set; }
        public List<ModelOrder> donhangs { get; set; }
    }
    public class ModeCustomerPost
    {
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
        public string diemsp { get; set; }
        public Nullable<System.DateTime> ngaydangky { get; set; }

    }
}