using System.Collections.Generic;
using Utilities.Interface;

namespace SampleCode.Models.Public
{
    public class MenuTree : Menu, IChildren<MenuTree>
    {
        /// <summary>
        /// 子功能表
        /// </summary>
        public List<MenuTree> Children { get; set; }
    }
}