using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluedragon.weixin.tool.common
{
    /// <summary>
    /// 配置文件辅助类
    /// </summary>
    public class ConfigurationHelper
    {
        #region 专用
        /// <summary>
        /// appId
        /// </summary>
        public static string AppId
        {
            get { return ConfigurationManager.AppSettings["appId"]; }
        }

        /// <summary>
        /// appSecret
        /// </summary>
        public static string AppSecret
        {
            get { return ConfigurationManager.AppSettings["appSecret"]; }
        }

        /// <summary>
        /// token
        /// </summary>
        public static string Token
        {
            get { return ConfigurationManager.AppSettings["token"]; }
        }
        #endregion

        #region 通用
        /// <summary>
        /// 判断指定键值的AppSetting是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsAppSettingExist(string key)
        {
            return !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// 为配置文件添加指定AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (cfg.AppSettings.Settings[key] == null)
            {
                //配置节点不存在，添加
                cfg.AppSettings.Settings.Add(key, value);
                cfg.Save(ConfigurationSaveMode.Full);
            }
            else if (cfg.AppSettings.Settings[key].Value != value)
            {
                //配置节点存在，值变化，更新值
                cfg.AppSettings.Settings[key].Value = value;
                cfg.Save(ConfigurationSaveMode.Full);
            }
        }
        #endregion
    }
}
