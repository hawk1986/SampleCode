using ResourceLibrary;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SampleCode.ViewModel.SearchModel
{
    public class EnableSearchModel : SearchModel
    {
        /// <summary>
        /// 是否啟用
        /// </summary>
        [Display(Name = "IsEnable", ResourceType = typeof(Resource))]
        public bool? IsEnable { get; set; }

        /// <summary>
        /// 是/否下拉
        /// </summary>
        public SelectList YesNoList { get; set; }
    }
}
