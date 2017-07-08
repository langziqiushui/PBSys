using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

using YX.Core;
using YX.Factory;

namespace YX.Web.Framework
{
    /// <summary>
    /// 根据验证控件自动生成客户端表单验证脚本。
    /// </summary>
    public class FormValidation
    {
        #region 变量

        /// <summary>
        /// 验证信息集合。
        /// </summary>
        private List<ValidationInfo> validations = new List<ValidationInfo>();
        /// <summary>
        /// 所有可见并且需要验证数据提交的按钮编号。
        /// </summary>
        private string buttonIDs = string.Empty;

        #endregion

        #region 公用方法

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="buttons">要注册的目标按钮</param>
        public virtual void RegValidateAllInput(Control[] buttons)
        {
            this.RegValidateAllInput(buttons, null);
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="buttons">要注册的目标按钮</param>
        /// <param name="validationSuccessScript">当验证成功后要执行的脚本</param>
        public virtual void RegValidateAllInput(Control[] buttons, string validationSuccessScript)
        {
            string script = "if(!common.validateAllInput()){return false;}else{" + validationSuccessScript + "}";

            foreach (Control _control in buttons)
            {
                if (_control is IButtonControl)
                {
                    IButtonControl button = _control as IButtonControl;
                    if (button.CausesValidation)
                    {
                        WebControl button2 = button as WebControl;
                        button2.Attributes.Add("onclick", script + button2.Attributes["onclick"] + ";");
                    }
                }
                else if (_control is HtmlInputButton)
                {
                    HtmlInputButton button = _control as HtmlInputButton;
                    if (button.CausesValidation)
                        button.Attributes.Add("onclick", script + button.Attributes["onclick"] + ";");
                }
            }
        }

        /// <summary>
        /// 获取所有验证信息并注册脚本。
        /// </summary>
        public void RegValidation(Control parent)
        {
            //从缓存中获取脚本。
            string scripts = string.Empty;
            string cacheKey = "RegValidation_" + HttpContext.Current.Request.PhysicalPath.ToLower();
            object _scripts = DataCacheFactory.DataCacher[cacheKey];
            if (null != _scripts)
            {
                scripts = _scripts.ToString();
            }
            else
            {
                //获取相关控件。
                this.FindValidationControls(parent);
                //创建脚本。
                if (this.validations.Count > 0)
                {
                    scripts = "\nvar _____vad_parms = new Array();";
                    for (int i = 0; i < this.validations.Count; i++)
                        scripts += "\nvar _____vad_parm" + i.ToString() + " = {" + this.validations[i].ToJSON() + "};_____vad_parms.push(_____vad_parm" + i.ToString() + ");";
                }
                //加入缓存中。
                DataCacheFactory.DataCacher.Insert(cacheKey, scripts);
            }

            //注册脚本。
            ScriptManager.RegisterStartupScript(parent, parent.GetType(), "RegValidation", scripts, true);
        }

        #endregion

        #region 其它方法

        /// <summary>
        /// 获取或创建验证信息对象。
        /// </summary>
        /// <param name="controlID">控件ID</param>
        /// <returns></returns>
        private ValidationInfo GetValidationInfo(string controlID, Control parent)
        {
            WebControl c = parent.FindControl(controlID) as WebControl;
            if (null == c)
                AppException.ThrowWaringException("对不起，未找到验证控件ControlToValidate对应的控件“" + controlID + "”！");

            foreach (ValidationInfo _entity in this.validations)
            {
                if (_entity.InputControlID == c.ClientID)
                    return _entity;
            }

            ValidationInfo entity = new ValidationInfo() { InputControlID = c.ClientID, WarningMessage = c.Attributes["Message"] };
            this.validations.Add(entity);
            return entity;
        }

        /// <summary>
        /// 查找所有验证控件。
        /// </summary>
        /// <param name="parent">父控件</param>
        private void FindValidationControls(Control parent)
        {
            foreach (Control _c in parent.Controls)
            {
                ValidationInfo entityRF = null;
                if (_c is RequiredFieldValidator)
                {
                    //必填验证。
                    RequiredFieldValidator rfv = _c as RequiredFieldValidator;
                    entityRF = this.GetValidationInfo(rfv.ControlToValidate, parent);
                    entityRF.RFVID = rfv.ClientID;
                }
                else if (_c is CompareValidator)
                {
                    //比较验证。
                    CompareValidator cv = _c as CompareValidator;
                    entityRF = this.GetValidationInfo(cv.ControlToValidate, parent);
                    entityRF.CVID = cv.ClientID;
                }
                else if (_c is RegularExpressionValidator)
                {
                    //正则式验证。
                    RegularExpressionValidator rev = _c as RegularExpressionValidator;
                    entityRF = this.GetValidationInfo(rev.ControlToValidate, parent);
                    entityRF.REVS.Add(rev.ClientID);
                }
                else if (_c is RangeValidator)
                {
                    //正则式验证。
                    RangeValidator rv = _c as RangeValidator;
                    entityRF = this.GetValidationInfo(rv.ControlToValidate, parent);
                    entityRF.RVID = rv.ClientID;
                }


                this.FindValidationControls(_c);
            }
        }

        #endregion

        #region 表示一个控件引用的所有验证信息

        /// <summary>
        /// 表示一个控件引用的所有验证信息。
        /// </summary>
        private class ValidationInfo
        {
            /// <summary>
            /// 正则验证ClientID集合。
            /// </summary>
            private List<string> _revs = new List<string>();

            /// <summary>
            /// 获取或设置 输入控件的ClientID。
            /// </summary>
            public string InputControlID { get; set; }
            /// <summary>
            /// 获取或设置 输入控件的提示信息。
            /// </summary>
            public string WarningMessage { get; set; }
            /// <summary>
            /// 获取或设置 必填验证ClientID。
            /// </summary>
            public string RFVID { get; set; }
            /// <summary>
            /// 获取或设置 比较验证ClientID。
            /// </summary>
            public string CVID { get; set; }
            /// <summary>
            /// 获取或设置 数据范围验证(RangeValidator)ClientID。
            /// </summary>
            public string RVID { get; set; }
            /// <summary>
            /// 获取 正则验证ClientID集合。
            /// </summary>
            public List<string> REVS { get { return this._revs; } }

            /// <summary>
            /// 输出JSON格式的数据。
            /// </summary>
            /// <returns></returns>
            public string ToJSON()
            {
                /*
                 * JSON数据格式：
                var _____parms = {ControlID : "txtPassword", warningMessage : "密码为6位字符；不区分大小写", rfvID : "rfvPassword", cvID : "cvPassword", revIDs : ["revPassword1", "revPassword2"] };
                 */

                string revText = "null";
                if (this.REVS.Count > 0)
                {
                    revText = "[";
                    foreach (string _rev in this.REVS)
                        revText += "\"" + _rev + "\",";
                    revText += "]";
                }

                if (null == this.WarningMessage)
                    this.WarningMessage = string.Empty;

                return string.Format(@"
    inputControlID : ""{0}"", warningMessage : ""{1}"", rfvID : ""{2}"", cvID : ""{3}"", rvID : ""{4}"", revIDs : {5} 
",
      this.InputControlID,
      this.WarningMessage.Replace("\"", "&quot;"),
      null == this.RFVID ? "null" : this.RFVID,
      null == this.CVID ? "null" : this.CVID,
      null == this.RVID ? "null" : this.RVID,
      revText
      );
            }
        }

        #endregion
    }
}
