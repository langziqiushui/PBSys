<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PBConfigDialog.aspx.cs" Inherits="YX.Web.Admin.Glo.Dialog.PBConfigDialog" %>
<%@ Register src="../UC/UCPBConfig.ascx" tagname="UCPBConfig" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>屏蔽管理</title>   
    <style type="text/css">
        body::before{ background-color:White;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="dialog-container">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:UCPBConfig ID="uc" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
