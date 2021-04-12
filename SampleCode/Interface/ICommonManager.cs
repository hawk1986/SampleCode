using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SampleCode.Models;

namespace SampleCode.Interface
{
    public interface ICommonManager : IManager
    {
        /// <summary>
        /// 取得對應 controller 中的 所有回傳型態為 ActionResult 的 method
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        SelectList GetActionList(string controllerName);

        /// <summary>
        /// 取得組件中所有 controller name
        /// </summary>
        /// <returns></returns>
        SelectList GetControllerList();

        /// <summary>
        /// 取得 Department 下拉
        /// </summary>
        /// <returns></returns>
        SelectList GetDepartmentList();

        /// <summary>
        /// 取得功能下拉
        /// </summary>
        /// <returns></returns>
        SelectList GetFunctionList(List<Guid> roles);

        /// <summary>
        /// 取得 Module 下拉
        /// </summary>
        /// <returns></returns>
        SelectList GetModuleList();

        /// <summary>
        /// 取得 Role 下拉
        /// </summary>
        /// <returns></returns>
        SelectList GetRoleList();

        /// <summary>
        /// 取得 User 下拉
        /// </summary>
        /// <returns></returns>
        SelectList GetUserList();

        /// <summary>
        /// 取得【是、否】下拉選單
        /// </summary>
        /// <returns></returns>
        SelectList GetYesNoList();

        /// <summary>
        /// 取得語系的下拉選單
        /// </summary>
        /// <returns></returns>
        SelectList GetCultureList();

        /// <summary>
        /// 取得選項的下拉選單
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        SelectList GetOptionList(string category);

        /// <summary>
        /// 取得選項的複合下拉選單
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        Dictionary<string, SelectList> GetOptionFormatter(params string[] pars);

        /// <summary>
        /// 取得功能表下拉選單
        /// </summary>
        /// <returns></returns>
        SelectList GetMenuList();

        /// <summary>
        /// 取得ActClass名稱
        /// </summary>
        /// <returns></returns>
        List<string> GetActClassList();

        /// <summary>
        /// 取得 GetCardTitleSelectList 下拉選單
        /// </summary>
        /// <param name="entity"></param>
        SelectList GetCardTitleSelectList(Guid? id);

        /// <summary>
        /// 取得 GeOptionSelectList 下拉選單
        /// </summary>
        /// <param name="entity"></param>
        SelectList GetOptionSelectList(string category, string value);

        /// <summary>
        /// 取得名稱
        /// </summary>
        /// <returns></returns>
        List<string> GetNameList(string category);

        /// <summary>
        /// 取得 GetOptionSelectListByID 下拉選單
        /// </summary>
        /// <param name="entity"></param>
        SelectList GetOptionSelectListByID(string category, string value);

    }
}
