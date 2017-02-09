using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public partial class sys_tags_RefModel
    {
        public int Id { get; set; }
        public Nullable<int> RefId { get; set; }
        public Nullable<int> RefTag { get; set; }
        public string Tags { get; set; }
    }
    public partial class sys_tags_SummaryModel
    {
        public int Id { get; set; }
        public string Tags { get; set; }
        public Nullable<int> RefType { get; set; }
        public Nullable<int> RefId { get; set; }
    }
}