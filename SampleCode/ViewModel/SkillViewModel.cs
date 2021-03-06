#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Generated at 01/27/2021 15:55:18
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
    public class SkillBaseModel
    {
        public SkillBaseModel()
        {
        }

        #region == DB Fields ==

        /// <summary>
        /// rowId
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "rowId", ResourceType = typeof(Resource))]
        public int rowId { get; set; }

        /// <summary>
        /// name
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(32, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "name", ResourceType = typeof(Resource))]
        public string name { get; set; }

        /// <summary>
        /// isSelected
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "isSelected", ResourceType = typeof(Resource))]
        public bool isSelected { get; set; }

        /// <summary>
        /// type
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(32, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "type", ResourceType = typeof(Resource))]
        public string type { get; set; }

        /// <summary>
        /// abc
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "abc", ResourceType = typeof(Resource))]
        public string abc { get; set; }

        #endregion == DB Fields ==
    }

    public class SkillViewModel : SkillBaseModel
    {
        #region == View Fields ==

        #endregion == View Fields ==
    }
}
#pragma warning restore 1591