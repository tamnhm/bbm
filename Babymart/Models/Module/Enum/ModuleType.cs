using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module.Enum
{
    public enum ModuleType
    {
        [Display(Name = "Game")]
        Game = 1,
        [Display(Name = "Âm nhạc")]
        Music = 2,
        [Display(Name = "Hoạt hình")]
        Animation = 3,
        [Display(Name = "Tin Tức")]
        News = 4,
        [Display(Name = "Tạp chí")]
        Magazine = 5


    }
}