using System.Configuration;

namespace Common.DBX
{
    /// <summary>
    /// DBFactory 的摘要说明
    /// </summary>
    public class Factory
    {
        public Factory()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private static IDBHelper instance = null;

        public static IDBHelper Default
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateDBHelper();
                }

                return instance;
            }
        }

        //private static IPagerQueryString pagerInstance = null;

        public static IPagerQueryString DefaultPagerQuery
        {
            get
            {
                //if (pagerInstance == null)
                //{
                //    pagerInstance = CreatePagerQueryString();
                //}

                //return pagerInstance;

                return CreatePagerQueryString();
            }
        }

        public static IDBHelper CreateDBHelper()
        {
            return CreateDBHelper("default");
        }

        public static IDBHelper CreateDBHelper(string connectionKey)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connectionKey];

            if (setting.ProviderName.IndexOf("System.Data.SqlClient") >= 0)
            {
                return new SQLServer.DBHelper(setting.ConnectionString);
            }
            //else if (setting.ProviderName.IndexOf("MySql.Data.MySqlClient") >= 0)
            //{
            //    return new MySql.DBHelper(setting.ConnectionString);
            //}

            return null;
        }

        public static IPagerQueryString CreatePagerQueryString()
        {
            return CreatePagerQueryString("default");
        }

        public static IPagerQueryString CreatePagerQueryString(string connectionKey)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connectionKey];

            if (setting.ProviderName.IndexOf("System.Data.SqlClient") >= 0)
            {
                return new SQLServer.PagerQueryString();
            }
            //else if (setting.ProviderName.IndexOf("MySql.Data.MySqlClient") >= 0)
            //{
            //    return new MySql.PagerQueryString();
            //}

            return null;
        }
    }
}