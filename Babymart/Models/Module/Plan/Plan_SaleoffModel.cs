using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module.Plan
{
    public class Plan_SaleoffModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string OperatorOption { get; set; }
        public int OperatorValue { get; set; }
        public bool Visible { get; set; }
    }
}