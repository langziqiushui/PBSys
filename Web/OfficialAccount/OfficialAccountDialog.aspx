<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OfficialAccountDialog.aspx.cs" Inherits="YX.Web.OfficialAccount.OfficialAccountDialog" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公众号管理</title>
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

        function save() {

            if (!$("#hidwebsite").val()) {
                alert("请选择站点！");
                return false;
            }

            if (!$("#drptype").val()) {
                alert("请选择类别！");
                return false;
            }

            if (!$("#drpcolumn").val()) {
                alert("请选择字段！");
                return false;
            }

            if (!$("#txtvalue").val()) {
                alert("请填写value！");
                return false;
            }

            $.ajax({
                type: "POST",
                url: "/OfficialAccount/OfficialAccountDialog.aspx",
                data: {
                    action: "save",
                    id: $("#txtId").val(),
                    website: $("#txtwebsite").val(),
                    hidwebsite: $("#hidwebsite").val(),
                    type: $("#drptype").val(),
                    column: $("#drpcolumn").val(),
                    value: $("#txtvalue").val()
                },
                dataType: "text",
                success: function (data) {
                    if (data == "ok") {
                        alert("保存成功！");
                        window.parent.doSearch();
                        window.parent.common.closeDialog();
                    } else {
                        alert("保存失败！" + data);
                    }
                }
            });
        }
    </script>

    <style type="text/css">
        body::before {
            background-color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="txtId" runat="server" />
        <div class="dialog-container">
            <div id="divDataEditor">
                <dl class="de_field">
                    <dt><span>*</span> 站点：</dt>
                    <dd>
                        <jk:MyTextBox runat="server" ID="txtwebsite" MaxLength="25" ClientIDMode="Static"></jk:MyTextBox>
                        <asp:HiddenField ID="hidwebsite" ClientIDMode="Static" runat="server" />
                    </dd>
                </dl>
                <dl class="de_field">
                    <dt><span>*</span> 类别</dt>
                    <dd>
                        <asp:DropDownList runat="server" ID="drptype">
                            <asp:ListItem Text=" - 请选择 - " Value=""></asp:ListItem>
                            <asp:ListItem Text="软件" Value="soft">软件</asp:ListItem>
                            <asp:ListItem Text="新闻" Value="news">新闻</asp:ListItem>
                        </asp:DropDownList>
                    </dd>
                </dl>

                <dl class="de_field">
                    <dt><span>*</span> 字段</dt>
                    <dd>
                        <asp:DropDownList runat="server" ID="drpcolumn">
                            <asp:ListItem Text=" - 请选择 - " Value=""></asp:ListItem>
                            <asp:ListItem Text="分类ID" Value="cateid"></asp:ListItem>
                            <asp:ListItem Text="软件ID" Value="pk"></asp:ListItem>
                            <asp:ListItem Text="关键字" Value="keywords"></asp:ListItem>
                        </asp:DropDownList>
                    </dd>
                </dl>
                <dl class="de_field">
                    <dt><span>*</span> value：</dt>
                    <dd>
                        <jk:MyTextBox runat="server" ID="txtvalue">
                        </jk:MyTextBox>
                    </dd>
                </dl>
                <ol>
                    <jk:MyButton ID="btSave" runat="server" Text="保存" OnClientClick="save(); return false;" />
                    &nbsp;&nbsp;
                    <jk:MyButton ID="btCancel" runat="server" OnClientClick="window.parent.common.closeDialog();" Text="取消" />
                </ol>
            </div>
        </div>
    </form>
</body>
</html>
