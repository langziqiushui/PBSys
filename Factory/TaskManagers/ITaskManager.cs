using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Factory.TaskManagers
{
    /// <summary>
    /// 定时任务管理器接口。
    /// </summary>
    public interface ITaskManager
    {
        /// <summary>
        /// 获取 是否需要进行操作。
        /// </summary>
        /// <param name="timerPeriod">定时器时间间隔(单位：秒)</param>
        bool IsNeedWork(int timerPeriod);

        /// <summary>
        /// 开始工作。
        /// </summary>
        void Work();
    }
}
