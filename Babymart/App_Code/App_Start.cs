using System;
using System.Collections.Generic;
using System.Linq;
using Babymart.Infractstructuree;
using System.Web.Optimization;
namespace Babymart.App_Code
{
    public class App_Start
    {
        public static void AppInitialize()
        {
            Mappers.Boot();
            //BundleTable.EnableOptimizations = true;
        }
    }

}