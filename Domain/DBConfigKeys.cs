using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;

namespace YX.Domain
{
    /// <summary>
    /// 数据库配置参数。
    /// </summary>
    public enum DBConfigKeys
    {
        //特别提示：比较复杂的配置，请在枚举项的属性上加上取值说明！
        //EnumDescription 第一个参数为显示标题，第二个参数为说明(如取值范围、boolean 或 数字等)
        //修改配置参数请进入服务中心 -> 系统管理 -> 基本设置 -> 配置参数管理
        //同一类配置请以相同前缀加_区别。

        #region 日志相关

        /// <summary>
        /// 日志的URL 过滤的关键词。
        /// </summary>
        [EnumDescription("日志404的UR过滤的关键词L", "日志404的UR过滤的关键词L(多个关键词之间以|号分隔)")]
        Log_404filterKeywords,

        /// <summary>
        /// 日志的URL 过滤的关键词。
        /// </summary>
        [EnumDescription("日志500的UR过滤的关键词L", "日志500的UR过滤的关键词L(多个关键词之间以|号分隔)")]
        Log_500filterKeywords,

        #endregion
    }
}
