using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Globalization;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data.OleDb;

namespace YX.Core
{
    /// <summary>
    /// Excel报表导出。
    /// </summary>
    public static class ExcelUtil
    {
     
        public static string ExportToExcel(string reportTitle, DataTable data, string[] fields, string[] headTexts, int[] ColumnWidth, string[] dataFormat, string fileFolder, string fileName, bool useStyle)
        {
            GridView gvw = new GridView();
            // int ColCount, i;

            if (fields.Length != 0 && fields.Length == headTexts.Length)
            {
                //****************************
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
                ICell celltitle = sheet1.CreateRow(0).CreateCell(0);
                //sheet1.CreateRow(0).Height = 25 * 20;
                celltitle.SetCellValue(reportTitle);  // 设置表头

                ICellStyle style = hssfworkbook.CreateCellStyle();
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER; //水平居中
                style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;//垂直居中
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 13; //字号
                //style.BorderBottom = CellBorderType.THIN;
                //style.BorderLeft = CellBorderType.THIN;
                //style.BorderRight = CellBorderType.THIN;
                //style.BorderTop = CellBorderType.THIN;
                //font.Boldweight = HSSFBorderFormatting.BORDER_DASH_DOT;
                style.SetFont(font);

                //if (useStyle)
                    celltitle.CellStyle = style;
                //合并标题行
                //sheet1.AddMergedRegion(new Region(0, 0, 0, headTexts.Length - 1));
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, headTexts.Length - 1));

                IRow row;
                ICell cell;
                //HSSFCellStyle celStyle = getCellStyle();
                //表头
                int rowindex = 1;
                row = sheet1.CreateRow(rowindex);
                ICellStyle head_style = hssfworkbook.CreateCellStyle();
                for (int i = 0; i < headTexts.Length; i++)
                {
                    row.Height = 20 * 20;
                    cell = row.CreateCell(i);
                    cell.SetCellValue(headTexts[i]); //设置表头

                    head_style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER; //水平居中
                    head_style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;//垂直居中
                    head_style.BorderBottom = CellBorderType.THIN;
                    head_style.BorderLeft = CellBorderType.THIN;
                    head_style.BorderRight = CellBorderType.THIN;
                    head_style.BorderTop = CellBorderType.THIN;
                    if (useStyle)
                        cell.CellStyle = head_style; //附加表头样式
                    //设置列宽
                    sheet1.SetColumnWidth(i, ColumnWidth[i] * 256);
                }
                IDataFormat format = hssfworkbook.CreateDataFormat();

                ICellStyle fields_style = hssfworkbook.CreateCellStyle();
                fields_style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER; //水平居中
                fields_style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;//垂直居中
                fields_style.WrapText = true; //制动换行
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    rowindex++;
                    DataRow dr = data.Rows[i];
                    row = sheet1.CreateRow(rowindex);
                    for (int j = 0; j < fields.Length; j++)
                    {
                        //row.Height = 20 * 20;
                        cell = row.CreateCell(j);
                        if (dataFormat[j] != "")
                        {
                            //fields_style.DataFormat = f(dataFormat[j]);
                            if (dataFormat[j] == "0.00")
                            {
                                fields_style.DataFormat = HSSFDataFormat.GetBuiltinFormat(dataFormat[j]);
                                string temp_value = dr[fields[j]].ToString();
                                if (string.IsNullOrEmpty(temp_value))
                                    temp_value = "0";
                                cell.SetCellValue(Convert.ToDouble(temp_value));
                            }
                            else if (dataFormat[j] == "0")
                            {
                                fields_style.DataFormat = HSSFDataFormat.GetBuiltinFormat(dataFormat[j]);
                                string temp_value = dr[fields[j]].ToString();
                                if (string.IsNullOrEmpty(temp_value))
                                    temp_value = "0";
                                cell.SetCellValue(Convert.ToInt32(temp_value));
                            }
                            else
                            {
                                fields_style.DataFormat = format.GetFormat(dataFormat[j]);
                                cell.SetCellValue(new HSSFRichTextString(dr[fields[j]].ToString()));
                            }
                        }
                        else
                        {
                            cell.SetCellValue(new HSSFRichTextString(dr[fields[j]].ToString()));
                        }
                        // cell.SetCellValue(dr[fields[j]].ToString());
                        //加边框
                        fields_style.BorderBottom = CellBorderType.THIN;
                        fields_style.BorderLeft = CellBorderType.THIN;
                        fields_style.BorderRight = CellBorderType.THIN;
                        fields_style.BorderTop = CellBorderType.THIN;
                        if (useStyle)
                            cell.CellStyle = fields_style;
                    }
                }

                if (!string.IsNullOrEmpty(fileName))
                    reportTitle += "-" + fileName;
                var filePath = string.Format(fileFolder + "/{0}.xls", reportTitle);
                using (var fs = new FileStream(HttpContext.Current.Server.MapPath(filePath), FileMode.Create, FileAccess.ReadWrite))
                {
                    hssfworkbook.Write(fs);
                }

                return filePath;
            }
            else
            {
                throw new Exception("报表头和字段长度不一致");
            }
        }

        #region  解析EXCEL

        /// <summary>
        /// 从EXCEL中获取数据
        /// </summary>
        /// <param name="path">文件路劲</param>
        /// <param name="tablename">EXCEL中sheet名称</param>
        /// <returns></returns>
        public static DataTable ImportFromExcel(string path, string sheetname)
        {
            sheetname = sheetname + "$";
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0";
            string selectCommand = "select * from  [" + sheetname + "]";
            DataTable dt = new DataTable();
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connString);
            try
            {
                conn.Open();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(selectCommand, conn);
                System.Data.OleDb.OleDbDataAdapter adt = new System.Data.OleDb.OleDbDataAdapter(cmd);
                adt.AcceptChangesDuringFill = false;
                adt.Fill(dt);
                return dt;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
            finally
            {
                conn.Close();
                System.IO.File.Delete(path);
            }
        }

        #endregion

        #region 创建Excel连接

        /// <summary>
        /// 创建Excel连接。
        /// </summary>
        /// <param name="filePath">EXCEL文件路径</param>
        /// <returns></returns>
        public static OleDbConnection CreateConn(string filePath)
        {
            OleDbConnection conn = null;
            var connTexts = new string[]{
                "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'",
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"
            };

            foreach (var _connText in connTexts)
            {
                try
                {
                    conn = new OleDbConnection(_connText);
                    conn.Open();
                    break;
                }
                catch
                {
                    conn = null;
                }
            }

            if (null == conn)
                AppException.ThrowWaringException("对不起，连接Excel文件(" + System.IO.Path.GetFileName(filePath) + ")失败！");

            return conn;
        }

        #endregion
    }
}
