using ResourceLibrary;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SampleCode.ViewModel
{
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Remote("EmailBeUse", "Account", ErrorMessageResourceName = "EmailNotFoundError", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "EmailError", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "EmailError", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        /// <summary>
        /// Now Time
        /// </summary>
        public DateTime NowTime { get; set; }
    }
}