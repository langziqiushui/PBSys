using System;
using System.Collections.Generic;
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
    public class MyPagerDesigner : ControlDesigner
    {
        /// <summary>
        /// 呈现控件空白时样式
        /// </summary>
        /// <returns></returns>
        protected override string GetEmptyDesignTimeHtml()
        {
            MyPager control = (MyPager)Component;
            return CreatePlaceHolderDesignTimeHtml("");
        }

        /// <summary>
        /// 呈现控件的正常显示样式
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            MyPager control = (MyPager)Component;

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            try
            {               
                LiteralControl lit = new LiteralControl(@"
<style type=""text/css""> 
<!-- 
.MyPager { 
    font-size:12px; font-family:verdana, 宋体;  overflow:auto; color:#494949;
} 
.MyPager .box { 
    width:25px; height:17px; padding-top:2px; float:left; cursor:pointer; margin-right:3px; _display:inline;font-size:12px;
    text-align:center; border:solid 1px #DDDDDD; background-color:white; color:#494949;
} 
.MyPager .current { 
    width:25px; height:16px; padding-top:3px; float:left; margin-right:3px; _display:inline;font-size:12px; 
    color:#FF663A; font-weight:bold; text-align:center;
} 
.MyPager .overBox { 
    background-color:#EEEEEE; color:#F9665F; 
    
} 
.MyPager .disabledBox { 
    width:25px; height:17px; padding-top:2px; float:left; margin-right:3px; _display:inline;font-size:12px; 
    text-align:center;  color:#999999;
} 
.MyPager .go { 
    width:100px; float:left; text-align:right;
} 
.MyPager .go .input { 
    width:25px; height:15px; border:solid 1px #DDDDDD; text-align:center; vertical-align:middle; margin:0px 2px 0px 2px;color:red; 
} 
.MyPager .go .button { 
    width:35px; height:19px; border:solid 1px #DDDDDD; background-color:#E8E7E7; text-align:center; vertical-align:middle;color:#494949; 
} 
--> 
</style> 

<div class=""MyPager"">
    <div class=""disabledBox"" style=""width:50px;""><< 首页</div>
    <div class=""disabledBox"" style=""width:45px;"">< 上页</div>
    <div class=""current"">1</div>
    <a href=""/WebHR2009/latestJob_2""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">2</div></a>
    <a href=""/WebHR2009/latestJob_3""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">3</div></a>
    <a href=""/WebHR2009/latestJob_4""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">4</div></a>
    <a href=""/WebHR2009/latestJob_5""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">5</div></a>
    <a href=""/WebHR2009/latestJob_6""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">6</div></a>
    <a href=""/WebHR2009/latestJob_7""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">7</div></a>
    <a href=""/WebHR2009/latestJob_8""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">8</div></a>
    <a href=""/WebHR2009/latestJob_9""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"">9</div></a>
    <a href=""/WebHR2009/latestJob_2"">    <div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"" style=""width:54px;"">下页 ></div></a>
    <a href=""/WebHR2009/latestJob_145""><div class=""box"" onmouseover=""this.className='box overBox';"" onmouseout=""this.className='box';"" style=""width:54px;"">尾页 >></div></a>
    <div class=""go"">第<input type=""text"" id=""MyPager1_txtPager"" class=""input"" value=""1"" />页 <input type=""button"" value=""跳转"" class=""button"" onclick=""MyPager_Go('MyPager1', 145);"" /></div>
</div> 
");
                lit.RenderControl(htw);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                return CreatePlaceHolderDesignTimeHtml("创建显示界面时发生错误：" + ex.Message);
            }

        }
    }
}
