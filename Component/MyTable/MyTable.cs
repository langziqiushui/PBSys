using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;

using YX.Core;

namespace YX.Component
{
    /// <summary>
    /// 自定义Table数据控件。
    /// </summary>
    [Designer(typeof(MyTableDesigner))]
    [ToolboxData("<{0}:MyTable runat=server></{0}:MyTable>")]
    public class MyTable : Table, INamingContainer
    {
        #region 变量

        /// <summary>
        /// MyTable提供数据的配置参数对象。
        /// </summary>
        private MTOptions _options = null;
        /// <summary>
        /// 与数据源匹配的主键数据。
        /// </summary>
        private IList<string> _primaryKeyData = null;
        /// <summary>
        /// 解析列配置参数的正则式。
        /// </summary>
        private const string PATTERN_CP = @"'([^']+)'";
        /// <summary>
        /// 解析列配置参数列直接样式设置的正则式。
        /// </summary>
        private const string PATTERN_CELL_STYLE = "<span style=(?:\"|')(.*?)(?:\"|')>(.*?)</span>";
        /// <summary>
        /// 解析列配置参数列直接样式设置的正则式。
        /// </summary>
        private const string PATTERN_CELL_CLASS = "<span class=(?:\"|')(.*?)(?:\"|')>(.*?)</span>";
        /// <summary>
        /// 用于保存当前选择行主键数据的隐藏域的ID。
        /// </summary>
        private string hidPrimaryKeyDataID = null;
        /// <summary>
        /// 当前的命令名。
        /// </summary>
        private string currentCommandName = null;
        /// <summary>
        /// 最后一次处于选择状态的数据量。
        /// </summary>
        private int numCheckedPrimaryKeyData = 0;
        /// <summary>
        /// 分页控件。
        /// </summary>
        private MyPager myPager = null;

        #endregion

        #region 自定义事件

        /// <summary>
        /// 当为某行创建编辑按钮时(已自动为按钮添加事件)。
        /// </summary>
        public event CreateRowControlEventHandler OnCreateRowControl;
        /// <summary>
        /// 当创建底部按钮时(已自动为按钮添加事件)。
        /// </summary>
        public event CreateControlEventHandler OnCreateBottomControl;
        /// <summary>
        /// 当服务器端控件触发事件时。
        /// </summary>
        public event CommandEventHandler OnCommand;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置 SimpleGrid提供数据的配置参数对象。
        /// </summary>
        public MTOptions Options
        {
            get
            {
                if (null == this._options)
                {
                    this._options = ViewState["Options"] as MTOptions;
                    if (null == this._options)
                        this._options = new MTOptions();
                }

                return this._options;
            }
            private set
            {
                ViewState["Options"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 分页页码。
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (null == ViewState["PageIndex"])
                    return 1;

                return Convert.ToInt32(ViewState["PageIndex"]);
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 要绑定的数据源。
        /// </summary>
        public IList<string[]> DataSource
        {
            get
            {
                return ViewState["DataSource"] as IList<string[]>;
            }
            set
            {
                ViewState["DataSource"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 原始数据集合。
        /// </summary>
        public ArrayList OriginalData
        {
            get
            {
                return ViewState["OriginalData"] as ArrayList;
            }
            set
            {
                ViewState["OriginalData"] = value;
            }
        }

        /// <summary>
        /// 获取或设置 数据源长度。
        /// </summary>
        private int DataSourceLength
        {
            get
            {
                return Convert.ToInt32(ViewState["DataSourceLength"]);
            }
            set
            {
                ViewState["DataSourceLength"] = value;
            }
        }


        /// <summary>
        /// 获取或设置 当前是否为最后一页。
        /// </summary>
        private bool IsLastPage
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsLastPage"]);
            }
            set
            {
                ViewState["IsLastPage"] = value;
            }
        }



        /// <summary>
        /// 获取或设置 与数据源匹配的主键数据。
        /// </summary>
        public IList<string> PrimaryKeyData
        {
            get
            {
                if (null == this._primaryKeyData)
                    this._primaryKeyData = ViewState["PrimaryKeyData"] as IList<string>;
                return this._primaryKeyData;
            }
            set
            {
                this._primaryKeyData = value;
            }
        }

        #endregion

        #region 重写基类事件

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.hidPrimaryKeyDataID = "hidPrimaryKeyData_" + this.ClientID;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "init_sg_clientID956", "sgParentClientID94893 = '" + this.ClientID.Substring(0, this.ClientID.LastIndexOf(this.ID)) + "';", true);
            this.myPager = new MyPager();
            this.PageIndex = this.myPager.CurrentPageIndex;

            this.Page.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                this.DataBind();
        }

        #endregion

        #region 绑定数据

        /// <summary>
        /// 获取所有处于选择状态的数据行的主键数据。
        /// </summary>
        /// <returns></returns>
        public string[] GetCheckedPrimaryKeyData()
        {
            IList<string> data = new List<string>();
            foreach (TableRow _row in this.Rows)
            {
                if (!string.IsNullOrEmpty(_row.ID) && _row.ID.IndexOf("trSDI") == 0)
                {
                    CheckBox chk = _row.Cells[0].Controls[0] as CheckBox;
                    if (null != chk && chk.Checked)
                        data.Add(((HiddenField)_row.Cells[0].Controls[1]).Value);
                }
            }

            return data.ToArray();
        }

        /// <summary>
        /// 获取 当前选择行的主键数据。
        /// </summary>
        public string SelectedPrimaryKeyData
        {
            get { return ((HiddenField)this.FindControl(this.hidPrimaryKeyDataID)).Value; }
        }

        /// <summary>
        /// 绑定数据。
        /// </summary>
        public override void DataBind()
        {
            if (null == this.DataSource)
                return;

            //验证参数设置。
            this.ValidateOptions();

            //设置数据源长度。
            this.DataSourceLength = this.DataSource.Count;
            //验证参配置数。
            this.Options.Validate();
            //保存到状态包中。
            this.Options = this.Options;
            //清除以前创建的。
            this.Rows.Clear();
            //table设置。
            this.CssClass = "myTable123";
            if (this.Options.Width > 0)
                this.Style.Add("width", this.Options.Width.ToString() + "px");

            //计算列数。
            int nc = this.Options.ColumnNames.Length + 1;
            if (this.Options.ShowEditColumn)
                nc += 1;
            if (this.Options.AddSpaceColumn)
                nc += 1;

            #region 绘制Column--------------------------------------------------------------//

            TableRow rowColumn = new TableRow() { CssClass = "myTable123_rc", Visible = this.Options.IsShowCaptionRow };
            this.Rows.Add(rowColumn);

            //加入选择列。
            var cellChecked = new TableCell();
            cellChecked.Controls.Add(new LiteralControl("<nobr>选择</nobr>"));
            cellChecked.Style.Add("width", this.Options.ColumnWidth_Check + "px");
            cellChecked.Attributes.Add("__myWidth", this.Options.ColumnWidth_Check.ToString());
            rowColumn.Cells.Add(cellChecked);
            if (!this.Options.ShowCheckColumn)
                cellChecked.Style.Add("display", "none");

            //创建用于保存当前选择行主键数据的隐藏域。
            if (null == cellChecked.FindControl(this.hidPrimaryKeyDataID))
                cellChecked.Controls.Add(new HiddenField() { ID = this.hidPrimaryKeyDataID, ClientIDMode = System.Web.UI.ClientIDMode.Static });

            //加入数据列。
            for (int i = 0; i < this.Options.ColumnNames.Length; i++)
            {
                TableCell cellColumn = new TableCell();
                if (this.Options.ColumnWidths[i] > 0)
                {
                    cellColumn.Style.Add("width", this.Options.ColumnWidths[i] + "px");
                    cellColumn.Attributes.Add("__myWidth", this.Options.ColumnWidths[i].ToString());
                }
                else
                {
                    cellColumn.ID = "tdNoSize";
                }
                cellColumn.Text = "<nobr>" + this.Options.ColumnNames[i] + "</nobr>";
                rowColumn.Cells.Add(cellColumn);
            }

            //加入编辑列。
            if (this.Options.ShowEditColumn)
            {
                var cellEditColumn = new TableCell() { Text = this.Options.EditColumnCaption, Width = this.Options.ColumnWidth_Edit };
                cellEditColumn.Style.Add("width", this.Options.ColumnWidth_Edit + "px");
                cellEditColumn.Attributes.Add("__myWidth", this.Options.ColumnWidth_Edit.ToString());
                rowColumn.Cells.Add(cellEditColumn);
            }

            #endregion

            #region 绘制数据行--------------------------------------------------------------//

            if (null != this.DataSource && this.DataSource.Count > 0)
            {
                //验证数据和主键数据。
                if (null != this.PrimaryKeyData && this.DataSource.Count != this.PrimaryKeyData.Count)
                    throw new AppException("对不起，数据源和主键数据长度不一致。", ExceptionLevels.Warning);

                //保存到状态包中。
                ViewState["DataSource"] = this.DataSource;
                ViewState["PrimaryKeyData"] = this.PrimaryKeyData;

                int dataIndex = 0;
                foreach (string[] _data in this.DataSource)
                {
                    //创建数据行。
                    TableRow rowData = new TableRow() { ID = "trSDI" + dataIndex.ToString() };
                    rowData.Attributes.Add("_mValue", (dataIndex % 2).ToString());
                    this.Rows.Add(rowData);

                    //加入选择列。
                    cellChecked = new TableCell();
                    rowData.Cells.Add(cellChecked);
                    CheckBox chk = new CheckBox() { ID = "chkChecked" + dataIndex.ToString() };
                    chk.Attributes.Add("__primarykeydata", PrimaryKeyData[dataIndex]);
                    cellChecked.Controls.Add(chk);

                    //添加主键数据。
                    if (null != this.PrimaryKeyData)
                        cellChecked.Controls.Add(new HiddenField() { ID = "hidSDI" + dataIndex.ToString(), Value = PrimaryKeyData[dataIndex] });
                    if (!this.Options.ShowCheckColumn)
                        cellChecked.Style.Add("display", "none");

                    //加入数据列。
                    for (int i = 0; i < _data.Length; i++)
                    {
                        if (null == _data[i])
                            _data[i] = string.Empty;

                        TableCell cellData = new TableCell();
                        rowData.Cells.Add(cellData);

                        cellData.Text = _data[i];
                    }

                    //加入编辑列。
                    if (this.Options.ShowEditColumn)
                    {
                        TableCell cellData = new TableCell();
                        rowData.Cells.Add(cellData);
                        cellData.CssClass = "myTable123_editColumn";

                        //编辑按钮。
                        if (this.Options.ShowModifyButton)
                        {
                            var btEdit = new LinkButton() { ID = "btSEdit" + dataIndex.ToString(), Text = "修改", ToolTip = "修改数据", CausesValidation = false, CommandName = CommandNames.Modify };
                            btEdit.Click += new EventHandler(ItemButtonClick);
                            cellData.Controls.Add(btEdit);
                        }

                        //删除按钮。
                        if (this.Options.ShowDeleteButton)
                        {
                            cellData.Controls.Add(this.CreateWhiteSpace());
                            var btDelete = new LinkButton() { ID = "btSDel" + dataIndex.ToString(), Text = "删除", ToolTip = "删除数据", CausesValidation = false, CommandName = CommandNames.Delete };
                            btDelete.Attributes.Add("onclick", "return confirm('您将要删除当前数据，是否确定？');");
                            btDelete.Click += new EventHandler(ItemButtonClick);
                            cellData.Controls.Add(btDelete);
                        }

                        //添加自定义的按钮。
                        if (null != this.OnCreateRowControl)
                        {
                            IList<Control> controls = new List<Control>();
                            this.OnCreateRowControl(controls, null != this.OriginalData && this.OriginalData.Count > 0 ? this.OriginalData[dataIndex] : null);

                            //加入控件。
                            int rwIndex = 0;
                            foreach (Control _control in controls)
                            {
                                cellData.Controls.Add(this.CreateWhiteSpace());
                                if (string.IsNullOrEmpty(_control.ID))
                                    _control.ID = "_colOBItem_" + rwIndex.ToString() + "_" + dataIndex.ToString();
                                cellData.Controls.Add(_control);

                                //如果是按钮控件。
                                if (_control is IButtonControl)
                                {
                                    ((IButtonControl)_control).CausesValidation = false;
                                    ((IButtonControl)_control).Click += new EventHandler(ItemButtonClick);
                                }
                                else if (_control is LiteralControl)
                                {
                                    ((LiteralControl)_control).Text = ((LiteralControl)_control).Text.Replace("{PKD}", PrimaryKeyData[dataIndex]).Replace("{PINDEX}", dataIndex.ToString());
                                }

                                rwIndex += 1;
                            }
                        }
                    }

                    //加入空白列。
                    if (this.Options.AddSpaceColumn)
                        rowData.Cells.Add(new TableCell());

                    dataIndex += 1;
                }
            }

            #endregion

            #region 解析列配置参数--------------------------------------------------------------//

            if (null != this.Options.ColumnProperty)
            {
                foreach (string _cp in this.Options.ColumnProperty)
                {
                    string[] ps = _cp.Split(',');
                    string cn = ps[0];
                    string style = string.Empty;
                    string cssClass = string.Empty;
                    bool pop = false;
                    bool autoHidden = false;
                    bool overflowHidden = false;

                    for (int i = 1; i < ps.Length; i++)
                    {
                        if (ps[i].Trim().IndexOf("style", StringComparison.OrdinalIgnoreCase) == 0)
                            style = Regex.Match(ps[i], PATTERN_CP).Groups[1].Value;
                        else if (ps[i].Trim().IndexOf("class", StringComparison.OrdinalIgnoreCase) == 0)
                            cssClass = Regex.Match(ps[i], PATTERN_CP).Groups[1].Value;
                        else if (ps[i].Trim().IndexOf("pop", StringComparison.OrdinalIgnoreCase) == 0)
                            pop = true;
                        else if (ps[i].Trim().IndexOf("autoHidden", StringComparison.OrdinalIgnoreCase) == 0)
                            autoHidden = true;
                        else if (ps[i].Trim().IndexOf("overflowHidden", StringComparison.OrdinalIgnoreCase) == 0)
                            overflowHidden = true;
                    }

                    //查找当前设置了属性的列的索引号。
                    int columnIndex = -1;
                    for (int i = 0; i < rowColumn.Cells.Count; i++)
                    {
                        string cellText = Regex.Replace(rowColumn.Cells[i].Text, "<.*?>", string.Empty).Trim();
                        if (cellText == cn)
                        {
                            columnIndex = i;
                            break;
                        }
                    }

                    //设置当前列下面所有cell的属性。
                    if (columnIndex > -1)
                    {
                        int rowIndex = 0;
                        foreach (TableRow _row in this.Rows)
                        {
                            if (!string.IsNullOrEmpty(_row.ID) && _row.ID.IndexOf("trSDI") == 0)
                            {
                                //设置style。
                                if (!string.IsNullOrEmpty(style))
                                {
                                    foreach (string _css in style.Split(';'))
                                    {
                                        if (_css.Trim().Length > 0)
                                        {
                                            string[] ns = _css.Split(':');
                                            _row.Cells[columnIndex].Style.Add(ns[0], ns[1]);
                                        }
                                    }
                                }

                                //设置class。
                                if (!string.IsNullOrEmpty(cssClass))
                                    _row.Cells[columnIndex].CssClass += " " + cssClass;

                                //设置pop。
                                if (pop)
                                {
                                    string objID = this.ClientID + "_{0}" + rowIndex.ToString() + "_" + columnIndex.ToString();
                                    string scripts = "mt.showPop(this);";
                                    if (!string.IsNullOrEmpty(_row.Cells[columnIndex].Text))
                                        _row.Cells[columnIndex].Text = "<ol>" + _row.Cells[columnIndex].Text + "</ol><span class=\"myTable123_link234\" id=\"" + string.Format(objID, "lnkS") + "\" onmouseover=\"" + scripts + "\">显示</span>";
                                    else
                                        _row.Cells[columnIndex].Text = "无";
                                }

                                //自动隐藏多余文本。
                                if (autoHidden)
                                    _row.Cells[columnIndex].Text = "<div class=\"myTable123_ah\" style=\"width:1px;\">" + _row.Cells[columnIndex].Text + "</div>";

                                if (overflowHidden)
                                    _row.Cells[columnIndex].Text = "<div style=\"width:" + this.Options.ColumnWidths[columnIndex - 1].ToString() + "px;\" class=\"myTable123_overflowHidden\">" + _row.Cells[columnIndex].Text + "</div>";
                            }

                            rowIndex += 1;
                        }
                    }
                }
            }

            #endregion

            #region 如果没有数据--------------------------------------------------------//

            if (this.DataSource.Count == 0)
            {
                TableRow rowNone = new TableRow() { CssClass = "myTable123_rb" };
                this.Rows.Add(rowNone);
                TableCell cellNone = new TableCell() { ColumnSpan = nc, Text = "<div class=\"myTable123_noData\"><i class=\"fa  fa-search\"></i> 当前没有任何数据！</div>" };
                rowNone.Cells.Add(cellNone);
            }

            #endregion

            #region 绘制Bottom--------------------------------------------------------------//

            TableCell footerCellLeft = null;
            TableCell footerCllRight = null;

            if (this.Options.ShowFooter)
            {
                TableRow rowBottom = new TableRow() { CssClass = "myTable123_rb" };
                this.Rows.Add(rowBottom);
                TableCell cellBottom = new TableCell() { ColumnSpan = nc };
                rowBottom.Cells.Add(cellBottom);

                //嵌套一个子表--------------------------------------------------------------//
                Table tabBottom = new Table() { CellPadding = 0, CellSpacing = 0 };
                tabBottom.Style.Add("width", "100%;");
                cellBottom.Controls.Add(tabBottom);
                TableRow rowBottom2 = new TableRow();
                tabBottom.Rows.Add(rowBottom2);

                //加入左边。
                footerCellLeft = new TableCell() { CssClass = "myTable123_action", ID = "footerCellLeft" };
                rowBottom2.Cells.Add(footerCellLeft);

                //加入右边。
                footerCllRight = new TableCell() { CssClass = "myTable123_pager", ID = "footerCllRight" };
                rowBottom2.Cells.Add(footerCllRight);

                //加入全选。    
                if (this.Options.ShowCheckColumn)
                {
                    CheckBox chkAll = new CheckBox() { Text = "全选", ID = "chkSAll" };
                    chkAll.Attributes.Add("onclick", "mt.checkAll(this, '" + this.ClientID + "');");
                    footerCellLeft.Controls.Add(chkAll);
                }

                //加入删除选择数据的按钮。
                if (this.Options.ShowCheckColumn && this.Options.ShowDeleteCheckedButton)
                {
                    LiteralControl space = this.CreateWhiteSpace();
                    space.Text = "&nbsp;&nbsp;&nbsp;&nbsp;";
                    footerCellLeft.Controls.Add(space);
                    Button btSDelChecked = new Button() { ID = "btSDelChecked", Text = "- 删除选择", CommandName = CommandNames.DeleteChecked, CausesValidation = false, CssClass = "myTable123_button", ToolTip = "删除所有处于选择状态的数据" };
                    btSDelChecked.Attributes.Add("onclick", "return confirm('您将要删除所有选择的数据，是否确定？');");
                    btSDelChecked.Click += new EventHandler(ButtonClick);
                    footerCellLeft.Controls.Add(btSDelChecked);
                }

                //加入删除所有数据的按钮。
                if (this.Options.ShowDeleteAllButton)
                {
                    LiteralControl space = this.CreateWhiteSpace();
                    space.Text = "&nbsp;&nbsp;&nbsp;&nbsp;";
                    footerCellLeft.Controls.Add(space);
                    Button btSDelAll = new Button() { ID = "btSDelAll", Text = Options.DeleteAllButtonText, Width = 100, CommandName = CommandNames.DeleteAll, CausesValidation = false, CssClass = "myTable123_button", ToolTip = "删除所有数据" };
                    btSDelAll.Attributes.Add("onclick", "return confirm('您将要清空所有数据，请慎重操作，您确定要继续吗？');");
                    btSDelAll.Click += new EventHandler(ButtonClick);
                    footerCellLeft.Controls.Add(btSDelAll);
                }

                //加入创建。
                if (this.Options.ShowCreateNew)
                {
                    footerCellLeft.Controls.Add(this.CreateWhiteSpace());
                    Button btCreateNew = new Button() { ID = "btCreateNew", Text = this.Options.CreateNewButtonText, CommandName = CommandNames.CreateNew, CausesValidation = false, CssClass = "myTable123_button" };
                    btCreateNew.Click += new EventHandler(ButtonClick);
                    footerCellLeft.Controls.Add(btCreateNew);
                }

                //添加自定义的按钮。
                IList<Control> customControls = null;
                if (null != this.OnCreateBottomControl)
                {
                    customControls = new List<Control>();
                    this.OnCreateBottomControl(customControls);
                    int index = 0;

                    //加入控件。
                    foreach (Control _control in customControls)
                    {
                        footerCellLeft.Controls.Add(this.CreateWhiteSpace());
                        if (string.IsNullOrEmpty(_control.ID))
                            _control.ID = "colBottom_" + index.ToString();
                        footerCellLeft.Controls.Add(_control);

                        //如果是按钮控件。
                        if (_control is IButtonControl)
                        {
                            ((WebControl)_control).CssClass = "myTable123_button";
                            ((IButtonControl)_control).CausesValidation = false;
                            ((IButtonControl)_control).Click += new EventHandler(ButtonClick);
                        }

                        index += 1;
                    }
                }


                #region 绘制分页--------------------------------------------------------------//

                int pageCount = 0;
                if (this.Options.NumRecord > 0)
                {
                    pageCount = this.Options.NumRecord / this.Options.PageSize;
                    if (this.Options.NumRecord % this.Options.PageSize > 0)
                        pageCount += 1;
                }

                if (this.Options.UsePages && pageCount > 1)
                {
                    this.myPager.RecordCount = this.Options.NumRecord;
                    this.myPager.ShowInputBox = true;
                    this.myPager.ButtonCount = 5;
                    this.myPager.PageSize = this.Options.PageSize;
                    this.myPager.CurrentPageIndex = this.PageIndex;
                    this.myPager.RenderPager();

                    footerCllRight.Controls.Add(myPager);
                }

                #endregion

                //智能判断是否显示Bottom。
                rowBottom.Visible =
                    (this.Options.ShowCheckColumn || this.Options.ShowDeleteCheckedButton) ||
                    this.Options.ShowDeleteAllButton ||
                    this.Options.ShowCreateNew ||
                    (null != customControls && customControls.Count > 0) ||
                    (this.Options.UsePages && pageCount > 1)
                ;
            }

            #endregion

            string cellLeftID = null == footerCellLeft ? string.Empty : footerCellLeft.ClientID;
            string cellRightID = null == footerCllRight ? string.Empty : footerCllRight.ClientID;
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, "mt.init('" + this.ClientID + "','" + cellLeftID + "','" + cellRightID + "');", true);
            base.DataBind();
        }

        #endregion

        #region 事件处理

        void ItemButtonClick(object sender, EventArgs e)
        {
            if (null == this.OnCommand)
                return;

            IButtonControl bt = sender as IButtonControl;
            Control c = bt as Control;
            this.currentCommandName = bt.CommandName;
            bool r = this.OnCommand(bt.CommandName, ((HiddenField)((TableRow)c.Parent.Parent).Cells[0].Controls[1]).Value);
            if (r && bt.CommandName == CommandNames.Modify)
            {
                if (this.Options.IsOpenOverlay)
                    this.OpenOverlay();
                else
                    this.DisplayEditor();
            }
        }

        void ButtonClick(object sender, EventArgs e)
        {
            if (null == this.OnCommand)
                return;

            IButtonControl bt = sender as IButtonControl;
            this.currentCommandName = bt.CommandName;
            string[] commandArguments = this.GetCheckedPrimaryKeyData();
            this.numCheckedPrimaryKeyData = commandArguments.Length;

            if (bt.CommandName == CommandNames.DeleteChecked)
            {
                if (commandArguments.Length == 0)
                    return;
            }

            if (bt.CommandName == CommandNames.CreateNew)
                this.CreateNew();
            else
                this.OnCommand(bt.CommandName, commandArguments);
        }

        void lstControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == this.OnCommand)
                return;

            this.OnCommand(CommandNames.Search, ((Control)sender).ID);
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 准备创建新对象。
        /// </summary>
        public void CreateNew()
        {
            bool r = this.OnCommand(CommandNames.CreateNew, null);
            if (r)
            {
                if (this.Options.IsOpenOverlay)
                    this.OpenOverlay();
                else
                    this.DisplayEditor();
            }
        }

        /// <summary>
        /// 使用JS触发准备创建新对象。
        /// </summary>
        public void CreateNewWithJS()
        {
            var btCreateNew = this.FindControl("btCreateNew");
            if (null != btCreateNew)
                ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "$(document).ready(function(){$2('" + btCreateNew.ClientID + "').click();});", true);
        }

        /// <summary>
        /// 以Overlay方式打开编辑模板。
        /// </summary>
        public void OpenOverlay()
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "mt.openOverlay();", true);
        }

        /// <summary>
        /// 关闭编辑模板。
        /// </summary>
        public void CloseOverlay()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "mt.closeOverlay();", true);
        }

        /// <summary>
        /// 直接显示编辑模板。
        /// </summary>
        public void DisplayEditor()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "mt.displayEditor();", true);
        }

        /// <summary>
        /// 关闭直接显示的编辑模板。
        /// </summary>
        public void CloseEditor()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "mt.closeEditor();", true);
        }

        /// <summary>
        /// 根据主键值和列索引号获取列。
        /// </summary>
        /// <param name="primarkKey">主键值</param>
        /// <param name="columnIndex">列索引号(不包括选择列)</param>
        /// <returns></returns>
        public TableCell GetTableCell(string primarkKey, int columnIndex)
        {
            primarkKey = primarkKey.Trim();

            //加上选择列，所以索引号加1.
            columnIndex += 1;
            foreach (TableRow _row in this.Rows)
            {
                if (null != _row.ID && _row.ID.IndexOf("trSDI") == 0)
                {
                    foreach (Control _c in _row.Cells[0].Controls)
                    {
                        if (null != _c.ID && _c.ID.IndexOf("hidSDI") == 0)
                        {
                            HiddenField hid = _c as HiddenField;
                            if (hid.Value.Trim() == primarkKey)
                                return _row.Cells[columnIndex];
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region 其它方法

        /// <summary>
        /// 验证参数设置。
        /// </summary>
        private void ValidateOptions()
        {
            if (null == this.Options.ColumnWidths)
                return;

            //列宽度。
            var zeroCount = 0;
            foreach (var _width in this.Options.ColumnWidths)
            {
                if (_width == 0)
                    zeroCount += 1;
            }

            if (zeroCount > 1)
                AppException.ThrowWaringException("对不起，列表页的宽度设置只允许一个列设置为“0”！");
        }

        /// <summary>
        /// 创建空白分隔符。
        /// </summary>
        /// <returns></returns>
        private LiteralControl CreateWhiteSpace()
        {
            return new LiteralControl("<span class=\"myTable123_whiteSpace\"></span>");
        }

        #endregion
    }
}
