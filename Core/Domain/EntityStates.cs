using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// 业务实体对象的状态 枚举。
    /// </summary>
    public enum EntityStates
    {
        Added = 1,
        Modified = 2,
        Deleted = 3,
        Unchanged = 0
    }
}
