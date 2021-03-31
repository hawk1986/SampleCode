using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Utilities.Utility;

namespace Utilities.Extensions
{
    public static class StringExt
    {
        #region 加解密所需參數
        private static string password = "彆斕艘雅涴僇趼憩褫眕蚚涴淕跺艘雅腔趼揹絞傖躇鎢";
        private static byte[] salt = Encoding.Unicode.GetBytes("wwwexam.blog.miniasp.com TXT");
        private static PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt);
        private static byte[] Key = pdb.GetBytes(32);
        private static byte[] IV = pdb.GetBytes(16);
        #endregion 加解密所需參數

        #region 匯出文件
        /// <summary>
        /// 產生Word文件
        /// </summary>
        /// <param name="htmlText"></param>
        /// <param name="domain"></param>
        /// <param name="isRotate"></param>
        /// <returns></returns>
        public static byte[] ConvertHtmlTextToDoc(this string htmlText, string domain, bool isRotate = false)
        {
            //取代成完整的URL路徑
            string html = htmlText.Replace("/Content/", string.Concat(domain, "/Content/"));
            //判斷是否轉換成橫向
            string size = isRotate ? "841.9pt 595.3pt" : "595.3pt 841.9pt";
            string define = @"
<style>
@page
        {mso-page-border-surround-header:no;
        mso-page-border-surround-footer:no;}
@page Section1
        {size:[size];
        margin:1.0cm 1.0cm 1.0cm 1.0cm;
        mso-header-margin:42.55pt;
        mso-footer-margin:49.6pt;
        mso-paper-source:0;
        layout-grid:18.0pt;}
div.Section1
        {page:Section1;}
-->
</style>
<div class=Section1 style='layout-grid:18.0pt'>".Replace("[size]", size);
            return Encoding.UTF8.GetBytes(string.Concat(define, html, "</div>"));
        }

        /// <summary>
        /// 產生Word文件
        /// </summary>
        /// <param name="htmlText"></param>
        /// <param name="imageRootPath"></param>
        /// <returns></returns>
        public static byte[] ConvertHtmlTextToDocx(this string htmlText, string imageRootPath)
        {
            string altChunkID = "AltChunkId1";
            MemoryStream ms = new MemoryStream();

            using (WordprocessingDocument myDoc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = myDoc.MainDocumentPart;

                if (mainPart == null)
                {
                    mainPart = myDoc.AddMainDocumentPart();
                    DocumentFormat.OpenXml.Wordprocessing.Body body = new DocumentFormat.OpenXml.Wordprocessing.Body();
                    DocumentFormat.OpenXml.Wordprocessing.Document doc = new DocumentFormat.OpenXml.Wordprocessing.Document(new OpenXmlElement[] { body });
                    doc.Save(mainPart);
                }

                //Create alternative format import part
                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkID);
                chunk.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/image",
                    new System.Uri(imageRootPath, System.UriKind.Absolute));
                //Feed HTML data into format import part (chunk)
                chunk.FeedData(new MemoryStream(Encoding.UTF8.GetBytes(htmlText)));

                DocumentFormat.OpenXml.Wordprocessing.AltChunk altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                altChunk.Id = altChunkID;
                mainPart.Document.Body.InsertAt(altChunk, 0);
                mainPart.Document.Save();
            }
            return ms.ToArray();
        }
        #endregion

        #region 呼叫API
        /// <summary>
        /// 解析Youtube的觀看次數
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<string, int> YoutubeViewCountParser(this string source)
        {
            //初始化解析所需要的變數
            string id, kv;
            string[] kvAry;
            int idxStart, idxStop, viewCount, pos = 0;
            //初始化回傳結果
            Dictionary<string, int> result = new Dictionary<string, int>();

            //開始解析
            while ((idxStart = source.IndexOf("\"id\"", pos)) > -1)
            {
                pos = idxStart + 1;
                idxStop = source.IndexOf(",", pos);
                if (idxStop > idxStart)
                {
                    pos = idxStop + 1;
                    kv = source.Substring(idxStart - 1, idxStop - idxStart);
                    kvAry = kv.Split(':');
                    if (kvAry.Length != 2)
                        continue;

                    //取得ID
                    id = kvAry[1].Trim().Trim('\"');
                    //尋找播放次數
                    if ((idxStart = source.IndexOf("\"viewCount\"", pos)) > -1)
                    {
                        pos = idxStart + 1;
                        idxStop = source.IndexOf(",", pos);
                        if (idxStop > idxStart)
                        {
                            pos = idxStop + 1;
                            kv = source.Substring(idxStart - 1, idxStop - idxStart);
                            kvAry = kv.Split(':');
                            if (kvAry.Length == 2 && int.TryParse(kvAry[1].Trim().Trim('\"'), out viewCount))
                                result[id] = viewCount;
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region 字串加解密
        /// <summary>
        /// 字串解密
        /// </summary>
        /// <param name="cipherText">加密過的文字</param>
        /// <returns></returns>
        public static string Decrypt(this string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentException("字串不能為 null 或 empty.");
            }
            var cipherData = Convert.FromBase64String(cipherText);
            var ms = new MemoryStream(cipherData);
            var alg = new RijndaelManaged();
            var decryptor = alg.CreateDecryptor(Key, IV);
            var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            var fromcipherData = new byte[cipherData.Length];
            cs.Read(fromcipherData, 0, fromcipherData.Length);
            cs.Close();
            ms.Close();

            return Encoding.Unicode.GetString(fromcipherData).Replace("\0", "");
        }

        /// <summary>
        /// 字串加密
        /// </summary>
        /// <param name="clearText">待加密的文字</param>
        /// <returns></returns>
        public static string Encrypt(this string clearText)
        {
            if (string.IsNullOrEmpty(clearText))
            {
                throw new ArgumentException("字串不能為 null 或 empty.");
            }
            var clearData = Encoding.Unicode.GetBytes(clearText);
            var ms = new MemoryStream();
            var alg = new RijndaelManaged();
            var encryptor = alg.CreateEncryptor(Key, IV);
            var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.FlushFinalBlock();
            var encryptedData = ms.ToArray();
            cs.Close();
            ms.Close();

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// TripleDES 加密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESEncryptor(this string source, string key, string iv)
        {
            byte[] data = Encoding.UTF8.GetBytes(source);
            byte[] keys = Encoding.UTF8.GetBytes(key);
            byte[] ivs = Encoding.UTF8.GetBytes(iv);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = keys,
                IV = ivs,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryp = des.CreateEncryptor();

            return Convert.ToBase64String(cryp.TransformFinalBlock(data, 0, data.Length));
        }

        /// <summary>
        /// TripleDES 解密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESDecryptor(this string source, string key, string iv)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            try
            {
                byte[] data = Convert.FromBase64String(source);
                byte[] keys = Encoding.UTF8.GetBytes(key);
                byte[] ivs = Encoding.UTF8.GetBytes(iv);
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
                {
                    Key = keys,
                    IV = ivs,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cryp = des.CreateDecryptor();

                return Encoding.UTF8.GetString(cryp.TransformFinalBlock(data, 0, data.Length));
            }
            catch (CryptographicException)
            {
                return source;
            }
            catch (FormatException)
            {
                return source;
            }
        }
        #endregion

        /// <summary>
        /// 將 String.Format 掛載為 String 的 Extensions Method, 
        /// String 本身即為 format, 傳入要兜進去的參數與 IFormatProvider 即可
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider">IFormatProvider</param>
        /// <param name="source">要兜進去的物件</param>
        /// <returns></returns>
        public static string FitWith(this string format, IFormatProvider provider, object source)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format), "必須要有格式!");
            }

            var r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            var values = new List<object>();

            string rewrittenFormat = r.Replace(format, delegate (Match m)
            {
                Group startGroup = m.Groups["start"];
                Group propertyGroup = m.Groups["property"];
                Group formatGroup = m.Groups["format"];
                Group endGroup = m.Groups["end"];

                values.Add((propertyGroup.Value == "0")
                           ? source
                           : DataBinder.Eval(source, propertyGroup.Value));

                return new string('{', startGroup.Captures.Count)
                       + (values.Count - 1)
                       + formatGroup.Value
                       + new string('}', endGroup.Captures.Count);
            });

            return string.Format(provider, rewrittenFormat, values.ToArray());
        }

        /// <summary>
        /// 將 String.Format 掛載為 String 的 Extensions Method, 
        /// String 本身即為 format, 傳入要兜進去的參數與 IFormatProvider 即可
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider">IFormatProvider</param>
        /// <param name="args">要兜進去的物件陣列</param>
        /// <returns></returns>
        public static string FitWith(this string format, IFormatProvider provider, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format), "必須要有格式!");
            }

            return string.Format(provider, format, args);
        }

        /// <summary>
        /// 將 String.Format 掛載為 String 的 Extensions Method, 
        /// String 本身即為 format, 傳入要兜進去的參數即可
        /// </summary>
        /// <param name="format"></param>
        /// <param name="source">要兜進去的物件</param>
        /// <returns></returns>
        public static string FitWith(this string format, object source)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format), "必須要有格式!");
            }

            return format.FitWith(null, source);
        }

        /// <summary>
        /// 將 String.Format 掛載為 String 的 Extensions Method, 
        /// String 本身即為 format, 傳入要兜進去的參數即可
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args">要兜進去的物件陣列</param>
        /// <returns></returns>
        public static string FitWith(this string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format), "必須要有格式!");
            }

            return string.Format(format, args);
        }

        /// <summary>
        /// 驗證字串是否含有 XSS Attack
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool HasXSSAttack(this string format)
        {
            return !string.IsNullOrEmpty(format) && format.Length != Microsoft.Security.Application.Encoder.HtmlEncode(format).Length;
        }

        /// <summary>
        /// 將字串以 HtmlEncode 編碼過後回傳
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string input)
        {
            if (input == null)
            {
                throw new ArgumentException("HTML 編碼錯誤, 字串為 null.");
            }
            return Microsoft.Security.Application.Encoder.HtmlEncode(input);
        }

        /// <summary>
        /// 將字串本身以 MD5 雜湊編碼
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string MD5(this string inputString)
        {
            var md5 = new MD5CryptoServiceProvider();
            var encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", encryptedBytes[i]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 如果字串本身為 null 回傳 String.Empty 非 null 回傳自己
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string NullToEmpty(this string me)
        {
            return me ?? string.Empty;
        }

        /// <summary>
        /// 驗證字串是否符合傳入格式
        /// </summary>
        /// <param name="me"></param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static bool RegexMatch(this string me, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format), "必須要有格式!");
            }

            return Regex.IsMatch(me, format);
        }

        /// <summary>
        /// 將字串中的換行符號前面加上 br tag
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string RNToBR(this string format)
        {
            return string.IsNullOrEmpty(format) ? string.Empty : format.Replace("\r\n", "<br />\r\n");
        }

        /// <summary>
        /// 將字串本身以 SHA1 雜湊編碼
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string SHA1(this string inputString)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var encryptedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", encryptedBytes[i]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 將字串本身以 SHA256 雜湊編碼
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string SHA256(this string inputString)
        {
            var sha256 = new SHA256Managed();
            var encryptedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", encryptedBytes[i]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 將字串本身以 SHA384 雜湊編碼
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string SHA384(this string inputString)
        {
            var sha384 = new SHA384Managed();
            var encryptedBytes = sha384.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", encryptedBytes[i]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 將字串本身以 SHA512 雜湊編碼
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string SHA512(this string inputString)
        {
            var sha512 = new SHA512Managed();
            var encryptedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", encryptedBytes[i]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 將字串本身以 PBKDF2 with SHA256 雜湊編碼
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string PBKDF2SHA256(this string inputString, string salt = null)
        {
            salt = salt ?? Pbkdf2.GetRandomString(12);
            var interactions = 20000;
            var type = "pbkdf2_sha256";
            var format = "{0}${1}${2}${3}";
            var result = string.Empty;

            using (var hmac = new HMACSHA256())
            {
                var df = new Pbkdf2(hmac, inputString, salt, interactions);
                result = Convert.ToBase64String(df.GetBytes(32));
            }

            return string.Format(format, type, interactions, salt, result);
        }

        public static bool CheckPbkdf2(this string inputString, string cipherText)
        {
            var result = false;
            var tempSet = cipherText.Split('$');
            if (tempSet.Length == 4)
            {
                var interactions = int.Parse(tempSet[1]);
                var encrypt = string.Empty;

                using (var hmac = new HMACSHA256())
                {
                    var df = new Pbkdf2(hmac, inputString, tempSet[2], interactions);
                    encrypt = Convert.ToBase64String(df.GetBytes(32));
                }
                result = encrypt == tempSet[3];
            }

            return result;
        }

        /// <summary>
        /// 使用字串當作分隔符號，並且輸入要取得的第幾個字串，回傳值包含空字串。
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="split"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Split(this string inputString, string split, int index)
        {
            var tmpCode = inputString.Split(new string[] { split }, StringSplitOptions.None);

            return (tmpCode.Length >= (index + 1)) ? tmpCode[index] : string.Empty;
        }

        /// <summary>
        /// 將數值字串轉為 int 型態
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int ToInteger(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentException("字串不能為 null 或 empty.");
            }

            int result = 0;
            bool canParse = int.TryParse(inputString, out result);

            if (!canParse)
            {
                throw new ArgumentException("字串無法解析為 int");
            }

            return result;
        }

        /// <summary>
        /// 擷取字串長度, 超過以...表示
        /// </summary>
        /// <param name="inputString">輸入的字串</param>
        /// <param name="showLength">擷取的字串長度</param>
        /// <returns></returns>
        public static string Trancate(this string inputString, int showLength)
        {
            if (inputString.Length > showLength)
            {
                return inputString.Substring(0, showLength) + "...";
            }
            return inputString;
        }

        /// <summary>
        /// 將數值字串以","分隔後轉為 guid 陣列型態
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="separator"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static bool ToGuidArray(this string inputString, char separator, out List<Guid> results)
        {
            results = new List<Guid>();
            string[] arys = inputString.Split(separator);
            foreach (string str in arys)
                if (Guid.TryParse(str, out Guid guid))
                    results.Add(guid);
            return results.Count > 0;
        }

        /// <summary>
        /// 將數值字串以","分隔後轉為 int 陣列型態
        /// </summary>
        /// <param name="inputString">輸入的字串</param>
        /// <param name="separator"></param>
        /// <param name="results"></param>
        /// <returns>int 陣列</returns>
        public static bool ToIntegerArray(this string inputString, char separator, out List<int> results)
        {
            results = new List<int>();
            string[] arys = inputString.Split(separator);
            foreach (string str in arys)
                if (int.TryParse(str, out int n))
                    results.Add(n);
            return results.Count > 0;
        }

        /// <summary>
        /// 回傳 LDAP domain 字串
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string GetDomainName(this string domain)
        {
            StringBuilder DomainName = new StringBuilder(128);
            //Domain
            if (domain.Contains("."))
            {
                string[] SplitStr = domain.Split('.');
                foreach (string item in SplitStr)
                    DomainName.Append("DC=").Append(item).Append(",");
                DomainName.Remove(DomainName.Length - 1, 1);
            }
            else
            {
                DomainName.Append("DC=").Append(domain);
            }

            return DomainName.ToString();
        }

        /// <summary>
        /// 字串轉換成時間
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static short TimeString2Time(this string time)
        {
            string[] arys = time?.Split(':');
            if (arys != null && arys.Length == 2 && int.TryParse(arys[0], out int hour) && int.TryParse(arys[1], out int minute))
                return (short)(hour * 60 + minute);
            return 0;
        }

        /// <summary>
        /// 取得特殊字元
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static string ReplaceSpecialChars(this string value, Dictionary<char, char> mapping)
        {
            if (!string.IsNullOrEmpty(value) && mapping != null && mapping.Count > 0)
            {
                StringBuilder sb = new StringBuilder(value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    if (mapping.ContainsKey(c))
                        sb.Append(mapping[c]);
                    else
                        sb.Append(c);
                }
                return sb.ToString();
            }
            return value;
        }
    }
}