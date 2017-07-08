using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Configuration;

namespace YX.Core
{
    /// <summary>
    /// 配置节处理类。
    /// </summary>
    public static class ConfigUtil
    {
        /// <summary>
        /// 获取AppSettings配置节点的值。
        /// </summary>
        /// <param name="key">AppSettings键值</param>
        internal static string GetAppSetting(string key)
        {
            if (null == WebConfigurationManager.AppSettings[key])
                throw new AppException("未能在配置文件AppSettings配置节中找到名称为 " + key + " 的项！");

            return WebConfigurationManager.AppSettings[key].Trim();
        }

        /// <summary>
        /// 获取AppSettings配置节点的值。
        /// </summary>
        /// <param name="key">AppSettings键值</param>
        /// <param name="defaultValue">如果不存在此配置节，则返回此默认值</param>
        internal static string GetAppSetting(string key, string defaultValue)
        {
            if (null == WebConfigurationManager.AppSettings[key])
                return defaultValue;

            return WebConfigurationManager.AppSettings[key].Trim();
        }

        /// <summary>
        /// 保存AppSettings配置节点的值。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        internal static void SetAppSetting(string key, string value)
        {
            bool c = false;
            var config = WebConfigurationManager.OpenWebConfiguration("~");

            if (null == config.AppSettings.Settings[key])
            {
                config.AppSettings.Settings.Add(key, value);
                c = true;
            }
            else if (config.AppSettings.Settings[key].Value != value)
            {
                config.AppSettings.Settings[key].Value = value;
                c = true;
            }

            //如果值已经更改，则保存web.config，应用程序会重启。
            if (c)
            {
                try
                {
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
                catch (Exception)
                {
                    throw new Exception("对不起，保存配置文件出错可能没有设置文件写入权限！");
                }
            }
        }
    }

}
