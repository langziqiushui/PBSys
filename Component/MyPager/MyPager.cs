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
    [ToolboxData("<{0}:MyPager runat=server></{0}:MyPager>")]
    [Designer(typeof(MyPagerDesigner))]
    public class MyPager : WebControl
    {
        #region 属性

        /// <summary>
        /// 获取或设置 每页显示的记录数。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager")]
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
        [Category("MyPager")]
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
        [Category("MyPager")]
        [DefaultValue("1")]
        [Description("获取或设置 当前页码")]
        [Localizable(true)]
        public int CurrentPageIndex
        {
            get
            {
                if (null != this.ViewState["CurrentPageIndex"])
                    return Convert.ToInt32(this.ViewState["CurrentPageIndex"]);

                var pageIndex = 1;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
                    pageIndex = this.DecryptPageIndex(HttpContext.Current.Request.QueryString["page"].Trim());
                this.ViewState["CurrentPageIndex"] = pageIndex;
                return pageIndex;
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
        [Category("MyPager")]
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
        /// 获取或设置 是否显示输入页码的文本框。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager")]
        [DefaultValue("true")]
        [Description("获取或设置 是否显示输入页码的文本框")]
        [Localizable(true)]
        public bool ShowInputBox
        {
            get
            {
                if (null == ViewState["ShowInputBox"])
                    return true;
                return Convert.ToBoolean(ViewState["ShowInputBox"]);
            }
            set
            {
                this.ViewState["ShowInputBox"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 链接格式。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager")]
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
        /// 获取或设置 页面锚点标记。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager-Mini")]
        [Description("获取或设置 页面锚点标记")]
        [Localizable(true)]
        public string AnchorTag
        {
            get
            {
                if (null == ViewState["AnchorTag"])
                    return null;
                return ViewState["AnchorTag"].ToString();
            }
            set
            {
                this.ViewState["AnchorTag"] = value;
            }
        }


        /// <summary>
        /// 获取或设置 是否显示回到顶部的按钮。
        /// </summary>
        [Bindable(true)]
        [Category("MyPager")]
        [DefaultValue("false")]
        [Description("获取或设置 是否显示回到顶部的按钮")]
        [Localizable(true)]
        public bool ShowToTop
        {
            get
            {
                if (null == ViewState["ShowToTop"])
                    return false;
                return Convert.ToBoolean(ViewState["ShowToTop"]);
            }
            set
            {
                this.ViewState["ShowToTop"] = value;
            }
        }

        #endregion

        #region 重载基类       

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RenderPager();
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("div");
            writer.WriteAttribute("id", this.ClientID);
            writer.Write(">");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag("div");
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
        public void RenderPager()
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

            //定位锚点。
            var anchorText = string.Empty;
            if (!string.IsNullOrEmpty(this.AnchorTag))
                anchorText = "#" + this.AnchorTag;           

            string _urlFormat = this.UrlFormat;
            if (string.IsNullOrEmpty(_urlFormat))
                _urlFormat = this.GenUrl() + "page={0}" + anchorText;
            string hrefOthers = "<a href=\"" + _urlFormat + "\">";

            #endregion

            #region 绘制分页按钮

            var urlPrefix = this.GenUrl();

            sb = new StringBuilder();
            sb.Append("\n<div class=\"myTablePager\">\n");

            if (pns.Count > 0)
            {
                //首页，上一页 按钮。
                if (this.CurrentPageIndex == 1)
                {
                    sb.Append("    <a class=\"firstPageGray\"></a>\n");
                    sb.Append("    <a class=\"prevPageGray\"></a>\n");
                }
                else
                {
                    sb.Append("<a href=\"" + string.Format(_urlFormat, 1) + "\" class=\"firstPage\"></a>\n");
                    sb.Append("<a href=\"" + string.Format(_urlFormat, this.CurrentPageIndex -1) + "\" class=\"prevPage\"></a>\n");
                }

                //页码。
                foreach (int _pn in pns)
                {
                    if (_pn == this.CurrentPageIndex)
                    {
                        sb.Append("    <a class=\"currentPage\" href=\"" + string.Format(_urlFormat, _pn) + "\">" + _pn.ToString() + "</a>\n");
                    }
                    else
                    {
                        sb.Append("    <a href=\"" + string.Format(_urlFormat, _pn) + "\">" + _pn.ToString() + "</a>\n");
                    }
                }

                //下一页、尾页。
                if (this.CurrentPageIndex == pageCount)
                {
                    sb.Append("    <a class=\"nextPageGray\"></a>\n");
                    sb.Append("    <a class=\"lastPageGray\"></a>\n");
                }
                else
                {
                    sb.Append("    <a href=\"" + string.Format(_urlFormat, this.CurrentPageIndex + 1) + "\" class=\"nextPage\"></a>\n");
                    sb.Append("    <a href=\"" + string.Format(_urlFormat, pageCount) + "\" class=\"lastPage\"></a>\n");
                }
            }

            //回到顶部。
            if (this.ShowToTop)
            {
                sb.Append("    <a href=\"#\"><span class=\"MyPager_top\"></span></a>\n");
            }

            sb.Append("<p class=\"pagerInfos\">");
            sb.Append("<strong>" + this.CurrentPageIndex + "</strong>/" + pageCount + "页 共" + this.RecordCount + "条数据&nbsp;");
            if (this.ShowInputBox)
                sb.Append("转到 <input id=\"txtPageNum_" + this.ClientID + "\" class=\"pagerNum\" type=\"text\"> 页 <input onclick=\"mt.goPager('" + _urlFormat + "', 'txtPageNum_" + this.ClientID + "', " + pageCount + ");\" class=\"pagerOk\" type=\"button\" value=\"确定\" />&nbsp;");
            sb.Append("</p>");
            sb.Append("</div> \n"); //end MyPager

            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(sb.ToString()));

            #endregion
        }

        /// <summary>
        /// 对页码进行编码。
        /// </summary>
        /// <param name="pageIndex">需要编码的页码</param>
        /// <returns></returns>
        public string EncryptPageIndex(int pageIndex)
        {
            return pageIndex.ToString();

            //用一个简单的算法混淆。
            pageIndex = pageIndex * 7 + 368;
            return HttpUtility.UrlEncode(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pageIndex.ToString())));
        }

        /// <summary>
        /// 对页码进行解码。
        /// </summary>
        /// <param name="pageIndex">待解码的页面</param>
        /// <returns></returns>
        private int DecryptPageIndex(string pageIndex)
        {
            return int.Parse(pageIndex);

            int _p = 0;
            try
            {
                _p = int.Parse(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(pageIndex))));
                _p = (_p - 368) / 7;
            }
            catch (Exception)
            {
                AppException.ThrowWaringException("对不起，页码不正确或被恶意篡改！");
            }

            return _p;
        }

        /// <summary>
        /// 当用户没有设置URL格式时生成默认的未分页网址。
        /// </summary>
        /// <returns></returns>
        private string GenUrl()
        {
            var dic = HttpContext.Current.Request.QueryString.ToDictionary();
            dic.Remove("page");
            dic.Remove("nocache");
            string qs = string.Empty;
            if (dic.Count > 0)
                qs = dic.ToQueryString() + "&";

            return HttpContext.Current.Request.ServerVariables["URL"] + "?" + qs;
        }

        #endregion
    }
}
