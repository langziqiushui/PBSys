using System.Collections.Generic;
namespace Common.DBX
{
    /// <summary>
    /// ResultList 的摘要说明
    /// </summary>
    public class ResultList : List<Common.DBX.NVCollection>
    {
        public ResultList()
            : base(10)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public ResultList(int capacity)
            : base(capacity)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
    }

}