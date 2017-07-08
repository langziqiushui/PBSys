using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using YX.Domain;
using YX.Core;

namespace YX.Factory.TaskManagers.PlugIns
{
    /// <summary>
    /// 定时备份系统日志管理器。
    /// </summary>
    public class BackupLogTaskManager : ITaskManager
    {
        #region 变量

        /// <summary>
        /// 服务对象(日志)。
        /// </summary>
        private IServices.Glo.IDblogServices serviceDBLog = Factory.ServicesFactory.CreateGloDblogServices();
       
        /// <summary>
        /// 判断当前是否正操作。
        /// </summary>
        private bool isInProgress = false;
        /// <summary>
        /// 记录最后一次操作时间的文件路径。
        /// </summary>
        private string filePathLatestLogTime = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\LatestLogTime_BackupLog.txt";
        /// <summary>
        /// 自上次清零后定时器累计运行次数。
        /// </summary>
        private int numTotalRun = 0;
        /// <summary>
        /// 工作频率(单位：秒，180)
        /// </summary>
        private const int runFrequency = 180;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置 最后一次操作时间。
        /// </summary>
        private DateTime? LatestLogTime
        {
            get
            {
                try
                {
                    if (File.Exists(this.filePathLatestLogTime))
                        return DateTime.Parse(File.ReadAllText(this.filePathLatestLogTime, Encoding.UTF8));
                }
                catch { }

                return null;
            }
            set
            {
                File.WriteAllText(this.filePathLatestLogTime, value.ToString(), Encoding.UTF8);
            }
        }

        #endregion

        #region ITaskManager

        /// <summary>
        /// 获取 是否需要进行操作。
        /// </summary>
        /// <param name="timerPeriod">定时器时间间隔(单位：秒)</param>
        public bool IsNeedWork(int timerPeriod)
        {
            //定时器总运行次数加1。
            this.numTotalRun += 1;

            //如果当前正在操作。
            if (this.isInProgress)
                return false;

            //如果未达到执行频率。
            if (this.numTotalRun * timerPeriod < runFrequency)
                return false;            

            //定时执行备份日志的时间(凌晨2~3时)。
            var dtStart = DateTime.Today.AddHours(2);
            var dtEnd = DateTime.Today.AddHours(3);

            //判断是否需要执行备份。
            if (DateTime.Now >= dtStart && DateTime.Now <= dtEnd)
            {
                var dt = this.LatestLogTime;
                if (null == dt || dt.Value < dtStart)
                {
                    //计数器清零。
                    this.numTotalRun = 0;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 开始工作(备份日志)。
        /// </summary>
        public void Work()
        {
            try
            {
                this.isInProgress = true;

                //定时删除日志。
                var num = Convert.ToInt32(this.serviceDBLog.Delete_Regular());
                this.LatestLogTime = DateTime.Now;
                //LogFactory.Write(Domain.ApplicationTypes.Globals, Domain.LogTypes.消息, "定时删除日志成功，共删除 " + num + " 条数据！");
            }
            catch (Exception ex)
            {
                //LogFactory.Write(Domain.ApplicationTypes.Globals, Domain.LogTypes.错误, "定时删除日志出错：" + ex.ToString());
            }
            finally
            {
                this.isInProgress = false;
            }
        }

        #endregion
    }
}
