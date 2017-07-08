using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;

namespace YX.Web.Framework
{
    /// <summary>
    /// 用户控件基类。
    /// </summary>
    public class BaseUserControl : UserControl
    {
        /// <summary>
        /// 是否自动注册验证信息(默认为false)。
        /// </summary>
        protected bool autoRegValidation = false;
        /// <summary>
        /// 根据验证控件自动生成客户端表单验证脚本。
        /// </summary>
        private FormValidation formValidation = new FormValidation();

        protected override void OnInit(EventArgs e)
        {
            this.EditorInit();
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            this.EditorLoad();
        }

        /// <summary>
        /// 编辑器初始化。
        /// </summary>
        protected virtual void EditorInit()
        {

        }
        /// <summary>
        /// 编辑器初始化。
        /// </summary>
        protected virtual void EditorLoad()
        {

        }


        protected override void OnPreRender(EventArgs e)
        {
            //获取所有验证信息并注册脚本。
            if (this.autoRegValidation)
                this.formValidation.RegValidation(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="target">要注册的目标按钮</param>
        protected void RegValidateAllInput(Control target)
        {
            this.RegValidateAllInput(new Control[] { target });
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="target">要注册的目标按钮</param>
        /// <param name="validationSuccessScript">当验证成功后要执行的脚本</param>
        protected void RegValidateAllInput(Control target, string validationSuccessScript)
        {
            this.formValidation.RegValidateAllInput(new Control[] { target }, validationSuccessScript);
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="buttons">要注册的目标按钮</param>
        protected virtual void RegValidateAllInput(Control[] buttons)
        {
            this.formValidation.RegValidateAllInput(buttons);
        }
    }
}
