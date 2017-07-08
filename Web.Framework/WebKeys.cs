using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;

namespace YX.Web.Framework
{
    /// <summary>
    /// web项目关键词。
    /// </summary>
    public static class WebKeys
    {
        /// <summary>
        /// 用于asp.net ajax请求显示的属性名。
        /// </summary>
        public const string RequestText = "_requestText";
        /// <summary>
        /// 应用程序最后一次错误键。
        /// </summary>
        public const string APPLICATION_LAST_ERROR = "APPLICATION_LAST_ERROR";
        /// <summary>
        /// 验证码Session：SESSION_VerifyCode_{0}
        /// </summary>
        public const string SESSION_VerifyCode = "SESSION_VerifyCode_{0}";
        /// <summary>
        /// 自定义对象Session键前缀：SESSION_PRINCIPAL_
        /// </summary>
        public const string SESSION_PRINCIPAL = "SESSION_PRINCIPAL_";
        /// <summary>
        /// APP平台Session键前缀：SESSION_APPPLAT
        /// </summary>
        public const string SESSION_APPPLAT = "SESSION_APPPLAT";


        /// <summary>
        /// 表示创建了持久性自动登录的Cookie：COOKIE_LOGIN_PERSISTENT
        /// </summary>
        public const string COOKIE_LOGIN_PERSISTENT = "COOKIE_LOGIN_PERSISTENT_YB2016";
        /// <summary>
        /// 表示创建了持久性保存用户名的Cookie：COOKIE_LOGIN_USERID
        /// </summary>
        public const string COOKIE_LOGIN_USERID = "COOKIE_LOGIN_USERID_YB2016";
        /// <summary>
        /// 表示创建了持久性保存子账户用户名的Cookie：COOKIE_LOGIN_CUSERID
        /// </summary>
        public const string COOKIE_LOGIN_CUSERID = "COOKIE_LOGIN_CUSERID_YB2016";
        /// <summary>
        /// 强制重新登录Cookie：COOKIE_LOGIN_Coercived
        /// </summary>
        public const string COOKIE_LOGIN_Coercived = "COOKIE_LOGIN_Coercived";
        /// <summary>
        /// M站请求APP来源的Cookie：COOKIE_MWEB_APPSOURCE
        /// </summary>
        public const string COOKIE_MWEB_APPSOURCE = "COOKIE_MWEB_APPSOURCE";


        /// <summary>
        /// 日历控件背景。
        /// </summary>
        public const string BG_City = "url(/App_Themes/images/city-icon.png) white no-repeat right;";
        /// <summary>
        /// 日历控件背景。
        /// </summary>
        public const string BG_Calendar = "url(/App_Themes/images/calendar.png) white no-repeat right;";
        /// <summary>
        /// 日历事件(使用最小日期为今天的限制)。
        /// </summary>
        public const string JS_WdatePicker_MinDate = "WdatePicker({doubleCalendar:true,dateFmt:'yyyy-MM-dd',minDate:'%y-%M-{%d+0}'})";
        /// <summary>
        /// 日历事件(使用最大日期为今天的限制)。
        /// </summary>
        public const string JS_WdatePicker_MaxDate = "WdatePicker({doubleCalendar:true,dateFmt:'yyyy-MM-dd HH:mm:ss',maxDate:'%y-%M-{%d+0}'})";
        /// <summary>
        /// 日历事件(适用于选择生日)。
        /// </summary>
        public const string JS_WdatePicker_BirthDate = "WdatePicker({doubleCalendar:false,dateFmt:'yyyy-MM-dd',startDate:'{%y-20}-%M-01'})";
        /// <summary>
        /// 日历事件(适用于选择生日)。
        /// </summary>
        public const string JS_WdatePicker_BirthDate_40 = "WdatePicker({doubleCalendar:false,dateFmt:'yyyy-MM-dd',startDate:'{%y-40}-%M-01'})";

        /// <summary>
        /// 日历事件时间格式。
        /// </summary>
        public const string JS_WdatePicker_Time = "WdatePicker({doubleCalendar:true,dateFmt:'yyyy-MM-dd HH:mm'})";


        /// <summary>
        ///日历事件(最大值日历框参考最小值日历，需要用对应的日历控件ID替换{txtMinDateID})。
        /// </summary>
        public const string JS_WdatePicker_TimeReference = "WdatePicker({doubleCalendar:true,dateFmt:'yyyy-MM-dd HH:mm',minDate:'#F{$dp.$D(\\'{txtMinDateID}\\',{d:0})}',maxDate:'#F{$dp.$D(\\'{txtMinDateID}\\',{d:3600})}'})";
        /// <summary>
        /// 日历事件。
        /// </summary>
        public const string JS_WdatePicker = "WdatePicker({doubleCalendar:true,dateFmt:'yyyy-MM-dd'})";
        /// <summary>
        /// 日历事件（单个日期框）
        /// </summary>
        public const string JS_WdatePickerSingle = "WdatePicker({doubleCalendar:false,dateFmt:'yyyy-MM-dd'})";
        /// <summary>
        /// 日历事件（单个日期框）
        /// </summary>
        public const string JS_WdatePickerSingle_MinDate = "WdatePicker({doubleCalendar:false,dateFmt:'yyyy-MM-dd',minDate:'%y-%M-{%d+0}'})";
        /// <summary>
        /// 日历事件(最大值日历框参考最小值日历，需要用对应的日历控件ID替换{txtMinDateID})。
        /// </summary>
        public const string JS_WdatePickerReference = "WdatePicker({doubleCalendar:true,dateFmt:'yyyy-MM-dd',minDate:'#F{$dp.$D(\\'{txtMinDateID}\\',{d:0})}',maxDate:'#F{$dp.$D(\\'{txtMinDateID}\\',{d:3600})}'})";

        /// <summary>
        /// url格式：{0}为href，{1}为text
        /// </summary>
        public const string UrlFormat_Blank = "<a target=\"_blank\" href=\"{0}\">{1}</a>";
        /// <summary>
        /// url格式：{0}为href，{1}为text，{2}为title
        /// </summary>
        public const string UrlFormat_Blank_Title = "<a target=\"_blank\" title=\"{2}\" href=\"{0}\">{1}</a>";
        /// <summary>
        /// url格式：{0}为href，{1}为text，{2}为title，{3}为class
        /// </summary>
        public const string UrlFormat_Blank_Title_Class = "<a target=\"_blank\" title=\"{2}\" class=\"{3}\" href=\"{0}\">{1}</a>";

        /// <summary>
        /// 文件上传基础目录。
        /// </summary>
        public const string UPLOADPATH_BASE = "/uploads";
        /// <summary>
        /// 临时图片上传目录：/uploads/temp
        /// </summary>
        public const string UPLOADPATH_TEMP = "/uploads/temp";
        
    }
}
