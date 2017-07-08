<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogList.aspx.cs" Inherits="YX.Web.Admin.Glo.LogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统日志管理</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
</head>
<body style="padding:3px;">
    <form id="form1" runat="server">
    <div class="widget-main ">
        <div class="tabbable">
            <ul class="nav nav-tabs" id="divTabStatus" runat="server">
            </ul>
            <div class="tab-content">
                站点：
                <jk:MyTextBox runat="server" ID="txtApplicationType" MaxLength="25" ClientIDMode="Static" Style="width: 160px;"></jk:MyTextBox>
                <asp:HiddenField ID="hidApplicationType" ClientIDMode="Static" runat="server" />
                类别：
                <asp:DropDownList ID="drRestype" runat="server" Width="150">
                </asp:DropDownList> 
                类型：
                <asp:DropDownList ID="drLogTypes" runat="server" Width="150">
                </asp:DropDownList> 
                关键词：
                <jk:MyTextBox runat="server" IsInlineControl="True" PlaceHolder="关键词" ID="txtKeyword" style="width:120px;" ClientIDMode="Static">
                </jk:MyTextBox>                    
                &nbsp;&nbsp;
                发生日期：
                <jk:MyTextBox runat="server" IsInlineControl="True"  ID="txtStartDate" style="width:110px;" ClientIDMode="Static">
                </jk:MyTextBox>
                -
                <jk:MyTextBox runat="server" IsInlineControl="True"  ID="txtEndDate" style="width:110px;" ClientIDMode="Static">
                </jk:MyTextBox>
                &nbsp;&nbsp;
                <input id="btSearch" class="btn btn-blue" type="button" value="搜索" style="width:80px;" onclick="logList.beginSearch();" />
                &nbsp;&nbsp;
                <a href="/Admin/Glo/LogList"><i class="fa fa-refresh"></i> 全部载入</a>
                            
            </div>
        </div>
    </div>
    <p style="height: 5px;">
    </p>
    <jk:MyTable ID="sg" runat="server">
    </jk:MyTable>
    </form>
</body>
</html>
