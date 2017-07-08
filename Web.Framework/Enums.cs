using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;

namespace YX.Web.Framework
{
    #region 事件委托   

    #endregion

    #region 用户控件工作模式

    /// <summary>
    /// 用户控件工作模式。
    /// </summary>
    public enum WorkModes
    {
        /// <summary>
        /// 创建。
        /// </summary>
        Create = 1,
        /// <summary>
        /// 更新。
        /// </summary>
        Modify = 2,
        /// <summary>
        /// 删除。
        /// </summary>
        Delete = 3,
        /// <summary>
        /// 查看。
        /// </summary>
        View = 4,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 5,
        /// <summary>
        /// 无。
        /// </summary>
        NONE = 9999
    }

    #endregion

    #region 验证码类别

    /// <summary>
    /// 验证码类别。
    /// </summary>
    public enum VerifyCodeTypes
    {
        /// <summary>
        /// 管理员登录
        /// </summary>
        AdminLogin,
        CardPost,        
    }

    #endregion
}
