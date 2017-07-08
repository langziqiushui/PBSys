using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using YX.Core;
using YX.Web.Framework;
using YX.Domain;
using YX.Component;
using System.Data;

namespace YX.Web.OfficialAccount
{
    public partial class OfficialAccountDialog : BaseSystemConfig
    {
        protected override void OnInit(EventArgs e)
        {
            Button _btSave = (Button)this.FindControl("btSave");
            _btSave.Click += new EventHandler(_btSave_Click);
            base.OnInit(e);
        }

        protected void _btSave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), "try{common.hideAllPopupPanel();sg.closeEditor();sg.closeOverlay();}catch(e){}", true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["action"] == "save")
            {
                string website = Request["website"] ?? "";
                string hidwebsite = Request["hidwebsite"] ?? "";
                string type = Request["type"] ?? "";
                string column = Request["column"] ?? "";
                string value = Request["value"] ?? "";
                string id = Request["id"] ?? "";

                //保存数据
                try
                {
                    string sql = "insert into OfficialAccount(website,hidwebsite,[type],[column],value) values ('" + website + "','" + hidwebsite + "','" + type + "','" + column + "','" + value + "')";
                    if (id != "")
                    {
                        sql = "update OfficialAccount set website='" + website + "',hidwebsite='" + hidwebsite + "',[type]='" + type + "',[column]='" + column + "',value='" + value +
                            "',updatetime=getdate() where id=" + id;
                    }
                    DBHelper.execSql(sql);
                    Response.Write("ok");
                }
                catch (Exception ex)
                {
                    Response.Write("error:" + ex.Message);
                }
                Response.End();
            }

            if (Request["id"] + "" != "")
            {
                int id = 0;
                if (int.TryParse(Request["id"], out id))
                {
                    DataTable data = DBHelper.getData("select * from OfficialAccount where id=" + id);
                    if (data.Rows.Count > 0)
                    {
                        txtId.Value = data.Rows[0]["Id"] + "";
                        txtwebsite.Text = data.Rows[0]["website"] + "";
                        hidwebsite.Value = data.Rows[0]["hidwebsite"] + "";
                        drptype.SelectedValue = data.Rows[0]["type"] + "";
                        drpcolumn.SelectedValue = data.Rows[0]["column"] + "";
                        txtvalue.Text = data.Rows[0]["value"] + "";
                    }
                }
            }

            //this.uc.OnSubmit += new Action<BaseDataEditor, WorkModes>(uc_OnSubmit);
            this.RefJavascript(false, "DataSelector.js");
            this.RefJavascript(false, "myTable.js");
            this.RefStyle(false, "myTable.css");
        }

        void uc_OnSubmit(BaseDataEditor arg1, WorkModes arg2)
        {
            if (arg2 == WorkModes.Create || arg2 == WorkModes.Modify)
                WFUtil.RegisterStartupScript("common.refurParentPage();");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
            {
                var id = Request.QueryString["id"];
                if (string.IsNullOrEmpty(id))
                {
                    //this.uc.OnBeginCreate();
                }
                else
                {
                    //this.uc.OnBeginModify(id);
                }
            }
        }
    }
}