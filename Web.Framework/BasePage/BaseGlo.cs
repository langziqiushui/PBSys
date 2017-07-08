using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Web.Framework
{
    /// <summary>
    /// Glo架构基类。
    /// </summary>
    public class BaseGlo : BaseAdmin
    {
        /// <summary>
        /// 注册页面基础对象。
        /// </summary>
        protected override void RegPageObjects()
        {
            base.RegPageObjects();

            this.RefJavascript(false, "Modules/Glo.js");
            this.RefStyle(false, "Modules/Glo.css");
        }
    }
}
