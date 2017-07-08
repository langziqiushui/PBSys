using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace YX.Component
{
    /// <summary>
    /// 自定义Button对象。
    /// </summary>
    public class MyButton : Button
    {
        /// <summary>
        /// 按钮类别。
        /// </summary>
        public enum ButtonTypes
        {
            /// <summary>
            /// 普通。
            /// </summary>
            Normal,
            /// <summary>
            /// 小型通用
            /// </summary>
            MiniNormal,
            /// <summary>
            /// 锁定。
            /// </summary>
            Lock,
            /// <summary>
            /// 保存。
            /// </summary>
            Save,
            /// <summary>
            /// 返回、取消
            /// </summary>
            Cancel,
            /// <summary>
            /// 搜索
            /// </summary>
            Search
        }

        /// <summary>
        /// 获取或设置 按钮类别。
        /// </summary>
        public ButtonTypes ButtonType
        {
            get { return null != ViewState["ButtonType"] ? (ButtonTypes)Enum.Parse(typeof(ButtonTypes), ViewState["ButtonType"].ToString()) : ButtonTypes.Normal; }
            set { ViewState["ButtonType"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CssClass))
            {
                this.CssClass = "btn btn-blue";

                var buttonType = this.ButtonType;
                if (buttonType != ButtonTypes.Normal)
                    this.CssClass += "_" + buttonType.ToString();
            }

            if (this.Width.Value == 0)
                this.Width = 100;
            this.Style.Add("width", this.Width.Value + "px !important");

            base.OnLoad(e);
        }
    }
}
