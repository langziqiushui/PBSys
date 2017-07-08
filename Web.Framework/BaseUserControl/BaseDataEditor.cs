using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;

using YX.Core;

namespace YX.Web.Framework
{
    /// <summary>
    /// 表单数据编辑器基类。
    /// </summary>
    public abstract class BaseDataEditor : BaseUserControl
    {
        #region 初始操作

        protected override void OnInit(EventArgs e)
        {
            this._btAccept = (Button)this.FindControl("btAccept");
            this._btCancel = this.FindControl("btCancel") as Button;

            if (null != this._btCancel)
            {
                this._btCancel.Text = "取消";
                this._btCancel.CausesValidation = false;
                this._btCancel.Click += new EventHandler(_btCancel_Click);
            }

            this._btAccept.Click += new EventHandler(_btAccept_Click);

            if (!Page.IsPostBack)
            {
                this.Visible = false;
                this._btAccept.Text = "添加";
            }

            WFUtil.RegisterStartupScript("MyDataEditorClientID = '" + this.ClientID + "';");

            base.OnInit(e);
        }

        #endregion

        #region 变量

        /// <summary>
        /// 提交按钮。
        /// </summary>
        protected Button _btAccept = null;
        /// <summary>
        /// 取消按钮。
        /// </summary>
        protected Button _btCancel = null;
        /// <summary>
        /// 是否在进入编辑模式时自动验证。
        /// </summary>
        protected bool needValidateAllInput = true;

        #endregion

        #region 自定义事件

        /// <summary>
        /// 当提交数据时发出的通知事件委托。
        /// </summary>
        public event Action<BaseDataEditor, WorkModes> OnSubmit = null;          

        /// <summary>
        /// 当需要验证权限时。
        /// </summary>
        public event Action<BaseDomain, object> Permission;

        #endregion

        #region 属性

        /// <summary>
        /// 获取 当前工作模式并触发相关操作。
        /// </summary>
        public WorkModes WorkMode
        {
            get
            {
                if (null == ViewState["WorkMode"])
                    return WorkModes.NONE;

                return (WorkModes)ViewState["WorkMode"];
            }
            private set
            {
                ViewState["WorkMode"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 主键数据。
        /// </summary>
        public IList<string> PrimaryKeyValues
        {
            get
            {
                if (null == ViewState["PrimaryKeyValues"])
                    return null;

                return (IList<string>)ViewState["PrimaryKeyValues"];
            }
            set
            {
                ViewState["PrimaryKeyValues"] = value;
            }
        }

        /// <summary>
        /// 获取 是否一直显示为编辑模式。
        /// </summary>
        public bool AlwaysModify
        {
            get
            {
                if (null == ViewState["AlwaysModify"])
                    return false;

                return Convert.ToBoolean(ViewState["AlwaysModify"]);
            }
            private set
            {
                ViewState["AlwaysModify"] = value;
            }
        }

        /// <summary>
        /// 获取 是否一直显示为创建模式。
        /// </summary>
        public bool AlwaysCreate
        {
            get
            {
                if (null == ViewState["AlwaysCreate"])
                    return false;

                return Convert.ToBoolean(ViewState["AlwaysCreate"]);
            }
            private set
            {
                ViewState["AlwaysCreate"] = value;
            }
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 当准备创建数据时。
        /// </summary>
        public virtual void OnBeginCreate()
        {
            this.SetWorkMode(WorkModes.Create, null);
        }

        /// <summary>
        /// 当准备更新数据时。
        /// </summary>
        /// <param name="_primaryKeyValue">准备更新的主键值</param>
        public virtual void OnBeginModify(string _primaryKeyValue)
        {
            this.SetWorkMode(WorkModes.Modify, new string[] { _primaryKeyValue });
        }

        /// <summary>
        /// 当删除数据时。
        /// </summary>
        /// <param name="_primaryKeyValues">准备删除数据的主键值集合</param>
        public virtual void OnDelete(IList<string> _primaryKeyValues)
        {
            this.SetWorkMode(WorkModes.Delete, _primaryKeyValues);
        }

        /// <summary>
        /// 设置当前模式为总是显示编辑模式。
        /// </summary>
        /// <param name="_primaryKeyValue">准备更新的主键值</param>
        public virtual void SetAlwaysModify(string _primaryKeyValue)
        {
            this.AlwaysModify = true;
            this.AlwaysCreate = false;
            this.needValidateAllInput = false;
            if (null != this._btCancel)
                this._btCancel.Visible = false;
            this.SetWorkMode(WorkModes.Modify, new string[] { _primaryKeyValue });
        }

        /// <summary>
        /// 设置当前模式为总是显示创建模式。
        /// </summary>
        public virtual void SetAlwaysCreate()
        {
            this.AlwaysCreate = true;
            this.AlwaysModify = false;
            this.SetWorkMode(WorkModes.Create, null);
        }

        #endregion

        #region 重写基类方法

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            /*
            //如果进入编辑状态，在页面载入时验证用户输入。
            if (this.needValidateAllInput && this.WorkMode == WorkModes.Modify)
                WFUtil.RegisterStartupScript("common.onReady(common.validateAllInput());");
            */
        }

        /// <summary>
        /// 编辑器初始化。
        /// </summary>
        protected override void EditorInit()
        {
            base.EditorInit();
        }

        /// <summary>
        /// 绑定数据。
        /// </summary>
        public override void DataBind()
        {
            this._btAccept.Text = "修改";
            if (null != this._btCancel)
                this._btCancel.Text = "取消";
            this.Visible = true;
        }

        #endregion

        #region 继承类需要重写的方法

        /// <summary>
        /// 绑定列表控件。
        /// </summary>
        protected virtual void BindListControl() { }

        /// <summary>
        /// 验证用户输入。
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateInput()
        {
            if (!this.Page.IsValid)
                return false;

            return true;
        }

        /// <summary>
        /// 添加数据。
        /// </summary>
        protected virtual bool AddData() { return false; }

        /// <summary>
        /// 修改数据。
        /// </summary>
        protected virtual bool ModifyData() { return false; }

        /// <summary>
        /// 删除数据。
        /// </summary>
        /// <returns></returns>
        protected virtual bool DeleteData() { return false; }

        /// <summary> 
        /// 从表单获取数填充数据实体。 
        /// </summary> 
        /// <param name="create">是否为添加数据模式</param>
        protected virtual void FillData(bool create) { }

        /// <summary>
        /// 重置控件状态。
        /// </summary>
        protected virtual void ResetControl()
        {
            this.PrimaryKeyValues = null;
            this._btAccept.Text = "添加";
            if (null != this._btCancel)
                this._btCancel.Text = "取消";
            this.Visible = this.WorkMode != WorkModes.NONE;
        }

        /// <summary>
        /// 关闭高亮蒙版。
        /// </summary>
        protected virtual void CloseOverlay()
        {
            //注册脚本(关闭编辑器高亮蒙版及所有弹窗)。
            ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "try{common.hideAllPopupPanel();sg.closeEditor();sg.closeOverlay();}catch(e){}", true);
        }

        /// <summary>
        /// 其它提交。
        /// </summary>
        /// <param name="wm">工作模式</param>
        protected virtual void OtherSubmit(WorkModes wm)
        {
            if (null != this.OnSubmit)
                this.OnSubmit(this, wm);
        }

        /// <summary>
        /// 提交数据。
        /// </summary>
        protected virtual bool Submit()
        {
            //验证用户输入。
            if (!this.ValidateInput())
                return false;

            switch (this.WorkMode)
            {
                case WorkModes.Create:
                    return this.AddData();
                case WorkModes.Modify:
                    return this.ModifyData();
            }

            this.ResetControl();
            return true;
        }

        /// <summary>
        /// 当需要验证权限时。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="obj"></param>
        protected void OnPermission(BaseDomain entity, object obj)
        {
            if (null != this.Permission)
                this.Permission(entity, obj);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 设置工作模式。
        /// </summary>
        /// <param name="_workMode">工作模式</param>
        /// <param name="_primaryKeyValues">主键值集合</param>
        private void SetWorkMode(WorkModes _workMode, IList<string> _primaryKeyValues)
        {
            switch (_workMode)
            {
                case WorkModes.Create:
                    this.PrimaryKeyValues = null;
                    this.WorkMode = _workMode;
                    this.ResetControl();
                    break;
                case WorkModes.Modify:
                    if (_primaryKeyValues.Count == 0)
                        throw new AppException("对不起，未接收到任何主键数据。", ExceptionLevels.Warning);

                    this.WorkMode = _workMode;
                    this.ResetControl();
                    this.PrimaryKeyValues = _primaryKeyValues;
                    this.DataBind();
                    break;
                case WorkModes.Delete:
                    this.PrimaryKeyValues = _primaryKeyValues;
                    this.WorkMode = _workMode;
                    if (this.DeleteData())
                    {
                        if (this.AlwaysCreate)
                            this.SetWorkMode(WorkModes.Create, null);
                        else
                            this.SetWorkMode(WorkModes.NONE, null);

                        if (null != this.OnSubmit)
                            this.OnSubmit(this, WorkModes.Delete);
                    }
                    break;
                case WorkModes.NONE:
                    this.PrimaryKeyValues = null;
                    this.WorkMode = _workMode;
                    this.ResetControl();
                    break;
            }
        }

        #endregion

        #region 事件处理

        protected void _btCancel_Click(object sender, EventArgs e)
        {
            if (this.AlwaysCreate)
                this.SetWorkMode(WorkModes.Create, null);
            else
                this.SetWorkMode(WorkModes.NONE, null);

            //发出事件通知主程序。
            if (null != this.OnSubmit)
                this.OnSubmit(this, WorkModes.Cancel);

            //注册脚本(关闭编辑器高亮蒙版及所有弹窗)。
            this.CloseOverlay();
        }

        protected void _btAccept_Click(object sender, EventArgs e)
        {
            if (this.Submit())
            {
                //发出事件通知主程序。
                if (null != this.OnSubmit)
                    this.OnSubmit(this, this.WorkMode);
                //设置工作模式为NONE。
                if (this.AlwaysCreate)
                    this.SetWorkMode(WorkModes.Create, null);
                else if (!this.AlwaysModify)
                    this.SetWorkMode(WorkModes.NONE, null);

                //关闭编辑器高亮蒙版。
                if (!(this.AlwaysModify || this.AlwaysCreate))
                    this.CloseOverlay();
            }
            //else
            //{
            //    //如果当前页面未使用ScriptManager或者ScriptManager禁用了局部更新功能，则要求在页面post后重新打开模态窗口。
            //    var sm = ((BaseWeb)this.Page).CurrentScriptManager;
            //    if (null == sm || !sm.EnablePartialRendering)
            //        WFUtil.RegisterStartupScript("___needOpenOverlay951753 = true;");
            //}
        }

        #endregion
    }
}
