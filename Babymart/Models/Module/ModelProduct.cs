using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.Models
{
    public class ModelProduct
    {
        public int id { get; set; }

        public string masp { get; set; }
        public string tensp { get; set; }
        public string tensp_us { get; set; }
        public Nullable<int> gia { get; set; }
        public Nullable<int> giakm { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<bool> ischeckout { get; set; }
        public Nullable<bool> ischecksaleoff { get; set; }
        public Nullable<bool> ischeckgift { get; set; }
        public string gift { get; set; }
        public Nullable<System.DateTime> timestart { get; set; }
        public Nullable<System.DateTime> timeend { get; set; }
        public Nullable<int> madanhmuc { get; set; }
        public Nullable<int> madanhmuccon { get; set; }
        public string tenhieu { get; set; }
        public Nullable<int> mahieu { get; set; }
        public Nullable<int> maloai { get; set; }
        public int idloai { get; set; }
        public int iddm { get; set; }
        public int iddmc { get; set; }
        public string thongtin { get; set; }
        public string chitiet { get; set; }
        public string thongtin_us { get; set; }
        public string chitiet_us { get; set; }
        public string img { get; set; }
        public List<ModelBienthe> shop_bienthe { get; set; }
        public List<ModelImage> shop_image { get; set; }
        public ModelThuongHieu shop_thuonghieu { get; set; }
        public double countdown { get; set; }
        public string tendanhmuc { get; set; }
        public string tendanhmucon { get; set; }
        public string tendanhmuc_us { get; set; }
        public string tendanhmucon_us { get; set; }
        public string urldanhmuc { get; set; }
        public string urldanhmuccon { get; set; }
        public string urldanhmuc_us { get; set; }
        public string urldanhmuccon_us { get; set; }
        public string spurl { get; set; }
        public string sptitle { get; set; }
        public string spdescription { get; set; }
        public string spkeywords { get; set; }
        public int countsale { get; set; }
        public Nullable<double> kg { get; set; }
        public Nullable<double> chieudai { get; set; }
        public Nullable<double> chieurong { get; set; }
        public Nullable<double> chieucao { get; set; }
        public Nullable<double> chieudaisd { get; set; }
        public Nullable<double> chieurongsd { get; set; }
        public Nullable<double> chieucaosd { get; set; }
        public Nullable<bool> showsptangkemvaomuckm { get; set; }
        public Nullable<bool> showkg { get; set; }
        public Nullable<bool> showcm { get; set; }
        public Nullable<bool> showhome { get; set; }
        public Nullable<bool> showbanner { get; set; }
        public int plantype { get; set; }
        public string[] Tag { get; set; }
        public List<sys_tags_RefModel> ListTagsArticle { get; set; }
        public string spurl_us { get; set; }
        public string sptitle_us { get; set; }
        public string spdescription_us { get; set; }
        public string spkeywords_us { get; set; }
    }
    public class ModelPropetiesProduct
    {
        public int id { get; set; }

        public string masp { get; set; }
        public string tensp { get; set; }

        public string tensp_us { get; set; }
        public Nullable<int> gia { get; set; }
        public Nullable<int> giakm { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<bool> ischeckout { get; set; }
        public Nullable<bool> ischecksaleoff { get; set; }
        public Nullable<bool> ischeckgift { get; set; }
        public string gift { get; set; }
        public Nullable<System.DateTime> timestart { get; set; }
        public Nullable<System.DateTime> timeend { get; set; }
        public Nullable<int> madanhmuc { get; set; }
        public Nullable<int> madanhmuccon { get; set; }
        public string tenhieu { get; set; }
        public Nullable<int> mahieu { get; set; }
        public Nullable<int> maloai { get; set; }
        public int idloai { get; set; }
        public int iddm { get; set; }
        public int iddmc { get; set; }
        public string thongtin { get; set; }
        public string chitiet { get; set; }
        public string thongtin_us { get; set; }
        public string chitiet_us { get; set; }
        public string img { get; set; }
        public double countdown { get; set; }
        public string spurl { get; set; }
        public Nullable<double> kg { get; set; }
        public Nullable<double> chieudai { get; set; }
        public Nullable<double> chieurong { get; set; }
        public Nullable<double> chieucao { get; set; }
        public Nullable<double> chieudaisd { get; set; }
        public Nullable<double> chieurongsd { get; set; }
        public Nullable<double> chieucaosd { get; set; }
        public Nullable<bool> showsptangkemvaomuckm { get; set; }
        public Nullable<bool> showkg { get; set; }
        public Nullable<bool> showcm { get; set; }
        public Nullable<bool> showhome { get; set; }
        public string sptitle { get; set; }
        public string spdescription { get; set; }
        public string spkeywords { get; set; }
        public Nullable<bool> showbanner { get; set; }
        public Nullable<int> plantype { get; set; }
        public int countsale { get; set; }
        public string spurl_us { get; set; }
        public string sptitle_us { get; set; }
        public string spdescription_us { get; set; }
        public string spkeywords_us { get; set; }
    }
    public class ModelCollection
    {
        public int id { get; set; }
        public Nullable<int> idloai { get; set; }
        public Nullable<int> iddm { get; set; }
        public Nullable<int> iddmc { get; set; }
        public Nullable<int> idsp { get; set; }
        public string lable { get; set; }
        public ModelProduct shop_sanpham { get; set; }
    }
    public class ModelBienthe
    {
        public long id { get; set; }
        public Nullable<int> idsp { get; set; }
        public string title { get; set; }
        public string title_us { get; set; }
        public Nullable<int> gia { get; set; }
        public Nullable<int> giasosanh { get; set; }

        public Nullable<int> giasosanh2 { get; set; }
        public int soluong { get; set; }
        public string masp { get; set; }
        public string tensp { get; set; }
        public string tensp_us { get; set; }
        public string spurl { get; set; }

        public string spurl_us { get; set; }
        public string imgsp { get; set; }
        public Nullable<bool> ischecksaleoff { get; set; }
        public Nullable<double> kg { get; set; }
        public Nullable<double> chieudai { get; set; }
        public Nullable<double> chieurong { get; set; }
        public Nullable<double> chieucao { get; set; }

        public Nullable<double> chieudaisd { get; set; }
        public Nullable<double> chieurongsd { get; set; }
        public Nullable<double> chieucaosd { get; set; }

        public Nullable<bool> showsptangkemvaomuckm { get; set; }

        public bool isdelete { get; set; }

    }
    public class ModelImage
    {
        public int id { get; set; }
        public Nullable<int> RefId { get; set; }
        public string url { get; set; }
        public string alt { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<bool> ImgPrimary { get; set; }
        public Nullable<int> idtype { get; set; }
    }
    public class ModelThuongHieu
    {
        public int mahieu { get; set; }
        public string tenhieu { get; set; }

        public string tenhieu_us { get; set; }
        public string noidung { get; set; }
        public string noidung_us { get; set; }
        public string hinh { get; set; }
        public string url { get; set; }
        public Nullable<int> sort_metro { get; set; }
        public Nullable<bool> showhome { get; set; }
        public string metakeywords { get; set; }
        public string metadescription { get; set; }
        public Nullable<bool> hide { get; set; }
        public Nullable<int> maloai { get; set; }
    }

}