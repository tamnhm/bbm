using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace Babymart.Infractstructure
{
    public static class BHNEnumHelper
    {
        public static string ToJson(Type enumType)
        {
            Dictionary<string, string> listEnumField = new Dictionary<string, string>();
            Type type = enumType;
            foreach (var evalue in type.GetEnumValues())
            {
                var valueName = type.GetField(evalue.ToString());
                string displayLabel = "";
                DisplayAttribute displayAtt = valueName.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayAtt != null)
                    displayLabel = displayAtt.GetName();
                listEnumField.Add(evalue.ToString(), displayLabel);
            }
            return JsonConvert.SerializeObject(listEnumField.Select(m => new { Key = m.Key, Value = m.Value }));
        }
        public static string ConvertToJson(Type e)
        {
            var ret = "{";
            foreach (var val in Enum.GetValues(e))
            {
                var name = Enum.GetName(e, val);
                ret += name + ":" + ((int)val).ToString() + ",";
            }
            ret += "}";
            return ret;
        }
    }
}