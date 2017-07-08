using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;

namespace YX.Factory
{
    /// <summary>
    /// 通用工厂对象。
    /// </summary>
    public static class ServicesFactory
    {
        #region 全局(Glo)

        /// <summary>
        /// 创建 系统日志服务对象。
        /// </summary>
        public static YX.IServices.Glo.IDblogServices CreateGloDblogServices()
        {
            return (YX.IServices.Glo.IDblogServices)CoreUtil.CreateObject("YX.Services", "YX.Services.Glo.DblogServices", null);
        }

        /// <summary>
        /// 创建 配置参数服务对象。
        /// </summary>
        public static YX.IServices.Glo.IDbconfigServices CreateGloDbconfigServices()
        {
            return (YX.IServices.Glo.IDbconfigServices)CoreUtil.CreateObject("YX.Services", "YX.Services.Glo.DbconfigServices", null);
        }

        /// <summary>
        /// 创建 管理员用户服务对象。
        /// </summary>
        public static YX.IServices.Glo.IAdminuserServices CreateGloAdminuserServices()
        {
            return (YX.IServices.Glo.IAdminuserServices)CoreUtil.CreateObject("YX.Services", "YX.Services.Glo.AdminuserServices", null);
        }

        /// <summary>
		/// 创建 屏蔽服务对象。
		/// </summary>
		public static YX.IServices.Glo.IPbconfigServices CreateGloPbconfigServices()
        {
            return (YX.IServices.Glo.IPbconfigServices)CoreUtil.CreateObject("YX.Services", "YX.Services.Glo.PbconfigServices", null);
        }
        #endregion
    }
}
