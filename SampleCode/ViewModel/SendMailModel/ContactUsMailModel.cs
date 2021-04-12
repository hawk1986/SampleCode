using ResourceLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCode.ViewModel
{
    public class ContactUsMailModel
    {     
        public string Classify { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardType { get; set; }
        public string MemberID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }        

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