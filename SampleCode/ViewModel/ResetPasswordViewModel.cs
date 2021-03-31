using ResourceLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCode.ViewModel
{
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Hash Token
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string HashToken { get; set; }

        /// <summary>
        /// New Password
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm Password
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "CompareError", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Now Time
        /// </summary>
        public DateTime NowTime { get; set; }
    }
}