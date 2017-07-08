using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using YX.Core;
using YX.Web.Framework;

namespace YX.Web.UC
{
    /// <summary>
    /// 用于UpdatePanel内文件上传的用户控件。
    /// </summary>
    public partial class UCFileUpload : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.MaxNumberUpload == 0)
                this.MaxNumberUpload = 10;

            ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "REG_ajaxupload.3.5.js", "/JScripts/ajaxupload.3.5.js");
            WFUtil.RegisterStartupScript("ajaxUpload.init('" + this.ClientID + "'); ");
        }

        /// <summary>
        /// 获取或设置 上传的文件名列表(注意：不是文件路径)。
        /// </summary>
        public IList<string> UploadedFiles
        {
            get
            {
                return this.hidUploadFiles.Value.Split("###@@$@@###");
            }
            set
            {
                this.hidUploadFiles.Value = "";
                if (null != value)
                {
                    foreach (var _f in value)
                        this.hidUploadFiles.Value += _f + "###@@$@@###";
                }
            }
        }

        /// <summary>
        /// 获取或设置 最多允许上传的文件数(默认值：10)。
        /// </summary>
        public int MaxNumberUpload
        {
            get
            {
                if (string.IsNullOrEmpty(this.hidMaXNumberUpload.Value))
                    return 0;
                return int.Parse(this.hidMaXNumberUpload.Value);
            }
            set { this.hidMaXNumberUpload.Value = value.ToString(); }
        }

        /// <summary>
        /// 获取或设置 是否显示插入按钮(需要实现JS方法：__ajaxUploadInsertImage("图片引用网址"))。
        /// </summary>
        public bool IsShowInsertButton
        {
            get
            {
                if (string.IsNullOrEmpty(this.hidIsShowInsertButton.Value))
                    return false;
                return bool.Parse(this.hidIsShowInsertButton.Value);
            }
            set { this.hidIsShowInsertButton.Value = value.ToString().ToLower(); }
        }

        /// <summary>
        /// 获取或设置 文件上传虚拟目录。
        /// </summary>
        public string UploadPath
        {
            get
            {
                return this.hidUploadPath.Value;
            }
            set
            {
                hidUploadPath.Value = value;
                this.hidIsUploadToImageServer.Value = (value.IgnoreCaseEquals(Factory.DBConfigFactory.Global_ImageResourceUrl)).ToString().ToLower();
            }
        }

        /// <summary>
        /// 获取或设置 动态子目录名称(如以年月生成的目录名称)。
        /// </summary>
        public string DymicFolderName
        {
            get
            {
                return this.hidDymicFolderName.Value;
            }
            set
            {
                this.hidDymicFolderName.Value = value;
            }
        }

        /// <summary>
        /// 获取或设置 图片是否发生变化。
        /// </summary>
        public bool Modified
        {
            get
            {
                if (string.IsNullOrEmpty(this.hidModified.Value))
                    return false;

                return bool.Parse(this.hidModified.Value);
            }
            private set
            {
                this.hidModified.Value = value.ToString();
            }
        }

        /// <summary>
        /// 获取或设置 图片缩略图尺寸(格式：106,80)。
        /// </summary>
        public string ThumbImageSize
        {
            get { return this.hidThumbSize.Value; }
            set { this.hidThumbSize.Value = value; }
        }

        /// <summary>
        /// 获取或设置 图片同比例缩小尺寸(格式：L,640,240|M,512,192|S,320,120)。
        /// </summary>
        public string ScaleImageSize
        {
            get { return this.hidScaleImageSize.Value; }
            set { this.hidScaleImageSize.Value = value; }
        }

        /// <summary>
        /// 获取或设置 图片是否打水印。
        /// </summary>
        public bool IsMakeWatermark
        {
            get
            {
                if (string.IsNullOrEmpty(this.hidIsMakeWatermark.Value))
                    return false;

                return bool.Parse(this.hidIsMakeWatermark.Value);
            }
            set
            {
                this.hidIsMakeWatermark.Value = value.ToString().ToLower();
            }
        }

        /// <summary>
        /// 获取或设置 当前图片上传管理器对应的文本编辑器。
        /// </summary>
        public string TargetEditor
        {
            get { return this.hidTargetEditor.Value; }
            set { this.hidTargetEditor.Value = value; }
        }

        /// <summary>
        /// 获取或设置 当前处于选择状态的文件。
        /// </summary>
        public string SelectedFile
        {
            get { return this.hidCheckedFile.Value; }
            set { this.hidCheckedFile.Value = value; }
        }

        /// <summary>
        /// 获取或设置 是否显示复选框。
        /// </summary>
        public bool IsShowCheckBox
        {
            get
            {
                return bool.Parse(this.hidIsShowCheckBox.Value.ToLower());
            }
            set
            {
                this.hidIsShowCheckBox.Value = value.ToString().ToLower();
            }
        }

        /// <summary>
        /// 获取或设置 当前处于勾选状态的文件。
        /// </summary>
        public IList<string> CheckedFiles
        {
            get
            {
                return this.hidCheckedFiles.Value.Split("###@@$@@###");
            }
            set
            {
                this.hidCheckedFiles.Value = "";
                if (null != value)
                {
                    foreach (var _f in value)
                        this.hidCheckedFiles.Value += _f + "###@@$@@###";
                }
            }
        }

        /// <summary>
        /// 删除临时文件。
        /// </summary>
        public void DeleteTempFile()
        {
            foreach (var _file in this.UploadedFiles)
            {
                //只删除临时目录中的文件。
                if (_file.IgnoreCaseIndexOf(WebKeys.UPLOADPATH_TEMP) == 0 && System.IO.File.Exists(Server.MapPath(_file)))
                    System.IO.File.Delete(Server.MapPath(_file));
            }
        }

        /// <summary>
        /// 重置控件状态。
        /// </summary>
        public void Reset()
        {
            this.UploadedFiles = null;
            this.UploadedFiles = new List<string>();

            this.SelectedFile = null;
            this.Modified = false;
        }
    }
}