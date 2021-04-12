#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Generated at 09/26/2019 09:31:06
//     Runtime Version: 4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using SampleCode.ViewModel;
using SampleCode.ViewModel.ListResult;
using SampleCode.ViewModel.SearchModel;

namespace SampleCode.Interface
{
    public interface ISystemConfigManager : IManager
    {
        /// <summary>
        /// 建立 SystemConfig
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Create(SystemConfigViewModel entity);

        /// <summary>
        /// 根據 id 刪除 SystemConfig
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(List<Guid> id);

        /// <summary>
        /// 根據 id 取得 SystemConfig
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SystemConfigViewModel GetByID(Guid id);

        /// <summary>
        /// 分頁
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Paging<SystemConfigListResult> Paging(SystemConfigSearchModel searchModel);

        /// <summary>
        /// 更新 SystemConfig
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(SystemConfigViewModel entity);
    }
}
#pragma warning restore 1591