<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="YX.Web.Passport.Login" %>
<%@ Register Src="/UC/UCVerifyCode.ascx" TagName="UCVerifyCode" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>用户登录</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!--头部-->
    <div class="header">
        <%--<div class="logo">
            <img src="/App_Themes/Modules/images/login/logo.png" />
        </div>--%>
    </div>
    <!--头部结束-->
    <!--内容开始-->
    <div class="login-content">
        <!--登录-->
        <div class="login-box">
            <div class="login-list">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>                    
                        <ul>
                            <li><span class="login-title">账户名称：</span>                       
                                <jk:MyTextBox ID="txtUserName" runat="server" CssClass="login-input"></jk:MyTextBox>
                            </li>
                            <li><span class="login-title">登录密码：</span>
                                <jk:MyTextBox ID="txtPassword" TextMode="Password" PlaceHolder="密码" CssClass="login-input" runat="server"></jk:MyTextBox>
                            </li>                            
                            <li style="margin-bottom: 5px;"><span class="login-title"></span>
                                <asp:Button ID="btLogin" runat="server" Text="登录" CssClass="login-button"
                                    OnClientClick="return login.beginLogin(false);" />
                            </li>
                            <%--<li style="margin-top: 15px; "><a class="largeLink"  style="float:right; margin-right:30px;" href="javascript:void(0);" onclick="login.retrievePassword();">忘记密码</a>
                            </li>--%>
                        </ul>
                        <!--提示-->
                        <div class="login-error-title" id="divErrorMsg" runat="server" visible="false">                    
                        </div>
                        <!--提示结束-->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--登录结束-->
    </div>
    <!--内容结束-->
    <!--底部-->
    <div class="footer">
        <p>
            Copyright© <%= DateTime.Now.Year %> yxdown.com. All rights reserved. 
        </p>
        <p>
            游讯网综合信息管理系统 版权所有 </p>
    </div>
    <!--底部结束-->
    </form>
    <p id="divVerifyCodeStatus">
    </p>
</body>
</html>
