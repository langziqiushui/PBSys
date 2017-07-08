using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YX.Core
{
    /// <summary>
    /// 通用Command 的参数。
    /// </summary>
    public class MyParameter
    {
        public MyParameter()
        {
            IsNullable = true;
        }

        public string ParameterName { get; set; }
        public DbType DbType { get; set; }
        public object Value { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsNullable { get; set; }
        public int Size { get; set; }
    }
}
