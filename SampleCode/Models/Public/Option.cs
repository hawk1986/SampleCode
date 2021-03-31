using Newtonsoft.Json;
using System;

namespace SampleCode.Models.Public
{
    public class Option
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonIgnore]
        public Guid ID { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [JsonIgnore]
        public string Category { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
    }
}