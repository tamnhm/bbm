using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models.Module
{
    public class PlanModel
    {
        public int IdProdut { get; set; }
        public int Count { get; set; }
        public int Id { get; set; }
        public int typePlan { get; set; }
        public string tensp { get; set; }
        public string tensp_us { get; set; }
        public string masp { get; set; }
        public string hinhanh { get; set; }
        public int gia { get; set; }
        public int sum { get; set; }
        public string namePlan { get; set; }
        public double kg { get; set; }
    }

    public class PlanBienthe
    {
        public long id { get; set; }
        public int idsp { get; set; }
        public string masp { get; set; }
        public string fullname { get; set; }
        public Nullable<int> gia { get; set; }
        public Nullable<int> giasosanh { get; set; }
        public string spurl { get; set; }
        public string spurl_us { get; set; }
        public string imgsp { get; set; }
        public double kg { get; set; }
        public bool ischeckout { get; set; }
    }
    public class Plan_type
    {
        public int Id { get; set; }
        public string Type
        {
            get;
            set;
        }
        public string Type_us
        {
            get;
            set;
        }
    }
}