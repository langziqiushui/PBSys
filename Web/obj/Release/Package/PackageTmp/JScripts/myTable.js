$(document).ready(function () {
 
});

//自定义表格JS。
var mt = {
    beginOverlayState: false,
    preFocusedRowID: null,

    //初始化。
    init: function (mtID) {
        $(document).ready(function () {
            //自然设置列宽。
            mt.autoWidth(mtID);

            //注册数据行事件。
            var trs = $("tr[id^='" + mtID + "_trSDI']");
            $.each(trs, function (i, tr) {
                //设置取模值为1的行不同的背景颜色。
                var $tr = $(tr);
                if (tr.getAttribute("_mValue") == "1") {
                    $tr.css("background-color", "#fbfbfb");
                    $tr.attr("_oBgColor", "#fbfbfb");
                }
                else {
                    $tr.attr("_oBgColor", "white");
                }

                //鼠标置上的背景色。
                $(tr).bind("mouseover", function () {
                    $.each(trs, function (i2, _tr) {
                        var $tr = $(_tr);
                        $tr.css("background-color", $tr.attr("_oBgColor"));
                    });

                    $(this).css("background-color", "#f2dede");
                });
                setTimeout(function () {

                }, 500);

                //行单击事件。
                $(tr).bind("click", function () {
                    //鼠标单击当前行的背景色。
                    $.each(trs, function (i2, _tr) {
                        _tr.className = "";
                    });
                    this.className = "myTable123_currentTr";

                    //设置当前行的主键值到隐藏域中。
                    var hidSDI = $("#" + this.id + " input[id^='" + mtID + "_hidSDI']")[0];
                    $2("hidPrimaryKeyData_" + mtID).value = hidSDI.value;

                    //查找自动隐藏行并设置相关功能。
                    $.each($("#" + mtID + " div[class='myTable123_aw']"), function () {
                        this.className = "myTable123_ah";
                    });

                    //间隔打开或关闭自动隐藏行。
                    if (mt.preFocusedRowID != this.id) {
                        //打开隐藏行。
                        $.each($("#" + this.id + " div[class='myTable123_ah']"), function () {
                            this.className = "myTable123_aw";
                        });
                        //设置为当前行编号。
                        mt.preFocusedRowID = this.id;
                    }
                    else {
                        //置空，以便下次能够打开。
                        mt.preFocusedRowID = null;
                    }

                    try {
                        //重新设置父框架的高度。
                        window.parent.resetMainframeHeight();
                    }
                    catch (e) { }
                });
            });
        });
    },

    //获取所有选择行的主键值(以@^@分隔)。
    getCheckedPrimaryKeyValue: function (mtID) {
        var v = "";
        $.each($("input[id^='" + mtID + "_chkChecked']"), function (i, chk) {
            if (chk.checked)
                v += chk.parentNode.getAttribute("__primarykeydata") + "@^@";
        });

        return v;
    },

    //跳转到指定分页。
    goPager: function (urlFormat, txtPagerID, pageCount) {
        var txtPager = $2(txtPagerID);
        var v = $.trim(txtPager.value);
        if (v.length == 0) {
            txtPager.focus();
            return false;
        }

        var pattern = /[1-9]\d*/;
        if (!pattern.test(v)) {
            txtPager.value = "";
            txtPager.focus();
            return false;
        }

        if (parseInt(v) > pageCount) {
            txtPager.value = pageCount;
            v = pageCount;
        }

        window.location.href = urlFormat.replace("{0}", v);
    },

    //选择或取消所有选择。
    checkAll: function (chk, mtID) {
        $.each($("#" + mtID + " input[id^='" + mtID + "_chkChecked']"), function () {
            this.checked = chk.checked;
        });
    },

    //自然设置列宽。
    autoWidth: function (mtID) {
        //列宽度是以1280分辨率为基准，Table的myTable123_rc宽度1000。
        //获取myTable123_rc宽度。
        var sgWidth = $(".myTable123_rc")[0].offsetWidth;
        var bl = 1;
        if (sgWidth > 980) {
            bl = parseFloat(sgWidth / 1200);
        }

        //获取自动宽度列。
        var tdNoSizeID = mtID + "_tdNoSize";
        var tdNoSize = $2(tdNoSizeID);

        var maxWidth = 0;
        var totalWidth = 0;
        $.each($("#" + mtID + " tr[class='myTable123_rc'] td"), function (i, td) {
            //自动宽度列。
            if (td.id == tdNoSizeID)
                return;

            //隐藏列。       
            var styleText = $(td).attr("style");
            if (null != styleText) {
                if (styleText.toLowerCase().indexOf("display:none") >= 0)
                    return;
            }

            var tdWidth = parseInt(td.getAttribute("__myWidth"));
            if (bl > 1) {
                tdWidth = parseInt(tdWidth * bl);
                td.style.width = tdWidth + "px";
            }
            totalWidth += tdWidth + 3;
        });

        //如果totalWidth为0，说明未匹配到width。
        if (totalWidth == 0)
            return;

        var w = (sgWidth - totalWidth) + "px";
        if (null != tdNoSize)
            tdNoSize.style.width = w;
        $.each($("#" + mtID + " div[class='myTable123_ah']"), function (i, div) {
            div.style.width = w;
        });
    },

    //重置搜索条件。
    resetSearch: function () {
        $.each($(".mySearch98562 input[type='text']"), function (i, item) {
            item.value = "";
        });

        $.each($(".mySearch98562 select"), function (i, item) {
            item.options.selectedIndex = 0;
        });
    },

    //往前或往后推一天。
    //isNext 为true为往后推一天；反之
    npDate: function (isNext, txtStartDateID, txtEndDateID) {
        var n = isNext ? 1 : -1;
        var txtStartDate = $2(null != txtStartDateID ? txtStartDateID : "txtStartDate");
        var txtEndDate = $2(null != txtEndDateID ? txtEndDateID : "txtEndDate");

        txtStartDate.value = common.addDay(txtStartDate.value, n);
        txtEndDate.value = common.addDay(txtEndDate.value, n);
    }
};
