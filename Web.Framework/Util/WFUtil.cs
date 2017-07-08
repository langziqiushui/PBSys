using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;

using YX.Core;
using YX.Factory;
using YX.Domain;
using System.Globalization;
using System.Net;


namespace YX.Web.Framework
{
    /// <summary>
    /// 页面相关处理。
    /// </summary>
    public static class WFUtil
    {
        #region 变量

        /// <summary>
        /// 对JS、CSS进行加密的加密对象。
        /// </summary>
        private static CryptographyManager cryJsCss = new CryptographyManager(EncryptionFormats.DES2QueryString);

        #endregion

        #region 页面上下文

        /// <summary>
        /// 获取 当前上下文请求的页面。
        /// </summary>
        /// <returns></returns>
        public static Page CurrentPage
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.Handler)
                    return null;

                return HttpContext.Current.Handler as Page;
            }
        }

        /// <summary>
        /// 清除页面缓存。
        /// </summary>
        public static void ClearPageCache()
        {
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AddHeader("Pragma", "No-Cache");
            HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }

        #endregion

        #region 服务器异常处理

        /// <summary>
        /// 处理服务器异常。
        /// </summary>
        public static void HandServerError()
        {
            if (null == HttpContext.Current)
                return;

            //获取最后的异常。
            Exception ex = HttpContext.Current.Server.GetLastError();
            if (null == ex)
                return;

            //记录错误日志。
            LogException(ex);

            //如果是本地，不转向错误页面。
            //if (HttpContext.Current.Request.Url.ToString().IgnoreCaseIndexOf("localhost") >= 0)
            //    return;

            //清除系统异常。
            HttpContext.Current.Server.ClearError();

            if (null != HttpContext.Current)
            {
                //保存异常信息到全局变量中。    
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[WebKeys.APPLICATION_LAST_ERROR + HttpContext.Current.Session.SessionID] = ex;
                HttpContext.Current.Application.UnLock();

                //转向异常处理页面。
                HttpContext.Current.Response.Redirect("~/error?r=" + CoreUtil.RandomNumeric(10));
            }
        }

        #endregion

        #region 注册脚本

        /// <summary>
        /// 注册脚本。
        /// </summary>
        /// <param name="script">脚本(请不要包含scritp和/script标识)</param>
        public static void RegisterStartupScript(string script)
        {
            RegisterStartupScript(Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 注册脚本。
        /// </summary>
        /// <param name="key">注册键</param>
        /// <param name="script">脚本(请不要包含scritp和/script标识)</param>
        public static void RegisterStartupScript(string key, string script)
        {
            Page currentPage = CurrentPage;
            ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), key, script, true);
        }

        /// <summary>
        /// 注册脚本。
        /// </summary>
        /// <param name="script">脚本(请不要包含scritp和/script标识)</param>
        public static void RegisterClientScriptBlock(string script)
        {
            RegisterClientScriptBlock(Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 注册脚本。
        /// </summary>
        /// <param name="key">注册键</param>
        /// <param name="script">脚本(请不要包含scritp和/script标识)</param>
        public static void RegisterClientScriptBlock(string key, string script)
        {
            Page currentPage = CurrentPage;
            ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), key, script, true);
        }

        /// <summary>
        /// 为服务器控件添加客户端执行的命令(执行客户端正则表达式验证，将不执行服务器端的命令)。
        /// </summary>
        /// <param name="control">服务器控件</param>
        /// <param name="clientSideCommand">客户端将要执行的命令</param>
        public static void AddClientSideCommand(System.Web.UI.WebControls.WebControl control, string clientSideCommand)
        {
            control.Attributes.Add("onclick", "javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(\"" + control.ClientID + "\", \"\", true, \"\", \"\", false, false));if(WebForm_OnSubmit()){" + clientSideCommand + "};return false;");
        }

        #endregion

        #region 封装一个Try、Catch过程

        /// <summary>
        /// 封装一个Try、Catch过程(屏蔽异常，不写日志)。
        /// </summary>
        /// <param name="fun">在Try模块中执行的方法</param>
        public static void UIDoTryCatch(Action fun)
        {
            try
            {
                if (null != fun)
                    fun();
            }
            catch { }
        }

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="fun">在Try模块中执行的方法</param>
        public static bool UIDoTryCatch(string caption, Action fun)
        {
            return UIDoTryCatch(caption, fun, true, null, null);
        }

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="showMsg">是否显示弹出信息</param>
        /// <param name="fun">在Try模块中执行的方法</param>
        public static bool UIDoTryCatch(string caption, Action fun, bool showMsg)
        {
            return UIDoTryCatch(caption, fun, showMsg, null, null);
        }

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="showMsg">是否显示弹出信息</param>
        /// <param name="fun">在Try模块中执行的方法</param>
        /// <param name="funError">在Catch模块中执行的方法</param>
        public static bool UIDoTryCatch(string caption, Action fun, bool showMsg, Action<Exception> funError)
        {
            return UIDoTryCatch(caption, fun, showMsg, funError, null);
        }


        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="showMsg">是否显示弹出信息</param>
        /// <param name="fun">在Try模块中执行的方法</param>
        /// <param name="funError">在Catch模块中执行的方法</param>
        /// <param name="funFinaly">在Finaly模块中执行的方法</param>
        public static bool UIDoTryCatch(string caption, Action fun, bool showMsg, Action<Exception> funError, Action funFinaly)
        {
            try
            {
                if (null != fun)
                    fun();

                return true;
            }
            catch (Exception ex)
            {
                //记录错误日志。
                LogException(ex, caption);

                //弹出错误消息。
                if (showMsg)
                    Message.Alert(ex.Message.Replace("{<WARNING>}", string.Empty), caption + "时出错");

                //异常处理方法。
                if (null != funError)
                    funError(ex);
            }
            finally
            {
                if (null != funFinaly)
                    funFinaly();
            }

            return false;
        }

        /// <summary>
        /// 记录错误日志。
        /// </summary>
        /// <param name="ex">Exception异常</param>
        public static void LogException(Exception ex)
        {
            LogException(ex, null);
        }

        /// <summary>
        /// 记录错误日志。
        /// </summary>
        /// <param name="ex">Exception异常</param>
        /// <param name="caption">标题</param>
        public static void LogException(Exception ex, string caption)
        {
            if (null == ex || ex is System.Threading.ThreadAbortException || ex is System.ServiceModel.EndpointNotFoundException)
                return;

            var lt = LogTypes.自定义错误;

            //记录异常。
            var errorMessage = (string.IsNullOrEmpty(caption) ? string.Empty : caption + "：") + ex.ToString();
        }

        #endregion

        #region 为OpenAPI封装一个Try、Catch过程

        /// <summary>
        /// 生成错误返回JSON字符串。
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns></returns>
        public static string MakeErrorJsonData(string errorMessage)
        {
            return "{\"r\":\"F\",\"m\":\"" + CoreUtil.JsonTransferred(errorMessage) + "\"}";
        }

        /// <summary>
        /// 生成成功返回JSON字符串。
        /// </summary>
        /// <param name="jsonData">JSON数据内容</param>
        /// <returns></returns>
        public static string MakeSuccessJsonData(string jsonData)
        {
            return "{\"r\":\"T\",\"d\":" + jsonData + "}";
        }

        /// <summary>
        /// 为OpenAPI封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="fun">在Try块中执行的方法</param>
        /// <param name="funError">在Catch块中执行的方法</param>
        /// <param name="defaultErrorMessage">当发生异常时默认显示的错误提示</param>
        /// <param name="funFinally">在finally块中执行的方法</param>
        public static string DoTryCatch(string caption, Func<string> fun, Action<Exception> funError, string defaultErrorMessage, Action funFinally)
        {
            var json = string.Empty;

            try
            {
                json = fun();
            }
            catch (Exception ex)
            {
                //当发生异常时处理。
                if (null != funError)
                    funError(ex);

                //生成错误json串。
                json = MakeErrorJsonData(defaultErrorMessage ?? ex.Message);

                //记录日志。
                WFUtil.LogException(ex, caption);
            }
            finally
            {
                if (null != funFinally)
                    funFinally();
            }

            return json;
        }

        /// <summary>
        /// 为OpenAPI封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="fun">在Try块中执行的方法</param>
        public static string DoTryCatch(string caption, Func<string> fun)
        {
            return DoTryCatch(caption, fun, null, null, null);
        }

        /// <summary>
        /// 为OpenAPI封装一个Try、Catch过程。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="fun">在Try块中执行的方法</param>
        /// <param name="funError">在Catch块中执行的方法</param>
        /// <param name="defaultErrorMessage">当发生异常时默认显示的错误提示</param>
        public static string DoTryCatch(string caption, Func<string> fun, Action<Exception> funError, string defaultErrorMessage)
        {
            return DoTryCatch(caption, fun, funError, defaultErrorMessage, null);
        }

        #endregion

        #region 页面引用资源

        /// <summary>
        /// 获取 当前网站的主题界面风格(对应MasterPages下的风格目录)。
        /// </summary>
        /// <returns></returns>
        public static string CurrentTheme
        {
            get
            {
                return "Normal";
            }
        }

        /// <summary>
        /// 获取 当前网站的颜色风格(对应相关风格主题下的ColorStyle目录)。
        /// </summary>
        /// <returns></returns>
        public static string CurrentChildTheme
        {
            get
            {
                return "Normal";
            }
        }

        /// <summary>
        /// 获取 当前网站主题图片路径(如：/App_Themes/Normal/images，最后没有/号)。
        /// </summary>
        /// <returns></returns>
        public static string SkinImagePath
        {
            get { return SkinPath + "/images"; }
        }

        /// <summary>
        /// 获取 当前网站主题路径(如：/App_Themes/Normal，最后没有/号)。
        /// </summary>
        /// <returns></returns>
        public static string SkinPath
        {
            get { return "/App_Themes/Modules/" + CurrentTheme; }
        }

        /// <summary>
        /// 引用样式文件。
        /// </summary>
        /// <param name="cssFileName">样式文件名称(已自动添加~/App_Themes)</param>
        public static void RefStyle(string cssFileName)
        {
            RefStyle(false, cssFileName);
        }

        /// <summary>
        /// 引用当前模板风格下的样式文件。
        /// </summary>
        /// <param name="cssFileName">样式文件名称(已自动添加~/App_Themes)</param>
        public static void RefThemeStyle(string cssFileName)
        {
            RefStyle(true, cssFileName);
        }

        /// <summary>
        /// 引用样式文件。
        /// </summary>
        /// <param name="cssFileNames">样式文件名称(不以/或~/打头的css将自动添加~/App_Themes目录)</param>
        /// <param name="useTheme">是否为引用某个主题内的样式</param>
        public static void RefStyle(bool useTheme, params string[] cssFileNames)
        {
            var page = CurrentPage as AbsBasePage;
            if (null == page)
                return;

            string t = useTheme ? CurrentTheme + "/" : string.Empty;
            foreach (string _cssFileName in cssFileNames)
            {
                var cssFile = "";
                if (_cssFileName.StartsWith("~/") || _cssFileName.StartsWith("/"))
                {
                    //以~/ 或 /打头的，表示完整路径的css(不在App_Themes目录中)的CSS。
                    cssFile = _cssFileName;
                }
                else
                {
                    //获取当前主题下的css文件。
                    cssFile = "~/App_Themes/ThemeX/" + t + _cssFileName;
                    //如果css文件不存在，则使用默认主题下的CSS文件。
                    if (!File.Exists(HttpContext.Current.Server.MapPath(cssFile)))
                    {
                        if (useTheme)
                            cssFile = "~/App_Themes/ThemeX/Normal/" + _cssFileName;
                        else
                            cssFile = "~/App_Themes/" + _cssFileName;
                    }
                }

                cssFile = page.ResolveUrl(cssFile);
                if (!page.CssFiles.IgnoreCaseContains(cssFile))
                    page.CssFiles.Add(cssFile);
            }
        }

        //private static string GenCss

        /// <summary>
        /// 在页面中引用Javascript文件。
        /// </summary>
        /// <param name="jsFileNames">Javascript文件名称(已自动添加~/Scripts)</param>
        public static void RefJavascript(params string[] jsFileNames)
        {
            RefJavascript(false, jsFileNames);
        }

        /// <summary>
        /// 在页面中引用当前模板风格下的Javascript文件。
        /// </summary>
        /// <param name="jsFileNames">Javascript文件名称(已自动添加~/Scripts)</param>
        public static void RefThemeJavascript(params string[] jsFileNames)
        {
            RefJavascript(true, jsFileNames);
        }

        /// <summary>
        /// 在页面中引用Javascript文件。
        /// </summary>
        /// <param name="jsFileNames">Javascript文件名称(已自动添加~/Scripts)</param>
        public static void RefJavascript(bool useTheme, params string[] jsFileNames)
        {
            var page = CurrentPage as AbsBasePage;
            if (null == page)
                return;

            string t = useTheme ? "ThemeX/" + CurrentTheme + "/" : string.Empty;

            foreach (string _jsFilePath in jsFileNames)
            {
                string jsPath = string.Empty;
                if (_jsFilePath.IgnoreCaseIndexOf("http://") == 0 || _jsFilePath.StartsWith("~/") || _jsFilePath.StartsWith("/"))
                {
                    //以~/ 或 /打头的，表示完整路径的JS。
                    jsPath = _jsFilePath;
                }
                else
                {
                    jsPath = page.ResolveUrl("/JScripts/" + t + _jsFilePath);
                    if (!File.Exists(HttpContext.Current.Server.MapPath(jsPath)))
                    {
                        if (useTheme)
                            jsPath = "/JScripts/ThemeX/Normal/" + _jsFilePath;
                        else
                            jsPath = "/JScripts/" + _jsFilePath;
                    }
                }

                if (!page.JSFiles.IgnoreCaseContains(jsPath))
                    page.JSFiles.Add(jsPath);
            }
        }

        /// <summary>
        /// 引用日历控件JS。
        /// </summary>
        public static void RefJS_WdatePicker()
        {
            WFUtil.RefJavascript("/JScripts/My97DatePicker4.8/WdatePicker.js");
        }

        /// <summary>
        /// Ajax上传。
        /// </summary>
        public static void RefJS_AjaxUpload()
        {
            WFUtil.RefJavascript("/JScripts/My97DatePicker4.8/ajaxupload.3.5.js");
        }

        /// <summary>
        /// 输出js、css。
        /// </summary>
        public static void RenderJsCss()
        {
            var page = CurrentPage as AbsBasePage;
            if (null == page)
                return;

            var litHeader = WFUtil.CreateHeaderLiteral();
            var sb = new StringBuilder();

            //判断是否需要合并压缩CSS。
            var isCompressionCSS = ConfigAppSettings.IsCompressionCSS;
            if (null != page.IsCompressionCSS)
                isCompressionCSS = page.IsCompressionCSS.Value;
            if (isCompressionCSS)
            {
                //如果压缩CSS。
                //App_Themes/a.css
                //App_Themes/x.css
                //App_Themes/Normal/b.css
                //App_Themes/Normal/c.css
                //以css文件所在目录进行分组压缩css。
                var dirs = new List<string>();
                foreach (var _cssFile in page.CssFiles)
                {
                    var li = _cssFile.LastIndexOf("/");
                    var dir = _cssFile.Substring(0, li + 1);
                    if (!dirs.Contains(dir))
                        dirs.Add(dir);
                }

                foreach (var _dir in dirs)
                {
                    var cssFileString = string.Empty;
                    foreach (var _cssFile in page.CssFiles)
                    {
                        var li = _cssFile.LastIndexOf("/");
                        if (_dir == _cssFile.Substring(0, li + 1))
                            cssFileString += _cssFile + "|";
                    }

                    //去除最后一个|号。
                    cssFileString = cssFileString.Substring(0, cssFileString.Length - 1);

                    //输出CSS。
                    var cssResource = _dir + "css.axd?f=" + cryJsCss.Encrypt(cssFileString) + "&v=" + ConfigAppSettings.CssVersion;
                    sb.AppendFormat(
                        "\n\t<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />",
                        cssResource
                    );
                }
            }
            else
            {
                //不压缩CSS。
                foreach (var _cssFile in page.CssFiles)
                {
                    sb.AppendFormat(
                        "\n\t<link href=\"{0}?v={1}\" rel=\"stylesheet\" type=\"text/css\" />",
                        _cssFile,
                        ConfigAppSettings.CssVersion
                    );
                }
            }


            //判断是否需要合并压缩JS。
            var isCompressionJS = ConfigAppSettings.IsCompressionJS;
            if (null != page.IsCompressionJS)
                isCompressionJS = page.IsCompressionJS.Value;

            if (isCompressionJS)
            {
                //如果压缩JS。
                var jsFileString = string.Empty;
                foreach (var _jsFile in page.JSFiles)
                {
                    if (_jsFile.IgnoreCaseIndexOf("My97DatePicker") >= 0 || _jsFile.IgnoreCaseIndexOf("http://") >= 0)
                    {
                        //特殊JS下面处理。
                        continue;
                    }
                    else
                    {
                        jsFileString += _jsFile + "|";
                    }
                }
                jsFileString = jsFileString.Substring(0, jsFileString.Length - 1);
                var jsResource = "js.axd?f=" + cryJsCss.Encrypt(jsFileString) + "&v=" + ConfigAppSettings.JSVersion;
                sb.AppendFormat(
                "\n\t<script src=\"{0}\" language=\"javascript\"></script>",
                    jsResource
                );

                //特殊JS处理。
                foreach (var _jsFile in page.JSFiles)
                {
                    if (_jsFile.IgnoreCaseIndexOf("My97DatePicker") >= 0 || _jsFile.IgnoreCaseIndexOf("http://") >= 0)
                    {
                        sb.AppendFormat(
                                "\n\t<script src=\"{0}\" language=\"javascript\"></script>",
                            _jsFile
                        );
                    }
                }
            }
            else
            {
                //如果不压缩JS。
                foreach (var _jsFile in page.JSFiles)
                {
                    var src = _jsFile;

                    //外网的JS不带上版本号。
                    if (_jsFile.IgnoreCaseIndexOf("http://") < 0)
                        src += "?v=" + ConfigAppSettings.JSVersion;

                    sb.AppendFormat(
                        "\n\t<script src=\"{0}\" language=\"javascript\"></script>",
                        src
                    );
                }
            }

            litHeader.Text = sb.ToString();
        }

        #endregion

        #region 获取或创建头部litHeader控件

        /// <summary>
        /// 获取或创建头部litHeader控件。
        /// </summary>
        /// <returns></returns>
        public static LiteralControl CreateHeaderLiteral()
        {
            Page currentPage = CurrentPage;

            //查找litHeader控件。            
            foreach (Control _c in currentPage.Header.Controls)
            {
                if (_c.ID == "litHeader")
                    return (LiteralControl)_c;
            }

            LiteralControl litHeader = new LiteralControl()
            {
                ID = "litHeader",
                Text = string.Empty
            };

            int index = currentPage.Header.Controls.Count > 0 && currentPage.Header.Controls[0] is HtmlTitle ? 1 : 0;
            currentPage.Header.Controls.AddAt(index, litHeader);
            return litHeader;
        }

        #endregion

        #region 设置页面Meta标签

        /// <summary>
        /// 设置页面Meta标签。
        /// </summary>
        /// <param name="entityMeta">meta信息对象</param>
        public static void SetPageMeta(MetaInfo entityMeta)
        {
            var currentPage = WFUtil.CurrentPage as AbsBasePage;
            if (null == currentPage || null == currentPage.Header)
                return;

            var litMeta = currentPage.FindControl("litMeta") as LiteralControl;
            if (null == litMeta)
            {
                litMeta = new LiteralControl();
                litMeta.ID = "litMeta";
                currentPage.Header.Controls.Add(litMeta);
            }

            if (!string.IsNullOrEmpty(entityMeta.Title))
                currentPage.Title = entityMeta.Title;
            if (!string.IsNullOrEmpty(entityMeta.Keywords))
                litMeta.Text += "\n\t<meta name=\"Keywords\" content=\"" + entityMeta.Keywords + "\" />";
            if (!string.IsNullOrEmpty(entityMeta.Description))
                litMeta.Text += "\n\t<meta name=\"Description\" content=\"" + entityMeta.Description + "\" />";
            litMeta.Text += "\n";
        }

        #endregion

        #region 上传相关

        /// <summary>
        /// 验证上传文件。
        /// </summary>
        /// <param name="pf">上传的文件对象</param>
        /// <param name="maxLength">允许上传文件大小的最大值(单位：KB)</param>
        /// <param name="message">输出消息</param>
        /// <param name="extensions">允许上传的文件扩展名集合(扩展名以.打头)</param>
        /// <returns></returns>
        public static bool ValidateUploadFile(HttpPostedFile pf, int maxLength, ref string message, params string[] extensions)
        {
            //png  image/x-png or image/png
            //gif image/gif
            //jpg image/jpeg or image/pjpeg
            //swf application/x-shockwave-flash  
            //rar application/octet-stream  
            //zip application/x-zip-compressed

            if (pf.ContentLength == 0)
            {
                message = "请选择将要上传的文件！";
                return false;
            }

            var lastIndex = pf.FileName.LastIndexOf(".");
            if (lastIndex < 0)
            {
                message = "系统不允许上传未知类型文件！";
                return false;
            }

            //判断扩展名。
            var ex = pf.FileName.Substring(lastIndex).Trim().ToLower();
            if (!extensions.IgnoreCaseContains(ex))
            {
                var extensionText = string.Empty;
                foreach (var _ext in extensions)
                    extensionText += _ext + ",";
                if (extensionText.Length > 0)
                    extensionText = extensionText.Substring(0, extensionText.Length - 1);

                message = "系统仅允许上传指定文件格式(" + extensionText + ")的文件！";
                return false;
            }

            //判断文件大小。
            if (pf.ContentLength > maxLength * 1024)
            {
                message = "上传文件的大小超过系统规定的最大上限(" + maxLength + "KB)！";
                return false;
            }

            //如果是图片护展名，则进一步验证。
            if (ex == ".gif" || ex == ".jpg" || ex == ".jpeg" || ex == "png")
            {
                var isImage = false;
                try
                {
                    var img = System.Drawing.Image.FromStream(pf.InputStream);
                    isImage = true;
                }
                catch { }

                if (!isImage)
                {
                    message = "上传的图像文件疑为非法！";
                    return false;
                }
            }

            ////如果是flash文件则判断ContentType。
            //if (ex == ".swf" && !pf.ContentType.IgnoreCaseEquals("application/x-shockwave-flash"))
            //{
            //    Log.Write(LogTypes.黑客攻击报警, "用户企图上传非法的flash文件(上传文件：" + pf.FileName + "，ContentType为：" + pf.ContentType + ")！");
            //    message = "上传非法的flash文件！";
            //    return false;
            //}

            ////如果是zip文件则判断ContentType。
            //if (ex == ".zip" && !pf.ContentType.IgnoreCaseEquals("application/x-zip-compressed"))
            //{
            //    Log.Write(LogTypes.黑客攻击报警, "用户企图上传非法的zip文件(上传文件：" + pf.FileName + "，ContentType为：" + pf.ContentType + ")！");
            //    message = "上传非法的zip压缩文件！";
            //    return false;
            //}


            return true;
        }


        /// <summary>
        /// 判断上传的文件是否为图像文件(仅支持.gif,.png,.jpg格式)。
        /// </summary>
        /// <param name="pf">上传的文件对象</param>
        /// <returns></returns>
        public static bool IsUploadImage(HttpPostedFile pf)
        {
            //png  image/x-png or image/png
            //gif image/gif
            //jpg image/jpeg or image/pjpeg

            var lastIndex = pf.FileName.LastIndexOf(".");
            if (lastIndex < 0)
                return false;

            var ex = pf.FileName.Substring(lastIndex).Trim().ToLower();
            if (!new string[] { ".gif", ".jpg", ".png" }.IgnoreCaseContains(ex))
                return false;

            //如果是图像文件则判断ContentType。
            if (pf.ContentType.IgnoreCaseIndexOf("image/") < 0)
                return false;

            return true;
        }

        /// <summary>
        /// 判断上传的文件是否为压缩文件(仅支持.zip,.rar格式)。
        /// </summary>
        /// <param name="pf">上传的文件对象</param>
        /// <returns></returns>
        public static bool IsUploadCompressFile(HttpPostedFile pf)
        {
            //rar application/octet-stream  
            //zip application/x-zip-compressed

            var lastIndex = pf.FileName.LastIndexOf(".");
            if (lastIndex < 0)
                return false;

            var ex = pf.FileName.Substring(lastIndex).Trim().ToLower();
            return new string[] { ".zip", ".rar" }.IgnoreCaseContains(ex);
        }

        /// <summary>
        /// 判断是否上传flash文件。
        /// </summary>
        /// <param name="pf">上传的文件对象</param>
        /// <returns></returns>
        public static bool IsUploadFlash(HttpPostedFile pf)
        {
            //swf  application/x-shockwave-flash

            var lastIndex = pf.FileName.LastIndexOf(".");
            if (lastIndex < 0)
                return false;

            var ex = pf.FileName.Substring(lastIndex).Trim().ToLower();
            return ex == ".swf";
        }

        /// <summary>
        /// 上传文件到文件服务器。
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名</param>
        /// <param name="listName">目录名称</param>
        /// <param name="thumbImageSize">图片缩略图尺寸(格式：106,80)</param>
        /// <param name="scaleImageSize">图片同比例缩小尺寸(格式：500,500)</param>
        /// <param name="newFileName">上传成功后新文件名</param>
        /// <param name="errorMessage">上传失败原因</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadToImageServer(Stream stream, string fileName, string listName, string thumbImageSize, string scaleImageSize, ref string newFileName, ref string errorMessage)
        {
            try
            {
                string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
                if (!(ext == ".jpg" || ext == ".gif" || ext == ".png" || ext == ".bmp"))
                {
                    errorMessage = "仅允许上传.jpg、.gif、.png、.bmp图片文件！";
                    return false;
                }

                var buffer = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(buffer, 0, buffer.Length);
                var r = HttpUtil.PostData("https://www.baidu.com?method=RemoteUploadFile&filename=" + fileName + "&listname=" + listName + "&thumbSize=" + thumbImageSize + "&sis=" + scaleImageSize, buffer, Encoding.UTF8);
                if (r.IgnoreCaseIndexOf("SUCCESS:") == 0)
                {
                    newFileName = r.Replace("SUCCESS:", "");
                    return true;
                }

                errorMessage = r;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }


        /// <summary>
        /// 删除图片服务器上的图片文件。
        /// </summary>
        /// <param name="fileName">将要删除的文件名</param>
        /// <param name="errorMessage">删除文件失败原因</param>
        /// <returns></returns>
        public static bool DeleteFileFromImageServer(string fileName, ref string errorMessage)
        {
            try
            {
                var qs = "action=delete&file=" + fileName;
                var r = HttpUtil.PostData("https://www.baidu.com", qs);
                if (r.IgnoreCaseEquals("SUCCESS"))
                    return true;

                errorMessage = r;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion

        #region 日期替换

        /// <summary>
        /// 替换国际机票出发、返程日期。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReplaceIFlightDate(string url)
        {
            var sd = DateTime.Today.AddDays(2);
            return
                url.IgnoreCaseReplace("{SDate}", HttpUtility.UrlEncode(sd.ToString("yyyy-MM-dd")))
                .IgnoreCaseReplace("{BDate}", HttpUtility.UrlEncode(sd.AddDays(7).ToString("yyyy-MM-dd")))
            ;
        }

        #endregion

        #region 刷新父页面

        /// <summary>
        /// 刷新父页面。
        /// </summary>
        public static void RefurParentPage()
        {
            WFUtil.RegisterStartupScript("common.refurParentPage();");
        }

        #endregion

        #region 通过身份证号码获取出生年月日

        public static string GetBirdthDay(string cardNo)
        {
            if (cardNo.Length == 18)
            {
                string year = cardNo.Substring(6, 4);
                string month = cardNo.Substring(10, 2);
                string day = cardNo.Substring(12, 2);
                var _s = year + "-" + month + "-" + day;
                try
                {
                    DateTime dt = DateTime.Parse(_s);
                    return dt.ToString("yyyy-MM-dd");
                }
                catch
                {
                    return "";
                }
            }
            else
                return "";
        }
        #endregion

 


        #region 验证开放接口签名参数

        /// <summary>
        /// 验证开放接口签名参数。
        /// </summary>
        /// <param name="parms">加入签名的参数(转小写)</param>
        /// <param name="inputSign">客户输入的签名</param>
        /// <returns></returns>
        //public static bool VerifyOpenAPISign(string parms, string inputSign)
        //{
        //    var mySign = CoreUtil.MakeMD5_UTF8(Factory.DBConfigFactory.Global_OPenAPIKey + parms.ToLower());
        //    return mySign == inputSign;
        //}

        #endregion

    }
}
