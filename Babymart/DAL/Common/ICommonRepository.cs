using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Common
{
    public interface ICommonRepository : IDisposable
    {
        List<shop_rightcol> ColumnRight(int StoreId = 1000);
        List<module_menu> MenuModule(int type, int StoreId = 1000);
        List<module_detail> ArticlesRelated(int? idmenu, int typlemodule, int numberget, int StoreId = 1000);
        List<module_group> MenuModuleGroup(int idmenu, int StoreId = 1000);
        string Encryptdata(string password);
        string Decryptdata(string encryptpwd);
        string SplitString(string s, int length);
        void SendEmail(string address, string subject, string message);
        string RandomString(int length);
    }
}