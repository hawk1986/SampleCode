using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utilities.Utility
{
    public class SimpleAliYunOSS
    {
        private string _accessKeyId;
        private string _accessKeySecret;
        private string _endpoint;

        private string _bucketName;

        private OssClient _client;

        /// <summary>
        /// 初始化 SimpleAliYunOSS 物件，建立 OssClient
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="accessKeyId"></param>
        /// <param name="accessKeySecret"></param>
        public SimpleAliYunOSS(string endpoint, string accessKeyId, string accessKeySecret)
        {
            _accessKeyId = accessKeyId;
            _accessKeySecret = accessKeySecret;
            _endpoint = endpoint;
            _client = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
        }

        /// <summary>
        /// 建立 Bucket
        /// </summary>
        /// <param name="bucketName"></param>
        public void CreateBucket(string bucketName)
        {
            try
            {
                var bucket = _client.CreateBucket(bucketName);
                _bucketName = bucketName;

                Console.WriteLine("Create bucket succeeded. Name: {0}", bucket.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create bucket failed, Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="key"></param>
        public void DeleteObject(string key)
        {
            try
            {
                _client.DeleteObject(_bucketName, key);
                Console.WriteLine("Delete object succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete object failed, Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 取得暫時的 Url
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Uri GenerateUri(string key, string method, int expires)
        {
            int http = 0;
            switch (method)
            {
                case "Get":
                    http = 0;
                    break;
                case "Put":
                    http = 4;
                    break;
            }
            var req = new GeneratePresignedUriRequest(_bucketName, key, (SignHttpMethod)http)
            {
                Expiration = DateTime.Now.AddSeconds(expires)
            };
            var uri = _client.GeneratePresignedUri(req);

            return uri;
        }

        /// <summary>
        /// 讀取檔案並寫到指定位置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileToDownload"></param>
        public void GetObject(string key, string fileToDownload)
        {
            try
            {
                var ossObject = _client.GetObject(_bucketName, key);
                var bufferSize = 1024;
                using (var requestStream = ossObject.Content)
                {
                    byte[] buffer = new byte[bufferSize];
                    using (var fileStream = File.Open(fileToDownload, FileMode.OpenOrCreate))
                    {
                        var len = 0;
                        while ((len = requestStream.Read(buffer, 0, bufferSize)) != 0)
                        {
                            fileStream.Write(buffer, 0, len);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get object failed, Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 讀取檔案 Stream
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Stream GetObjectStream(string key)
        {
            Stream result = null;
            try
            {
                var ossObject = _client.GetObject(_bucketName, key);

                result = ossObject.Content;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get object failed, Message: {0}", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 列出目前 Bucket 內的檔案
        /// </summary>
        public List<string> ListObjects()
        {
            var result = new List<string>();
            try
            {
                var listObjectsRequest = new ListObjectsRequest(_bucketName);
                var listObjects = _client.ListObjects(listObjectsRequest);

                Console.WriteLine("List object succeeded");
                result.AddRange(listObjects.ObjectSummaries.Select(x => x.Key));
            }
            catch (Exception ex)
            {
                Console.WriteLine("List object failed, Message: {0}", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 上傳指定檔案到 OSS 上
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileToUpload"></param>
        public void PutObject(string key, string fileToUpload)
        {
            try
            {
                var result = _client.PutObject(_bucketName, key, fileToUpload);

                Console.WriteLine("Put object succeeded, ETag: {0}", result.ETag);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Put object failed, Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 指定目前要操作的 Bucket
        /// </summary>
        /// <param name="bucketName"></param>
        public void SetBucket(string bucketName)
        {
            _bucketName = bucketName;
        }
    }
}
