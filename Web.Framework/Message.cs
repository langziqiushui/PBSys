using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;

namespace YX.Web.Framework
{
    /// <summary>
    /// 弹出对话框。
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// 输出消息。
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        public static void Info(string message, string title)
        {
            ShowModal("info", message, title, string.Empty);
        }

        /// <summary>
        /// 输出消息。
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="callback">回调JS</param>
        public static void Info(string message, string title, string callback)
        {
            ShowModal("info", message, title, callback);
        }


        /// <summary>
        /// 弹出警告框。
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        public static void Alert(string message, string title)
        {
            ShowModal("warning", message, title, string.Empty);
        }

        /// <summary>
        /// 弹出警告框。
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="callback">回调JS</param>
        public static void Alert(string message, string title, string callback)
        {
            ShowModal("warning", message, title, callback);
        }

        /// <summary>
        /// 弹出错误。
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        public static void Error(string message, string title)
        {
            ShowModal("error", message, title, string.Empty);
        }

        /// <summary>
        /// 弹出错误。
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="callback">回调JS</param>
        public static void Error(string message, string title, string callback)
        {
            ShowModal("error", message, title, callback);
        }

        /// <summary>
        /// 弹出警告框。
        /// </summary>
        /// <param name="type">类型(取值：取值为info,warning,error)</param>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="callback">回调函数</param>
        public static void ShowModal(string type, string message, string title, string callback)
        {
            message = message
                .Replace("\"", "&quot;")
                .Replace("\r\n", "<br/>")
                .Replace("\n", "<br/>")
                .Replace("\r", "<br/>")
                .Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
                ;

            title = title.Replace("\"", "&quot;");
            if (!string.IsNullOrEmpty(callback))
                callback = "function(){" + callback + "}";
            else
                callback = "null";

            WFUtil.RegisterStartupScript("common.__alert(\"" + message + "\", \"" + title + "\", \"" + type + "\", " + callback + ");");
        }

        /// <summary>
        /// 显示提示小弹框。
        /// </summary>
        /// <param name="controlID">停靠控件编号</param>
        /// <param name="message">消息</param>
        public static void ShowPop(string controlID, string message)
        {
            WFUtil.RegisterStartupScript("common.showPop('" + controlID + "', '" + message + "', true);");
        }
    }
}
