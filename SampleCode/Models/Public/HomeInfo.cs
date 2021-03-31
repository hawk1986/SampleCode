using SampleCode;
using System.Collections.Generic;
using System.Linq;

namespace SampleCode.Models.Public
{
    public class HomeInfo
    {
        /// <summary>
        /// 系統組態
        /// </summary>
        public List<Config> Config { get; set; }

        /// <summary>
        /// 功能選項
        /// </summary>
        public List<MenuTree> Menu { get; set; }

        /// <summary>
        /// 轉換函數
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static HomeInfo Parser(HomeMapping mapping, Culture culture)
        {
            if (mapping == null)
                return new HomeInfo();

            //增加瀏覽次數
            var count = mapping.Config?.Values.FirstOrDefault(x => x.Key == "BrowseCount");
            if (count != null && int.TryParse(count.Value, out int n))
            {
                count.Value = (++n).ToString();
            }

            //初始化回傳物件
            return new HomeInfo
            {
                Config = mapping.Config?.Values.ToList(),
                Menu = Global.GetMenuTrees(),
            };
        }
    }
}