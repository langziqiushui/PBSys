<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OfficialAccountList.aspx.cs" Inherits="YX.Web.OfficialAccount.OfficialAccountList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公众号管理</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <script>

        $(function () {
            //绑定站点事件
            $('#txtwebsite').dataSelector({
                ajaxData: {
                    "method": "RenderDataSource",
                    "dsType": "siteCategory",
                    "rand": Math.random() * 10000
                },
                ajaxURL: '/API/MyHandler.ashx',
                width: 380,
                originalValue: null,
                onDataSelected: function (t, v) {

                    $2("txtwebsite").value = t;
                    $2("hidwebsite").value = v;
                }
            });
        });

        function openDialog(isAdd, id) {
            var caption = isAdd ? "添加公众号" : "修改公众号";
            var url = "/OfficialAccount/OfficialAccountDialog.aspx";
            if (!isAdd)
                url += "?id=" + id;

            common.openDialog_iframe(url, caption, 560, 250);
        }

        function doSearch() {
            var qs = new Array();
            var value = $.trim($2("txtvalue").value);
            if (value.length > 0)
                qs.push("value=" + keyword);

            var website = $.trim($2("hidwebsite").value);
            if (website.length > 0)
                qs.push("website=" + website);

            var type = $("#drptype").val();
            if (type.length > 0)
                qs.push("type=" + type);

            //叠加或替换传递的参数。
            qs = common.addOrReplaceQS(qs);

            var qsText = "";
            $.each(qs, function (i, _qs) {
                qsText += _qs + "&";
            });

            if (qsText.length == 0) {
                window.location.reload();
                return;
            }

            qsText = qsText.substr(0, qsText.length - 1);
            location.href = "/OfficialAccount/OfficialAccountList?" + qsText;
        }
    </script>
</head>
<body style="padding: 3px;">
    <form id="form1" runat="server">
        <div class="widget-main ">
            <div class="tabbable">
                <ul class="nav nav-tabs" id="divTabStatus" runat="server">
                    <li class="<%=Request.QueryString["type"]+""==""?"active" :""%>"><a href="/OfficialAccount/OfficialAccountList">所有</a></li>
                    <li class="<%=Request.QueryString["type"]+""=="soft"?"active" :""%>"><a href="/OfficialAccount/OfficialAccountList?type=soft">软件</a></li>
                    <li class="<%=Request.QueryString["type"]+""=="news"?"active" :""%>"><a href="/OfficialAccount/OfficialAccountList?type=news">新闻</a></li>
                </ul>
                <div class="tab-content">
                    站点：
                <jk:MyTextBox runat="server" ID="txtwebsite" MaxLength="25" PlaceHolder="请选择" ClientIDMode="Static" Style="width: 160px;">
                </jk:MyTextBox>
                    <asp:HiddenField ID="hidwebsite" ClientIDMode="Static" runat="server" />
                    类别：
                <asp:DropDownList ID="drptype" runat="server" Width="150">
                    <asp:ListItem Text=" - 请选择 - " Value=""></asp:ListItem>
                    <asp:ListItem Text="软件" Value="soft"></asp:ListItem>
                    <asp:ListItem Text="新闻" Value="news"></asp:ListItem>
                </asp:DropDownList>
                    value：
                <jk:MyTextBox runat="server" IsInlineControl="True" ID="txtvalue" Style="width: 120px;" ClientIDMode="Static">
                </jk:MyTextBox>
                    &nbsp;&nbsp;<input id="btSearch" class="btn btn-blue" type="button" value="搜索" style="width: 80px;" onclick="doSearch();" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<a class="largeLink" href="javascript:void(0);" onclick="openDialog(true);">+ 添加公众号</a>
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
