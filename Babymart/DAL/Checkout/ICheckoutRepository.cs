using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Checkout
{
    public interface ICheckoutRepository : IDisposable
    {
        long AddOrder(donhang model);
        void AddOrder_CT(List<donhang_ct> model);
        void UpdateOrderContent(long Id, string contents);
        void UpdateOrderContentVnPay(long Id,string rspcode_vnp);
    }
}