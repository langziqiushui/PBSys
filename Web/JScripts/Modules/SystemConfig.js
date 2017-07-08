//日志管理。
var logList = {
    init: function () {
        common.kd("txtKeyword", logList.beginSearch);

        $('#txtApplicationType').dataSelector({
            ajaxData: {
                "method": "RenderDataSource",
                "dsType": "siteCategory",               
                "rand": Math.random() * 10000
            },
            ajaxURL: '/API/MyHandler.ashx',
            width: 380,
            originalValue: null,
            onDataSelected: function (t, v) {
                
                $2("txtApplicationType").value = t;
                $2("hidApplicationType").value = v;
            }
        });
    },

    //准备搜索。
    beginSearch: function () {
        var qs = new Array();
        var keyword = $.trim($2("txtKeyword").value);
        if (keyword.length > 0)
            qs.push("k=" + keyword);

        var sd = $.trim($2("txtStartDate").value);
        if (sd.length > 0)
            qs.push("sd=" + sd);

        var ed = $.trim($2("txtEndDate").value);
        if (ed.length > 0)
            qs.push("ed=" + ed);

        var c = $.trim($2("hidApplicationType").value);
        if (c.length > 0)
            qs.push("c=" + c);

        var rs = $("#drRestype").val();
        if (rs.length > 0)
            qs.push("rs=" + rs);

        //叠加或替换传递的参数。
        qs = common.addOrReplaceQS(qs);

        var qsText = "";
        $.each(qs, function (i, _qs) {
            qsText += _qs + "&";
        });

        if (qsText.length == 0)
            return;

        qsText = qsText.substr(0, qsText.length - 1);
        location.href = "/Admin/Glo/LogList?" + qsText;
    }
};


//屏蔽管理。
var pbConfigList = {
    init: function () {
        common.kd("txtKeyword", pbConfigList.beginSearch);

        $('#txtApplicationType').dataSelector({
            ajaxData: {
                "method": "RenderDataSource",
                "dsType": "siteCategory",
                "rand": Math.random() * 10000
            },
            ajaxURL: '/API/MyHandler.ashx',
            width: 380,
            originalValue: null,
            onDataSelected: function (t, v) {

                $2("txtApplicationType").value = t;
                $2("hidApplicationType").value = v;
            }
        });
    },

    //准备添加或修改。
    beginAddOrModify: function (isAdd, id) {
        var caption = isAdd ? "添加屏蔽数据" : "修改屏蔽数据";
        var url = "/Admin/Glo/Dialog/PBConfigDialog.aspx";
        if (!isAdd)
            url += "?id=" + id;

        common.openDialog_iframe(
            url,
            caption,
            560,
            250
        );
    },
   

    //准备搜索。
    beginSearch: function () {
        var qs = new Array();
        var keyword = $.trim($2("txtKeyword").value);
        if (keyword.length > 0)
            qs.push("k=" + keyword);

        var c = $.trim($2("hidApplicationType").value);
        if (c.length > 0)
            qs.push("c=" + c);

        //var v = $("#drIsNormal").val();
        //if (lt.length > 0)
        //    qs.push("v=" + v);

        var lt = $("#drPbType").val();
        if (lt.length > 0)
            qs.push("lt=" + lt);

        //叠加或替换传递的参数。
        qs = common.addOrReplaceQS(qs);

        var qsText = "";
        $.each(qs, function (i, _qs) {
            qsText += _qs + "&";
        });

        if (qsText.length == 0)
            return;

        qsText = qsText.substr(0, qsText.length - 1);
        location.href = "/Admin/Glo/PBConfigList?" + qsText;
    }
};


//屏蔽类别编辑。
var pbConfigDialog = {
    //初始化。
    init: function () {
        $('#txtApplicationType').dataSelector({
            ajaxData: {
                "method": "RenderDataSource",
                "dsType": "siteCategory",
                "rand": Math.random() * 10000
            },
            ajaxURL: '/API/MyHandler.ashx',
            width: 380,
            originalValue: null,
            onDataSelected: function (t, v) {
                $2("txtApplicationType").value = t;
                $2("hidApplicationType").value = v;
            }
        });
    },
    
};



//操作日志管理。
var operateLogList = {
    init: function () {
        common.kd("txtKeyword", operateLogList.beginSearch);
    },

    //准备搜索。
    beginSearch: function () {
        var qs = new Array();
        var keyword = $.trim($2("txtKeyword").value);
        if (keyword.length > 0)
            qs.push("k=" + keyword);

        var t = $("#drOperateType").val();
        if (t.length > 0)
            qs.push("t=" + t);

        var sd = $.trim($2("txtStartDate").value);
        if (sd.length > 0)
            qs.push("sd=" + sd);

        var ed = $.trim($2("txtEndDate").value);
        if (ed.length > 0)
            qs.push("ed=" + ed);

        var qsText = "";
        $.each(qs, function (i, _qs) {
            qsText += _qs + "&";
        });

        if (qsText.length == 0)
            return;

        qsText = qsText.substr(0, qsText.length - 1);
        location.href = "/Admin/Glo/OperateLogList.aspx?" + qsText;
    }
};