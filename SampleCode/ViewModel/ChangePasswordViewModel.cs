using ResourceLibrary;
using System.ComponentModel.DataAnnotations;

namespace SampleCode.ViewModel
{
    public class ChangePasswordViewModel
    {
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
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        /// <summary>
        /// New Password
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "NewPassword", ResourceType = typeof(Resource))]
        [RegularExpression("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$", ErrorMessageResourceName = "PasswordError", ErrorMessageResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        /// <summary>
        /// ConfirmPassword
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
}