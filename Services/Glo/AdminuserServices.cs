// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2013-8-12 10:24:44 
// Description：业务服务处理。 
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using YX.Core;
using YX.IServices.Glo;
using YX.Domain;
using YX.Domain.Glo;

namespace YX.Services.Glo
{
    /// <summary> 
    /// 业务服务处理(对应表 Glo.AdminUser(管理员用户表) )。 
    /// </summary> 
    public partial class AdminuserServices : BaseServices, IAdminuserServices
    {
        public AdminuserServices() : base() { }

        #region 变量

        /// <summary> 
        /// 数据访问对象(AdminuserDAL(管理员用户表))。 
        /// </summary> 
        internal static readonly YX.SqlserverDAL.Glo.AdminuserDAL dal = new YX.SqlserverDAL.Glo.AdminuserDAL();

        #endregion

        #region 获取数据

        /// <summary> 
        /// 获取某一管理员用户。 
        /// </summary> 
        /// <param name="username">用户名</param> 
        /// <returns>查询获取的数据</returns> 
        public AdminuserInfo GetData(string username)
        {
            AdminuserInfo r = default(AdminuserInfo);

            ContextDB.DoDBExec(() =>
            {
                r = dal.GetData(username);
            });

            return r;
        }

        /// <summary> 
        /// 分页获取管理员用户。 
        /// </summary> 
        /// <param name="pagesize">单页显示的记录数</param> 
        /// <param name="pageindex">当前页码</param> 
        /// <param name="numrecord">所有符合查询条件的总记录数</param> 
        /// <param name="username">用户名</param> 
        /// <param name="admintype">管理员类别([AdminTypes]普通管理员=1,超级管理员=2)</param> 
        /// <param name="isactived">管理员是否激活</param> 
        /// <returns>查询获取的数据集</returns> 
        public IList<AdminuserInfo> GetData_Paging(int? pagesize, int? pageindex, ref int numrecord, string username, byte? admintype, bool? isactived)
        {
            IList<AdminuserInfo> r = default(IList<AdminuserInfo>);
            int __numrecord = default(int);

            ContextDB.DoDBExec(() =>
            {
                r = dal.GetData_Paging(pagesize, pageindex, ref __numrecord, username, admintype, isactived);
            });

            numrecord = __numrecord;
            return r;
        }

        /// <summary> 
        /// 获取所有管理员用户。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        public IList<AdminuserInfo> GetData_All()
        {
            IList<AdminuserInfo> r = default(IList<AdminuserInfo>);

            ContextDB.DoDBExec(() =>
            {
                r = dal.GetData_All();
            });

            return r;
        }


        /// <summary> 
        /// 用户登陆。 
        /// </summary> 
        /// <param name="userName">用户名</param> 
        /// <param name="password">密码</param> 
        /// <returns>登录是否成功</returns> 
        public AdminuserInfo Login(string userName, string password)
        {
            AdminuserInfo entityAdminuser = null;

            ContextDB.DoDBExec(() =>
            {
                //根据帐户获取用户信息。
                entityAdminuser = dal.GetData(userName);
                if (null == entityAdminuser)
                    AppException.ThrowWaringException("对不起，用户名或密码不正确！");

                //验证密码是否正确。
                //创建字符加、解密管理器。
                var cm = new CryptographyManager((EncryptionFormats)entityAdminuser.EncryptionFormat);
                if (cm.Encrypt(password) != entityAdminuser.Password)
                    AppException.ThrowWaringException("对不起，用户名或密码不正确！");

                //验证用户能否登录。
                if (!entityAdminuser.IsActived)
                    AppException.ThrowWaringException("对不起，此用户已被禁止登录！");
            });

            return entityAdminuser;
        }

        #endregion

        #region 注册管理员

        /// <summary> 
        /// 注册管理员。 
        /// </summary> 
        /// <param name="entity">AdminuserInfo业务实体对象</param> 
        /// <returns>注册管理员是否成功</returns> 
        public bool RegAdmin(AdminuserInfo entity)
        {
            ContextDB.DoDBExec(() =>
            {
                //判断当前操作者是否有注册管理员的权限。
                var entityCurrentAdminUser = dal.GetData(CoreUtil.CurrentUserName);
                if (null == entityCurrentAdminUser || !entityCurrentAdminUser.IsActived || entityCurrentAdminUser.AdminType != (byte)AdminTypes.Super)
                    AppException.ThrowWaringException("对不起，您没有权限注册管理员！");

                //设置用户类别为管理员。
                entity.AdminType = (byte)AdminTypes.Normal;

                //验证用户信息。
                this.ValidateRegInfo(entity);

                //对密码进行加密。
                var cManager = new CryptographyManager(EncryptionFormats.MD5SHA256);
                entity.Password = cManager.Encrypt(entity.Password);
                entity.EncryptionFormat = (byte)cManager.Format;

                var r = Convert.ToInt32(dal.Add(entity));
                if (r == -10)
                    AppException.ThrowWaringException("对不起，管理员用户名“" + entity.UserName + "”已经存在！");
                else if (r == -1)
                    AppException.ThrowWaringException("对不起，注册用户失败！");
            });

            Factory.Glo.AdminuserDataProxy.ClearCache();

            return true;
        }

        /// <summary>
        /// 验证用户信息。
        /// </summary>
        /// <param name="entityAdminuser">用户信息</param>
        private void ValidateRegInfo(AdminuserInfo entityAdminuser)
        {
            //用户名。
            if (entityAdminuser.UserName.Length < 5 || entityAdminuser.UserName.Length > 15)
                AppException.ThrowWaringException("对不起，管理员用户名必须为5～15位字符！");

            //密码。
            if (entityAdminuser.Password.Length < 5 || entityAdminuser.Password.Length > 15)
                AppException.ThrowWaringException("对不起，密码必须为5～15位字符！");
        }

        #endregion

        #region 修改数据

        /// <summary> 
        /// 修改某一管理员用户。 
        /// </summary> 
        /// <param name="entity">AdminuserInfo业务实体对象</param> 
        /// <param name="newPassword">新密码(如果不需要修改密码请为null)</param>
        /// <returns>数据修改是否成功</returns> 
        public object Modify(AdminuserInfo entity, string newPassword)
        {
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                //判断当前操作者是否有权限修改用户资料。
                var entityCurrentAdminUser = dal.GetData(CoreUtil.CurrentUserName);
                if (null == entityCurrentAdminUser || !entityCurrentAdminUser.IsActived || entityCurrentAdminUser.AdminType != (byte)AdminTypes.Super)
                    AppException.ThrowWaringException("对不起，您没有权限修改用户资料！");
                if (entity.AdminType == (byte)AdminTypes.Super && !CoreUtil.CurrentUserName.IgnoreCaseEquals(entity.UserName))
                    AppException.ThrowWaringException("对不起，不能修改其它超级管理员的资料！");

                if (!string.IsNullOrEmpty(newPassword))
                {
                    var cm = new CryptographyManager((EncryptionFormats)entity.EncryptionFormat);
                    entity.Password = cm.Encrypt(newPassword);
                }
                r = dal.Modify(entity);
            });

            Factory.Glo.AdminuserDataProxy.ClearCache();

            return r;
        }


        /// <summary>
        /// 激活或禁用用户。
        /// </summary>
        /// <param name="isActived">是否激活用户</param>
        /// <param name="adminUserNames">管理员用户集合</param>
        /// <returns></returns>
        public bool ActivateOrDisableUser(bool isActived, string[] adminUserNames)
        {
            var r = false;

            ContextDB.DoDBExec(() =>
            {
                //判断当前操作者是否有操作权限。
                var entityCurrentAdminUser = dal.GetData(CoreUtil.CurrentUserName);
                if (null == entityCurrentAdminUser || !entityCurrentAdminUser.IsActived || entityCurrentAdminUser.AdminType != (byte)AdminTypes.Super)
                    AppException.ThrowWaringException("对不起，您没有操作权限！");

                foreach (var _userID in adminUserNames)
                {
                    var entity = dal.GetData(_userID);
                    if (null != entity && entity.IsActived != isActived)
                    {
                        entity.IsActived = isActived;
                        dal.Modify(entity);
                        r = true;
                    }
                }
            });

            Factory.Glo.AdminuserDataProxy.ClearCache();

            return r;
        }

        #endregion

        #region 删除数据

        /// <summary> 
        /// 删除某一管理员用户。 
        /// </summary> 
        /// <param name="username">用户名</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete(string username)
        {
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                var entity = dal.GetData(username);
                if (null != entity)
                {
                    if (entity.AdminType == (byte)AdminTypes.Super)
                        AppException.ThrowWaringException("对不起，超级管理员的帐号不能删除！");

                    r = dal.Delete(username);
                }
            });

            Factory.Glo.AdminuserDataProxy.ClearCache();

            return r;
        }

        #endregion

    }
}
