using MultipartDataMediaFormatter.Infrastructure;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using SampleCode.Models;
using SampleCode.ViewModel;
using Utilities.Utility;

namespace SampleCode.Controllers
{
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 紀錄
        /// </summary>
        protected static Logger logger { get; set; }

        /// <summary>
        /// 不允許的附檔名清單
        /// </summary>
        protected static readonly List<string> notAllowExts = new List<string>
        {
            ".aspx", ".ashx", ".asmx", ".cs", ".vb", ".py"
        };

        /// <summary>
        /// 取得目前時間
        /// </summary>
        public DateTime NowTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 取得今天日期
        /// </summary>
        public DateTime Today
        {
            get
            {
                return new DateTime(NowTime.Year, NowTime.Month, NowTime.Day, 0, 0, 0);
            }
        }

        /// <summary>
        /// 處理附件檔案上傳
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="genThumbImage"></param>
        protected static void fileAttachedHandler(PostedFileBaseModel viewModel, Guid parent, string path, string type, bool genThumbImage = true)
        {
            //處理上傳的檔案
            if (viewModel.HttpFiles != null && viewModel.HttpFiles.Count > 0)
            {
                for (int i = 0; i < viewModel.HttpFiles.Count; i++)
                {
                    var file = viewModel.HttpFiles[i];
                    if (uploadFile(file, path, out string result, out string extension, out bool isImage, out Guid id, genThumbImage))
                    {
                        viewModel.NewAttachedFiles.Add(new FileAttached
                        {
                            ID = id,
                            ParentID = parent,
                            TableType = type,
                            ItemType = viewModel.PostedCategory.Count > i ? viewModel.PostedCategory[i] ?? string.Empty : string.Empty,
                            FileName = file.FileName,
                            FileExtension = extension,
                            FilePath = result,
                            FileSize = file.Buffer.Length,
                            IsImageFile = isImage,
                            IsThumbImage = isImage && genThumbImage,
                            Description = viewModel.PostedCaption.Count > i ? viewModel.PostedCaption[i] ?? string.Empty : string.Empty,
                            CreateTime = DateTime.Now
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="filenames"></param>
        protected static void deleteFiles(List<string> filenames)
        {
            if (filenames != null && filenames.Count > 0)
            {
                foreach (var file in filenames)
                {
                    string fullPath = HttpContext.Current.Server.MapPath(string.Concat("~", file));
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                }
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ids"></param>
        protected static void deleteFiles(string path, List<Guid> ids)
        {
            if (!string.IsNullOrEmpty(path) && ids != null && ids.Count > 0)
            {
                string fullPath = HttpContext.Current.Server.MapPath(path);
                if (Directory.Exists(fullPath))
                {
                    foreach (var id in ids)
                    {
                        string[] files = Directory.GetFiles(fullPath, $"{id}*");
                        if (files.Length > 0)
                            foreach (var file in files)
                                File.Delete(file);
                    }
                }
            }
        }

        /// <summary>
        /// 上傳影像
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected static string uploadImage(HttpFile file, string folder, string fileName)
        {
            if (imageUploadVerify(file))
            {
                //取得上傳的副檔名
                string extension = Path.GetExtension(file.FileName);
                //儲存路徑
                string path = string.Concat(folder, fileName, extension);
                string fullFolder = Path.Combine(HttpContext.Current.Server.MapPath(folder));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(path), file.Buffer);
                return path.Substring(1);
            }
            return string.Empty;
        }

        /// <summary>
        /// 上傳影像
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="result"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected static bool uploadImage(HttpFile file, string folder, string fileName, out string result, out int width, out int height)
        {
            if (imageUploadVerify(file))
            {
                //取得上傳的副檔名
                string extension = Path.GetExtension(file.FileName);
                //儲存路徑
                string path = string.Concat(folder, fileName, extension);
                string fullFolder = Path.Combine(HttpContext.Current.Server.MapPath(folder));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(path), file.Buffer);
                result = path.Substring(1);
                //取得影像的大小
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(file.Buffer)))
                {
                    width = image.Width;
                    height = image.Height;
                }
                return true;
            }
            result = string.Empty;
            width = 0; height = 0;
            return false;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="result"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        protected static bool uploadFile(HttpFile file, string folder, string fileName, out string result, out string extension)
        {
            if (file != null && file.Buffer.Length > 0)
            {
                //取得上傳的副檔名
                extension = Path.GetExtension(file.FileName);
                if (notAllowExts.Contains(extension.ToLower()))
                {
                    result = string.Empty;
                    return false;
                }

                //儲存路徑
                string path = string.Concat(folder, fileName, extension);
                string fullFolder = Path.Combine(HttpContext.Current.Server.MapPath(folder));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(path), file.Buffer);
                result = path.Substring(1);
                return true;
            }
            result = string.Empty;
            extension = string.Empty;
            return false;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="result"></param>
        /// <param name="extension"></param>
        /// <param name="isImage"></param>
        /// <param name="id"></param>
        /// <param name="genImageThumb"></param>
        /// <param name="thumbWidth"></param>
        /// <returns></returns>
        protected static bool uploadFile(HttpFile file, string folder, out string result, out string extension, out bool isImage, out Guid id, bool genImageThumb = false, int thumbWidth = 150)
        {
            if (file != null && file.Buffer.Length > 0)
            {
                //取得上傳的副檔名
                extension = Path.GetExtension(file.FileName);
                if (notAllowExts.Contains(extension.ToLower()))
                {
                    id = Guid.Empty;
                    isImage = false;
                    result = string.Empty;
                    return false;
                }

                //儲存路徑
                id = Guid.NewGuid();
                string path = string.Concat(folder, id, extension);
                string fullFolder = HttpContext.Current.Server.MapPath(folder);
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(path), file.Buffer);
                //判斷是否產生影像縮圖
                isImage = imageUploadVerify(file);
                if (genImageThumb && isImage)
                {
                    //指定縮圖的路徑
                    path = string.Concat(folder, id, "_thumb", extension);
                    ImageThumb.SaveThumbPicWidth(new MemoryStream(file.Buffer), thumbWidth, HttpContext.Current.Server.MapPath(path));
                }
                result = path.Substring(1);
                return true;
            }

            result = string.Empty;
            extension = string.Empty;
            isImage = false;
            id = Guid.Empty;
            return false;
        }

        /// <summary>
        /// 影像檔案上傳格式驗證
        /// </summary>
        /// <param name="file">file context</param>
        /// <returns></returns>
        protected static bool imageUploadVerify(HttpFile file)
        {
            //可上傳的檔案類型
            string[] imgTypes =
            {
                "image/jpg",
                "image/jpeg",
                "image/pjpeg",
                "image/gif",
                "image/x-png",
                "image/png",
                "Application/Octet-Stream"
            };

            return file != null && file.Buffer.Length > 0 && imgTypes.Contains(file.MediaType);
        }

        /// <summary>
        /// 影像檔案上傳格式驗證
        /// </summary>
        /// <param name="file">file context</param>
        /// <returns></returns>
        protected static bool imageUploadVerify(List<HttpFile> files)
        {
            //可上傳的檔案類型
            string[] imgTypes =
            {
                "image/jpg",
                "image/jpeg",
                "image/pjpeg",
                "image/gif",
                "image/x-png",
                "image/png",
                "Application/Octet-Stream"
            };
            var result = true;
            foreach (var file in files)
            {
                result &= file != null && file.Buffer.Length > 0 && imgTypes.Contains(file.MediaType);
            }
            return result;
        }

        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mail"></param>
        /// <param name="subject"></param>
        /// <param name="values"></param>
        protected void sendMail(string path, string mail, string subject, params string[] values)
        {
            //取得電子信範本資料
            var fileName = HostingEnvironment.MapPath(path);
            if (System.IO.File.Exists(fileName))
            {
                string content = string.Format(System.IO.File.ReadAllText(fileName), values);
                //發送電子郵件認證
                SendMail sm = new SendMail(Tools.GetConfigValue("AdminMail", string.Empty), new string[] { mail }, subject, content);
                sm.Send();
            }
        }

        /// <summary>
        /// 取得IP位址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string getClientIP(HttpRequestMessage request = null)
        {
            request = request ?? Request;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }
}