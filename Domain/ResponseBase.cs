using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Domain
{
    /// <summary>
    /// web服务输出基类。
    /// </summary>
    [Serializable]
    public class ResponseBase
    {         
        /// <summary>
        /// 获取或设置 执行是否成功。
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 获取或设置 执行失败的消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置 其它信息。
        /// </summary>
        public string OtherInfo { get; set; }
    }

}
