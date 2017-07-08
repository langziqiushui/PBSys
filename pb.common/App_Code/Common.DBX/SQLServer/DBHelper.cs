using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Common.DBX.SQLServer
{
    /// <summary>
    /// SQLServer DBHelper 的摘要说明
    /// </summary>
    public class DBHelper : IDBHelper
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlCommand CreateCommand()
        {
			var cmd = new SqlCommand();
			cmd.CommandTimeout = 5;
            return cmd;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection();
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public SqlParameter CreateParameter(string name, object value)
        {
            SqlParameter parameter = CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        public SqlParameter CreateParameter(string name)
        {
            SqlParameter parameter = CreateParameter();
            parameter.ParameterName = name;
            return parameter;
        }

        public SqlParameter CreateParameter()
        {
            return new SqlParameter();
        }

        public SqlParameter[] CopyParameters(SqlParameter[] dbps)
        {
            List<SqlParameter> list = new List<SqlParameter>(dbps.Length);

            for (int i = 0; i < dbps.Length; i++)
            {
                var p = dbps[i];

                var np = CreateParameter(p.ParameterName, p.Value);

                list.Add(np);
            }

            return list.ToArray();
        }

        private static Type NVC_Type = typeof(NVCollection);

        public NVCollection ConvertToNVC(object[] parameters)
        {
            NVCollection nvc = null;

            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    var obj = parameters[0];

                    if (obj.GetType() == NVC_Type)
                    {
                        nvc = obj as NVCollection;

                        return nvc;
                    }

                    nvc = new NVCollection(parameters.Length);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        nvc.Add(i.ToString(), parameters[i]);
                    }
                }
            }

            return nvc;
        }

        private void Attach(SqlCommand cmd, NVCollection nvc)
        {
            if (nvc != null)
            {
                if (nvc.Count > 0)
                {
                    foreach (var item in nvc)
                    {
                        var p = CreateParameter("@" + item.Key, item.Value);
                        cmd.Parameters.Add(p);
                    }

                    /*
                    for (int i = 0; i < nvc.Count; i++)
                    {
                        string key = nvc.GetKey(i);
                        var p = CreateParameter("@" + key, nvc[i]);
                        cmd.Parameters.Add(p);
                    }*/
                }
            }
        }

        public T ExecuteScalarP<T>(string sql, NVCollection parameters = null)
        {
            SqlConnection connection = GetConnection();

            SqlCommand cmd = CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;

            Attach(cmd, parameters);

            connection.Open();
            object o = cmd.ExecuteScalar();
            connection.Close();

            cmd.Parameters.Clear();
            cmd.Dispose();

            return (T)Convert.ChangeType(o, typeof(T));
        }

        public DbDataReader ExecuteReaderP(string sql, NVCollection parameters = null)
        {
            SqlConnection connection = GetConnection();
            SqlCommand cmd = CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;

            Attach(cmd, parameters);

            connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ExecuteNoneQueryP(string sql, NVCollection parameters = null)
        {
            SqlConnection connection = GetConnection();
            SqlCommand cmd = CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;

            Attach(cmd, parameters);

            connection.Open();
            int num = cmd.ExecuteNonQuery();
            connection.Close();

            cmd.Parameters.Clear();
            cmd.Dispose();

            return num;
        }

        public List<NVCollection> GetDataListP(string sql, NVCollection parameters = null)
        {
            List<NVCollection> list = new List<NVCollection>();

            using (DbDataReader reader = ExecuteReaderP(sql, parameters))
            {
                while (reader.Read())
                {
                    NVCollection nvc = new NVCollection(reader.FieldCount);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        nvc.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    list.Add(nvc);
                }
                reader.Close();
                reader.Dispose();
            }

            return list;
        }

        public NVCollection GetDataP(string sql, NVCollection parameters = null)
        {
            NVCollection nv = null;
            using (DbDataReader reader = ExecuteReaderP(sql, parameters))
            {
                while (reader.Read())
                {
                    nv = new NVCollection(reader.FieldCount);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        nv.Add(reader.GetName(i), reader.GetValue(i));
                    }
                }
                reader.Close();
                reader.Dispose();
            }

            return nv;
        }

        public T ExecuteScalar<T>(string sql, params object[] parameters)
        {
            return ExecuteScalarP<T>(sql, ConvertToNVC(parameters));
        }

        public DbDataReader ExecuteReader(string sql, params object[] parameters)
        {
            return ExecuteReaderP(sql, ConvertToNVC(parameters));
        }

        public int ExecuteNoneQuery(string sql, params object[] parameters)
        {
            return ExecuteNoneQueryP(sql, ConvertToNVC(parameters));
        }

        public NVCollection GetData(string sql, params object[] parameters)
        {
            return GetDataP(sql, ConvertToNVC(parameters));
        }

        public List<NVCollection> GetDataList(string sql, params object[] parameters)
        {
            return GetDataListP(sql, ConvertToNVC(parameters));
        }

        DbConnection IDBHelper.GetConnection()
        {
            return this.GetConnection();
        }

        DbCommand IDBHelper.CreateCommand()
        {
            return this.CreateCommand();
        }

        DbParameter IDBHelper.CreateParameter(string name, object value)
        {
            return this.CreateParameter(name, value);
        }

        DbParameter IDBHelper.CreateParameter(string name)
        {
            return this.CreateParameter(name);
        }

        DbParameter IDBHelper.CreateParameter()
        {
            return this.CreateParameter();
        }

        public DbParameter[] CopyParameters(DbParameter[] dbps)
        {
            return this.CopyParameters(dbps);
        }

        public List<Hashtable> GetHashDataList(string sql, params DbParameter[] parameters)
        {
            //System.Web.HttpContext.Current.Response.Write("\r\n/*" + sql + "*/\r\n");

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

        /*
        public T ExecuteScalarP<T>(string sql, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReaderP(string sql, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNoneQueryP(string sql, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public NVCollection GetDataP(string sql, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public List<NVCollection> GetDataListP(string sql, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }*/

    }
}