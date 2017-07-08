using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YX.Web.UC
{
    /// <summary>
    /// 从UMeditor封装一个服务器用户控件。
    /// </summary>
    /// <remarks>
    ///     官网地址：http://ueditor.baidu.com/website/umeditor.html
    ///     如何使用：
    ///         引用CSS：
    ///         /JScripts/UMeditor/1_2_2/themes/default/css/umeditor.css
    ///         引用JS：
    ///         /JScripts/UMeditor/1_2_2/umeditor.config.js
    ///         /JScripts/UMeditor/myEditor.js
    ///         父页面在Page_Load中调用：
    ///         var arr = new string[] { this.UCSRichTextBox1.ClientID, this.UCSRichTextBox2.ClientID, this.UCSRichTextBox3.ClientID};
    ///         WFUtil.RegisterStartupScript("umEditor.init('" + arr.ToJoinString("|") + "');");
    /// </remarks>
    public partial class UCSRichTextBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.hidWidthHeight.Value = this.Width + "|" + this.Height.ToString();
        }

        /// <summary>
        /// 获取或设置 宽度。
        /// </summary>
        public int Width
        {
            get
            {
                if (null == ViewState["Width"])
                    return 660;

                return Convert.ToInt32(ViewState["Width"]);
            }
            set
            {
                ViewState["Width"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 高度。
        /// </summary>
        public int Height
        {
            get
            {
                if (null == ViewState["Height"])
                    return 100;

                return Convert.ToInt32(ViewState["Height"]);
            }
            set
            {
                ViewState["Height"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 html源代码。
        /// </summary>
        public string Html
        {
            get
            {
                return this.hidMyEditor.Value;
            }
            set
            {
                this.hidMyEditor.Value = value;
            }
        }
    }
}