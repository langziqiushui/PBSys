<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBConfig.aspx.cs" Inherits="YX.Web.Admin.Glo.DBConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统配置参数管理</title>
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
                    <dt>分组选择：</dt>
                    <dd>
                        <asp:DropDownList ID="drGroup" runat="server" style="width:260px;" AutoPostBack="true">
                        </asp:DropDownList>
                    </dd>
                </dl>
                <dl>
                    <dt>配置参数键：</dt>
                    <dd>
                        <asp:RadioButtonList ID="radDBConfigKey" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="2" AutoPostBack="true">
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="radDBConfigKey" Display="Static" ErrorMessage="请选择配置参数键！"
                            ID="RequiredFieldValidator1" runat="server"></asp:RequiredFieldValidator>
                    </dd>
                </dl>
                <dl id="divDescriptionContainer" runat="server" visible="false">
                    <dt>参数配置说明：</dt>
                    <dd  id="divDescription" runat="server" style="color:Red;">
                    </dd>
                </dl>
                <dl id="divValueContainer" runat="server">
                    <dt>配置参数值：</dt>
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
