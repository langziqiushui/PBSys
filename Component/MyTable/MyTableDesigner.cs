using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.IO;

namespace YX.Component
{
    /// <summary>
    /// 在设置器中呈视自定义分页控件的外观。
    /// </summary>
    public class MyTableDesigner : ControlDesigner
    {
        /// <summary>
        /// 呈现控件空白时样式
        /// </summary>
        /// <returns></returns>
        protected override string GetEmptyDesignTimeHtml()
        {
            var control = (MyTable)Component;
            return CreatePlaceHolderDesignTimeHtml("");
        }

        /// <summary>
        /// 呈现控件的正常显示样式
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            var control = (MyTable)Component;

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            try
            {
                LiteralControl lit = new LiteralControl(@"
<style type=""text/css""> 
<!-- 
table.sg123{width:100%; border:solid 1px #DADADA; padding:0px; color:#5E5E5E; font-size:13px; font-family:""verdana"",""宋体""; }
    .sg123_ch{height:24px; padding-left:5px; vertical-align:middle; font-weight:bold; background:url(images/bgSG.gif) repeat-X left -101px;}
    .sg123_rc{ height:22px; border-top:bolid 1px #DADADA; background:url(images/bgSG.gif) #EDEEF0 repeat left -76px;}
    .sg123_rc td{ text-align:center; border-right:solid 1px #CCCCCC; }
    .sg123_rd, .sg123_rd2{ height:22px; text-align:center;}
    .sg123_rd2{ background-color:#EDEAEA;}
    .sg123_rd td, .sg123_rd2 td{border-right:solid 1px #CCCCCC; }
    .sg123_rb{height:40px; background:url(images/bgSG.gif) repeat left -30px;}
    .sg123_rb table{width:100%; }
    .sg123_rb table tr td{height:35px; padding-top:5px; *height:34px; *padding-top:6px; vertical-align:top;}
    .sg123_action{ padding-left: 5px;}
    .sg123_button, .sg123_button_o{ width:80px; height:21px; border-width:0px; color:White; background:url(images/bgSG.gif) no-repeat left -128px; }
    .sg123_button_o{background:url(images/bgSG.gif) no-repeat left -150px;}
    .sg123_pager{width:225px; text-align:right; padding-right:5px;}

    .sg123_rd_o{ background-color:#FFA500; border:solid 1px #E19201;}
    .sg123_rd_s{ background-color:#E69A0F; border:solid 1px #AD7A1A;}  
    
    .sg123_btEdit, .sg123_btDel{width:16px; height:16px;border-width:0px;} 
    .sg123_btEdit{ background:url(images/bgSG.gif) no-repeat left -172px;} 
    .sg123_btDel{ background:url(images/bgSG.gif) no-repeat -20px -172px;} 
    .sg123_whiteSpace{font-size:10px;}
    .sg123_Separator{width:2px; height:22px; border-width:0px; background:url(images/bgSG.gif) no-repeat -66px -171px; margin:0px; padding:0px;}
    
    .sg123_btSGFirst, .sg123_btSGPrev, .sg123_btSGNext, .sg123_btSGLast, .sg123_btSGFirst_d, .sg123_btSGPrev_d, .sg123_btSGNext_d, .sg123_btSGLast_d{width:22px; height:22px; border-width:0px; cursor:pointer;}
    .sg123_btSGFirst{background:url(images/bgSG.gif) no-repeat -22px -190px;}
    .sg123_btSGPrev{background:url(images/bgSG.gif) no-repeat -44px -190px;}
    .sg123_btSGNext{background:url(images/bgSG.gif) no-repeat left -213px;}
    .sg123_btSGLast{background:url(images/bgSG.gif) no-repeat -22px -213px;}
    
    .sg123_btSGFirst_d{background:url(images/bgSG.gif) no-repeat -44px -213px;}
    .sg123_btSGPrev_d{background:url(images/bgSG.gif) no-repeat left -236px;}
    .sg123_btSGNext_d{background:url(images/bgSG.gif) no-repeat -22px -236px;}
    .sg123_btSGLast_d{background:url(images/bgSG.gif) no-repeat -44px -236px;}
    
    .sg123_btSGFirst_o, .sg123_btSGPrev_o, .sg123_btSGNext_o, .sg123_btSGLast_o{border:solid 1px #cccccc; width:22px; height:22px;cursor:pointer;}
    .sg123_btSGFirst_o{background:url(images/bgSG.gif) white no-repeat -23px -191px;}
    .sg123_btSGPrev_o{background:url(images/bgSG.gif) white no-repeat -45px -191px;}
    .sg123_btSGNext_o{background:url(images/bgSG.gif) white no-repeat -1px -214px;}
    .sg123_btSGLast_o{background:url(images/bgSG.gif) white no-repeat -23px -214px;}
    
    .sg123_drPager{width:80px; height:20px; margin:0px 5px 0px 5px; border:solid 1px #ABADB3; text-align:center; font-size:12px; color:black;}
--> 
</style> 

<table id=""SimpleGrid1"" class=""sg123"">
        <tr>
            <td class=""sg123_ch"" colspan=""6"">
                SimpleGrid
            </td>
        </tr>
        <tr class=""sg123_rc"">
            <td style=""width: 45px;"">
                选择
            </td>
            <td style=""width: 150px;"">
                Column1
            </td>
            <td style=""width: 150px;"">
                Column2
            </td>
            <td style=""width: 150px;"">
                Column3
            </td>
            <td>
                Column4
            </td>
            <td style=""width: 60px;"">
                相关功能
            </td>
        </tr>
        <tr id=""SimpleGrid1_trSDI0"" class=""sg123_rd"">
            <td>
                <input id=""SimpleGrid1_chkS0"" type=""checkbox"" name=""SimpleGrid1$chkS0"" /><input type=""hidden""
                    name=""SimpleGrid1$hidSDI0"" id=""SimpleGrid1_hidSDI0"" value=""Data1_1"" /><input type=""hidden""
                        name=""SimpleGrid1$hidSPop2_4"" id=""SimpleGrid1_hidSPop2_4"" value=""Data1_4"" />
            </td>
            <td>
                Data1_1
            </td>
            <td>
                Data1_2
            </td>
            <td class="" C_B"">
                Data1_3
            </td>
            <td>
                <a id=""lnkS2_4"" href=""#"" onmouseover=""javascript:sg.showPop('lnkS_2_4', 'hidSPop2_4');"">
                    显示</a>
            </td>
            <td>
                <input type=""submit"" name=""SimpleGrid1$btSEdit0"" value="""" id=""SimpleGrid1_btSEdit0""
                    title=""编辑数据"" class=""sg123_btEdit"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input
                        type=""submit"" name=""SimpleGrid1$btSDel0"" value="""" onclick=""return confirm(&#39;您将要删除当前数据，是否确定？&#39;);""
                        id=""SimpleGrid1_btSDel0"" title=""删除数据"" class=""sg123_btDel"" />
            </td>
        </tr>
        <tr id=""SimpleGrid1_trSDI1"" class=""sg123_rd2"">
            <td>
                <input id=""SimpleGrid1_chkS1"" type=""checkbox"" name=""SimpleGrid1$chkS1"" /><input type=""hidden""
                    name=""SimpleGrid1$hidSDI1"" id=""SimpleGrid1_hidSDI1"" value=""Data3_1"" /><input type=""hidden""
                        name=""SimpleGrid1$hidSPop3_4"" id=""SimpleGrid1_hidSPop3_4"" value=""Data3_4"" />
            </td>
            <td>
                Data3_1
            </td>
            <td>
                Data3_2
            </td>
            <td class="" C_B"">
                <a id='aaxxyy' onclick='alert(sg.getCPKData(""aaxxyy""));'>xxx</a>
            </td>
            <td>
                <a id=""lnkS3_4"" href=""#"" onmouseover=""javascript:sg.showPop('lnkS_3_4', 'hidSPop3_4');"">
                    显示</a>
            </td>
            <td>
                <input type=""submit"" name=""SimpleGrid1$btSEdit1"" value="""" id=""SimpleGrid1_btSEdit1""
                    title=""编辑数据"" class=""sg123_btEdit"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input
                        type=""submit"" name=""SimpleGrid1$btSDel1"" value="""" onclick=""return confirm(&#39;您将要删除当前数据，是否确定？&#39;);""
                        id=""SimpleGrid1_btSDel1"" title=""删除数据"" class=""sg123_btDel"" />
            </td>
        </tr>
        <tr id=""SimpleGrid1_trSDI2"" class=""sg123_rd"">
            <td>
                <input id=""SimpleGrid1_chkS2"" type=""checkbox"" name=""SimpleGrid1$chkS2"" /><input type=""hidden""
                    name=""SimpleGrid1$hidSDI2"" id=""SimpleGrid1_hidSDI2"" value=""Data4_1"" /><input type=""hidden""
                        name=""SimpleGrid1$hidSPop4_4"" id=""SimpleGrid1_hidSPop4_4"" value=""Data4_4"" />
            </td>
            <td>
                Data4_1
            </td>
            <td>
                Data4_2
            </td>
            <td class="" C_B"">
                Data4_3
            </td>
            <td>
                <a id=""lnkS4_4"" href=""#"" onmouseover=""javascript:sg.showPop('lnkS_4_4', 'hidSPop4_4');"">
                    显示</a>
            </td>
            <td>
                <input type=""submit"" name=""SimpleGrid1$btSEdit2"" value="""" id=""SimpleGrid1_btSEdit2""
                    title=""编辑数据"" class=""sg123_btEdit"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input
                        type=""submit"" name=""SimpleGrid1$btSDel2"" value="""" onclick=""return confirm(&#39;您将要删除当前数据，是否确定？&#39;);""
                        id=""SimpleGrid1_btSDel2"" title=""删除数据"" class=""sg123_btDel"" />
            </td>
        </tr>
        <tr id=""SimpleGrid1_trSDI3"" class=""sg123_rd2"">
            <td>
                <input id=""SimpleGrid1_chkS3"" type=""checkbox"" name=""SimpleGrid1$chkS3"" /><input type=""hidden""
                    name=""SimpleGrid1$hidSDI3"" id=""SimpleGrid1_hidSDI3"" value=""Data5_1"" /><input type=""hidden""
                        name=""SimpleGrid1$hidSPop5_4"" id=""SimpleGrid1_hidSPop5_4"" value=""Data5_4"" />
            </td>
            <td>
                Data5_1
            </td>
            <td>
                Data5_2
            </td>
            <td class="" C_B"">
                Data5_3
            </td>
            <td>
                <a id=""lnkS5_4"" href=""#"" onmouseover=""javascript:sg.showPop('lnkS_5_4', 'hidSPop5_4');"">
                    显示</a>
            </td>
            <td>
                <input type=""submit"" name=""SimpleGrid1$btSEdit3"" value="""" id=""SimpleGrid1_btSEdit3""
                    title=""编辑数据"" class=""sg123_btEdit"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input
                        type=""submit"" name=""SimpleGrid1$btSDel3"" value="""" onclick=""return confirm(&#39;您将要删除当前数据，是否确定？&#39;);""
                        id=""SimpleGrid1_btSDel3"" title=""删除数据"" class=""sg123_btDel"" />
            </td>
        </tr>
        <tr id=""SimpleGrid1_trSDI4"" class=""sg123_rd"">
            <td>
                <input id=""SimpleGrid1_chkS4"" type=""checkbox"" name=""SimpleGrid1$chkS4"" /><input type=""hidden""
                    name=""SimpleGrid1$hidSDI4"" id=""SimpleGrid1_hidSDI4"" value=""Data6_1"" /><input type=""hidden""
                        name=""SimpleGrid1$hidSPop6_4"" id=""SimpleGrid1_hidSPop6_4"" value=""Data6_4"" />
            </td>
            <td>
                Data6_1
            </td>
            <td>
                Data6_2
            </td>
            <td class="" C_B"">
                Data6_3
            </td>
            <td>
                <a id=""lnkS6_4"" href=""#"" onmouseover=""javascript:sg.showPop('lnkS_6_4', 'hidSPop6_4');"">
                    显示</a>
            </td>
            <td>
                <input type=""submit"" name=""SimpleGrid1$btSEdit4"" value="""" id=""SimpleGrid1_btSEdit4""
                    title=""编辑数据"" class=""sg123_btEdit"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input
                        type=""submit"" name=""SimpleGrid1$btSDel4"" value="""" onclick=""return confirm(&#39;您将要删除当前数据，是否确定？&#39;);""
                        id=""SimpleGrid1_btSDel4"" title=""删除数据"" class=""sg123_btDel"" />
            </td>
        </tr>
        <tr class=""sg123_rb"">
            <td colspan=""6"">
                <table cellspacing=""0"" cellpadding=""0"" style=""border-width: 0px; border-collapse: collapse;"">
                    <tr>
                        <td id=""SimpleGrid1_cellSLeft"" class=""sg123_action"">
                            <input id=""SimpleGrid1_chkSAll"" type=""checkbox"" name=""SimpleGrid1$chkSAll"" onclick=""sg.checkAll(this, &#39;SimpleGrid1&#39;);"" /><label
                                for=""SimpleGrid1_chkSAll"">全选/取消</label>&nbsp;&nbsp;&nbsp;&nbsp;<input type=""submit""
                                    name=""SimpleGrid1$btSDelAll"" value=""删除选择"" id=""SimpleGrid1_btSDelAll"" title=""删除所有处于选择状态的数据""
                                    class=""sg123_button"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input type=""submit""
                                        name=""SimpleGrid1$btOB"" value=""我的功能"" id=""SimpleGrid1_btOB"" class=""sg123_button"" />
                        </td>
                        <td id=""SimpleGrid1_cellSRight"" class=""sg123_pager"">
                            <input type=""submit"" name=""SimpleGrid1$btSGFirst"" value="""" id=""SimpleGrid1_btSGFirst""
                                disabled=""disabled"" title=""转到第一页"" class=""aspNetDisabled sg123_btSGFirst"" /><span
                                    class=""sg123_whiteSpace"">&nbsp;</span><input type=""submit"" name=""SimpleGrid1$btSGPrev""
                                        value="""" id=""SimpleGrid1_btSGPrev"" disabled=""disabled"" title=""转到上一页"" class=""aspNetDisabled sg123_btSGPrev"" /><input
                                            type=""button"" disabled=""disabled"" class=""sg123_Separator"" />
                            转到<select name=""SimpleGrid1$drSGPager"" onchange=""javascript:setTimeout(&#39;__doPostBack(\&#39;SimpleGrid1$drSGPager\&#39;,\&#39;\&#39;)&#39;, 0)""
                                id=""SimpleGrid1_drSGPager"" class=""sg123_drPager"">
                                <option selected=""selected"" value=""1"">第1页</option>
                                <option value=""2"">第2页</option>
                            </select><input type=""button"" disabled=""disabled"" class=""sg123_Separator"" /><span
                                class=""sg123_whiteSpace"">&nbsp;</span><input type=""submit"" name=""SimpleGrid1$btSGNext""
                                    value="""" id=""SimpleGrid1_btSGNext"" title=""转到下一页"" class=""sg123_btSGNext"" /><span class=""sg123_whiteSpace"">&nbsp;</span><input
                                        type=""submit"" name=""SimpleGrid1$btSGLast"" value="""" id=""SimpleGrid1_btSGLast""
                                        title=""转到最后一页"" class=""sg123_btSGLast"" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
");

                lit.RenderControl(htw);
                return sw.ToString();
            }
            catch(Exception ex)
            {
                return CreatePlaceHolderDesignTimeHtml("创建显示界面时发生错误：" + ex.Message);
            }
        }
    }
}
