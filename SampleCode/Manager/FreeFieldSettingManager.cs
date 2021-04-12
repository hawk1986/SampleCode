#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Generated at 11/06/2017 14:27:52
//     Runtime Version: 4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Extensions;
using SampleCode.Interface;
using SampleCode.Models;
using SampleCode.ViewModel;
using SampleCode.ViewModel.ListResult;
using SampleCode.ViewModel.SearchModel;
using ResourceLibrary;

namespace SampleCode.Manager
{
    public class FreeFieldSettingManager : IFreeFieldSettingManager
    {
        readonly IFreeFieldSettingRepository _freeFieldSettingRepository;

        public FreeFieldSettingManager(IFreeFieldSettingRepository freeFieldSettingRepository)
        {
            _freeFieldSettingRepository = freeFieldSettingRepository;
        }

        /// <summary>
        /// 建立 FreeFieldSetting
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Create(FreeFieldSettingViewModel entity)
        {
            var item = (FreeFieldSetting)entity;

            using (var transaction = _freeFieldSettingRepository.dbContext.Database.BeginTransaction())
            {
                try
                {
                    _freeFieldSettingRepository.Create(item);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 根據 id 刪除 FreeFieldSetting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete(List<Guid> id)
        {
            using (var transaction = _freeFieldSettingRepository.dbContext.Database.BeginTransaction())
            {
                try
                {
                    var itemSet = _freeFieldSettingRepository.Where(x => id.Contains(x.ID)).ToList();
                    if (itemSet.Any())
                    {
                        foreach (var item in itemSet)
                        {
                            _freeFieldSettingRepository.Delete(item);
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 根據 id 取得 FreeFieldSetting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FreeFieldSettingViewModel GetByID(Guid id)
        {
            var item = _freeFieldSettingRepository.GetByID(id);
            var result = (FreeFieldSettingViewModel)item;

            return result;
        }

        /// <summary>
        /// 分頁
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public Paging<FreeFieldSettingListResult> Paging(FreeFieldSettingSearchModel searchModel)
        {
            // 需要依語系轉換內容
            var strings = LanguageTool.GetResourceValue("Strings");
            var numerical = LanguageTool.GetResourceValue("Numerical");

            // 預設集合
            var temp = _freeFieldSettingRepository.GetAll();

            // 將 DB 資料轉換為列表頁呈現資料
            var tempResult = from x in temp
                             select new FreeFieldSettingListResult
                             {
                                 SequenceNo = x.SequenceNo,
                                 ID = x.ID,
                                 TableName = x.TableName,
                                 FieldName = x.FieldName,
                                 DataType = x.DataType ? strings : numerical,
                             };

            // 如有篩選條件，進行篩選
            if (!string.IsNullOrWhiteSpace(searchModel.Search))
            {
                var search = searchModel.Search.ToLower();
                tempResult = tempResult.Where(x =>
                    x.TableName.Contains(search) ||
                    x.FieldName.Contains(search) ||
                    false);
            }

            // 進行分頁處理
            var result = new Paging<FreeFieldSettingListResult>();
            result.total = tempResult.Count();
            result.rows = tempResult
                .OrderBy(searchModel.Sort, searchModel.Order)
                .Skip(searchModel.Offset)
                .Take(searchModel.Limit)
                .ToList();

            return result;
        }

        /// <summary>
        /// 更新 FreeFieldSetting
        /// </summary>
        /// <param name="entity"></param>
        public void Update(FreeFieldSettingViewModel entity)
        {
            using (var transaction = _freeFieldSettingRepository.dbContext.Database.BeginTransaction())
            {
                try
                {
                    var source = _freeFieldSettingRepository.GetByID(entity.ID);
                    source.TableName = entity.TableName ?? string.Empty;
                    source.FieldName = entity.FieldName ?? string.Empty;
                    source.DataType = entity.DataType;
                    source.Description = entity.Description ?? string.Empty;

                    _freeFieldSettingRepository.Update(source);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
#pragma warning restore 1591