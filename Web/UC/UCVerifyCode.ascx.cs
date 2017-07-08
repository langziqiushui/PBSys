using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Web.Framework;
using YX.Domain;

namespace YX.Web.UC
{
    /// <summary>
    /// 验证码用户控件。
    /// </summary>
    public partial class UCVerifyCode : BaseUserControl
    {       
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.txtVerifyCode.Attributes.Add("verifyCode", "true");
            this.txtVerifyCode.Attributes.Add("_showError", "false");
            this.txtVerifyCode.Attributes.Add("pc", this.ClientID);

            this.ReloadImage(false);            
        }

        #region 静态值

        
        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置 验证码类别。
        /// </summary>
        public VerifyCodeTypes VerifyCodeType
        {
            get { if (null == ViewState["VerifyCodeType"]) { return VerifyCodeTypes.AdminLogin; } else { return (VerifyCodeTypes)Enum.Parse(typeof(VerifyCodeTypes), ViewState["VerifyCodeType"].ToString()); } }
            set { ViewState["VerifyCodeType"] = value; }
        }

        /// <summary>
        /// 获取 验证码。
        /// </summary>
        public string VerifyCode
        {
            get { return this.txtVerifyCode.Text.Trim(); }
        }

        /// <summary>
        /// 获取或设置 是否显示验证码输入文本框。
        /// </summary>
        public bool IsShowInput
        {
            get { return this.phInput.Visible; }
            set { this.phInput.Visible = value; }
        }

        /// <summary>
        /// 验证码图片高、宽比。
        /// </summary>
        private const decimal whScale = (decimal)0.40769230769230769230769230769231;

        /// <summary>
        /// 获取或设置 验证码图片高度。
        /// </summary>
        public int VImageHeight
        {
            get 
            {
                return Convert.ToInt32(this.imgVerifyCode.Height);
            }
            set 
            {
                this.imgVerifyCode.Height = Unit.Parse(value.ToString());
                int w = (int)((decimal)value / whScale);
                this.imgVerifyCode.Width = Unit.Parse(w.ToString());
            }
        }

        #endregion

        #region 载入验证码

        /// <summary>
        /// 载入验证码。
        /// </summary>
        public void ReloadImage()
        {
            this.ReloadImage(true);
        }

        /// <summary>
        /// 载入验证码。
        /// </summary>
        /// <param name="coercive">是否强制生成</param>
        private void ReloadImage(bool coercive)
        {
            if (!this.Visible)
                return;

            string verifyCodeURLFormat = Page.ResolveUrl("~/API/MyHandler.ashx?method=verifyCode&verType=" + this.VerifyCodeType.ToString() + "&r={0}");
            this.imgVerifyCode.Attributes.Add("onclick", "verifyCode.loadImage();");
            this.imgVerifyCode.Attributes.Add("url", verifyCodeURLFormat);

            if (!Page.IsPostBack || coercive)
            {
                this.imgVerifyCode.ImageUrl = string.Format(verifyCodeURLFormat, DateTime.Now.Ticks.ToString());
                this.txtVerifyCode.Text = string.Empty;
            }
        }

        #endregion
    }
}