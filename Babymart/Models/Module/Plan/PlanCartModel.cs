using Babymart.Models.Module.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class PlanCartModel
    {
        public List<PlanModel> PlanModel { get; set; }
        public int ToTal { get; set; }
        public int TotalSum { get; set; }
        public int Percent { get; set; }
        public List<PlanGift> Gift { get; set; }
        public string KgTotal { get; set; }

    }
}