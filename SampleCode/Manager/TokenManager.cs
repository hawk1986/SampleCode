using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Utilities.Utility;
using SampleCode.Models;
using SampleCode.ViewModel;

namespace SampleCode.Manager
{
    public static class TokenManager
    {
        /// <summary>
        /// 金鑰
        /// </summary>
        static readonly string key = Tools.GetConfigValue("TokenKey", string.Empty);

        /// <summary>
        /// 對應清單
        /// </summary>
        static readonly ConcurrentDictionary<Guid, UserViewModel> mapping = new ConcurrentDictionary<Guid, UserViewModel>();

        /// <summary>
        /// 恢復
        /// </summary>
        /// <param name="tokenDatas"></param>
        public static void Restore(IEnumerable<string> tokenDatas)
        {
            if (tokenDatas != null && tokenDatas.Any())
            {
                foreach (var data in tokenDatas)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        var user = JsonConvert.DeserializeObject<UserViewModel>(data);
                        mapping.TryAdd(user.ID, user);
                    }
                }
            }
        }

        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserToken Create(UserViewModel user)
        {
            //初始加密資料
            string tmp = string.Concat(user.ID, "@", user.LoginTime.Ticks);
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(tmp));
            string iv = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);

            //使用 AES 加密
            string encrypt = TokenCrypto.AESEncrypt(base64, key.Substring(0, 16), iv);
            //取得簽章
            string signature = TokenCrypto.ComputeHMACSHA256(string.Concat(iv, ".", encrypt), key);

            //更新對應清單
            if (mapping.ContainsKey(user.ID))
                mapping[user.ID] = user;
            else
                mapping.TryAdd(user.ID, user);

            //回傳Token
            return new UserToken
            {
                //Token 為 iv + encrypt + signature，並用 . 串聯
                Token = string.Concat(iv, ".", encrypt, ".", signature),
                Data = user
            };
        }

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <returns></returns>
        public static UserViewModel GetUser()
        {
            return GetUser(HttpContext.Current.Request.Headers["Authorization"]);
        }

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static UserViewModel GetUser(string token)
        {
            //取得簽章
            if (!string.IsNullOrEmpty(token))
            {
                //初始解密資料
                string[] split = token.Split('.');
                if (split.Length == 3)
                {
                    string iv = split[0];
                    string encrypt = split[1];
                    string signature = split[2];

                    //檢查簽章是否正確
                    if (signature != TokenCrypto.ComputeHMACSHA256(string.Concat(iv, ".", encrypt), key))
                        return null;

                    //使用 AES 解密 Payload
                    var base64 = TokenCrypto.AESDecrypt(encrypt, key.Substring(0, 16), iv);
                    var tmp = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                    split = tmp.Split('@');
                    if (split.Length == 2 &&
                        Guid.TryParse(split[0], out Guid id) &&
                        long.TryParse(split[1], out long ticks) &&
                        mapping.ContainsKey(id))
                    {
                        UserViewModel user = mapping[id];
                        if (user.LoginTime.Ticks == ticks)
                            return user;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 移除使用者
        /// </summary>
        public static bool RemoveUser(out UserViewModel viewModel)
        {
            //取得簽章
            string token = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                //初始解密資料
                string[] split = token.Split('.');
                if (split.Length == 3)
                {
                    string iv = split[0];
                    string encrypt = split[1];
                    string signature = split[2];

                    //檢查簽章是否正確
                    if (signature != TokenCrypto.ComputeHMACSHA256(string.Concat(iv, ".", encrypt), key))
                    {
                        viewModel = null;
                        return false;
                    }

                    //使用 AES 解密 Payload
                    var base64 = TokenCrypto.AESDecrypt(encrypt, key.Substring(0, 16), iv);
                    var tmp = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                    split = tmp.Split('@');
                    if (split.Length == 2 &&
                        Guid.TryParse(split[0], out Guid id) &&
                        long.TryParse(split[1], out long ticks))
                    {
                        return mapping.TryRemove(id, out viewModel);
                    }
                }
            }
            viewModel = null;
            return false;
        }

        /// <summary>
        /// 移除使用者
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveUser(Guid id)
        {
            mapping.TryRemove(id, out UserViewModel viewModel);
        }
    }
}