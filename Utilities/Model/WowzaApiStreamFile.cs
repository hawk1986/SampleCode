using System.Collections.Generic;

namespace Utilities
{
    public class WowzaApiStreamFile
    {
        /// <summary>
        /// Server Name
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Stream Files
        /// </summary>
        public List<StreamFile> StreamFiles { get; set; }
    }
}
