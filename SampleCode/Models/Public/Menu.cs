using Newtonsoft.Json;
using System;

namespace SampleCode.Models.Public
{
    public class Menu
    {
        /// <summary>
        /// SequenceNo
        /// </summary>
        [JsonIgnore]
        public int SequenceNo { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [JsonIgnore]
        public Guid ID { get; set; }

        /// <summary>
        /// ParentID
        /// </summary>
        [JsonIgnore]
        public Nullable<Guid> ParentID { get; set; }

        /// <summary>
        /// Sequence
        /// </summary>
        public byte Sequence { get; set; }

        /// <summary>
        /// MenuLevel
        /// </summary>
        public byte MenuLevel { get; set; }

        /// <summary>
        /// MenuCode
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// MenuName
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Layout
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// IconPath
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// HeaderPath
        /// </summary>
        public string HeaderPath { get; set; }

        /// <summary>
        /// ViewCount
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Parameter
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// ExternalUrl
        /// </summary>
        public string ExternalUrl { get; set; }

        /// <summary>
        /// IsInlinePage
        /// </summary>
        public bool IsInlinePage { get; set; }

        /// <summary>
        /// IsShowFooter
        /// </summary>
        public bool IsShowFooter { get; set; }

        /// <summary>
        /// IsEnable
        /// </summary>
        public bool IsEnable { get; set; }
    }
}