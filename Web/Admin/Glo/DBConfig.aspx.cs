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
    /// 系统配置参数管理。
    /// </summary>
    public partial class DBConfig : BaseSystemConfig
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                //绑定列表控件。
                this.BindListControl();
                //自动载入第一个分组的枚举项。
                this.drGroup_SelectedIndexChanged(this, EventArgs.Empty);
            }

            this.RegValidateAllInput(this.btAccept, "return confirm('温馨提示：您将要保存配置参数，是否确定？');");

            this.RefStyle(false, "myTable.css");
            this.txtValue.Style.Add("over-x", "auto");
            this.txtValue.Attributes.Add("wrap", "off");
            this.drGroup.SelectedIndexChanged += new EventHandler(drGroup_SelectedIndexChanged);
            this.btAccept.Click += new EventHandler(btAccept_Click);
            this.radDBConfigKey.SelectedIndexChanged += new EventHandler(radDBConfigKey_SelectedIndexChanged);
        }

        #region 绑定列表控件

        /// <summary>
        /// 绑定列表控件。
        /// </summary>
        private void BindListControl()
        {
            //获取所有配置枚举项的前缀。
            var groups = new List<string>();
            foreach (var _name in Enum.GetNames(typeof(DBConfigKeys)))
            {
                var prefix = _name;
                var index = _name.IndexOf("_");
                if (index >= 0)
                    prefix = _name.Substring(0, index);

                if (!groups.Contains(prefix))
                    groups.Add(prefix);
            }
            foreach (var _group in groups)
                this.drGroup.Items.Add(new ListItem(_group, _group));

            //从查询参数默认选择某个分组。
            var cGroup = Request.QueryString["group"];
            if (!string.IsNullOrEmpty(cGroup))
                this.drGroup.SelectedIndex = this.drGroup.GetIndex(cGroup);
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 当配置项选择时。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void radDBConfigKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            WFUtil.UIDoTryCatch("获取DB配置参数", () =>
            {
                if (!Enum.GetNames(typeof(DBConfigKeys)).Contains(this.radDBConfigKey.SelectedItem.Value))
                    return;

                //获取枚举值。
                var em = (DBConfigKeys)Enum.Parse(typeof(DBConfigKeys), this.radDBConfigKey.SelectedItem.Value);

                //获取配置值。
                this.txtValue.Text = Factory.DBConfigFactory.GetConfig(em);

                //获取枚举详细描述。 
                var desc = EnumDescription.GetFieldColorOrDescription(em);
                this.divDescriptionContainer.Visible = !string.IsNullOrEmpty(desc);
                this.divDescription.InnerHtml = CoreUtil.ConvertWinTagToWeb(desc);

                WFUtil.RegisterStartupScript("parent.resetMainframeHeight();");
            });
        }

        /// <summary>
        /// 根据分组载入配置项。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void drGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            WFUtil.UIDoTryCatch("载入DB配置参数", () =>
            {
                this.divDescription.InnerHtml = null;
                this.txtValue.Text = null;
                this.radDBConfigKey.Items.Clear();
                this.divDescriptionContainer.Visible = false;

                if (this.drGroup.SelectedIndex < 0)
                    return;

                var cGroup = this.drGroup.SelectedItem.Value;
                this.btAccept.Visible = true;
                this.divValueContainer.Visible = true;
                foreach (var _name in Enum.GetNames(typeof(DBConfigKeys)))
                {
                    var prefix = _name;
                    var index = _name.IndexOf("_");
                    if (index >= 0)
                        prefix = _name.Substring(0, index);

                    if (prefix == cGroup)
                    {
                        var _em = (DBConfigKeys)Enum.Parse(typeof(DBConfigKeys), _name);
                        var caption = _name;
                        var desc = EnumDescription.GetFieldText(_em);
                        if (!string.IsNullOrEmpty(desc))
                            caption += "(<font style='color:gray;'>" + desc + "</font>)";
                        this.radDBConfigKey.Items.Add(new ListItem(caption, _name));
                    }
                }

                WFUtil.RegisterStartupScript("parent.resetMainframeHeight();");
            });
        }

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
                var em = (DBConfigKeys)Enum.Parse(typeof(DBConfigKeys), this.radDBConfigKey.SelectedItem.Value);
                Factory.DBConfigFactory.SetConfig(
                    em,
                    this.txtValue.Text.Trim()
                );

                Message.Info("温馨提示：您已成功修改配置“" + EnumDescription.GetFieldText(em) + "”值！", "操作成功");
            });
        }

        #endregion
    }
}