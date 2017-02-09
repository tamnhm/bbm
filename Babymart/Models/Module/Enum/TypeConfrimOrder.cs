using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module.Enum
{
    public enum TypeConfrimOrder
    {
        [Display(Name = "Babymart.vn gọi điện thoại xác nhận trực tiếp")]
        Phone = 1,
        [Display(Name = "Xác nhận qua tin nhắn")]
        Message = 2,
        [Display(Name = "Không cần xác nhận (babymart.vn sẽ giao hàng theo thông tin và thời gian đã định).")]
        Nothing = 3,

    }
}