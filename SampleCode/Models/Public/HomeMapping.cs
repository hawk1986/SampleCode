using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SampleCode.Manager;

namespace SampleCode.Models.Public
{
    public class HomeMapping : IDisposable
    {
        #region Constructor
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            DbEntities dbContext = new DbEntities();
            var configRepository = new SystemConfigRepository(dbContext);
            //更新使用者
            UpdateUser(new UserRepository(dbContext));
            //更新系統組態
            UpdateConfig(configRepository);
            updateBrowseInfo(configRepository);
            //更新系統選項
            UpdateOption(new SystemOptionRepository(dbContext));
            //更新功能選項
            UpdateMenu(new VisualMenuRepository(dbContext));
        }

        /// <summary>
        /// 解構函數
        /// </summary>
        ~HomeMapping()
        {
            Dispose(false);
        }
        #endregion

        #region Cache Data
        /// <summary>
        /// 使用者對應清單
        /// </summary>
        public Dictionary<Guid, string> Users { get; set; }

        /// <summary>
        /// 系統組態
        /// </summary>
        public Dictionary<Guid, Config> Config { get; set; }

        /// <summary>
        /// 系統選項
        /// </summary>
        public Dictionary<Guid, Option> Option { get; set; }

        /// <summary>
        /// 功能選單
        /// </summary>
        public Dictionary<Guid, Menu> Menu { get; set; }
        #endregion

        #region Update Data (Repository)
        /// <summary>
        /// 更新使用者資料
        /// </summary>
        /// <param name="userRepository"></param>
        public void UpdateUser(IUserRepository userRepository)
        {
            var user = (from x in userRepository.GetAll()
                        where x.IsEnable
                        select new
                        {
                            x.ID,
                            x.Name,
                            x.IsToken,
                            x.HashToken,
                            x.TokenData
                        }).ToList();

            //紀錄使用者資料
            Users = user.ToDictionary(x => x.ID, y => y.Name);
            //復原Token紀錄
            TokenManager.Restore(user.Where(x => x.IsToken).Select(x => x.TokenData));
        }

        /// <summary>
        /// 更新系統組態
        /// </summary>
        /// <param name="configRepository"></param>
        public void UpdateConfig(ISystemConfigRepository configRepository)
        {
            Config = (from x in configRepository.GetAll()
                      select new Config
                      {
                          ID = x.ID,
                          Key = x.ConfigKey,
                          Value = x.ConfigValue
                      }).ToDictionary(x => x.ID, y => y);
        }

        /// <summary>
        /// 更新系統選項
        /// </summary>
        /// <param name="optionRepository"></param>
        public void UpdateOption(ISystemOptionRepository optionRepository)
        {
            Option = (from x in optionRepository.GetAll()
                      select new Option
                      {
                          ID = x.ID,
                          Category = x.Category,
                          Key = x.OptionKey,
                          Value = x.OptionValue
                      }).ToDictionary(x => x.ID, y => y);
        }

        /// <summary>
        /// 更新功能選項
        /// </summary>
        /// <param name="menuRepository"></param>
        public void UpdateMenu(IVisualMenuRepository menuRepository)
        {
            Menu = (from x in menuRepository.GetAll()
                    select x).ToDictionary(x => x.ID, y => (Menu)y);
        }

        /// <summary>
        /// 更新瀏覽資訊
        /// </summary>
        /// <param name="configRepository"></param>
        protected void updateBrowseInfo(ISystemConfigRepository configRepository)
        {
            var configs = Config.Values.ToList();
            //更新最近修改日期
            DateTime dtCreation = File.GetLastWriteTime(System.Web.Hosting.HostingEnvironment.MapPath("/bin/SampleCodedll"));
            string strCreation = dtCreation.ToString("yyyy/MM/dd HH:mm:ss");
            var configUpdateTime = configs.FirstOrDefault(x => x.Key == "UpdateTime");
            if (configUpdateTime == null)
            {
                var updateTime = new SystemConfig
                {
                    ID = Guid.NewGuid(),
                    ConfigKey = "UpdateTime",
                    ConfigValue = strCreation,
                    CreateUser = "WebAuto",
                    CreateTime = DateTime.Now,
                    UpdateUser = "WebAuto",
                    UpdateTime = DateTime.Now,
                };
                configRepository.Create(updateTime);
                Config.Add(updateTime.ID, new Config { ID = updateTime.ID, Key = updateTime.ConfigKey, Value = updateTime.ConfigValue });
            }
            //判斷日期有無修改
            else if (configUpdateTime.Value != strCreation)
            {
                configUpdateTime.Value = strCreation;
                var updateTime = configRepository.FirstOrDefault(x => x.ConfigKey == "UpdateTime");
                updateTime.ConfigValue = strCreation;
                configRepository.Update(updateTime);
            }

            //瀏覽次數
            if (!configs.Any(x => x.Key == "BrowseCount"))
            {
                var browseCount = new SystemConfig
                {
                    ID = Guid.NewGuid(),
                    ConfigKey = "BrowseCount",
                    ConfigValue = "1",
                    CreateUser = "WebAuto",
                    CreateTime = DateTime.Now,
                    UpdateUser = "WebAuto",
                    UpdateTime = DateTime.Now,
                };
                Config.Add(browseCount.ID, new Config { ID = browseCount.ID, Key = browseCount.ConfigKey, Value = browseCount.ConfigValue });
                configRepository.Create(browseCount);
            }
        }
        #endregion

        #region Update Data (Entity)
        /// <summary>
        /// 更新使用者
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="remove"></param>
        public void UpdateUser(User entity, List<Guid> remove)
        {
            //移除項目
            if (remove != null && remove.Count > 0)
            {
                foreach (var item in remove)
                    Users.Remove(item);
            }
            //新增或修改項目
            if (entity != null)
            {
                if (Users.ContainsKey(entity.ID))
                {
                    Users[entity.ID] = entity.Name;
                }
                else
                {
                    Users.Add(entity.ID, entity.Name);
                }
            }
        }

        /// <summary>
        /// 更新系統組態
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="remove"></param>
        public void UpdateConfig(SystemConfig entity, List<Guid> remove)
        {
            //移除項目
            if (remove != null && remove.Count > 0)
            {
                foreach (var item in remove)
                    Config.Remove(item);
            }
            //新增或修改項目
            if (entity != null)
            {
                if (Config.ContainsKey(entity.ID))
                {
                    var source = Config[entity.ID];
                    source.Key = entity.ConfigKey;
                    source.Value = entity.ConfigValue;
                }
                else
                {
                    Config.Add(entity.ID, new Config
                    {
                        ID = entity.ID,
                        Key = entity.ConfigKey,
                        Value = entity.ConfigValue
                    });
                }
            }
        }

        /// <summary>
        /// 更新系統選項
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="remove"></param>
        public void UpdateOption(SystemOption entity, List<Guid> remove)
        {
            //移除項目
            if (remove != null && remove.Count > 0)
            {
                foreach (var item in remove)
                    Option.Remove(item);
            }
            //新增或修改項目
            if (entity != null)
            {
                if (Option.ContainsKey(entity.ID))
                {
                    var source = Option[entity.ID];
                    source.Category = entity.Category;
                    source.Key = entity.OptionKey;
                    source.Value = entity.OptionValue;
                }
                else
                {
                    Option.Add(entity.ID, new Option
                    {
                        ID = entity.ID,
                        Category = entity.Category,
                        Key = entity.OptionKey,
                        Value = entity.OptionValue
                    });
                }
            }
        }

        /// <summary>
        /// 更新功能選項
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="remove"></param>
        public void UpdateMenu(VisualMenu entity, List<Guid> remove)
        {
            //移除項目
            if (remove != null && remove.Count > 0)
            {
                foreach (var item in remove)
                    Menu.Remove(item);
            }
            //新增或修改項目
            if (entity != null)
            {
                if (Menu.ContainsKey(entity.ID))
                {
                    Menu[entity.ID] = (Menu)entity;
                }
                else
                {
                    Menu.Add(entity.ID, (Menu)entity);
                }
            }
        }
        #endregion

        #region IDisposable
        //是否回收完畢
        bool _disposed;

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return; //如果已經被回收，就中斷執行
            if (disposing)
            {
                //更新瀏覽次數
                var count = Config?.Values.FirstOrDefault(x => x.Key == "BrowseCount");
                if (count != null)
                {
                    var configRepository = new SystemConfigRepository(new DbEntities());
                    var browseCount = configRepository.FirstOrDefault(x => x.ConfigKey == "BrowseCount");
                    if (browseCount != null)
                    {
                        browseCount.ConfigValue = count.Value;
                        configRepository.Update(browseCount);
                    }
                }
            }
            _disposed = true;
        }
        #endregion
    }
}