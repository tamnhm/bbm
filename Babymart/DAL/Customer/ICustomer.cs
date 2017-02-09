using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
using PagedList;
using Babymart.Infractstructuree;
namespace Babymart.DAL.Customer
{
    public interface ICustomer : IDisposable
    {
        List<khachhang> GetList();
        List<khachhangexcel> GetListCustoExcel();
        int Register(khachhang kh);
        khachhang Login(string tendn, string matkhau);
        khachhang Loginfacebook(string tendn);
        void Update(khachhang kh);
        void UpdateThongTin(khachhang kh);
        void Add(khachhang kh);
        long InsertForFacebook(khachhang entity);
        int AddVL(khachhang_vanglai kh);
        List<donhang_ct> GetDetailCart(long id);
        khachhang GetDetail(int makh);
        List<donhang_chuyenphat_tp> ListCity();
        List<donhang_chuyenphat_tinh> ListTinh(int? tp);
        khachhang GetCustomerByUserName(string username);
        khachhang GetCustomerByEmail(string email);
        IPagedList<khachhang> GetlistCustomerPaging(int? page, int pageSize, out long count, string serach = null);

    }
}