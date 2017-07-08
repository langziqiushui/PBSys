<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleDBConfig.aspx.cs"
    Inherits="YX.Web.Admin.Glo.SimpleDBConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>简单系统配置参数管理</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
</head>
<body style="padding: 3px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divDataEditor">
                <dl>
                    <dd id="divDescription" runat="server" style="color: Red;">
                    </dd>
                </dl>
                <dl>
                    <dd>
                        <jk:MyTextBox ID="txtValue" TextMode="MultiLine" Width="90%" Height="250" runat="server"></jk:MyTextBox>
                    </dd>
                </dl>
                <ol>
                    <jk:MyButton ID="btAccept" runat="server" Text="保存" Width="120" />
                </ol>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
