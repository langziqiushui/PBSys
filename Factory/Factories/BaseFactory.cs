using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using YX.Core;

namespace YX.Factory
{
    /// <summary>
    /// 工厂基类。
    /// </summary>
    public abstract class BaseFactory
    {
        /// <summary>
        /// 依赖于配置文件反射创建对象。
        /// </summary>
        /// <param name="appSettingKey">配置文件appSettings配置节中的key</param>
        /// <param name="required">此配置是否为必须的</param>
        /// <returns></returns>
        protected virtual object CreateObject(string appSettingKey, bool required)
        {
            return this.CreateObject(appSettingKey, required, null);
        }

        /// <summary>
        /// 依赖于配置文件反射创建对象。
        /// </summary>
        /// <param name="appSettingKey">配置文件appSettings配置节中的key</param>
        /// <param name="required">此配置是否为必须的</param>
        /// <param name="args">实例化对象时所需要的参数</param>
        /// <returns></returns>
        protected virtual object CreateObject(string appSettingKey, bool required, object[] args)
        {
            //检查配置项是否存在。
            if (null == ConfigurationManager.AppSettings[appSettingKey])
            {
                if (!required)
                    return null;
                throw new AppException("对不起，未能在配置文件中找到key为" + appSettingKey + "的appSettings配置项。", ExceptionLevels.Warning);
            }

            //反射创建对象。
            string[] ts = ConfigurationManager.AppSettings[appSettingKey].Split(',');
            return CoreUtil.CreateObject(ts[1], ts[0], args);
        }
    }
}
