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

namespace YX.Web.Admin.Glo.Dialog
{
    public partial class PBConfigDialog : BaseSystemConfig
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.uc.OnSubmit += new Action<BaseDataEditor, WorkModes>(uc_OnSubmit);
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
                    this.uc.OnBeginCreate();
                }
                else
                {
                    this.uc.OnBeginModify(id);
                }
            }
        }
    }
}