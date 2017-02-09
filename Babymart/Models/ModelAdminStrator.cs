using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models
{
  

    public partial class adminModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string Roles { get; set; }
    }
    public partial class admin_roleModel
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public int Role { get; set; }
        public string Note { get; set; }
    }
}