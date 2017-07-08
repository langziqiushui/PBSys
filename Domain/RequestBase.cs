using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Domain
{
    /// <summary>
    /// Web服务输入基类。
    /// </summary>
    [Serializable]
    public class RequestBase
    {
        /// <summary>
        /// 获取或设置 参数签名。
        /// </summary>
        public string Sign { get; set; }
    }
}
