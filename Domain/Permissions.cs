using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Domain
{
    /// <summary>
    /// 系统权限许可。
    /// </summary>
    /// <remarks>
    ///     许可枚举说明：
    ///     模块_类别_具体权限 = 模块值类别值具体权限值
    ///     许可值说明：模块值(3位)类别值(3位)许可值(3位)，如100100100
    ///     不同模块值以100递增；
    ///     不同类别值以1递增；
    ///     许可值以1递增;
    /// </remarks>
    public enum Permissions
    {
        #region 系统设置

        系统设置_系统日志_查看日志列表 = 100100100,
        系统设置_系统日志_删除日志_T = 100100101,
        系统设置_配置参数_查看参数配置 = 100103100,
        系统设置_配置参数_修改参数配置_T = 100103101,      
        系统设置_其他设置_系统工具 = 100199101,

        #endregion
    }
}
