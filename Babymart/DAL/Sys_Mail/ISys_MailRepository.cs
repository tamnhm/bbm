using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Sys_Mail
{
    public interface ISys_MailRepository : IDisposable
    {

        sys_mail Getmail(string type);
    }
}