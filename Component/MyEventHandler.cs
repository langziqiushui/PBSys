using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace YX.Component
{
    #region 自定义委托

    /// <summary>
    /// 当按钮触发命令时。
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <param name="commandArgument">命令参数</param>
    public delegate bool CommandEventHandler(string commandName, params string[] commandArgument);
    /// <summary>
    /// 当为某行创建编辑按钮时。
    /// </summary>
    /// <param name="controls">要加入的控件集合</param>
    public delegate void CreateControlEventHandler(IList<Control> controls);

    /// <summary>
    /// 当为某行创建编辑按钮时。
    /// </summary>
    /// <param name="controls">要加入的控件集合</param>
    /// <param name="originalData">原始数据</param>
    public delegate void CreateRowControlEventHandler(IList<Control> controls, object originalData);

    #endregion

    #region 命令参数

    /// <summary>
    /// 命令名称。
    /// </summary>
    public struct CommandNames
    {
        /// <summary>
        /// 修改。
        /// </summary>
        public const string Modify = "Modify";
        /// <summary>
        /// 删除。
        /// </summary>
        public const string Delete = "Delete";
        /// <summary>
        /// 删除所有处于选择状态的数据。
        /// </summary>
        public const string DeleteChecked = "DeleteChecked";
        /// <summary>
        /// 删除所有数据。
        /// </summary>
        public const string DeleteAll = "DeleteAll";
        /// <summary>
        /// 申请删除数据。
        /// </summary>
        public const string ApplyDelete = "ApplyDelete";
        /// <summary>
        /// 创建新对象。
        /// </summary>
        public const string CreateNew = "CreateNew";
        /// <summary>
        /// 分页。
        /// </summary>
        public const string Pager = "Pager";
        /// <summary>
        /// 搜索。
        /// </summary>
        public const string Search = "Search";
        /// <summary>
        /// 复制。
        /// </summary>
        public const string Copy = "Copy";
        /// <summary>
        /// 刷新。
        /// </summary>
        public const string Refur = "Refur";
        /// <summary>
        /// 预览。
        /// </summary>
        public const string View = "View";
        /// <summary>
        /// 复制选择的。
        /// </summary>
        public const string CopySelected = "CopySelected";
        /// <summary>
        /// 取消。
        /// </summary>
        public const string Cancel = "Cancel";
        /// <summary>
        /// 重新发布。
        /// </summary>
        public const string RePost = "RePost";
        /// <summary>
        /// 收藏。
        /// </summary>
        public const string Collect = "Collect";
        /// <summary>
        /// 拒绝。
        /// </summary>
        public const string Reject = "Reject";
        /// <summary>
        /// 邀请。
        /// </summary>
        public const string Invite = "Invite";
        /// <summary>
        /// 撤销。
        /// </summary>
        public const string Undo = "Undo";
        /// <summary>
        /// 载入所有数据。
        /// </summary>
        public const string LoadAllData = "LoadAllData";
        /// <summary>
        /// 接受。
        /// </summary>
        public const string Accept = "Accept";
        /// <summary>
        /// 已读。
        /// </summary>
        public const string Read = "Read";
        /// <summary>
        /// 锁定。
        /// </summary>
        public const string Lock = "Lock";
        /// <summary>
        /// 解锁。
        /// </summary>
        public const string UnLock = "UnLock";
        /// <summary>
        /// 代理用户。
        /// </summary>
        public const string RobotTrue = "RobotTrue";
        /// <summary>
        /// 真实用户。
        /// </summary>
        public const string RobotFalse = "RobotFalse";
        /// <summary>
        /// 禁止不通过。
        /// </summary>
        public const string Forbid = "Forbid";
        /// <summary>
        /// 通过审核。
        /// </summary>
        public const string Pass = "Pass";
        /// <summary>
        /// 全部通过审核。
        /// </summary>
        public const string AllPass = "AllPass";
        /// <summary>
        /// 发送激活邮件。
        /// </summary>
        public const string SendActivationEmail = "SendActivationEmail";
        /// <summary>
        /// 重置状态。
        /// </summary>
        public const string Reset = "Reset";
        /// <summary>
        /// 推荐。
        /// </summary>
        public const string Top = "Top";
        /// <summary>
        /// 获取密码。
        /// </summary>
        public const string GetPassword = "GetPassword";
        /// <summary>
        /// 短消息。
        /// </summary>
        public const string Message = "Message";
        /// <summary>
        /// 备注提示。
        /// </summary>
        public const string Comment = "Comment";
    }

    #endregion
}
