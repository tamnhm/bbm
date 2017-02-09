using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module.Enum
{
    public enum Tags
    {
        [Display(Name = "Tag Module Magazine")]
        TagsModuleMagazine = 1,
        [Display(Name = "Tag Product")]
        TagsProduct = 2,
    }
    public enum TagsCollection
    {
        [Display(Name = "Tag Danh mục")]
        TagsListCategories = 1,
        [Display(Name = "Tag Chuyên mục")]
        TagsCategories = 2,
        [Display(Name = "Tag Sản phẩm")]
        TagsProduct = 3,
        [Display(Name = "Tag sản phẩm - Cẩm nang ")]
        TagsProduct_Magazine = 4
    }
}