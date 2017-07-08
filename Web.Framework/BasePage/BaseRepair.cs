using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Web.Framework
{
    /// <summary>
    /// 错误页面基类。
    /// </summary>
    public class BaseRepair : BaseWeb
    {
        /// <summary>
        /// 注册页面基础对象。
        /// </summary>
        protected override void RegPageObjects()
        {
            base.RegPageObjects();
            this.RefStyle(false, "Modules/Repair.css");
        }
    }
}
