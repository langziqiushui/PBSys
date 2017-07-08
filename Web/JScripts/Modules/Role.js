//角色列表。
var roleList = {
    init: function () {
        common.kd("txtKeyword", roleList.beginSearch);
    },

    //准备搜索。
    beginSearch: function () {
        var qs = new Array();
        var keyword = $.trim($2("txtKeyword").value);
        if (keyword.length > 0)
            qs.push("k=" + keyword);

        var qsText = "";
        $.each(qs, function (i, _qs) {
            qsText += _qs + "&";
        });

        if (qsText.length == 0)
            return;

        qsText = qsText.substr(0, qsText.length - 1);
        location.href = "/Admin/Roles/RoleList.aspx?" + qsText;
    }
};

//角色管理。
var role = {
    //取消全选。
    unCheckAll: function (isChecked) {
        $.each($("#tabPermission input[type='checkbox']"), function (i, childChk) {
            childChk.checked = false;
        });
    },

    //选择或取消选择子对象。
    checkedOrCancelChild: function (chk) {
        var id = chk.parentNode.parentNode.id;
        $.each($("#" + id + " input[type='checkbox']"), function (i, childChk) {
            if (chk.id != childChk.id)
                childChk.checked = chk.checked;
        });
    },

    //准备创建新角色。
    beginCreateRole: function () {
        if ($.trim($2("txtRoleName").value).length < 4) {
            common.showPop("txtRoleName", "角色名称 不能少于4个字符！", true);
            return false;
        }

        var perIDs = "";
        $.each($("input[id^='chkPermission_']"), function (i, chk) {
            if (chk.checked)
                perIDs += chk.getAttribute("perValue") + "|";
        });
        if (perIDs.length == 0) {
            common.alert("对不起，请至少勾选一个权限！", "操作错误");
            return false;
        }

        if (!confirm("温馨提示：您将要保存当前角色，是否确定？"))
            return false;

        $2("hidPermissionIDs").value = perIDs;
        return true;
    }
}


//分配角色。
var setRole = {
    init: function () {
        $(document).ready(function () {
            $("#chkCheckOrCancelRoles").bind("change", function () {
                var _this = this;
                $.each($("#chkRoles input[type='checkbox']"), function (i, chk) {
                    chk.checked = _this.checked;
                });
            });
        });
    },

    //全部取消管理员选中状态。
    checkOrUncheckAdmin: function (checked) {
        $.each($("#chkAdmin input[type='checkbox']"), function (i, chk) {
            chk.checked = checked;
        });
    }
}
