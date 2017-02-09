using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Babymart.Models;
namespace Babymart.DAL.Product
{
    public interface IProductRepository : IDisposable
    {
        //List<Object> GetListSP();
        List<shop_collection> GetListSP(int danhmuc = 0, int danhmuccon = 0, int thuonghieu = 0, int site = -1, int storeId = 1000);
        shop_sanpham GetProductById(int id, int storeId = 1000);
        List<shop_sanpham> GetProduct(int storeId = 1000);
        List<shop_collection> GetProBySaleoff(int storeId = 1000);
        shop_sanpham GetProductByUrl(string url, int storeId = 1000);
        void InsertProduct(shop_sanpham sp);
        void RemoveImg(int id);
        void SetVisible(int id, bool visible);
        void Outset(int id, bool outset);
        void Remove(int id);
        void Saleoff(shop_sanpham sp);
        void UpdateProduct(shop_sanpham sp);

        //BUYER
        List<shop_collection> GetProByCollectionChild(string urldanhmuccha, string urldanhmuccon, int storeId = 1000);
        List<shop_collection> GetProByBrand(string url, int storeId = 1000);
        List<shop_collection> Search(string nameproduct, int storeId = 1000);
        List<shop_thuonghieu> Thuonghieu(List<int> list_id);
        List<shop_danhmuccon> Danhmuccon(List<int> list_id, int storeId = 1000);
        List<shop_collection> GetProductOrther(int iddmc, int storeId = 1000);
        List<shop_sanpham> ProductonHomepage(int storeId = 1000);
        shop_page Detail(string url);
        List<shop_page> Listtd(string type);
        void UpdateBienthe(shop_bienthe bt);
        void InsertBienthe(shop_bienthe bt);
        void RemoveBienthe(int id);
        List<shop_bienthe> listBienthe(int? idsp);
        List<shop_sanpham> ProductTop(string orderby, int take, int storeId = 1000);
        List<shop_sanpham> ProductIndex(int iddm, int take, int storeId = 1000);
        List<shop_danhmuccon> CollectionIndex(int iddm, int storeId = 1000);
        shop_danhmuc CollectionName(int iddm, int storeId = 1000);
        void UpdateListProduct(List<shop_sanpham> sp);
        void SetShowHome(int id);
        void ResetSaleoffProduct();
        List<shop_bienthe> GetListSpForPlan(int typePlan);
        List<shop_sanpham> GetlistProduct(int[] Ids);
        bool CheckingByUrl(string url, int igonId = 0);
        List<shop_collection> GetProByCollectionChildbyListIdProduct(List<int> Ids, int id = 0);
        List<shop_collection> Search_query(string stringquery);

        List<shop_collection> Relatedproduct(string nameproduct, int storeId = 1000);
    }
}