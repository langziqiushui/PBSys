namespace Common.DB.SQLServer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    ///SQLServerHandler 的摘要说明
    /// </summary>
    public class DBHelper : IDBHelper
    {
        string connectionString;
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public DbConnection CreateConnection()
        {

            return new SqlConnection(connectionString);
        }


        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter parameter = CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        public DbParameter CreateParameter(string name)
        {
            DbParameter parameter = CreateParameter();
            parameter.ParameterName = name;
            return parameter;
        }

        public DbParameter CreateParameter()
        {
            return new SqlParameter();
        }


        public T ExecuteScalar<T>(string sql, params DbParameter[] parameters)
        {

            using (DbConnection connection = CreateConnection())
            {
                DbCommand cmd = CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                }
                connection.Open();
                object o = cmd.ExecuteScalar();
                connection.Close();

                cmd.Parameters.Clear();
                cmd.Dispose();

                return (T)Convert.ChangeType(o, typeof(T));
            }
        }


        public DbDataReader ExecuteReader(string sql, params DbParameter[] parameters)
        {
            DbConnection connection = CreateConnection();

            DbCommand cmd = CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
            }

            connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ExecuteNoneQuery(string sql, params DbParameter[] parameters)
        {
            using (DbConnection connection = CreateConnection())
            {

                DbCommand cmd = CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                }

                connection.Open();
                int num = cmd.ExecuteNonQuery();
                connection.Close();

                cmd.Parameters.Clear();
                cmd.Dispose();

                return num;
            }
        }

        public List<Hashtable> GetDataList(string sql, params DbParameter[] parameters)
        {
            List<Hashtable> list = new List<Hashtable>();
            using (DbDataReader reader = ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    Hashtable hs = new Hashtable();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        hs[reader.GetName(i)] = reader.GetValue(i);
                    }

                    list.Add(hs);
                }
                reader.Close();
                reader.Dispose();
            }

            return list;
        }

        public Hashtable GetData(string sql, params DbParameter[] parameters)
        {
            Hashtable hs = null;
            using (DbDataReader reader = ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    hs = new Hashtable();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        hs[reader.GetName(i)] = reader.GetValue(i);
                    }
                }
                reader.Close();
                reader.Dispose();
            }

            return hs;
        }


        public DbParameter[] CopyParameters(DbParameter[] dbps)
        {
            List<DbParameter> list = new List<DbParameter>(dbps.Length);

            for (int i = 0; i < dbps.Length; i++)
            {
                var p = dbps[i];

                var np = CreateParameter(p.ParameterName, p.Value);

                list.Add(np);
            }

            return list.ToArray();
        }

    }


}