using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Babymart.Models;
using Babymart.DAL;
using Babymart.DAL.Common;

using Babymart.DAL.Photo;
using Babymart.DAL.Customer;
namespace Babymart.Areas.Admin.Controllers
{
    public class CommonController : Controller
    {
        private ICommonRepository _commonRepository;
        private IPhotoRepository _imageRepository;
        private IFiles _files;
        public CommonController()
        {
            this._commonRepository = new CommonRepository(new babymart_vnEntities());
            this._imageRepository = new PhotoRepository(new babymart_vnEntities());
            this._files = new Files(new babymart_vnEntities());
        }

        [HttpPost]
        public JsonResult InsertImg(int refid, int typeid, string url)
        {

            var Messaging = new RenderMessaging();

            try
            {
                var i = new shop_image();
                i.RefId = refid;
                i.RefId = refid;
                //i.idtype = typeid;
                i.url = url;

                Messaging.model = _imageRepository.InsertImg(i);
                Messaging.success = true;
                Messaging.messaging = "Thêm ảnh thành công";

            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult UpdateImg(shop_image img)
        {

            var Messaging = new RenderMessaging();

            try
            {
                _imageRepository.UpdateImh(img);
                Messaging.success = true;
                Messaging.messaging = "Cập nhật thông tin ảnh thành công";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult RemoveImage(int idimg)
        {

            var Messaging = new RenderMessaging();

            try
            {
                _imageRepository.RemoveImg(idimg);
                Messaging.success = true;
                Messaging.messaging = "Xóa ảnh thành công !";
            }
            catch
            {
                Messaging.success = false;
                Messaging.messaging = "Do sự cố mạng, vui lòng thử lại !";
            }
            return Json(Messaging, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            string namefile = null;
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Images/hinhdulieu/upload");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        string titleFile = myFile.FileName.ToString();
                        string fullPath = Request.MapPath("~/Images/hinhdulieu/upload/" + titleFile);
                        if (System.IO.File.Exists(fullPath))
                        {
                            var f = new sys_file();
                            f.Files = titleFile;
                            //if (map.Equals("~/"))
                            //f.type = 10;
                            _files.Add(f);
                            namefile = myFile.FileName.ToString();
                            isUploaded = true;
                            message = "Upload hình thành công !";
                        }
                        else
                        {
                            isUploaded = false;
                            message = string.Format("Upload hình không thành công");
                        }
                    }
                    catch (Exception ex)
                    {
                        isUploaded = false;
                        message = string.Format("Lỗi upload hình : {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message, namefile = namefile }, "text/html");
        }
        [HttpPost]
        public virtual ActionResult UploadFileProduct()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            string namefile = null;
            if (myFile != null && myFile.ContentLength != 0)
            {

                string pathForSaving = Server.MapPath("~/Images/hinhdulieu/data");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "Upload hình thành công !";
                        namefile = myFile.FileName.ToString();
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Lỗi upload hình : {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message, namefile = namefile }, "text/html");
        }
        [HttpPost]
        public virtual ActionResult UploadFileProductThumbnail()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            string namefile = null;
            if (myFile != null && myFile.ContentLength != 0)
            {

                string pathForSaving = Server.MapPath("~/Images/hinhdulieu/thumbnail");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "Upload hình thành công !";
                        namefile = myFile.FileName.ToString();
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Lỗi upload hình : {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message, namefile = namefile }, "text/html");
        }
        [HttpPost]
        public virtual ActionResult UploadFileBrand()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            string namefile = null;
            if (myFile != null && myFile.ContentLength != 0)
            {

                string pathForSaving = Server.MapPath("~/Images/hinhdulieu/nhanhieu");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "Upload hình thành công !";
                        namefile = myFile.FileName.ToString();
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Lỗi upload hình : {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message, namefile = namefile }, "text/html");
        }
        [HttpPost]
        public virtual ActionResult UploadFileModule()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";
            string namefile = null;
            if (myFile != null && myFile.ContentLength != 0)
            {

                string pathForSaving = Server.MapPath("~/Images/hinhdulieu/module/thumbnail");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "Upload hình thành công !";
                        namefile = myFile.FileName.ToString();
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Lỗi upload hình : {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message, namefile = namefile }, "text/html");
        }
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }


    }
}
