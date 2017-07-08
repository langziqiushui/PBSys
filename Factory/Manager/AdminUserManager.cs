using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;
using YX.Domain;
using YX.Domain.Glo;

namespace YX.Factory
{
    /// <summary>
    /// 用户管理器 。
    /// </summary>
    public class AdminUserManager
    {
        #region 变量

        /// <summary>
        /// 当前后台用户缓存键。
        /// </summary>
        private const string CacheKey_User = "CacheKey-CurrentAdminuser-28356-{0}";
        /// <summary>
        /// 用户服务对象。
        /// </summary>
        private static IServices.Glo.IAdminuserServices server = Factory.ServicesFactory.CreateGloAdminuserServices();

        #endregion

        #region 属性

        /// <summary>
        /// 获取 用户是否登录。
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                //从登录票据中获取。
                if (null == System.Web.HttpContext.Current)
                    return false;
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    return true;

                //从Sesion中获取。
                return null != System.Web.HttpContext.Current.Session[Factory.GlobalKeys.SESSION_LOGIN];
            }
        }

        /// <summary>
        /// 获取 当前登录用户名。
        /// </summary>
        public static string LoginUser
        {
            get
            {
                if (null == System.Web.HttpContext.Current)
                    return null;

                //从登录票据中获取。
                var loginUser = CoreUtil.CurrentUserName;
                if (!string.IsNullOrEmpty(loginUser))
                    return loginUser;

                //从Sesion中获取。
                var loginUserObj = System.Web.HttpContext.Current.Session[Factory.GlobalKeys.SESSION_LOGIN];
                return null != loginUserObj ? loginUserObj.ToString() : null;
            }
        }

        /// <summary>
        /// 获取用户类别。
        /// </summary>
        public static Domain.AdminTypes UserType
        {
            get { return (Domain.AdminTypes)Current.AdminType; }
        }

        /// <summary>
        /// 获取　是否为账号超级管理员。
        /// </summary>
        public static bool IsMainSystemAdmin
        {
            get
            {
                return
                    UserType == Domain.AdminTypes.Super;
                ;
            }
        }

        

       
        /// <summary>
        /// 获取　是否为普通会员。
        /// </summary>
        public static bool IsNormalUser
        {
            get
            {
                var ut = UserType;
                return
                    ut == Domain.AdminTypes.Normal;
                ;
            }
        }


        /// <summary>
        /// 获取 当前登录者用户信息。
        /// </summary>
        public static AdminuserInfo  Current
        {
            get
            {
                //缓存键。
                var loginName = LoginUser;
                if (string.IsNullOrEmpty(loginName))
                    AppException.ThrowWaringException("用户未登录！");

                //从缓存中获取。
                var cacheKey = string.Format(CacheKey_User, loginName.ToLower());
                var entityUser = DataCacheFactory.DataCacher[cacheKey] as AdminuserInfo;
                if (null == entityUser)
                {
                    entityUser = server.GetData(loginName);
                    if (null == entityUser)
                        AppException.ThrowWaringException("不存在登录名为“" + loginName + "”的用户！");

                    //保存到缓存中。
                    DataCacheFactory.DataCacher.Insert(cacheKey, entityUser, DateTime.Now.AddMinutes(3));
                }

                return entityUser;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断会员是否被禁用。
        /// </summary>
        /// <param name="entityUser">会员信息</param>
        /// <returns></returns>
        public static bool IsForbidden(AdminuserInfo entityUser)
        {
            return entityUser.IsActived ;
        }

        #endregion
    }
}
