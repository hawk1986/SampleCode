using SampleCode.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities.Utility;
using SampleCode.Models;

namespace SampleCode.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 紀錄
        /// </summary>
        protected static Logger logger { get; set; }

        /// <summary>
        /// 登入的使用者
        /// </summary>
        private UserViewModel _user;

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
        /// 取得登入者 ID
        /// </summary>
        public Guid? ID
        {
            get
            {
                if (_user == null)
                    _user = getUser();
                return _user?.ID;
            }
        }

        /// <summary>
        /// 取得登入者姓名
        /// </summary>
        public string UserName
        {
            get
            {
                if (_user == null)
                    _user = getUser();
                return _user?.Name ?? string.Empty;
            }
        }

        /// <summary>
        /// 取得已登入 User 資料
        /// </summary>
        /// <returns></returns>
        private UserViewModel getUser()
        {
            if (null != UnobtrusiveSession.Session &&
                null != UnobtrusiveSession.Session["User"])
            {
                return UnobtrusiveSession.Session["User"] as UserViewModel;
            }

            return null;
        }

        /// <summary>
        /// 取得已登入 User 資料
        /// </summary>
        /// <returns></returns>
        public UserViewModel GetUser()
        {
            if (_user == null)
                _user = getUser();
            return _user;
        }

        /// <summary>
        /// 渲染檢視畫面
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string renderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// 紀錄訊息 on Debug Mode
        /// </summary>
        /// <param name="type">0: Info, 1: Trace, 2: Warn, 3: Error, 4: Fatal, default: Debug</param>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        protected void log(int type, string message)
        {
            switch (type)
            {
                case 0:
                    logger.Info(message);
                    break;
                case 1:
                    logger.Trace(message);
                    break;
                case 2:
                    logger.Warn(message);
                    break;
                case 3:
                    logger.Error(message);
                    break;
                case 4:
                    logger.Fatal(message);
                    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }

        #region 處理附件檔案上傳Orig
        /// <summary>
        /// 處理附件檔案上傳
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="viewModel"></param>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="genThumbImage"></param>
        //protected static void fileAttachedHandler(BaseController obj, PostedFileBaseModel viewModel, Guid parent, string path, string type, bool genThumbImage = true)
        //{
        //    //處理上傳的檔案
        //    if (viewModel.PostedFiles != null && viewModel.PostedFiles.Count > 0)
        //    {
        //        for (int i = 0; i < viewModel.PostedFiles.Count; i++)
        //        {
        //            var file = viewModel.PostedFiles[i];
        //            if (uploadFile(obj, file, path, out string result, out string extension, out bool isImage, out Guid id, genThumbImage))
        //            {
        //                viewModel.NewAttachedFiles.Add(new FileAttached
        //                {
        //                    ID = id,
        //                    ParentID = parent,
        //                    TableType = type,
        //                    ItemType = viewModel.PostedCategory.Count > i ? viewModel.PostedCategory[i] ?? string.Empty : string.Empty,
        //                    FileName = file.FileName,
        //                    FileExtension = extension,
        //                    FilePath = result,
        //                    FileSize = file.ContentLength,
        //                    IsImageFile = isImage,
        //                    IsThumbImage = isImage && genThumbImage,
        //                    Description = viewModel.PostedCaption.Count > i ? viewModel.PostedCaption[i] ?? string.Empty : string.Empty,
        //                    CreateTime = DateTime.Now
        //                });
        //            }
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 處理附件檔案上傳
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="viewModel"></param>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="genThumbImage"></param>
        protected static void fileAttachedHandler(BaseController obj, PostedFileBaseModel viewModel, Guid parent, string path, string type, bool genThumbImage = true)
        {
            var itemType = string.Empty;
            
            if (viewModel.PostedFiles != null && viewModel.PostedFiles.Count > 0)
                _fileAttachedHandler(obj, viewModel.PostedFiles, viewModel.NewAttachedFiles, viewModel.PostedCategory, viewModel.PostedCaption, parent, path, type, viewModel.AttachedArea, viewModel.AttachedBuyGuide, genThumbImage, itemType, viewModel.MainPic, viewModel.PicOrder);
        }

        protected static List<FileAttached> _fileAttachedHandler(BaseController obj, List<HttpPostedFileBase> PostedFiles, List<FileAttached> NewAttachedFiles, List<string> PostedCategory,
                                                   List<string> PostedCaption, Guid parent, string path, string type, List<FileAttached> areaKind, List<FileAttached> buyGuide,bool genThumbImage = true, string itemType = null, 
                                                   List<bool> mainPic = null, List<int?> order = null)
        {
            //處理上傳的檔案
            if (PostedFiles != null && PostedFiles.Count > 0)
            {
                for (int i = 0; i < PostedFiles.Count; i++)
                {
                    var file = PostedFiles[i];
                    if (uploadFile(obj, file, path, out string result, out string extension, out bool isImage, out Guid id, genThumbImage))
                    {
                        NewAttachedFiles.Add(new FileAttached
                        {
                            ID = id,
                            ParentID = parent,
                            TableType = type,
                            ItemType = string.IsNullOrWhiteSpace(itemType) ? PostedCategory.Count > i ? PostedCategory[i] ?? string.Empty : string.Empty : itemType,
                            FileName = file.FileName,
                            FileExtension = extension,
                            FilePath = result,
                            FileSize = file.ContentLength,
                            IsImageFile = isImage,
                            IsThumbImage = isImage && genThumbImage,
                            Description = PostedCaption.Count > i ? PostedCaption[i] ?? string.Empty : string.Empty,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        });
                    }
                }
            }
            return NewAttachedFiles;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="obj">BaseController</param>
        /// <param name="file">HttpPostedFileBase</param>
        /// <param name="id">Guid</param>
        /// <param name="dir">Dir</param>
        /// <param name="filepath">out Filepath</param>
        /// <returns></returns>
        protected static bool fileUploadSave(BaseController obj, HttpPostedFileBase file, Guid id, string dir, out string filepath)
        {
            if (file != null && file.ContentLength > 0)
            {
                //取得上傳的副檔名
                string extension = Path.GetExtension(file.FileName);
                //儲存路徑
                string path = string.Concat("~/Content/Uploads/", dir, "/", id, extension);
                string fullFolder = obj.Server.MapPath(String.Concat("~/Content/Uploads/", dir));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                file.SaveAs(obj.Server.MapPath(path));
                filepath = path.Substring(1);
                return true;
            }
            filepath = string.Empty;
            return false;
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        protected static void deleteFile(BaseController obj, string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                string fullPath = obj.Server.MapPath(string.Concat("~", filename));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filenames"></param>
        protected static void deleteFiles(BaseController obj, List<string> filenames)
        {
            if (filenames != null && filenames.Count > 0)
            {
                foreach (var file in filenames)
                {
                    string fullPath = obj.Server.MapPath(string.Concat("~", file));
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="ids"></param>
        protected static void deleteFiles(BaseController obj, string path, List<Guid> ids)
        {
            if (!string.IsNullOrEmpty(path) && ids != null && ids.Count > 0)
            {
                string fullPath = obj.Server.MapPath(path);
                //判斷路徑是否存在
                if (Directory.Exists(fullPath))
                {
                    foreach (var id in ids)
                    {
                        string[] files = Directory.GetFiles(fullPath, $"{id}*");
                        if (files.Length > 0)
                            foreach (var file in files)
                                System.IO.File.Delete(file);
                    }
                }
            }
        }

        /// <summary>
        /// 上傳影像
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected static string uploadImage(BaseController obj, HttpPostedFileBase file, string folder, string fileName)
        {
            if (imageUploadVerify(file))
            {
                //取得上傳的副檔名
                string extension = Path.GetExtension(file.FileName);
                //儲存路徑
                string path = string.Concat(folder, fileName, extension);
                string fullFolder = Path.Combine(obj.Server.MapPath(folder));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                file.SaveAs(obj.Server.MapPath(path));
                return path.Substring(1);
            }
            return string.Empty;
        }

        /// <summary>
        /// 上傳影像
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="result"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected static bool uploadImage(BaseController obj, HttpPostedFileBase file, string folder, string fileName, out string result, out int width, out int height)
        {
            if (imageUploadVerify(file))
            {
                //取得上傳的副檔名
                string extension = Path.GetExtension(file.FileName);
                //儲存路徑
                string path = string.Concat(folder, fileName, extension);
                string fullFolder = Path.Combine(obj.Server.MapPath(folder));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                file.SaveAs(obj.Server.MapPath(path));
                result = path.Substring(1);
                //取得影像的大小
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream))
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
        /// <param name="obj"></param>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="result"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        protected static bool uploadFile(BaseController obj, HttpPostedFileBase file, string folder, string fileName, out string result, out string extension)
        {
            if (file != null && file.ContentLength > 0)
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
                string fullFolder = Path.Combine(obj.Server.MapPath(folder));
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                file.SaveAs(obj.Server.MapPath(path));
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
        /// <param name="obj"></param>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="result"></param>
        /// <param name="extension"></param>
        /// <param name="isImage"></param>
        /// <param name="id"></param>
        /// <param name="genImageThumb"></param>
        /// <param name="thumbWidth"></param>
        /// <returns></returns>
        protected static bool uploadFile(BaseController obj, HttpPostedFileBase file, string folder, out string result, out string extension, out bool isImage, out Guid id, bool genImageThumb = false, int thumbWidth = 150)
        {
            if (file != null && file.ContentLength > 0)
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
                string fullFolder = obj.Server.MapPath(folder);
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                //儲存檔案
                file.SaveAs(obj.Server.MapPath(path));
                //判斷是否產生影像縮圖
                isImage = imageUploadVerify(file);
                if (genImageThumb && isImage)
                {
                    //指定縮圖的路徑
                    path = string.Concat(folder, id, "_thumb", extension);
                    ImageThumb.SaveThumbPicWidth(file.InputStream, thumbWidth, obj.Server.MapPath(path));
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
        protected static bool imageUploadVerify(HttpPostedFileBase file)
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

            return file != null && file.ContentLength > 0 && imgTypes.Contains(file.ContentType);
        }

        /// <summary>
        /// 影像檔案上傳格式驗證
        /// </summary>
        /// <param name="file">file context</param>
        /// <returns></returns>
        protected static bool imageUploadVerify(List<HttpPostedFileBase> files)
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
                result &= file != null && file.ContentLength > 0 && imgTypes.Contains(file.ContentType);
            }
            return result;
        }

        /// <summary>
        /// 語音檔案上傳格式驗證
        /// </summary>
        /// <param name="file">file context</param>
        /// <returns></returns>
        protected static bool voiceUploadVerify(HttpPostedFileBase file)
        {
            //可上傳的檔案類型
            string[] voiceTypes =
            {
                "audio/mpeg",
                "audio/mp3",
                "audio/wav"
            };

            return file != null && file.ContentLength > 0 && voiceTypes.Contains(file.ContentType);
        }

        /// <summary>
        /// 語音檔案上傳格式驗證
        /// </summary>
        /// <param name="file">file context</param>
        /// <returns></returns>
        protected static bool voiceUploadVerify(List<HttpPostedFileBase> files)
        {
            //可上傳的檔案類型
            string[] voiceTypes =
            {
                "audio/mpeg",
                "audio/mp3",
                "audio/wav"
            };
            var result = true;
            foreach (var file in files)
            {
                result &= file != null && file.ContentLength > 0 && voiceTypes.Contains(file.ContentType);
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
            var fileName = Server.MapPath(path);
            if (System.IO.File.Exists(fileName))
            {
                string content = string.Format(System.IO.File.ReadAllText(fileName), values);
                //發送電子郵件認證
                SendMail sm = new SendMail(Tools.GetConfigValue("AdminMail", string.Empty), new string[] { mail }, subject, content);
                sm.Send();
            }
        }
    }
}