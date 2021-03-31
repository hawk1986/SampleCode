using Castle.Windsor;
using Castle.Windsor.Installer;
using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SampleCode.IoC;
using SampleCode.Models;
using SampleCode.Models.Public;
using Utilities.Extensions;

namespace SampleCode
{
    public class Global : HttpApplication
    {
        #region IoC Container
        static IWindsorContainer _container;

        static void _bootstrapContainer()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            // Controller IoC
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            // Web Api IoC
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container);
        }
        #endregion

        #region Lock Object
        /// <summary>
        /// Lock Object
        /// </summary>
        static readonly object _objDataLocker = new object();
        #endregion

        #region Cache Object
        /// <summary>
        /// Home Object
        /// </summary>
        static HomeMapping _homeMapping { get; set; } = new HomeMapping();
        #endregion

        #region Get Cache Object
        /// <summary>
        /// 取得首頁資訊
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static HomeInfo GetHomeInfo(Culture culture)
        {
            lock (_objDataLocker)
                return HomeInfo.Parser(_homeMapping, culture);
        }

        /// <summary>
        /// 取得系統組態
        /// </summary>
        /// <returns></returns>
        public static List<Config> GetConfigs()
        {
            lock (_objDataLocker)
                return _homeMapping.Config.Values.ToList();
        }

        /// <summary>
        /// 取得系統選項
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static List<Option> GetOptions(string category)
        {
            lock (_objDataLocker)
                return _homeMapping.Option.Values.Where(x => x.Category == category).ToList();
        }

        /// <summary>
        /// 取得系統選項
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static Dictionary<string, List<Option>> GetOptions(params string[] pars)
        {
            lock (_objDataLocker)
                return _homeMapping.Option.Values.Where(x => pars.Contains(x.Category)).GroupBy(x => x.Category).ToDictionary(x => x.Key, y => y.ToList());
        }

        /// <summary>
        /// 取得功能選項的階層樹
        /// </summary>
        /// <returns></returns>
        public static List<MenuTree> GetMenuTrees()
        {
            //取得所有的功能選項
            List<MenuTree> menus = (from x in _homeMapping.Menu.Values
                                    where x.IsEnable
                                    select new MenuTree
                                    {
                                        SequenceNo = x.SequenceNo,
                                        ID = x.ID,
                                        ParentID = x.ParentID,
                                        Sequence = x.Sequence,
                                        MenuLevel = x.MenuLevel,
                                        MenuCode = x.MenuCode ?? string.Empty,
                                        MenuName = x.MenuName ?? string.Empty,
                                        Layout = x.Layout ?? string.Empty,
                                        IconPath = x.IconPath ?? string.Empty,
                                        HeaderPath = x.HeaderPath ?? string.Empty,
                                        ViewCount = x.ViewCount,
                                        Controller = x.Controller ?? string.Empty,
                                        Action = x.Action ?? string.Empty,
                                        Parameter = x.Parameter ?? string.Empty,
                                        ExternalUrl = x.ExternalUrl ?? string.Empty,
                                        IsInlinePage = x.IsInlinePage,
                                        IsShowFooter = x.IsShowFooter,
                                        IsEnable = x.IsEnable,
                                    }).ToList();
            //將單位依所屬上級分類
            var lookup = menus.ToLookup(x => x.ParentID);
            return lookup[null].SelectRecursiveChildren(x => lookup[x.ID]).ToList();
        }

        /// <summary>
        /// 取得功能表清單
        /// </summary>
        /// <returns></returns>
        public static List<Menu> GetMenus()
        {
            lock (_objDataLocker)
                return _homeMapping.Menu.Values.ToList();
        }
        #endregion

        #region Update Cache Object
        /// <summary>
        /// 更新對應清單
        /// </summary>
        /// <param name="type"></param>
        /// <param name="repository"></param>
        public static void UpdateMapping(string type, dynamic repository)
        {
            lock (_objDataLocker)
            {
                switch (type)
                {
                    case "User":
                        _homeMapping.UpdateUser((IUserRepository)repository);
                        break;
                    case "Config":
                        _homeMapping.UpdateConfig((ISystemConfigRepository)repository);
                        break;
                    case "Option":
                        _homeMapping.UpdateOption((ISystemOptionRepository)repository);
                        break;
                }
            }
        }

        /// <summary>
        /// 更新物件實體
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entity"></param>
        /// <param name="remove"></param>
        public static void UpdateEntity(string type, dynamic entity, List<Guid> remove = null)
        {
            lock (_objDataLocker)
            {
                switch (type)
                {
                    case "User":
                        _homeMapping.UpdateUser((User)entity, remove);
                        break;
                    case "Config":
                        _homeMapping.UpdateConfig((SystemConfig)entity, remove);
                        break;
                    case "Option":
                        _homeMapping.UpdateOption((SystemOption)entity, remove);
                        break;
                }
            }
        }
        #endregion

        #region Application Event
        protected void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _bootstrapContainer();
            MiniProfilerEF6.Initialize();
            _homeMapping.Initialize();

            //Add it to WebApi formatters collection
            GlobalConfiguration.Configuration.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter(new MultipartFormatterSettings()));
        }

        protected void Application_End()
        {
            _homeMapping.Dispose();
            _container.Dispose();
        }

        protected void Application_BeginRequest()
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.End();
            }

            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
        #endregion
    }
}