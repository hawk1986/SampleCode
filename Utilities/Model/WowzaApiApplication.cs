using System.Collections.Generic;

namespace Utilities
{
    public class WowzaApiApplication
    {
        /// <summary>
        /// Server Name
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Application List
        /// </summary>
        public List<Application> Applications { get; set; }
    }
}
