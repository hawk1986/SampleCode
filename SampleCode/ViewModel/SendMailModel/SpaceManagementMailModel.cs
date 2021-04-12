using ResourceLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCode.ViewModel
{
    public class SpaceManagementMailModel
    {     
        public string ActName { get; set; }
        public DateTime Date { get; set; }
        public string ActDesc { get; set; }
        public string FillPerson { get; set; }
        public string CompanyName { get; set; }
        public string Contactphone { get; set; }
        public string Email { get; set; }
        public string FileUpload { get; set; }
        public Guid SpaceID { get; set; }

        /// <summary>
        /// CaptchaID
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public Guid CaptchaID { get; set; }

        /// <summary>
        /// CaptchaData
        /// </summary>
        public string CaptchaData { get; set; }

        /// <summary>
        /// CaptchaCode
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CaptchaCode", ResourceType = typeof(Resource))]
        public string CaptchaCode { get; set; }
    }
}