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
using System.Data;

namespace YX.Web.OfficialAccount
{
    public partial class OfficialAccountList : BaseSystemConfig
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
            this.MenuTag = "公众号管理";
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
                this.sg.Options.ColumnNames = new string[] { "站点", "类别", "字段", "value" };
                this.sg.Options.ColumnWidths = new int[] { 100, 110, 110, 0 };
                //this.sg.Options.ColumnProperty = new string[] { "屏蔽名称, autoHidden, style='text-align:left;'" };

                //绑定数据。
                this.DataBind();
            }
        }

        #endregion

        #region 绑定数据

        /// <summary>
        /// 绑定数据。
        /// </summary>
        public override void DataBind()
        {
            #region 查询参数处理

            txtvalue.Text = Request["value"] ?? "";
            drptype.SelectedValue = Request["type"] ?? "";

            if (!string.IsNullOrEmpty(Request["website"]))
            {
                byte? searchType = byte.Parse(Request["website"]);
                this.txtwebsite.Text = EnumDescription.GetFieldText((ApplicationTypes)searchType);
                this.hidwebsite.Value = Request["website"].ToString();
            }

            #endregion

            this.sg.DataSource = new List<string[]>();
            this.sg.PrimaryKeyData = new List<string>();

            this.sg.Options.PageSize = 10;

            //查询数据
            DataTable data = DBHelper.getData("select top 10 * from OfficialAccount order by createtime desc");
            foreach (DataRow row in data.Rows)
            {
                this.sg.DataSource.Add(new string[]{
                   row["website"]+"",
                   row["type"]+"",
                   row["column"]+"",
                   row["value"]+""
                });

                this.sg.PrimaryKeyData.Add(row["Id"] + "");
            }

            this.sg.DataBind();
        }

        #endregion

        #region 事件处理

        void sg_OnCreateRowControl(IList<Control> controls, object originalData)
        {
            controls.Add(new LiteralControl("<a href=\"javascript:void(0);\" onclick=\"openDialog(false, '{PKD}');\">修改</a>"));
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
                        if (commandArgument.Count() > 0)
                        {
                            string sql = "";
                            foreach (var item in commandArgument)
                            {
                                sql += "delete OfficialAccount where id=" + item + "; ";
                            }
                            DBHelper.execSql(sql);
                        }
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