<%@ Control Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="UCVerifyCode.ascx.cs"
    Inherits="YX.Web.UC.UCVerifyCode" %>
<asp:PlaceHolder ID="phInput" runat="server">
    <jk:MyTextBox ID="txtVerifyCode" runat="server" MaxLength="4"></jk:MyTextBox>
    <asp:RequiredFieldValidator ID="rfvVerifyCode" runat="server" ControlToValidate="txtVerifyCode"
        Display="None" ErrorMessage="验证码 不能为空！"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="revVerifyCode" runat="server" ControlToValidate="txtVerifyCode"
        Display="None" ErrorMessage="验证码 必须为4位常规字符！" ValidationExpression="^[a-zA-Z]{4}$"></asp:RegularExpressionValidator>
    <p style="height:10px;"></p>
</asp:PlaceHolder>
<asp:Image ID="imgVerifyCode" runat="server" ImageAlign="AbsMiddle" ToolTip="点我，刷新验证码" />
