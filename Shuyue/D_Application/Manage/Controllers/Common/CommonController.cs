using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.Common
{
    public class CommonController : Controller
    {
        [HttpPost]
        public JsonResult Upload()
        {
            ParaModel model = new ParaModel();
            string resPath = "";
            try
            {
                if (Request.QueryString["filesrc"] != "" && Request.QueryString["filesrc"] != null)
                {
                    model.Filesrc = Request.QueryString["filesrc"];
                }
                #region 文件上传处理方法
                if (Request.Files["Filedata"] != null)
                {
                    HttpPostedFileBase file = Request.Files["Filedata"];
                    model.FileName = Request.Files["Filedata"].FileName;
                    model.FileSize = Request.Files["Filedata"].ContentLength;
                    model.FileType = Request.Files["Filedata"].ContentType;
                    //文件后缀
                    string extension = model.FileName.Substring(model.FileName.LastIndexOf('.') + 1).ToLower();
                    //保存文件
                    string filePath = "~/Content/UploadFile/" + (string.IsNullOrEmpty(model.Filesrc) ? "File" : model.Filesrc) + "/";
                    Directory.CreateDirectory(Server.MapPath(filePath));
                    file.SaveAs(Server.MapPath(filePath + model.FileName));
                    resPath = filePath + model.FileName;
                    //暂不处理
                    if (model.FileType.Split('/')[0] == "image")
                    {
                    }
                    else if (model.FileType == "application/msword" || model.FileType == "application/vnd.ms-excel" || model.FileType == "application/octet-stream" || model.FileType == "application/x-shockwave-flash")
                    {
                        if (extension == "doc" || extension == "xls" || extension == "xlsx" || extension == "rar" || extension == "swf")
                        {
                        }
                        else if (extension == "png" || extension == "jpeg" || extension == "jpg" || extension == "gif" || extension == "bmp")
                        {
                            //LogHelper.Wirte(model.FileName + "" + model.FileSize + "" + model.FileType);
                            model.FileType = "image/" + extension;//flash 上传时手动补充上图片格式
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
            return Json(new { Code = 0, Data = resPath }, JsonRequestBehavior.DenyGet);
        }
    }
    

    internal class ParaModel
    {
        public string FileName { get; internal set; }
        public int FileSize { get; internal set; }
        public string Filesrc { get; internal set; }
        public string FileType { get; internal set; }
        public string Href { get; internal set; }
    }
}