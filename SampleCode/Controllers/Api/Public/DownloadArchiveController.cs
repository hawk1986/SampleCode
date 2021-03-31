using Ionic.Zip;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using PRS_New.Models;

namespace PRS_New.Controllers.Api.Public
{
    [RoutePrefix("api/downloadArchive")]
    public class DownloadArchiveController : ApiController
    {
        readonly IFileAttachedRepository _fileAttachedRepository;

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="fileAttachedRepository"></param>
        public DownloadArchiveController(IFileAttachedRepository fileAttachedRepository)
        {
            _fileAttachedRepository = fileAttachedRepository;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(Guid id)
        {
            //初始化回傳物件
            var resp = new HttpResponseMessage();
            //取得查詢的資料
            var files = _fileAttachedRepository.Where(x => x.ParentID == id).ToList();
            if (files.Count > 0)
            {
                using (var zipStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipFile())
                    {
                        zipArchive.AlternateEncoding = System.Text.Encoding.UTF8;
                        zipArchive.AlternateEncodingUsage = ZipOption.AsNecessary;
                        foreach (var file in files)
                        {
                            //判斷是否為影像檔案
                            if (file.IsImageFile)
                                file.FilePath = file.FilePath.Replace("_thumb", string.Empty);

                            //取得檔案絕對路徑
                            var sourcePath = HttpContext.Current.Server.MapPath(string.Concat("~", file.FilePath));
                            if (File.Exists(sourcePath))
                                zipArchive.AddEntry(file.FileName, File.ReadAllBytes(sourcePath));
                        }
                        zipArchive.Save(zipStream);
                    }

                    byte[] result = zipStream.ToArray();
                    resp.StatusCode = HttpStatusCode.OK;
                    resp.Content = new ByteArrayContent(result);
                    resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    resp.Content.Headers.ContentDisposition.FileName = HttpUtility.UrlPathEncode("zipArchive.zip");
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                }
            }
            else
            {
                resp.StatusCode = HttpStatusCode.NotFound;
            }
            return resp;
        }
    }
}