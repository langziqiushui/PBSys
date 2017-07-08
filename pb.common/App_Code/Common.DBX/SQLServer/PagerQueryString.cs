using System.Text;

namespace Common.DBX.SQLServer
{
    public class PagerQueryString : IPagerQueryString
    {
        private int absolutePage = 1;

        private string fields = " * ";
        private int pageSize = 20;
        //private string identity = " ID ";

        private string sort = " ID desc ";
        private string table = string.Empty;
        private string where = " 1=1 ";

        public string GetQueryString()
        {
            int p = this.absolutePage;

            if (this.absolutePage < 1)
            {
                this.absolutePage = 1;
            }

            StringBuilder builder = new StringBuilder();
            if (p > 1)
            {
                builder.Append("select * from (");

                builder.Append("select ");
                builder.Append(this.fields);
                builder.Append(",ROW_NUMBER () OVER (ORDER BY " + this.sort + ") AS RowNumber ");

                builder.Append(" from " + this.table + " with(nolock) where ");
                builder.Append(this.where);
                builder.Append(") AS temptb");
                builder.Append(" WHERE temptb.RowNumber BETWEEN " + ((p - 1) * pageSize + 1) + "  AND " + (p * pageSize));
            }
            else
            {
                builder.Append(" select top ");
                builder.Append(this.pageSize);
                builder.Append(" ");
                builder.Append(this.fields);
                builder.Append(" from ");
                builder.Append(this.table);
                builder.Append(" with(nolock)  where ");
                builder.Append(this.where);
                builder.Append(" order by ");
                builder.Append(this.sort);
            }
            return builder.ToString();
        }

        public string GetCountQueryString()
        {
            return "select count(1) from " + this.table + " with(nolock)  where " + this.where;
        }

        public int AbsolutePage
        {
            get
            {
                return this.absolutePage;
            }

            set
            {
                this.absolutePage = value;
            }
        }

        private System.Text.RegularExpressions.Regex zeroFR = new System.Text.RegularExpressions.Regex(@"^0,");

        public string Fields
        {
            get
            {
                return this.fields;
            }
            set
            {
                this.fields = zeroFR.Replace(value, "");
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }

        /*
        public string Identity
        {
            get
            {
                return this.identity;
            }
            set
            {
                this.identity = value;
            }
        }
        */

        public string Sort
        {
            get
            {
                return this.sort;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.sort = value;
                }
            }
        }

        public string Table
        {
            get
            {
                return this.table;
            }
            set
            {
                this.table = value;
            }
        }

        public string Where
        {
            get
            {
                return this.where;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.where = value;
                }
            }
        }
    }
}