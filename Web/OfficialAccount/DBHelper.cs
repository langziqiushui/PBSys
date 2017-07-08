using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace YX.Web.OfficialAccount
{
    public static class DBHelper
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable getData(string sql)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            {
                //SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter dap = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                dap.Dispose();
                return dt;
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int execSql(string sql)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return i;
            }
        }
    }
}