using ResourceLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCode.ViewModel
{
    public class ValidateUserViewModel
    {
        /// <summary>
        /// CaptchaID
        /// </summary>
        //[Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public Guid CaptchaID { get; set; }

        /// <summary>
        /// CaptchaData
        /// </summary>
        public string CaptchaData { get; set; }

        /// <summary>
        /// CaptchaCode
        /// </summary>
        //[Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CaptchaCode", ResourceType = typeof(Resource))]
        public string CaptchaCode { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Account", ResourceType = typeof(Resource))]
        public string Account { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataType(DataType.Password)]
        //[Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        /// <summary>
        /// Return Url
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Now Time
        /// </summary>
        public DateTime NowTime { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// AcceptBenefit
        /// </summary>
        public bool AcceptBenefit { get; set; }

        public string Type { get; set; }
    }
}