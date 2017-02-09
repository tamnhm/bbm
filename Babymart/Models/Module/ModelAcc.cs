using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelAcc
    {
        [Required(ErrorMessage = "Tên đăng nhập không được trống !")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được trống !")]
        public string Password { get; set; }
    }
}