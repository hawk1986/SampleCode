using SampleCode;
using SampleCode.Interface;
using SampleCode.Models;
using ResourceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SampleCode.Manager
{
    public class CommonManager : ICommonManager
    {
        readonly IDepartmentRepository _departmentRepository;
        readonly IFunctionRepository _functionRepository;
        readonly IModuleRepository _moduleRepository;
        readonly IRoleRepository _roleRepository;
        readonly IUserRepository _userRepository;
        readonly IVisualMenuRepository _visualMenuRepository;
        readonly ISystemOptionRepository _systemOptionRepository;
        readonly IFileAttachedRepository _fileAttachedRepository;

        public CommonManager(
            IDepartmentRepository departmentRepository,
            IFunctionRepository functionRepository,
            IModuleRepository moduleRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IVisualMenuRepository visualMenuRepository,
            ISystemOptionRepository systemOptionRepository,
            IFileAttachedRepository fileAttachedRepository)
        {
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
            _moduleRepository = moduleRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _visualMenuRepository = visualMenuRepository;
            _systemOptionRepository = systemOptionRepository;
            _fileAttachedRepository = fileAttachedRepository;
        }

        /// <summary>
        /// 取得對應 controller 中的 所有回傳型態為 ActionResult 的 method
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public SelectList GetActionList(string controllerName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            var actionResult = assembly
                .GetTypes()
                .FirstOrDefault(type => typeof(Controller).IsAssignableFrom(type) && type.Name == (controllerName + "Controller"))
                .GetMethods()
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && method.ReturnType == typeof(ActionResult))
                .Select(v => new SelectListItem { Text = v.Name, Value = v.Name })
                .OrderBy(x => x.Text)
                .Distinct()
                .ToList();

            var result = new SelectList(actionResult, "Value", "Text");

            return result;
        }

        /// <summary>
        /// 取得組件中所有 controller name
        /// </summary>
        /// <returns></returns>
        public SelectList GetControllerList()
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            var controller = assembly
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .Select(v => new SelectListItem { Text = v.Name.Replace("Controller", ""), Value = v.Name.Replace("Controller", "") })
                .OrderBy(x => x.Text)
                .ToList();
            var result = new SelectList(controller, "Value", "Text");

            return result;
        }

        /// <summary>
        /// 取得 Department 下拉
        /// </summary>
        /// <returns></returns>
        public SelectList GetDepartmentList()
        {
            var objectSet = _departmentRepository
                .Where(x => x.IsEnable)
                .Select(x => new { x.ID, x.Name })
                .ToList();
            var result = new SelectList(objectSet, "ID", "Name");

            return result;
        }

        /// <summary>
        /// 取得功能下拉
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public SelectList GetFunctionList(List<Guid> roles)
        {
            if (null == roles || roles.Count == 0)
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
            var functionIds = _roleRepository
                .Where(x => roles.Contains(x.ID))
                .SelectMany(x => x.Function)
                .Select(x => x.ID)
                .Distinct();

            var objectSet = _functionRepository
                .Where(x => x.IsEnable && (x.DisplayTree || x.SimpleName == "View") && functionIds.Contains(x.ID))
                .Select(x => new { x.ID, x.DisplayName })
                .ToList();
            var result = new SelectList(objectSet, "ID", "DisplayName");

            return result;
        }

        /// <summary>
        /// 取得 Module 下拉
        /// </summary>
        /// <returns></returns>
        public SelectList GetModuleList()
        {
            var temp = _moduleRepository.Where(x => x.IsEnable).ToList();
            foreach (var item in temp)
            {
                item.Name = LanguageTool.GetResourceValue(item.Name);
            }
            var objectSet = temp
                .Select(x => new { x.ID, x.Name })
                .ToList();
            var result = new SelectList(objectSet, "ID", "Name");

            return result;
        }

        /// <summary>
        /// 取得 Role 下拉
        /// </summary>
        /// <returns></returns>
        public SelectList GetRoleList()
        {
            var objectSet = _roleRepository
                .Where(x => x.IsEnable)
                .Select(x => new { x.ID, x.Name })
                .ToList();
            var result = new SelectList(objectSet, "ID", "Name");

            return result;
        }

        /// <summary>
        /// 取得 User 下拉
        /// </summary>
        /// <returns></returns>
        public SelectList GetUserList()
        {
            var objectSet = _userRepository
                .Where(x => x.IsEnable)
                .Select(x => new { x.ID, x.Name })
                .ToList();
            var result = new SelectList(objectSet, "ID", "Name");

            return result;
        }

        /// <summary>
        /// 取得是/否下拉
        /// </summary>
        /// <returns></returns>
        public SelectList GetYesNoList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = true.ToString(), Text = "是" });
            list.Add(new SelectListItem { Value = false.ToString(), Text = "否" });
            var result = new SelectList(list, "Value", "Text");

            return result;
        }

        /// <summary>
        /// 取得語系的下拉選單
        /// </summary>
        /// <returns></returns>
        public SelectList GetCultureList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "繁體中文" });
            list.Add(new SelectListItem { Value = "2", Text = "英文" });
            var result = new SelectList(list, "Value", "Text");

            return result;
        }

        /// <summary>
        /// 取得選項的下拉選單
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public SelectList GetOptionList(string category)
        {
            var list = new List<SelectListItem>();
            var options = Global.GetOptions(category);
            if (options.Count > 0)
                list.AddRange(options.Select(x => new SelectListItem { Value = x.Key, Text = x.Value }));            
            var result = new SelectList(list, "Value", "Text");

            return result;
        }

        /// <summary>
        /// 取得選項的複合下拉選單
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Dictionary<string, SelectList> GetOptionFormatter(params string[] pars)
        {
            var result = new Dictionary<string, SelectList>();
            var map = Global.GetOptions(pars);
            foreach (string category in pars)
            {
                var list = new List<SelectListItem>();
                if (map.ContainsKey(category))
                    list.AddRange(map[category].ConvertAll(x => new SelectListItem { Value = x.Key, Text = x.Value }));
                result[string.Concat(category, "Formatter")] = new SelectList(list, "Value", "Text");
            }

            return result;
        }

        /// <summary>
        /// 取得功能表下拉選單
        /// </summary>
        /// <returns></returns>
        public SelectList GetMenuList()
        {
            return new SelectList(Global.GetMenus(), "ID", "MenuName");
        }

        /// <summary>
        /// 取得ActClass名稱
        /// </summary>
        /// <returns></returns>
        public List<string> GetActClassList()
        {
            var temp = new List<string>();
            var actClass = _systemOptionRepository.GetAll().Where(x => x.Category == "ActClass").ToList();
            temp.AddRange(actClass.Select(x => x.OptionValue));

            return temp;
        }

        /// <summary>
        /// 取得 GetCardTitleSelectList 下拉選單
        /// </summary>
        /// <param name="entity"></param>
        public SelectList GetCardTitleSelectList(Guid? id)
        {
            var source = _systemOptionRepository.Where(x => x.Category == "CardTitle").ToList();
            List<SelectListItem> SelectListItem = new List<SelectListItem>();
            if (source.Count > 0)
                SelectListItem.AddRange(source.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.OptionValue }));
            if (!id.HasValue)
            {
                var result = new SelectList(SelectListItem, "Value", "Text", null);
                return result;
            }
            else
            {
                var result = new SelectList(SelectListItem, "Value", "Text", id);
                return result;
            }
        }

        /// <summary>
        /// 取得 GeOptionSelectList 下拉選單
        /// </summary>
        /// <param name="entity"></param>
        public SelectList GetOptionSelectList(string category, string value)
        {
            var source = _systemOptionRepository.Where(x => x.Category == category).ToList();
            List<SelectListItem> SelectListItem = new List<SelectListItem>();
            if (source.Count > 0)
                SelectListItem.AddRange(source.Select(x => new SelectListItem { Value = x.OptionKey.ToString(), Text = x.OptionValue }));
            if (string.IsNullOrEmpty(value))
            {
                var result = new SelectList(SelectListItem, "Value", "Text", null);
                return result;
            }
            else
            {
                var result = new SelectList(SelectListItem, "Value", "Text", value);
                return result;
            }
        }

        /// <summary>
        /// 取得名稱
        /// </summary>
        /// <returns></returns>
        public List<string> GetNameList(string category)
        {
            var temp = new List<string>();
            var result = _systemOptionRepository.GetAll().Where(x => x.Category == category).ToList();
            temp.AddRange(result.Select(x => x.OptionValue));

            return temp;
        }

        /// <summary>
        /// 取得 GetOptionSelectListByID 下拉選單
        /// </summary>
        /// <param name="entity"></param>
        public SelectList GetOptionSelectListByID(string category, string value)
        {
            var source = _systemOptionRepository.Where(x => x.Category == category).ToList();
            List<SelectListItem> SelectListItem = new List<SelectListItem>();
            if (source.Count > 0)
                SelectListItem.AddRange(source.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.OptionValue }));
            if (string.IsNullOrEmpty(value))
            {
                var result = new SelectList(SelectListItem, "Value", "Text", null);
                return result;
            }
            else
            {
                var result = new SelectList(SelectListItem, "Value", "Text", value);
                return result;
            }
        }

    }
}