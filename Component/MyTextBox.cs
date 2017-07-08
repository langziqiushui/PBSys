using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using YX.Core;

namespace YX.Component
{
    /// <summary>
    /// 自定义文本框对象，封装自我逻辑。
    /// </summary>
    public class MyTextBox : TextBox
    {
        /// <summary>
        /// 获取或设置 提示消息。
        /// </summary>
        public string Message
        {
            get
            {
                if (null == ViewState["Message"])
                    return string.Empty;
                return ViewState["Message"].ToString();
            }
            set
            {
                ViewState["Message"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 PlaceHolder提示。
        /// </summary>
        public string PlaceHolder
        {
            get
            {
                if (null == ViewState["PlaceHolder"])
                    return string.Empty;
                return ViewState["PlaceHolder"].ToString();
            }
            set
            {
                ViewState["PlaceHolder"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 是否为内嵌占位(相当于style的display=inline-block)。
        /// </summary>
        public bool IsInlineControl
        {
            get
            {
                if (null == ViewState["IsInlineControl"])
                    return false;
                return Convert.ToBoolean(ViewState["IsInlineControl"]);
            }
            set
            {
                ViewState["IsInlineControl"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(this.CssClass))
                {
                    this.CssClass = "form-control";
                    if (this.IsInlineControl)
                        this.CssClass = "form-control inline-control";
                    this.Attributes.Add("__oc", this.CssClass);
                }

                if (!string.IsNullOrEmpty(this.Message))
                    this.Attributes.Add("Message", this.Message.Replace("\"", "&quot;"));

                if (!string.IsNullOrEmpty(this.PlaceHolder))
                    this.Attributes.Add("placeholder", this.PlaceHolder);

                this.Attributes.Add("onfocus", "common.inputFocus(this);" + this.Attributes["onfocus"] + ";");
                this.Attributes.Add("onblur", "common.inputBlur(this);" + this.Attributes["onblur"] + ";");
                this.Attributes.Add("onclick", "common.inputClick(this);" + this.Attributes["onclick"] + ";");

                //if (this.Width.IsEmpty)
                //    this.Style.Add("width", this.TextMode == TextBoxMode.MultiLine ? "540px" : "250px");
                //if (this.Height.IsEmpty && this.TextMode == TextBoxMode.MultiLine)
                //    this.Style.Add("height", "60px");
            }
        }
    }
}
