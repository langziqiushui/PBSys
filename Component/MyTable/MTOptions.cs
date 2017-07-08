using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Web.UI;

using YX.Core;

namespace YX.Component
{
    /// <summary>
    /// 为MyTable提供数据的配置参数对象。
    /// </summary>
    [Serializable]
    public class MTOptions
    {
        public MTOptions()
        {
            this.Width = 0;
            this.ColumnWidth_Check = 35;
            this.ColumnWidth_Edit = 65;
            this.Caption = "MyTable";
            this.NoDataMessage = "对不起，当前没有任何数据！";
            this.CreateNewButtonText = "+ 添加";
            this.AddSpaceColumn = true;
            this.EditColumnCaption = "功能";
            this.DeleteAllButtonText = "- 删除所有数据";
            this.IsOpenOverlay = true;
            this.PageSize = 10;
            this.IsShowCaptionRow = true;

            this.Reset();
        }

        /// <summary>
        /// 验证参配置数。
        /// </summary>
        public void Validate()
        {
            if (null == this.ColumnNames || this.ColumnNames.Length == 0)
                throw new AppException("对不起，未设置列标题(ColumnNames)。", ExceptionLevels.Warning);
            if (null == this.ColumnWidths || this.ColumnWidths.Length == 0)
                throw new AppException("对不起，未设置列宽度(ColumnWidths)。", ExceptionLevels.Warning);
            if (this.ColumnNames.Length != this.ColumnWidths.Length)
                throw new AppException("对不起，列标题项 与 列宽项数组长度不一致。", ExceptionLevels.Warning);
        }

        /// <summary>
        /// 重置为默认状态。
        /// </summary>
        public void Reset()
        {
            this.ShowCheckColumn = true;
            this.ShowEditColumn = true;
            this.ShowModifyButton = true;
            this.ShowDeleteButton = true;
            this.ShowDeleteCheckedButton = true;
            this.ShowDeleteAllButton = false;
            this.UsePages = true;
            this.ShowHeader = true;
            this.ShowFooter = true;
            this.ShowCreateNew = true;
            this.PageSize = 10;
        }
       
        /// <summary>
        /// 获取或设置 控件宽度。
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 获取或设置 是否显示多选列。
        /// </summary>
        public bool ShowCheckColumn { get; set; }

        /// <summary>
        /// 获取或设置 是否显示头部。
        /// </summary>
        public bool ShowHeader { get; set; }

        /// <summary>
        /// 获取或设置 是否显示脚本。
        /// </summary>
        public bool ShowFooter { get; set; }

        /// <summary>
        /// 获取或设置 标题。
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 获取或设置 列宽度(选择列)。
        /// </summary>
        public int ColumnWidth_Check { get; set; }

        /// <summary>
        /// 获取或设置 列宽度(编辑列)。
        /// </summary>
        public int ColumnWidth_Edit { get; set; }

        /// <summary>
        /// 获取或设置 是否显示编辑列(即每一行增加修改、删除按钮)。
        /// </summary>
        public bool ShowEditColumn { get; set; }

        /// <summary>
        /// 获取或设置 是否显示标题栏。
        /// </summary>
        public bool IsShowCaptionRow { get; set; }

        /// <summary>
        /// 获取或设置 编辑列的标题文本。
        /// </summary>
        public string EditColumnCaption { get; set; }

        /// <summary>
        /// 获取或设置 是否显示编辑列的修改按钮。
        /// </summary>
        public bool ShowModifyButton { get; set; }

        /// <summary>
        /// 获取或设置 是否显示编辑列的删除按钮。
        /// </summary>
        public bool ShowDeleteButton { get; set; }

        /// <summary>
        /// 获取或设置 是否显示删除所有选择数据的按钮。
        /// </summary>
        public bool ShowDeleteCheckedButton { get; set; }

        /// <summary>
        /// 获取或设置 是否显示删除所有数据的按钮。
        /// </summary>
        public bool ShowDeleteAllButton { get; set; }

        /// <summary>
        /// 获取或设置 是否显示创建的按钮。
        /// </summary>
        public bool ShowCreateNew { get; set; }

        /// <summary>
        /// 获取或设置 是否在最后添加空白列。
        /// </summary>
        public bool AddSpaceColumn { get; set; }

        /// <summary>
        /// 获取或设置 创建功能的按钮显示的文本。
        /// </summary>
        public string CreateNewButtonText { get; set; }

        /// <summary>
        /// 获取或设置 删除所有数据的按钮文本。
        /// </summary>
        public string DeleteAllButtonText { get; set; }

        /// <summary>
        /// 获取或设置 是否需要实现分页。
        /// </summary>
        public bool UsePages { get; set; }

        /// <summary>
        /// 获取或设置 总记录数。
        /// </summary>
        public int NumRecord = 0;

        /// <summary>
        /// 获取或设置 每页显示的记录数。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置 当没有任何数据时显示的消息。
        /// </summary>
        public string NoDataMessage { get; set; }

        /// <summary>
        /// 获取或设置 打开蒙版进行编辑操作。
        /// </summary>
        public bool IsOpenOverlay { get; set; }

        /// <summary>
        /// 获取或设置 列标题。
        /// </summary>
        public string[] ColumnNames { get; set; }
        /// <summary>
        /// 获取或设置 列宽度。
        /// </summary>
        public int[] ColumnWidths { get; set; }

        /// <summary>
        /// 获取或设置 列配置参数( {"列标题1,style='text-align:left;color:red',pop,autoHidden,overflowHidden,class='style123'", "列标题2,style='color:green'"} )。
        /// pop:在新的弹出层中显示(可任意多列)。
        /// autoHidden:自动隐藏仅显示第一行文本，当鼠标单击该行后显示所有文本(必须设置该行宽度为0[0表示自动计算宽度]，且仅能有一个autoHidden列)。
        /// overflowHidden:固定宽度当文本过长时隐藏显示(可任意多列，但必须设置列宽度的绝对值)。
        /// </summary>
        public string[] ColumnProperty { get; set; }

    }
}
