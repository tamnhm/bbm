using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Plan
{
    public interface IPlanRepository : IDisposable
    {
        List<shop_plan_saleoff> GetlistShop_plan_saleoff();
        List<shop_plan_type> GetlistShop_plan_type();
        int AddTypPlan(shop_plan_type model);
        int UpdateTypPlan(shop_plan_type model);
        void DeleteTypPlan(int id);
       
    }
}