using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JK.TiKu.Core;
using JK.TiKu.Domain.Member;

namespace JK.TiKu.Factory
{
    /// <summary>
    /// 用户管理 当且仅当使用在M站 和PC 站
    /// </summary>
    public class MUserManager
    {
        #region 变量

        /// <summary>
        /// 当前分销用户缓存键。
        /// </summary>
        private const string CacheKey_User = "CacheKey-CurrentMWebUser-898989-{0}";
        /// <summary>
        /// 用户服务对象。
        /// </summary>
        private static IServices.Member.IUsersServices server = Factory.ServicesFactory.CreateMemberTiKuUsersServices();

        #endregion

        #region 属性

        /// <summary>
        /// 获取 用户是否登录。
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return UserManager.IsAuthenticated;
            }
        }

        /// <summary>
        /// 获取 当前登录用户名。
        /// </summary>
        public static string LoginUser
        {
            get { return UserManager.LoginUser; }
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        public static UsersInfo CurrentUser
        {
            get
            {
                return UserManager.Current;
            }
        }

        /// <summary>
        /// 获取用户类别。
        /// </summary>
        public static Domain.MemberUserTypes UserType
        {
            get
            {
                return (Domain.MemberUserTypes)CurrentUser.UserType;
            }
        }

        #endregion
    }
}
