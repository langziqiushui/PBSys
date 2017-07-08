using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Core;
using YX.Web.Framework;
using YX.Domain;
using YX.Domain.Glo;
using YX.IServices.Glo;
using YX.Factory;



namespace YX.Web.Admin.Glo.UC
{
    public partial class UCPBConfig :  BaseDataEditor
    {
        #region 变量

        /// <summary>
        /// 创建业务服务对象。
        /// </summary>
        private IPbconfigServices busPbconfig = ServicesFactory.CreateGloPbconfigServices();
        /// <summary>
        /// 业务实体。
        /// </summary>
        private PbconfigInfo entityPbconfig = null;

        #endregion


        #region 重载基类方法

        /// <summary>
        /// 控件初始化。
        /// </summary>
        protected override void EditorLoad()
        {
            base.EditorLoad();

            //注册客户端自动验证用户输入。
            this.autoRegValidation = true;
            this.RegValidateAllInput(this.btAccept);

            if (!Page.IsPostBack)
            {
                //绑定列表控件。
                this.BindListControl();
            }

            WFUtil.RegisterStartupScript("pbConfigDialog.init();");
            this.txtApplicationType.Attributes.Add("readonly", "readonly");
        }

        /// <summary> 
        /// 绑定列表控件数据源。 
        /// </summary> 
        protected override void BindListControl()
        {
            #region 分类

            this.drPbType.Items.Add(new ListItem(" - 请选择 - ", string.Empty));
            foreach (string _name in Enum.GetNames(typeof(PbTypes)))
            {
                PbTypes _enItem = (PbTypes)Enum.Parse(typeof(PbTypes), _name);
                this.drPbType.Items.Add(new ListItem(EnumDescription.GetFieldText((PbTypes)_enItem), ((int)_enItem).ToString()));
            }
            this.drPbType.SelectedIndex = 0;

            #endregion
        }

        /// <summary> 
        /// 绑定表单数据。 
        /// </summary> 
        public override void DataBind()
        {
            base.DataBind();

            //从数据库中获取最新数据。 
            this.entityPbconfig = this.busPbconfig.GetData(int.Parse(this.PrimaryKeyValues[0]));

            if (null == this.entityPbconfig)
                throw new AppException("对不起，数据不存在或已经被删除！", ExceptionLevels.Warning);


            this.txtApplicationType.Text = EnumDescription.GetFieldText((ApplicationTypes)this.entityPbconfig.ApplicationType);
            this.hidApplicationType.Value = this.entityPbconfig.ApplicationType.ToString();
            this.drPbType.SelectedIndex = this.drPbType.GetIndex(this.entityPbconfig.PbType.ToString());
            this.txtKeyData.Text = this.entityPbconfig.KeyData.ToString();
        }

        /// <summary> 
        /// 添加数据。 
        /// </summary> 
        protected override bool AddData()
        {
            this.entityPbconfig = new PbconfigInfo();

            //从表单获取数填充数据实体。 
            this.FillData(true);
            //将数据添加到数据库中。 
            var id = 0;
            this.busPbconfig.Add(this.entityPbconfig, ref id);
            //Message.Alert("温馨提示：你已成功创建屏蔽数据！", "操作成功");
            return true;
        }

        /// <summary> 
        /// 更新数据。 
        /// </summary> 
        protected override bool ModifyData()
        {
            //从数据库中获取最新数据。 
            this.entityPbconfig = this.busPbconfig.GetData(int.Parse(this.PrimaryKeyValues[0]));
            if (null == this.entityPbconfig)
                throw new AppException("对不起，数据不存在或已经被删除！", ExceptionLevels.Warning);

            //从表单获取数填充数据实体。 
            this.FillData(false);
            this.busPbconfig.Modify(this.entityPbconfig);

            //Message.Alert("温馨提示：你已成功修改XX资料！", "操作成功");
            return true;
        }

        /// <summary> 
        /// 删除数据。 
        /// </summary> 
        protected override bool DeleteData()
        {
            this.busPbconfig.Delete(int.Parse(this.PrimaryKeyValues[0]));
            //清除缓存。
            //XXDataProxy.ClearCache();
            Message.Alert("温馨提示：你已成功删除屏蔽数据！", "操作成功");
            return true;
        }


        /// <summary>
        /// 提交数据。
        /// </summary>
        /// <returns></returns>
        protected override bool Submit()
        {
            bool r = false;
            WFUtil.UIDoTryCatch("添加或修改屏蔽数据", () =>
            {
                r = base.Submit();
                //清除缓存。
                //XXDataProxy.ClearCache();
            });

            return r;
        }

        /// <summary> 
        /// 从表单获取数据填充实体。 
        /// </summary> 
        /// <param name="create">是否为添加数据模式</param> 
        protected override void FillData(bool create)
        {
            base.FillData(create);

            if (create)
            {
                
            }
           

            this.entityPbconfig.ApplicationType = Convert.ToByte(this.hidApplicationType.Value.Trim());

            if (this.drPbType.SelectedItem.Value.Trim().Length > 0)
                this.entityPbconfig.PbType = Convert.ToByte(this.drPbType.SelectedItem.Value);
            this.entityPbconfig.KeyData = Convert.ToInt32(this.txtKeyData.Text.Trim());
        }

        /// <summary> 
        /// 验证用户输入。 
        /// </summary> 
        /// <returns></returns> 
        protected override bool ValidateInput()
        {
            if (!base.ValidateInput())
                return false;

            return true;
        }

        /// <summary> 
        /// 重置控件状态。 
        /// </summary> 
        protected override void ResetControl()
        {
            base.ResetControl();
        }

        #endregion
    }
}