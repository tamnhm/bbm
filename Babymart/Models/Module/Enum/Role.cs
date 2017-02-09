using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module.Enum
{
    public enum RoleAdmin
    {
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Product")]
        Product = 2,
        [Display(Name = "Product_Edit")]
        Product_Edit = 3,
        [Display(Name = "Brand")]
        Brand = 3,
        [Display(Name = "Brand_Edit")]
        Brand_Edit = 4,
        [Display(Name = "LinkList")]
        LinkList = 5,
        [Display(Name = "LinkList_Edit")]
        LinkList_Edit = 6,
        [Display(Name = "Customer")]
        Customer = 7,
        [Display(Name = "Customer_Edit")]
        Customer_Edit = 8,
        [Display(Name = "Order")]
        Order = 9,
        [Display(Name = "Order_Edit")]
        Order_Edit = 10,
        [Display(Name = "Shipping")]
        Shipping = 11,
        [Display(Name = "Shipping_Edit")]
        Shipping_Edit = 12,
        [Display(Name = "Page")]
        Page = 13,
        [Display(Name = "Page_Edit")]
        Page_Edit = 14,
        [Display(Name = "File")]
        File = 15,
        [Display(Name = "File_Edit")]
        File_Edit = 16,
        [Display(Name = "Page_SEO")]
        Page_SEO = 17,
        [Display(Name = "Page_SEO_Edit")]
        Page_SEO_Edit = 18,
        [Display(Name = "PlanSS")]
        PlanSS = 19,
        [Display(Name = "PlanSS_Edit")]
        PlanSS_Edit = 20,
        [Display(Name = "Module")]
        Module = 21,
        [Display(Name = "Module_Edit")]
        Module_Edit = 22
    }
}