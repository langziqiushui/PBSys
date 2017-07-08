<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PBConfigList.aspx.cs" Inherits="YX.Web.Admin.Glo.PBConfigList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>屏蔽管理</title>
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
                <jk:MyTextBox runat="server" ID="txtApplicationType" MaxLength="25" PlaceHolder="请选择"  ClientIDMode="Static" Style="width: 160px;"></jk:MyTextBox>
                <asp:HiddenField ID="hidApplicationType" ClientIDMode="Static" runat="server" />
                类别：
                <asp:DropDownList ID="drPbType" runat="server" Width="150">
                </asp:DropDownList> 
                对应ID：
                <jk:MyTextBox runat="server" IsInlineControl="True" PlaceHolder="对应的ID号" ID="txtKeyword" style="width:120px;" ClientIDMode="Static">
                </jk:MyTextBox>
              
                &nbsp;&nbsp;<input id="btSearch" class="btn btn-blue" type="button" value="搜索" style="width:80px;" onclick="pbConfigList.beginSearch();" />
                &nbsp;&nbsp;
                &nbsp;&nbsp;<a class="largeLink" href="javascript:void(0);" onclick="pbConfigList.beginAddOrModify(true);">+ 添加屏蔽数据</a>
                &nbsp;&nbsp;<a href="/Admin/Glo/PBConfigList"><i class="fa fa-refresh"></i> 全部载入</a>
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