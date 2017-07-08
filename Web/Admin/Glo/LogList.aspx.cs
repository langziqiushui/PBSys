using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Web.Framework;
using YX.Core;
using YX.Factory;
using YX.Component;
using YX.Domain;
using System.Text;

namespace YX.Web.Admin.Glo
{
    /// <summary>
    /// 日志管理。
    /// </summary>
    public partial class LogList : BaseSystemConfig
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                
                this.txtStartDate.Attributes.Add("onclick", WebKeys.JS_WdatePicker);
                this.txtEndDate.Attributes.Add("onclick", WebKeys.JS_WdatePicker);
            }

            //载入数据。
            this.LoadData();

            //引用css，JS。
            this.RefJavascript(false, "myTable.js");
            this.RefJavascript(false, "DataSelector.js");
            this.RefJavascript(false, "My97DatePicker4.8/WdatePicker.js");
            this.RefStyle(false, "myTable.css");
            WFUtil.RegisterStartupScript("logList.init();");

            //设置菜单标识。
            this.MenuTag = "日志管理";
            this.txtApplicationType.Attributes.Add("readonly", "readonly");
        }

        #region 变量

        /// <summary>
        /// 业务对象。
        /// </summary>
        private IServices.Glo.IDblogServices serviceLog = ServicesFactory.CreateGloDblogServices();

        #endregion

        #region 载入数据

        /// <summary>
        /// 载入数据。
        /// </summary>
        private void LoadData()
        {
            this.sg.OnCommand += new Component.CommandEventHandler(sg_OnCommand);
            this.sg.OnCreateRowControl += new CreateRowControlEventHandler(sg_OnCreateRowControl);
            this.sg.OnCreateBottomControl += new CreateControlEventHandler(sg_OnCreateBottomControl);

            if (!Page.IsPostBack)
            {
                this.sg.Options.ShowEditColumn = false;
                this.sg.Options.ShowModifyButton = this.sg.Options.ShowCreateNew = false;
                this.sg.Options.ColumnWidth_Edit = 90;
                this.sg.Options.AddSpaceColumn = false;
                this.sg.Options.ColumnNames = new string[] { "应用程序", "日志类别", "来源网址", "访问源/IP ","日志内容", "发生时间" };
                this.sg.Options.ColumnWidths = new int[] { 100, 80, 110, 110,  0, 60 };
                this.sg.Options.ColumnProperty = new string[] { "日志内容, autoHidden, style='text-align:left;'" };

                //绑定列表控件数据源。
                this.BindListControl();
                //绑定数据。
                this.DataBind();
            }
        }

        #endregion

        #region 绑定列表控件

        /// <summary> 
        /// 绑定列表控件数据源。 
        /// </summary> 
        private void BindListControl()
        {
            var captions = new List<string> { "所有日志","接口错误", "编辑后台", "移动M站", "PC站" };
            var urls = new List<string> { "/Admin/Glo/LogList", "/Admin/Glo/LogList?rs=4", "/Admin/Glo/LogList?rs=2", "/Admin/Glo/LogList?rs=0", "/Admin/Glo/LogList?rs=1", };

            
            foreach (var _name in Enum.GetNames(typeof(LogTypes)))
            {
                var _em = (LogTypes)Enum.Parse(typeof(LogTypes), _name, true);
                captions.Add(EnumDescription.GetFieldText(_em));
                urls.Add("/Admin/Glo/LogList?lt=" + ((int)_em).ToString());
            }
           
            var sb = new StringBuilder();
            for (int i = 0; i < captions.Count; i++)
            {
                sb.AppendFormat(
                    "<li class=\"{0}\"><a href=\"{1}\">{2}</a></li>",
                    this.IsCurrentTab(urls,urls[i]) ? "active" : "",
                    urls[i],
                    captions[i]
                );
            }
            this.divTabStatus.InnerHtml = sb.ToString();


            this.drRestype.Items.Add(new ListItem(" - 请选择 - ", string.Empty));
            foreach (string _name in Enum.GetNames(typeof(Restypes)))
            {
                Restypes _enItem = (Restypes)Enum.Parse(typeof(Restypes), _name);
                this.drRestype.Items.Add(new ListItem(EnumDescription.GetFieldText((Restypes)_enItem), ((int)_enItem).ToString()));
            }
            this.drRestype.SelectedIndex = 0;

            this.drLogTypes.Items.Add(new ListItem(" - 请选择 - ", string.Empty));
            foreach (string _name in Enum.GetNames(typeof(LogTypes)))
            {
                LogTypes _enItem = (LogTypes)Enum.Parse(typeof(LogTypes), _name);
                this.drLogTypes.Items.Add(new ListItem(EnumDescription.GetFieldText((LogTypes)_enItem), ((int)_enItem).ToString()));
            }
            this.drLogTypes.SelectedIndex = 0;
            
        }

        #endregion

        #region 绑定数据

        /// <summary>
        /// 绑定数据。
        /// </summary>
        public override void DataBind()
        {
            #region 查询参数处理

            string searchKeywords = null;
            if (!string.IsNullOrEmpty(Request["k"]))
                searchKeywords = Request["k"];
            this.txtKeyword.Text = searchKeywords;

            byte? searchLogType = null;
            if (!string.IsNullOrEmpty(Request["lt"]))
            {
                searchLogType = byte.Parse(Request["lt"]);
                this.drLogTypes.SelectedIndex = this.drLogTypes.GetIndex(Request["lt"]);
            }

            DateTime? searchStartDate = null;
            if (!string.IsNullOrEmpty(Request["sd"]))
                searchStartDate = DateTime.Parse(Request["sd"]);
            if (null == searchStartDate)
                searchStartDate = DateTime.Today.AddDays(-7);
            this.txtStartDate.Text = searchStartDate.Value.ToString("yyyy-MM-dd");

            DateTime? searchEndDate = null;
            if (!string.IsNullOrEmpty(Request["ed"]))
                searchEndDate = DateTime.Parse(Request["ed"]);
            if (null == searchEndDate)
                searchEndDate = DateTime.Today;
            searchEndDate = searchEndDate.Value.Date.AddDays(1).AddSeconds(-1);
            this.txtEndDate.Text = searchEndDate.Value.ToString("yyyy-MM-dd");

            string searchVersion = null;
            if (!string.IsNullOrEmpty(Request["v"]))
            {
                searchVersion = Request["v"];                
            }

            //站点。
            byte? searchApplicationType = null;
            if (!string.IsNullOrEmpty(Request["c"]))
            {
                searchApplicationType = byte.Parse(Request["c"]);
                this.txtApplicationType.Text = EnumDescription.GetFieldText((ApplicationTypes)searchApplicationType); 
                this.hidApplicationType.Value = searchApplicationType.ToString();
            }

            //类别。
            byte? searchRestype = null;
            if (!string.IsNullOrEmpty(Request["rs"]))
            {
                searchRestype = byte.Parse(Request["rs"]);
                this.drRestype.SelectedIndex = this.drRestype.GetIndex(Request["rs"]);
                
            }

            #endregion

            this.sg.DataSource = new List<string[]>();
            this.sg.PrimaryKeyData = new List<string>();

            this.sg.Options.PageSize = 10;
            var data = this.serviceLog.GetData_Paging(
                this.sg.Options.PageSize, this.sg.PageIndex, ref this.sg.Options.NumRecord, searchApplicationType,
                searchLogType, searchRestype, searchKeywords, searchStartDate, searchEndDate, searchVersion
            );

            //"应用程序", "日志类别", "来源网址", "IP", "日志内容", "发生时间" 
            foreach (var _entity in data)
            {
                var resurl = _entity.Resurl
                .IgnoreCaseReplace("iframe", "")
                .IgnoreCaseReplace("<br/>", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace("script", "")
                .Trim()
                ;
                
                this.sg.DataSource.Add(new string[]{
                   EnumDescription.GetFieldText((ApplicationTypes)_entity.ApplicationType)+ EnumDescription.GetFieldText((Restypes)_entity.Restype),
                   EnumDescription.GetFieldText((LogTypes)_entity.LogType),
                   string.Format(WebKeys.UrlFormat_Blank_Title, resurl, resurl, "点击跳转")
                    + "<BR/><em>浏览器：" + _entity.ResBrowser +"</em>",
                   string.Format(WebKeys.UrlFormat_Blank_Title, _entity.ResSource, _entity.ResSource, "点击跳转")
                    + "<BR/>" + 
                   string.Format(WebKeys.UrlFormat_Blank_Title, "http://www.ip138.com/ips138.asp?ip=" + _entity.ResIP + "&action=2", _entity.ResIP, "点击查询IP地址"),
                   CoreUtil.ConvertWinTagToWeb(_entity.LogContent),
                   _entity.CreateTime.ToString("yyyy-MM-dd HH:mm")
                });

                this.sg.PrimaryKeyData.Add(_entity.DBLogID.ToString());
            }

            this.sg.DataBind();
        }

        #endregion

        #region 事件处理

        void sg_OnCreateRowControl(IList<Control> controls, object originalData)
        {
        }

        bool sg_OnCommand(string commandName, params string[] commandArgument)
        {
            return WFUtil.UIDoTryCatch("处理列表请求", () =>
            {
                switch (commandName)
                {
                    case CommandNames.Delete:
                    case CommandNames.DeleteChecked:
                        this.serviceLog.Delete(commandArgument.ToIntArray());
                        this.RefurCurrPageWithoutPager();
                        break;
                    case "DeleteLT":
                       
                        if (!string.IsNullOrEmpty(Request.QueryString["lt"]))
                        {
                            this.serviceLog.Delete_LogType(byte.Parse(Request.QueryString["lt"]));
                            this.RefurCurrPageWithoutPager();
                        }
                        break;
                    case "DeleteAll":
                      
                        this.serviceLog.Delete_LogType(null);
                        this.RefurCurrPageWithoutPager();
                        break;
                }
            });
        }

        void sg_OnCreateBottomControl(IList<Control> controls)
        {
            var lt = Request.QueryString["lt"];
            if (!string.IsNullOrEmpty(lt))
            {
                var ltText = EnumDescription.GetFieldText((LogTypes)int.Parse(lt));
                var btDeleteLT = new Button() { ID = "btDeleteLT", Text = "删除(" + ltText + ")", CommandName = "DeleteLT" };
                btDeleteLT.Attributes.Add("onclick", "return confirm('温馨提示：您将要删除日志类别(" + ltText + ")，是否确定？');");
                controls.Add(btDeleteLT);
            }

            var btDeleteAll = new Button() { ID = "btDeleteAll", Text = "清空日志", CommandName = "DeleteAll" };
            btDeleteAll.Attributes.Add("onclick", "return confirm('温馨提示：您将要清空所有日志，是否确定？');");
            controls.Add(btDeleteAll);
        }

        #endregion
    }
}