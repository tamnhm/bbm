using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Administrator
{
    public interface IAdministratorRepository : IDisposable
    {
        sys_account_admin GetAdmin(int id);
        admin Administrator(int AdminId);
        List<admin_role> AdminRole(int AdminId);
       
    }
}