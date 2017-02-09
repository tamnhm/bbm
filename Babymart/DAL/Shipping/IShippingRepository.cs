using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Shipping
{
    public interface IShippingRepository : IDisposable
    {
        List<donhang_chuyenphat_tp> List_Tp();
        donhang_chuyenphat_tp GetTpById(int id);
        List<donhang_chuyenphat_tinh> List_CP_Tinh(int idtp);
        List<donhang_chuyenphat_vung> List_Vung();
        int Insercitys(donhang_chuyenphat_tp tp, List<donhang_chuyenphat_tinh> tinh);
        void UpdateCity(donhang_chuyenphat_tp tp);
        void RemoveCity(int idtp);
        void InsertTinh(int idtp, List<donhang_chuyenphat_tinh> tinh);
        void UpdateTinh(donhang_chuyenphat_tinh tp);
        void RemoveTinh(int idtp);
    
        List<donhang_gio_giaohang> List_Gio();
        List<donhang_chuyenphat_vung> List_CP_Vung(string mavung);
        void RemoveVung(int id);
        void AddVung(donhang_chuyenphat_vung vung);
    }
}