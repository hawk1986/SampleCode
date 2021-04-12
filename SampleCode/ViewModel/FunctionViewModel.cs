#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Generated at 09/25/2017 13:00:22
//	   Runtime Version: 4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ResourceLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SampleCode.ViewModel
{
    public class FunctionBaseModel
    {
        public FunctionBaseModel()
        {
            IsEnable = true;
        }

        #region == DB Fields ==

        /// <summary>
        /// SequenceNo
        /// </summary>
        public int SequenceNo { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// ModuleID
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Module", ResourceType = typeof(Resource))]
        public Guid ModuleID { get; set; }

        /// <summary>
        /// Sequence
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Sequence", ResourceType = typeof(Resource))]
        [Range(byte.MinValue, byte.MaxValue, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        public byte Sequence { get; set; }

        /// <summary>
        /// GroupSequence
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "GroupSequence", ResourceType = typeof(Resource))]
        [Range(byte.MinValue, byte.MaxValue, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        public byte GroupSequence { get; set; }

        /// <summary>
        /// BitCode
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "BitCode", ResourceType = typeof(Resource))]
        [Range(int.MinValue, int.MaxValue, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        public int BitCode { get; set; }

        /// <summary>
        /// Dependency
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Dependency", ResourceType = typeof(Resource))]
        [Range(int.MinValue, int.MaxValue, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        public int Dependency { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Code", ResourceType = typeof(Resource))]
        [Remote("CodeBeUse", "Function", AdditionalFields = "Initial", ErrorMessageResourceName = "BeUsedError", ErrorMessageResourceType = typeof(Resource))]
        public string Code { get; set; }

        /// <summary>
        /// GroupName
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "GroupName", ResourceType = typeof(Resource))]
        public string GroupName { get; set; }

        /// <summary>
        /// SimpleName
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "SimpleName", ResourceType = typeof(Resource))]
        public string SimpleName { get; set; }

        /// <summary>
        /// DisplayName
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DisplayName", ResourceType = typeof(Resource))]
        public string DisplayName { get; set; }

        /// <summary>
        /// DisplayTree
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DisplayTree", ResourceType = typeof(Resource))]
        public bool DisplayTree { get; set; }

        /// <summary>
        /// DisplayHeader
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DisplayHeader", ResourceType = typeof(Resource))]
        public bool DisplayHeader { get; set; }

        /// <summary>
        /// IsEnable
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "IsEnable", ResourceType = typeof(Resource))]
        public bool IsEnable { get; set; }

        /// <summary>
        /// ControllerName
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "ControllerName", ResourceType = typeof(Resource))]
        public string ControllerName { get; set; }

        /// <summary>
        /// ActionName
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "ActionName", ResourceType = typeof(Resource))]
        public string ActionName { get; set; }

        #endregion == DB Fields ==
    }

    public class FunctionViewModel : FunctionBaseModel
    {
        #region == View Fields ==

        public SelectList ModuleList { get; set; }

        /// <summary>
        /// Controller �U��
        /// </summary>
        public SelectList ControllerList { get; set; }

        /// <summary>
        /// Action �U��
        /// </summary>
        public SelectList ActionList { get; set; }

        public List<FunctionViewModel> DependencyFunctionList { get; set; }

        #endregion == View Fields ==
    }
}
#pragma warning restore 1591