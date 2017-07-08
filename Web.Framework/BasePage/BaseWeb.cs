using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;

using YX.Domain;
using YX.Core;

namespace YX.Web.Framework
{
    /// <summary>
    /// 页面基类。
    /// </summary>
    public class BaseWeb : AbsBasePage
    {
        #region 变量

        /// <summary>
        /// 当前网站的风格。
        /// </summary>
        protected string currentTheme = null;
        /// <summary>
        /// 是否自动注册验证信息(默认为false)。
        /// </summary>
        private bool autoRegValidation = false;
        /// <summary>
        /// 根据验证控件自动生成客户端表单验证脚本。
        /// </summary>
        private FormValidation formValidation = new FormValidation();

        #endregion

        #region 属性

        /// <summary>
        /// 获取 某一风格的图片路径(最后没有/号)。
        /// </summary>
        /// <returns></returns>
        public string SkinImagePath
        {
            get { return WFUtil.SkinImagePath; }
        }

        /// <summary>
        /// 获取 某一风格的路径。
        /// </summary>
        /// <returns></returns>
        public string SkinPath
        {
            get { return WFUtil.SkinPath; }
        }

        /// <summary>
        /// 获取或设置 是否显示页面功能标题。
        /// </summary>
        public bool IsShowCaption { get; set; }

        /// <summary>
        /// 获取或设置 当前页面Form是否需要上传文件。
        /// </summary>
        public bool IsNeedUploadFile { get; set; }

        #endregion

        #region 页面初始化

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            //获取当前网站风格。
            this.currentTheme = WFUtil.CurrentTheme;
            this.IsShowCaption = true;

            //根据当前网站设置的风格动态加载MasterPage页面。
            var mp = this.Page.MasterPageFile;
            if (!string.IsNullOrEmpty(mp))
            {
                var match = Regex.Match(mp, @"MasterPages/\w+/(\w+).Master", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    //当前主题下的MasterPage页面。
                    var newMP = string.Format(
                        "~/MasterPages/{0}/{1}.Master",
                        currentTheme,
                        match.Groups[1].Value
                    );

                    //如果存在当前主题下的MasterPage页面，则更换之。
                    if (File.Exists(Server.MapPath(newMP)))
                        this.MasterPageFile = newMP;
                }
            }
        }


        /// <summary>
        /// 页面初始化。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Error += new EventHandler(BasePage_Error);

            //注册页面对象。
            this.RegPageObjects();

            //引用脚本。
            WFUtil.RefJavascript(new string[] { 
                "jQuery/jquery-2.0.3.min.js",
                "BootStrap/bootstrap.min.js",
                "common.js",
                "BootStrap/skins.min.js",
                "BootStrap/toastr/toastr.js",
                "lhgdialog/lhgdialog.min.js"
            });

            WFUtil.RefStyle("BootStrap/bootstrap.min.css");
            WFUtil.RefStyle("BootStrap/font-awesome.min.css");
            WFUtil.RefStyle("BootStrap/beyond.min.css");
            WFUtil.RefStyle("common.css");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.Title = this.Title + "-" + "综合信息管理系统";

            //IE8的兼容方法。
            //this.Header.Controls.AddAt(0, new LiteralControl("\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\" />"));
            //输出JS、Css。
            WFUtil.RenderJsCss();

            //获取所有验证信息并注册脚本。
            if (this.autoRegValidation)
                this.formValidation.RegValidation(this);
        }

        #endregion

        #region 清除页面缓存

        /// <summary>
        /// 清除页面缓存。
        /// </summary>
        protected virtual void ClearCache()
        {
            WFUtil.ClearPageCache();
        }

        #endregion

        #region 当页面发生错误

        void BasePage_Error(object sender, EventArgs e)
        {
            WFUtil.HandServerError();
        }

        #endregion

        
        #region 注册页面基础对象

        /// <summary>
        /// 在页面中引用Javascript文件(已自动添加~/Scripts)。
        /// </summary>
        /// <param name="useTheme">是否使用样式</param>
        /// <param name="jsFileNames">Javascript文件名称(已自动添加~/Scripts)</param>
        protected virtual void RefJavascript(bool useTheme, params string[] jsFileNames)
        {
            if (useTheme)
                WFUtil.RefThemeJavascript(jsFileNames);
            else
                WFUtil.RefJavascript(jsFileNames);
        }

        /// <summary>
        /// 引用样式文件。
        /// </summary>
        /// <param name="cssFileNames">样式文件名称(不以/或~/打头的css将自动添加~/App_Themes目录)</param>
        /// <param name="useTheme">是否为引用某个主题内的样式</param>
        protected virtual void RefStyle(bool useTheme, params string[] cssFileNames)
        {
            WFUtil.RefStyle(useTheme, cssFileNames);
        }

        /// <summary>
        /// 注册页面基础对象。
        /// </summary>
        protected virtual void RegPageObjects()
        {
            //引用脚本。
            WFUtil.RefJavascript(new string[] { 
                "jQuery/jquery-2.0.3.min.js",
                "BootStrap/bootstrap.min.js",
                "common.js",
                "BootStrap/skins.min.js",
                "BootStrap/toastr/toastr.js",
                "lhgdialog/lhgdialog.min.js"
            });

            WFUtil.RefStyle("BootStrap/bootstrap.min.css");
            WFUtil.RefStyle("BootStrap/font-awesome.min.css");
            WFUtil.RefStyle("BootStrap/beyond.min.css");
            WFUtil.RefStyle("common.css");
        }

        #endregion

        #region 设置页面Meta标签

        /// <summary>
        /// 设置页面Meta标签。
        /// </summary>
        /// <param name="entityMeta">meta信息对象</param>
        protected virtual void SetPageMeta(MetaInfo entityMeta)
        {
            WFUtil.SetPageMeta(entityMeta);
        }

        #endregion

        #region 注册服务器端按钮

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="target">要注册的目标按钮</param>
        protected virtual void RegValidateAllInput(Control target)
        {
            this.RegValidateAllInput(new Control[] { target });
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="targets">要注册的目标按钮</param>
        protected virtual void RegValidateAllInput(Control[] targets)
        {
            this.autoRegValidation = true;
            this.formValidation.RegValidateAllInput(targets);
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="target">要注册的目标按钮</param>
        /// <param name="validationSuccessScript">当验证成功后要执行的脚本</param>
        protected void RegValidateAllInput(Control target, string validationSuccessScript)
        {
            this.autoRegValidation = true;
            this.formValidation.RegValidateAllInput(new Control[] { target }, validationSuccessScript);
        }

        /// <summary>
        /// 注册服务器端按钮(IButtonControl，HtmlInputButton)的客户端事件，即在提交前进行输入验证。
        /// </summary>
        /// <param name="targets">要注册的目标按钮</param>
        /// <param name="validationSuccessScript">当验证成功后要执行的脚本</param>
        protected void RegValidateAllInput(Control[] targets, string validationSuccessScript)
        {
            this.autoRegValidation = true;
            this.formValidation.RegValidateAllInput(targets, validationSuccessScript);
        }

        #endregion

        #region 去除分页刷新本页面

        /// <summary>
        /// 去除分页刷新本页面。
        /// </summary>
        protected void RefurCurrPageWithoutPager()
        {
            var url = Regex.Replace(Request.RawUrl, @"page=\d+", "page=1", RegexOptions.IgnoreCase);
            Response.Redirect(url);
        }

        /// <summary>
        /// 刷新本页面。
        /// </summary>
        protected void RefurCurrPage()
        {
            Response.Redirect(Request.RawUrl);
        }


        /// <summary>
        /// 判断是否为当前Tab(根据最大匹配值判断)。
        /// </summary>
        /// <param name="tabUrls">所有Tab栏中的网址</param>
        /// <param name="targetUrl">当前Tab栏中的网址</param>
        protected bool IsCurrentTab(List<string> tabUrls, string targetUrl)
        {
            //解析为查询参数数组。
            var qsCurrent = CoreUtil.QueryString(Request.RawUrl);
            var maxNumMatch = -1;
            var sortedUrls = new List<string>();

            //循环所有tab栏，将匹配度最大的url加入集合的第0位。
            foreach (var _tabUrl in tabUrls)
            {
                //解析为查询参数数组。
                var qsTab = CoreUtil.QueryString(_tabUrl);
                var numMatch = 0;
                foreach (var _item in qsTab)
                {
                    if (qsCurrent.Contains(_item))
                        numMatch += 1;
                    else
                        numMatch = -1;
                }

                if (numMatch > maxNumMatch)
                {
                    sortedUrls.Insert(0, _tabUrl);
                    maxNumMatch = numMatch;
                }
            }

            //如果没有参数返回。
            if (sortedUrls.Count == 0)
                return false;

            //取匹配度最大的比较。
            return sortedUrls[0].IgnoreCaseEquals(targetUrl);
        }

        #endregion
    }
}
