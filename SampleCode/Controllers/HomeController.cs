using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Utilities.Attribute;

namespace SampleCode.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 建構函數
        /// </summary>
        public HomeController()
        {
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [WebAuthorize(Code = "Pass")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        [WebAuthorize(Code = "Pass")]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 防止 Session Timeout
        /// </summary>
        /// <returns></returns>
        [WebAuthorize(Code = "Pass")]
        [HttpPost]
        public ActionResult TimeoutNever()
        {
            return Content("");
        }

        /// <summary>
        /// 上傳影像
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [WebAuthorize(Code = "Pass")]
        [HttpPost]
        public ActionResult UploadPicture(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0 && imageUploadVerify(upload))
            {
                //取得上傳圖檔的檔名
                string extension = Path.GetExtension(upload.FileName);
                //儲存路徑
                string fullFolder = Server.MapPath("~/Content/Uploads/Editor/");
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }

                //取得儲存的檔案名稱
                string filename = string.Concat(Guid.NewGuid().ToString(), extension);
                upload.SaveAs(Path.Combine(fullFolder, filename));
                var imageUrl = Url.Content(string.Concat("~/Content/Uploads/Editor/", filename));
                return Json(new
                {
                    uploaded = 1,
                    fileName = upload.FileName,
                    url = imageUrl
                });
            }
            return Content(string.Empty);
        }
    }
}