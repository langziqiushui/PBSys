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
    public partial class PBConfigList : BaseSystemConfig
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //载入数据。
            this.LoadData();

            //引用css，JS。
            this.RefJavascript(false, "myTable.js");
            this.RefJavascript(false, "DataSelector.js");
            this.RefJavascript(false, "My97DatePicker4.8/WdatePicker.js");
            this.RefStyle(false, "myTable.css");
            WFUtil.RegisterStartupScript("pbConfigList.init();");

            //设置菜单标识。
            this.MenuTag = "日志管理";
            this.txtApplicationType.Attributes.Add("readonly", "readonly");
        }

        #region 变量

        /// <summary>
        /// 业务对象。
        /// </summary>
        private IServices.Glo.IPbconfigServices serviceLog = ServicesFactory.CreateGloPbconfigServices();

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
                this.sg.Options.ShowEditColumn = true;
                this.sg.Options.ShowModifyButton = this.sg.Options.ShowCreateNew = false;
                this.sg.Options.ColumnWidth_Edit = 90;
                this.sg.Options.AddSpaceColumn = false;
                this.sg.Options.ColumnNames = new string[] { "应用程序", "屏蔽类别", "对应ID", "屏蔽名称" };
                this.sg.Options.ColumnWidths = new int[] { 100, 110, 110,0};
                this.sg.Options.ColumnProperty = new string[] { "屏蔽名称, autoHidden, style='text-align:left;'" };

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
            var captions = new List<string> { "所有屏蔽", "软件", "新闻", "专题" };
            var urls = new List<string> { "/Admin/Glo/PBConfigList", "/Admin/Glo/PBConfigList?lt=0", "/Admin/Glo/PBConfigList?lt=1", "/Admin/Glo/PBConfigList?lt=2" };
            
            var sb = new StringBuilder();
            for (int i = 0; i < captions.Count; i++)
            {
                sb.AppendFormat(
                    "<li class=\"{0}\"><a href=\"{1}\">{2}</a></li>",
                    this.IsCurrentTab(urls, urls[i]) ? "active" : "",
                    urls[i],
                    captions[i]
                );
            }
            this.divTabStatus.InnerHtml = sb.ToString();


            this.drPbType.Items.Add(new ListItem(" - 请选择 - ", string.Empty));
            foreach (string _name in Enum.GetNames(typeof(PbTypes)))
            {
                PbTypes _enItem = (PbTypes)Enum.Parse(typeof(PbTypes), _name);
                this.drPbType.Items.Add(new ListItem(EnumDescription.GetFieldText((PbTypes)_enItem), ((int)_enItem).ToString()));
            }
            this.drPbType.SelectedIndex = 0;

            //this.drIsNormal.Items.Add(new ListItem(" - 站属性 - ", ""));
            //this.drIsNormal.Attributes.Add("filter", "false");
            //this.drIsNormal.Items.Add(new ListItem("正式站", "1"));
            //this.drIsNormal.Items.Add(new ListItem("镜像站", "0"));
            //this.drIsNormal.SelectedIndex = 0;
        }

        #endregion

        #region 绑定数据

        /// <summary>
        /// 绑定数据。
        /// </summary>
        public override void DataBind()
        {
            #region 查询参数处理

            int? searchKeywords = null;
            if (!string.IsNullOrEmpty(Request["k"]))
                searchKeywords = Convert.ToInt32(Request["k"]) ;
            this.txtKeyword.Text = searchKeywords.ToString();

            byte? searchPbType = null;
            if (!string.IsNullOrEmpty(Request["lt"]))
            {
                searchPbType = byte.Parse(Request["lt"]);
                this.drPbType.SelectedIndex = this.drPbType.GetIndex(Request["lt"]);
            }
            //站点。
            byte? searchApplicationType = null;
            if (!string.IsNullOrEmpty(Request["c"]))
            {
                searchApplicationType = byte.Parse(Request["c"]);
                this.txtApplicationType.Text = EnumDescription.GetFieldText((ApplicationTypes)searchApplicationType);
                this.hidApplicationType.Value = searchApplicationType.ToString();
            }

            #endregion

            this.sg.DataSource = new List<string[]>();
            this.sg.PrimaryKeyData = new List<string>();

            this.sg.Options.PageSize = 10;
            var data = this.serviceLog.GetData_Paging(
                this.sg.Options.PageSize, this.sg.PageIndex, ref this.sg.Options.NumRecord, searchApplicationType,
                searchPbType, searchKeywords
            );

            //"应用程序", "屏蔽类别", "主键", "站类别"，屏蔽名称
            foreach (var _entity in data)
            {
              
                //var saleStatusText = _entity.IsNormal ? "<span class='badge badge-danger badge-square graded'>正式站</span>" : "镜像站";
                this.sg.DataSource.Add(new string[]{
                   EnumDescription.GetFieldText((ApplicationTypes)_entity.ApplicationType),
                   EnumDescription.GetFieldText((PbTypes)_entity.PbType) ,                  
                    "<em>" + _entity.KeyData +"</em>",
                    _entity.Title
                    //,saleStatusText

                });

                this.sg.PrimaryKeyData.Add(_entity.ID.ToString());
            }

            this.sg.DataBind();
        }

        #endregion

        #region 事件处理

        void sg_OnCreateRowControl(IList<Control> controls, object originalData)
        {
            controls.Add(new LiteralControl("<a href=\"javascript:void(0);\" onclick=\"pbConfigList.beginAddOrModify(false, '{PKD}');\">修改</a>"));
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
                }
            });
        }

        void sg_OnCreateBottomControl(IList<Control> controls)
        {

        }

        #endregion
    }
}