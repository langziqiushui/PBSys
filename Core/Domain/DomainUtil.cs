using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.IO;
using System.Management;

//using Net.AfritXia.Data;

namespace YX.Core
{
    /// <summary>
    /// 公用方法有用类。
    /// </summary>
    public static class DomainUtil
    {
        #region 填充业务实体

        /// <summary>
        /// 表示从候选者列表中选择一个成员，并执行实参类型到形参类型的类型转换。
        /// </summary>
        private static MyBinder myBinder = new MyBinder();

        /// <summary>
        /// 从IDataReader获取数据，填充业务实体。
        /// </summary>
        /// <typeparam name="T">业务实体对象(注：必须是继承自EntityBase的对象)</typeparam>
        /// <param name="reader">IDataReader对象</param>
        /// <returns>实例化并且填充了数据的业务实体对象</returns>
        public static T PopulateData<T>(IDataReader reader) where T : BaseDomain
        {
            //通过反射创建业务实体对象。
            T entity = null; ;

            //填充数据。
            if (reader.Read())
            {
                entity = Activator.CreateInstance<T>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo p = entity.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                    if (null != p && DBNull.Value != reader.GetValue(i))
                    {
                        p.SetValue(entity, reader.GetValue(i), BindingFlags.Public | BindingFlags.Instance, myBinder, null, null);
                    }
                }

                return entity;
            }

            return null;
        }

        /// <summary>
        /// 从IDataReader获取数据，填充业务实体泛型对象。
        /// </summary>
        /// <typeparam name="T">业务实体对象(注：必须是继承自EntityBase的对象)</typeparam>
        /// <param name="reader">IDataReader对象</param>
        /// <returns>实例化并且追加了成员的业务实体泛型对象</returns>
        public static IList<T> PopulateDataList<T>(IDataReader reader) where T : BaseDomain
        {
            //初始化泛型集合对象。
            var list = new List<T>();
            while (reader.Read())
            {
                //通过反射创建业务实体对象。
                T entity = Activator.CreateInstance<T>();

                //填充数据。
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var p = entity.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                    if (null != p && DBNull.Value != reader.GetValue(i))
                        p.SetValue(entity, reader.GetValue(i), BindingFlags.Public | BindingFlags.Instance, myBinder, null, null);
                }

                list.Add(entity);
            }

            return list;
        }


        /// <summary>
        /// 从DataRow获取数据，填充业务实体泛型对象。
        /// </summary>
        /// <typeparam name="T">业务实体对象(注：必须是继承自EntityBase的对象)</typeparam>
        /// <param name="dbRow">DataRow对象</param>
        /// <returns>实例化并且追加了成员的业务实体泛型对象</returns>
        public static T PopulateData<T>(DataRow dbRow) where T : BaseDomain
        {
            //通过反射创建业务实体对象。
            T entity = Activator.CreateInstance<T>();

            //填充数据。
            foreach (DataColumn _column in dbRow.Table.Columns)
            {
                PropertyInfo p = entity.GetType().GetProperty(_column.ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                if (null != p && null != dbRow[_column] && DBNull.Value != dbRow[_column])
                    p.SetValue(entity, dbRow[_column], BindingFlags.Public | BindingFlags.Instance, myBinder, null, null);
            }

            return entity;
        }


        /// <summary>
        /// 从DataTable获取数据，填充业务实体泛型对象。
        /// </summary>
        /// <typeparam name="T">业务实体对象(注：必须是继承自EntityBase的对象)</typeparam>
        /// <param name="dbTable">DataTable对象</param>
        /// <returns>实例化并且追加了成员的业务实体泛型对象</returns>
        public static IList<T> PopulateDataList<T>(DataTable dbTable) where T : BaseDomain
        {
            //初始化泛型集合对象。
            var list = new List<T>();

            foreach (DataRow _row in dbTable.Rows)
                list.Add(PopulateData<T>(_row));

            return list;
        }

        #endregion        

        #region 从基类对象克隆一个子类对象

        /// <summary>
        /// 从基类对象克隆一个子类对象。
        /// </summary>
        /// <typeparam name="T">子类对象类型</typeparam>
        /// <param name="baseOjbect">基类对象</param>
        /// <returns></returns>
        public static T CloneFromBaseOjbect<T>(object baseOjbect) where T : BaseDomain
        {
            var entity = Activator.CreateInstance<T>();
            var t = entity.GetType();

            foreach (var _pi in baseOjbect.GetType().GetProperties())
            {
                var p = t.GetProperty(_pi.Name, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                p.SetValue(entity, _pi.GetValue(baseOjbect, null), BindingFlags.Public | BindingFlags.Instance, myBinder, null, null);
            }

            return entity;
        }

        /// <summary>
        /// 从基类对象克隆一个子类对象。
        /// </summary>
        /// <typeparam name="T">子类对象类型</typeparam>
        /// <param name="baseOjbect">基类对象</param>
        /// <returns></returns>
        public static T CloneOjbect<T>(object baseOjbect)
        {
            var entity = Activator.CreateInstance<T>();
            var t = entity.GetType();

            foreach (var _pi in baseOjbect.GetType().GetProperties())
            {
                var p = t.GetProperty(_pi.Name, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                p.SetValue(entity, _pi.GetValue(baseOjbect, null), BindingFlags.Public | BindingFlags.Instance, myBinder, null, null);
            }

            return entity;
        }


        #endregion

        #region 对字符串进行格式化

        /// <summary>
        /// 对字符串进行格式化(首字大写，其它小写)。
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string FormatChars1(string chars)
        {
            if (string.IsNullOrEmpty(chars) || chars.Trim().Length == 0)
                return chars;

            chars = chars.Trim();
            if (chars.Length < 2)
                return chars.ToUpper();

            chars = chars.Substring(0, 1).ToUpper() + chars.Substring(1);
            return chars;
        }

        #endregion
    }
}
