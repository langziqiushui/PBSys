using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.IO;
using System.Web;
using YX.Factory.TaskManagers;
using YX.Factory.TaskManagers.PlugIns;

namespace YX.Factory
{
    /// <summary>
    /// 定时任务管理器。
    /// </summary>
    public class TaskManager
    {
        #region 变量

        /// <summary>
        /// 并发控制锁对象。
        /// </summary>
        private readonly object __LOCK__ = new object();
        /// <summary>
        /// 基础定时器。
        /// </summary>
        private Timer timerBase = null;
        /// <summary>
        /// 任务管理器集合。
        /// </summary>
        private IList<ITaskManager> dataTaskManager = null;
        /// <summary>
        /// 定时器运行时间间隔(单位：毫秒，10000)
        /// </summary>
        private const int timerPeriod = 10000;

        #endregion

        #region 控制

        /// <summary>
        /// 启动。
        /// </summary>
        public void Start()
        {
            if (null != this.timerBase)
                return;

            //添加任务管理器。
            this.dataTaskManager = new List<ITaskManager>()
            {
                new BackupLogTaskManager(),
               
            };

            //首次延时5秒，定时10秒执行。
            this.timerBase = new Timer(timerBaseCallback, null, 5000, timerPeriod);
        }

        /// <summary>
        /// 停止。
        /// </summary>
        public void Stop()
        {
            lock (__LOCK__)
            {
                if (null == this.timerBase)
                    return;

                this.timerBase.Dispose();
                this.timerBase = null;
                this.dataTaskManager.Clear();
                this.dataTaskManager = null;
            }
        }

        #endregion

        #region 定时执行

        /// <summary>
        /// 定时执行。
        /// </summary>
        /// <param name="state"></param>
        private void timerBaseCallback(object state)
        {
            lock (__LOCK__)
            {
                if (null == this.dataTaskManager)
                    return;

                foreach (var _taskManager in this.dataTaskManager)
                {
                    //如果需要则开启异步线路工作。
                    if (_taskManager.IsNeedWork(timerPeriod / 1000))
                    {
                        Action invoke = () => { _taskManager.Work(); };
                        invoke.BeginInvoke(null, null);
                    }

                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        #endregion

        #region 创建单例

        /// <summary>
        /// 获取 当前对象的一个单例。
        /// </summary>
        public static TaskManager Ins
        {
            get { return Nested.ins; }
        }

        class Nested
        {
            static Nested() { }
            internal readonly static TaskManager ins = new TaskManager();
        }

        #endregion
    }
}
