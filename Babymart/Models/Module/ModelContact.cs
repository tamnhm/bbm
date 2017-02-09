using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class ModelContact
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Họ tên không được trống.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được trống.")]
        public string Phone { get; set; }
        public string Mail { get; set; }
        [Required(ErrorMessage = "Nội dung không được trống.")]
        public string Contents { get; set; }
    }
}