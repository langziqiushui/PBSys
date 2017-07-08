using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Web.Framework
{
    /// <summary>
    /// 系统管理基类。
    /// </summary>
    public class BaseSystemConfig : BaseAdmin
    {
        /// <summary>
        /// 注册页面基础对象。
        /// </summary>
        protected override void RegPageObjects()
        {
            base.RegPageObjects();

            this.RefJavascript(false, "Modules/SystemConfig.js");
            this.RefStyle(false, "Modules/SystemConfig.css");
        }
    }
}
