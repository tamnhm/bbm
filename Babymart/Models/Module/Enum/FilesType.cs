using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module.Enum
{

    public enum FilesType
    {
        [Display(Name = "Tất cả")]
        All = 0,
        [Display(Name = "Banner Home")]
        BannerHome = 1,
        [Display(Name = "Banner Left")]
        BannerLeft = 2,
    }
}