using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Web.Framework;
using YX.Core;
using YX.Factory;
using YX.Component;
using YX.Domain;
using System.Text;

namespace YX.Web.Admin.Glo
{
    /// <summary>
    /// 简单系统配置参数管理。
    /// </summary>
    public partial class SimpleDBConfig : BaseSystemConfig
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["key"]))
                AppException.ThrowWaringException("非法访问！");
            this.configKey = (DBConfigKeys)Enum.Parse(typeof(DBConfigKeys), Request["key"], true);            

            if (!Page.IsPostBack)
            {
                
               //载入数据。
                this.LoadData();
            }

            this.RegValidateAllInput(this.btAccept, "return confirm('温馨提示：您将要保存配置参数，是否确定？');");

            this.RefStyle(false, "myTable.css");
            this.txtValue.Style.Add("over-x", "auto");
            this.txtValue.Attributes.Add("wrap", "off");
            this.btAccept.Click += new EventHandler(btAccept_Click);
        }

        /// <summary>
        /// 配置参数值。
        /// </summary>
        private DBConfigKeys? configKey = null;

        /// <summary>
        /// 载入数据。
        /// </summary>
        private void LoadData()
        {
            this.divDescription.InnerHtml = EnumDescription.GetFieldColorOrDescription(this.configKey.Value);
            this.txtValue.Text = DBConfigFactory.GetConfig(this.configKey.Value, "");
        }

        #region 事件处理

        /// <summary>
        /// 保存配置项。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btAccept_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            WFUtil.UIDoTryCatch("保存DB配置参数", () =>
            {             
                Factory.DBConfigFactory.SetConfig(
                    this.configKey.Value,
                    this.txtValue.Text.Trim()
                );

                Message.Info("温馨提示：您已成功修改配置！", "操作成功");
            });
        }

        #endregion
    }
}