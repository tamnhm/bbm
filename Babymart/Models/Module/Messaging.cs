using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models
{
    public class RenderMessaging
    {
        public bool success { get; set; }
        public string messaging { get; set; }
        public int id { get; set; }

        public object model { get; set; }
    }
}