using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Babymart.Models.Module.Enum
{
    public enum OrderTypeEnum
    {
        [Display(Name = "Đơn hàng Website")]
        Website = 1,
        [Display(Name = "Đơn hàng POS")]
        POS = 2
    }
    public enum Phuongthucthanhtoan
    {
        [Display(Name = "Tiền Mặt")]
        Money = 1,
        [Display(Name = "Chuyển khoản")]
        payment = 2,
        [Display(Name = "Thanh toán tín dụng")]
        martCard = 3,
        [Display(Name = "Thanh toán Ngân Lượng")]
        martCard1 = 4
    }
    public enum Phuongthucgiaohang
    {
        [Display(Name = "Chuyển phát nhanh")]
        Fax = 1,
        [Display(Name = "Xe đò")]
        Bus = 2
    }
    public enum Thoigiangiaohang
    {
        [Display(Name = "Bất kỳ giờ nào trong ngày.")]
        everytime = 1,
        [Display(Name = "Giờ hành chính")]
        timeworking = 2,
        [Display(Name = "Chỉ thứ 7 hoặc chủ nhật")]
        monday = 3,
        [Display(Name = "Thời gian cụ thể")]
        specialtime = 4
    }
}