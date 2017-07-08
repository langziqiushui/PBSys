using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Web.Framework
{
    /// <summary>
    /// 登录基类。
    /// </summary>
    public class BaseLogin : BaseWeb
    {
        /// <summary>
        /// 注册页面基础对象。
        /// </summary>
        protected override void RegPageObjects()
        {
            base.RegPageObjects();
              
            this.RefJavascript(false, "Modules/Login.js");
            this.RefStyle(false, "Modules/Login.css");
        }
    }
}
