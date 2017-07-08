using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;


using YX.Core;
namespace YX.Component
{
    /// <summary>
    /// 自定义分页控件。
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyMiniPager runat=server></{0}:MyMiniPager>")]
    public class MyMiniPager : WebControl
    {
        #region 属性

        /// <summary>
        /// 获取或设置 每页显示的记录数。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [DefaultValue("15")]
        [Description("获取或设置 每页显示的记录数")]
        [Localizable(true)]
        public int PageSize
        {
            get
            {
                if (null == ViewState["PageSize"])
                    return 15;
                return Convert.ToInt32(ViewState["PageSize"]);
            }
            set
            {
                this.ViewState["PageSize"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 记录总数。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [DefaultValue("0")]
        [Description("获取或设置 记录总数")]
        [Localizable(true)]
        public int RecordCount
        {
            get
            {
                if (null == ViewState["RecordCount"])
                    return 0;
                return Convert.ToInt32(ViewState["RecordCount"]);
            }
            set
            {
                this.ViewState["RecordCount"] = value;
            }
        }

        /// <summary>
        /// 获取 总页数。
        /// </summary>
        [Bindable(false)]
        public int PageCount
        {
            get
            {
                if (null == ViewState["PageCount"])
                {
                    int pageCount = this.CalculatePageCount();
                    ViewState["PageCount"] = pageCount;
                    return pageCount;
                }

                return Convert.ToInt32(ViewState["PageCount"]);
            }
        }

        /// <summary>
        /// 获取或设置 当前页码。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [DefaultValue("1")]
        [Description("获取或设置 当前页码")]
        [Localizable(true)]
        public int CurrentPageIndex
        {
            get
            {
                if (null == ViewState["CurrentPageIndex"])
                    return 1;
                return Convert.ToInt32(ViewState["CurrentPageIndex"]);
            }
            set
            {
                this.ViewState["CurrentPageIndex"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 显示的数字按钮数。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [DefaultValue("11")]
        [Description("获取或设置 显示的数字按钮数")]
        [Localizable(true)]
        public int ButtonCount
        {
            get
            {
                if (null == ViewState["ButtonCount"])
                    return 11;
                return Convert.ToInt32(ViewState["ButtonCount"]);
            }
            set
            {
                this.ViewState["ButtonCount"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 链接格式。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [DefaultValue("xxx.aspx?page={0}")]
        [Description("获取或设置 链接格式(格式如：xxx.aspx?page={0})")]
        [Localizable(true)]
        public string UrlFormat
        {
            get
            {
                if (null == ViewState["UrlFormat"])
                    return null;
                return ViewState["UrlFormat"].ToString();
            }
            set
            {
                this.ViewState["UrlFormat"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 第一页链接格式。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [DefaultValue("xxx.aspx?page={0}")]
        [Description("获取或设置 链接格式(格式如：xxx.aspx?page={0})")]
        [Localizable(true)]
        public string FirstPageUrlFormat
        {
            get
            {
                if (null == ViewState["FirstPageUrlFormat"])
                    return null;
                return ViewState["FirstPageUrlFormat"].ToString();
            }
            set
            {
                this.ViewState["FirstPageUrlFormat"] = value;
            }
        }


        #endregion

        #region 重载基类

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.CurrentPageIndex = 1;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["page"]))
                this.CurrentPageIndex = int.Parse(this.Page.Request.QueryString["page"]);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.BeginPager();
            }
        }

        #endregion

        #region 生成分页

        /// <summary>
        /// 计算分页数。
        /// </summary>
        /// <returns></returns>
        private int CalculatePageCount()
        {
            //获取分页数。
            int pageCount = 0;
            if (this.RecordCount > 0)
            {
                pageCount = this.RecordCount / this.PageSize;
                if (this.RecordCount % this.PageSize > 0)
                    pageCount += 1;
            }

            return pageCount;
        }

        /// <summary>
        /// 生成分页。
        /// </summary>
        public void BeginPager()
        {
            StringBuilder sb = null;

            #region 生成需要的数字

            //获取分页数。
            int pageCount = this.PageCount;
            //判断当前页码是否超出总页数。
            if (this.CurrentPageIndex > pageCount)
                this.CurrentPageIndex = pageCount;

            //将要显示的按钮数。
            int ct = Math.Min(pageCount, this.ButtonCount);
            int numRemain = 0;
            List<int> pns = new List<int>();
            int _cpi = this.CurrentPageIndex;

            //取左边数。
            for (int i = 0; i <= ct / 2; i++)
            {
                if (_cpi <= 0)
                    break;
                pns.Insert(0, _cpi);
                _cpi -= 1;
            }

            //取右边数。
            _cpi = this.CurrentPageIndex + 1;
            numRemain = ct - pns.Count;
            for (int i = 0; i < numRemain; i++)
            {
                if (_cpi > pageCount)
                    break;
                pns.Add(_cpi);
                _cpi += 1;
            }

            //因右边不足，补左边数。
            if (pns.Count < ct)
            {
                _cpi = pns[0] - 1;
                numRemain = ct - pns.Count;
                for (int i = 0; i < numRemain; i++)
                {
                    if (_cpi <= 0)
                        break;

                    pns.Insert(0, _cpi);
                    _cpi -= 1;
                }
            }

            #endregion

            #region 生成url

            string _firstPageUrlFormat = this.FirstPageUrlFormat;
            if (string.IsNullOrEmpty(_firstPageUrlFormat))
            {
                _firstPageUrlFormat = this.GenUrl();
                if (_firstPageUrlFormat.EndsWith("&"))
                    _firstPageUrlFormat = _firstPageUrlFormat.Substring(0, _firstPageUrlFormat.Length - 1);
            }
            string hrefFirstPage = "<a href=\"" + _firstPageUrlFormat + "\">";

            string _urlFormat = this.UrlFormat;
            if (string.IsNullOrEmpty(_urlFormat))
                _urlFormat = this.GenUrl() + "page={0}";
            string hrefOthers = "<a href=\"" + _urlFormat + "\">";

            #endregion

            #region 绘制分页按钮

            sb = new StringBuilder();
            sb.Append("\n<div class=\"MyPager-Mini\">\n");

            if (pns.Count > 0)
            {
                //首页，上一页 按钮。
                if (this.CurrentPageIndex == 1)
                {
                    sb.Append("    <span class=\"MyPager-Mini_disable\"><< 首页</span>\n");
                    sb.Append("    <span class=\"MyPager-Mini_disable\">< 上页</span>\n");
                }
                else
                {
                    sb.Append("    " + string.Format(hrefFirstPage, 1));
                    sb.Append("<span><< 首页</span></a>\n");
                    sb.Append("    " + string.Format(this.CurrentPageIndex - 1 == 1 ? hrefFirstPage : hrefOthers, this.CurrentPageIndex - 1));
                    sb.Append("<span>< 上页</span></a>\n");
                }

                //页码。
                foreach (int _pn in pns)
                {
                    if (_pn == this.CurrentPageIndex)
                    {
                        sb.Append("    <span class=\"MyPager-Mini_focus\">" + _pn.ToString("00") + "</span>\n");
                    }
                    else
                    {
                        sb.Append("    " + string.Format(_pn == 1 ? hrefFirstPage : hrefOthers, _pn));
                        sb.Append("<span>" + _pn.ToString("00") + "</span></a>\n");
                    }
                }

                //下一页、尾页。
                if (this.CurrentPageIndex == pageCount)
                {
                    sb.Append("    <span class=\"MyPager-Mini_disable\">下一页 ></span>\n");
                }
                else
                {
                    sb.Append("    " + string.Format(hrefOthers, this.CurrentPageIndex + 1));
                    sb.Append("    <span>下页 ></span></a>\n");
                }
            }

            sb.Append("</div> \n"); //end MyPager-Mini

            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(sb.ToString()));

            #endregion
        }

        /// <summary>
        /// 当用户没有设置URL格式时生成默认的未分页网址。
        /// </summary>
        /// <returns></returns>
        private string GenUrl()
        {
            var dic = this.Page.Request.QueryString.ToDictionary();
            dic.Remove("page");
            dic.Remove("nocache");
            string qs = string.Empty;
            if (dic.Count > 0)
                qs = dic.ToQueryString() + "&";

            return this.Page.Request.ServerVariables["URL"] + "?" + qs;
        }

        #endregion
    }
}
