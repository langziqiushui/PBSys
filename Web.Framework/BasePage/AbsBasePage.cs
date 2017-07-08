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
    /// Web/M站抽象基类。
    /// </summary>
    public abstract class AbsBasePage : Page
    {
        /// <summary>
        /// 本页面css文件集合。
        /// </summary>
        private List<string> cssFiles = new List<string>();
        /// <summary>
        /// 本页面js文件集合。
        /// </summary>
        private List<string> jsFiles = new List<string>();
        /// <summary>
        /// 是否合并压缩JS。
        /// </summary>
        public bool? IsCompressionJS = null;
        /// <summary>
        /// 是否合并压缩CSS。
        /// </summary>
        public bool? IsCompressionCSS = null;

        #region 属性

        /// <summary>
        /// 获取 本页面css文件集合。
        /// </summary>
        public List<string> CssFiles
        {
            get { return this.cssFiles; }
        }

        /// <summary>
        /// 获取 本页面JS文件集合。
        /// </summary>
        public List<string> JSFiles
        {
            get { return this.jsFiles; }
        }        

        #endregion
    }
}
