using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Babymart.Models;
using Babymart.Models.Module;
using Babymart.DAL.Product;
using Babymart.Models.Module.Plan;
using Babymart.Infractstructure;
namespace Babymart.Infractstructuree
{
    public static class Mappers
    {
        public static void Boot()
        {
            Mapper.CreateMap<shop_sanpham, ModelProduct>()
                .ForMember(emp => emp.img, map => map.MapFrom(p => p.shop_image != null ? p.shop_image.FirstOrDefault().url : ""))
                .ForMember(emp => emp.tenhieu, map => map.MapFrom(p => p.shop_thuonghieu != null ? p.shop_thuonghieu.tenhieu : "No Name"))
                 .ForMember(emp => emp.tendanhmuc, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.shop_danhmuc.tendanhmuc))
                 .ForMember(emp => emp.tendanhmucon, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.tendanhmuccon))
                 .ForMember(emp => emp.urldanhmuc, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.shop_danhmuc.url))
                 .ForMember(emp => emp.urldanhmuccon, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.url))
                 .ForMember(emp => emp.urldanhmuccon_us, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.url_us))
                 .ForMember(emp => emp.urldanhmuc_us, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.shop_danhmuc.url_us))

                 .ForMember(emp => emp.tendanhmuc_us, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.shop_danhmuc.tendanhmuc_us))
                  .ForMember(emp => emp.tendanhmucon_us, map => map.MapFrom(p => p.shop_collection.FirstOrDefault().shop_danhmuccon.tendanhmuccon_us));


            Mapper.CreateMap<ModelProduct, shop_sanpham>();
            Mapper.CreateMap<shop_image, ModelImage>();
            Mapper.CreateMap<ModelImage, shop_image>();
            Mapper.CreateMap<shop_bienthe, ModelBienthe>()
                .ForMember(emp => emp.imgsp, map => map.MapFrom(p => p.shop_sanpham.shop_image.FirstOrDefault().url))
                .ForMember(emp => emp.tensp, map => map.MapFrom(p => p.shop_sanpham.tensp))
                .ForMember(emp => emp.masp, map => map.MapFrom(p => p.shop_sanpham.masp))
                .ForMember(emp => emp.kg, map => map.MapFrom(p => p.shop_sanpham.kg))
                .ForMember(emp => emp.chieucao, map => map.MapFrom(p => p.shop_sanpham.chieucao))
                .ForMember(emp => emp.chieudai, map => map.MapFrom(p => p.shop_sanpham.chieudai))
                .ForMember(emp => emp.chieurong, map => map.MapFrom(p => p.shop_sanpham.chieurong))
                .ForMember(emp => emp.ischecksaleoff, map => map.MapFrom(p => p.shop_sanpham.ischecksaleoff))
                ;

            Mapper.CreateMap<ModelBienthe, shop_bienthe>();

            Mapper.CreateMap<ModelCollection, shop_collection>();
            Mapper.CreateMap<shop_thuonghieu, ModelThuongHieu>();
            Mapper.CreateMap<ModelThuongHieu, shop_thuonghieu>();
            Mapper.CreateMap<ModelCollection, shop_collection>();
            Mapper.CreateMap<shop_collection, ModelCollection>();
            Mapper.CreateMap<ModelPropetiesProduct, shop_sanpham>();
            Mapper.CreateMap<shop_sanpham, ModelPropetiesProduct>();

            Mapper.CreateMap<ModeCustomer, khachhang>();
            Mapper.CreateMap<khachhang, ModeCustomer>();
            Mapper.CreateMap<khachhang, ModeCustomerPost>();
            Mapper.CreateMap<ModeCustomerPost, khachhang>();
            Mapper.CreateMap<khachhang_vanglai, ModeCustomerPost>();
            Mapper.CreateMap<ModeCustomerPost, khachhang_vanglai>();
            Mapper.CreateMap<donhang, ModelOrderUpdate>();

            Mapper.CreateMap<ModelOrderUpdate, donhang>();
            Mapper.CreateMap<donhang, ModelOrder>()
                  .ForMember(emp => emp.diem, map => map.MapFrom(p => p.makh != null ? p.khachhang.diem : "0"))
                  .ForMember(emp => emp.tendn, map => map.MapFrom(p => p.makh != null ? p.khachhang.tendn : "Vãng lai"))
                  .ForMember(emp => emp.email, map => map.MapFrom(p => p.makh != null ? p.khachhang.email : p.khachhang_vanglai.email))
                   .ForMember(emp => emp.dienthoai, map => map.MapFrom(p => p.makh != null ? p.khachhang.dienthoai : p.khachhang_vanglai.dienthoai));
            Mapper.CreateMap<donhang_ct, ModelDonhang_ct>();

            Mapper.CreateMap<module_detail, ModelModuleDetail>();
            Mapper.CreateMap<ModelModuleDetail, module_detail>();
            Mapper.CreateMap<module_group, ModelModuleGroup>();
            Mapper.CreateMap<ModelModuleGroup, module_group>();
            Mapper.CreateMap<module_group, ModelModuleGroupAd>();
            Mapper.CreateMap<module_menu, ModelModuleMenu>();
            Mapper.CreateMap<module_menu, ModelModuleMenuAd>();
            Mapper.CreateMap<ModelModuleMenu, module_menu>();


            Mapper.CreateMap<shop_danhmuc, ModelDanhmuc>();
            Mapper.CreateMap<ModelDanhmuc, shop_danhmuc>();
            Mapper.CreateMap<shop_danhmuccon, ModeDanhmuccon>();
            Mapper.CreateMap<ModeDanhmuccon, shop_danhmuccon>();

            Mapper.CreateMap<donhang_chuyenphat_tp, ModelShipping>();
            Mapper.CreateMap<ModelShipping, donhang_chuyenphat_tp>();
            Mapper.CreateMap<donhang_chuyenphat_tinh, ModelShippingTinh>()
                  .ForMember(emp => emp.Phivanchuyen, map => map.MapFrom(p => p.ship));
            Mapper.CreateMap<ModelShippingTinh, donhang_chuyenphat_tinh>()
                .ForMember(emp => emp.ship, map => map.MapFrom(p => p.Phivanchuyen));
            Mapper.CreateMap<ModelShipping, ModelShippingUpdate>();
            Mapper.CreateMap<ModelShippingUpdate, donhang_chuyenphat_tp>();
            Mapper.CreateMap<ModeDonHangCT, donhang_ct>();
            Mapper.CreateMap<donhang_ct, ModeDonHangCT>();
            Mapper.CreateMap<donhang, ModelDonHang>();
            Mapper.CreateMap<ModelDonHang, ModelDonHangPost>();
            Mapper.CreateMap<ModelDonHangPost, donhang>();

            Mapper.CreateMap<ModelContact, khachhang_lienhe>();
            Mapper.CreateMap<ModelPages, shop_page>();
            Mapper.CreateMap<shop_page, ModelPages>();
            Mapper.CreateMap<ModelFile, sys_file>();
            Mapper.CreateMap<sys_file, ModelFile>();
            Mapper.CreateMap<ModelCart, cart>();
            Mapper.CreateMap<cart, ModelCart>();
            Mapper.CreateMap<shop_bienthe, PlanBienthe>()
                  .ForMember(emp => emp.gia, map => map.MapFrom(p => p.gia))
                  .ForMember(emp => emp.masp, map => map.MapFrom(p => p.shop_sanpham.masp))
                  .ForMember(emp => emp.kg, map => map.MapFrom(p => p.shop_sanpham.kg))
                  .ForMember(emp => emp.giasosanh, map => map.MapFrom(p => p.giasosanh))
                  .ForMember(emp => emp.idsp, map => map.MapFrom(p => p.shop_sanpham.id))
                  .ForMember(emp => emp.spurl, map => map.MapFrom(p => p.shop_sanpham.spurl))
                  .ForMember(emp => emp.spurl_us, map => map.MapFrom(p => p.shop_sanpham.spurl_us))
                  .ForMember(emp => emp.ischeckout, map => map.MapFrom(p => p.shop_sanpham.ischeckout))
                  .ForMember(emp => emp.fullname, map => map.ResolveUsing(p =>
                   {
                       if (UtilsBB.HasCookieLangEn() == true)
                       {
                           return p.title.Equals("default") ? p.shop_sanpham.tensp_us : p.shop_sanpham.tensp_us + " - " + p.title_us;
                       }
                       else
                       {
                           return p.title.Equals("default") ? p.shop_sanpham.tensp : p.shop_sanpham.tensp + " - " + p.title;
                       }
                   }))
                // .ForMember(emp => emp.fullname, map => map.MapFrom(p => p.title.Equals("default") ? p.shop_sanpham.tensp : p.shop_sanpham.tensp + " - " + p.title))
                  .ForMember(emp => emp.imgsp, map => map.MapFrom(p => p.shop_sanpham.shop_image.FirstOrDefault().url));
            Mapper.CreateMap<shop_plan_saleoff, Plan_SaleoffModel>();
            Mapper.CreateMap<shop_plan_type, Plan_type>();
            Mapper.CreateMap<Plan_type, shop_plan_type>();
            Mapper.CreateMap<sys_tags_Summary, sys_tags_SummaryModel>();
            Mapper.CreateMap<sys_tags_SummaryModel, sys_tags_Summary>();
            Mapper.CreateMap<sys_tags_RefModel, sys_tags_Ref>();
            Mapper.CreateMap<sys_tags_Ref, sys_tags_RefModel>();
            Mapper.CreateMap<ModelBanner, sys_Banner>();
            Mapper.CreateMap<sys_Banner, ModelBanner>();

        }

    }
}