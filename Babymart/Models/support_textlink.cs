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
    
    public partial class support_textlink
    {
        public int ma { get; set; }
        public string textlink { get; set; }
        public Nullable<int> maloai { get; set; }
        public Nullable<int> vetinh { get; set; }
    
        public virtual shop_loai shop_loai { get; set; }
    }
}
