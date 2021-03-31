using SampleCode.ViewModel;

namespace SampleCode.Models
{
    public class UserToken
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public UserViewModel Data { get; set; }
    }
}