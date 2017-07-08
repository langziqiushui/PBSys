using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;

namespace YX.SqlserverDAL
{
    /// <summary>
    /// 封装SqlParamete参数集合相关操作。
    /// </summary>
    public class SqlParameterList
    {
        public SqlParameterList()
        {
            this.dic = new Dictionary<string, SqlParameter>();
        }

        /// <summary>
        /// SqlParamete参数集合。
        /// </summary>
        private Dictionary<string, SqlParameter> dic = null;

        /// <summary>
        /// 添加参数。
        /// </summary>
        /// <param name="parm"></param>
        public void Add(SqlParameter parm)
        {
            this.dic.Add(parm.ParameterName, parm);
        }

        /// <summary>
        /// 通过参数名称获取Sql参数。
        /// </summary>
        /// <param name="parmName"></param>
        /// <returns></returns>
        public SqlParameter this[string parmName]
        {
            get { return this.dic[parmName]; }
            set { this.dic[parmName] = value; }
        }

        /// <summary>
        /// 呈现SqlParameter数组对象。
        /// </summary>
        /// <returns></returns>
        public SqlParameter[] ToArray()
        {
            int index = 0;
            SqlParameter[] parms = new SqlParameter[this.dic.Count];

            foreach (string _key in this.dic.Keys)
            {
                parms[index] = this.dic[_key];
                index += 1;
            }

            return parms;
        }
    }
}

