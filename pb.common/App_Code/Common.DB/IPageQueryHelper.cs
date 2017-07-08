using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DB
{
    public interface IPageQueryHelper
    {
        /// <summary>
        /// 获得数据查询SQL字符串
        /// </summary>
        /// <returns></returns>
        string GetQueryString();

        /// <summary>
        /// 获得数据数量查询SQL字符串
        /// </summary>
        /// <returns></returns>
        string GetCountQueryString();

        /// <summary>
        /// 锁定页
        /// </summary>
        int AbsolutePage { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        string Identity { get; set; }

        /// <summary>
        /// 筛选的字段
        /// </summary>
        string Fields { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        string Sort { get; set; }

        /// <summary>
        /// 表名
        /// </summary>        
        string Table { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        string Where { get; set; }
    }
}
