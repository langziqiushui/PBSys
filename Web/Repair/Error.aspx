<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="YX.Web.Repair.Error" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>页面发生错误！</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <style type="text/css">
        body::before{ background-color:White; padding:0px; margin:0px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" style="width:100%; height:100%;">
            <tr>
                <td>
                    <!--错误开始-->
                    <div class="error" id="divOtherError" runat="server" visible="false">
                      <dl>
                        <dt><img src="/App_Themes/Modules/images/Repair/error-icon.jpg" /></dt>
                        <dd>
                          <p class="error-name">很抱歉，您要访问的页面发生错误！</p>
                          <p class="error-reason" id="divErrorMsg" runat="server">页面发生错误！</p>
                          <p class="error-operating"> <strong>您还可以：</strong> <span>1、<a href="javascript:void(0);" onclick="history.back(-1);">返回上一页</a></span> <span>2、<a href="/">返回首页</a></span> </p>
                          <p style="height:100px;"></p>
                        </dd>
                      </dl>
                    </div>
                    <!--错误提示结束--> 


                    <!--错误开始横排-->
                    <div class="error-horizontal" id="div404" runat="server">
                      <p><img src="/App_Themes/Modules/images/Repair/error-pic.jpg" /></p>
                      <p class="error-horizontal-name">很抱歉，您要访问的页面不存在！</p>
                      <p>可能原因：你输入的不正确或链接可能已过期！</p>
                      <p class="error-horizontal-operating"><strong>您还可以：</strong><a href="javascript:void(0);" onclick="history.back(-1);">返回上一页</a> <a href="/">去首页</a></p>
                      <p style="height:100px;"></p>
                    </div>
                    <!--错误提示横排结束-->
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
