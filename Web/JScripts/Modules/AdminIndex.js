//供应首页相关JS。
var si = {  
    //准备添加或修改提现银行卡。
    beginAddOrModifyBank: function (isAdd, id) {
        var caption = isAdd ? "添加提现银行卡" : "修改提现银行卡";
        var url = "/Admin/Credits/Dialog/BankDialog.aspx";
        if (!isAdd)
            url += "?id=" + id;

        common.openDialog_iframe(
            url,
            caption,
            570,
            400
        );
    },

    //提现申请。
    beginAddCashApply: function () {
        common.openDialog_iframe(
            "/Admin/Credits/Dialog/CashApplyDialog.aspx",
            "提现申请",
            570,
            480
        );
    }
};




var branchIndex = {
    init: function () {
        //二维码
        $(document).ready(function () {
            var $divDimCode = $("#i_show_code");
            $divDimCode.bind("mouseover", function () {
                $("#div_show_code").slideDown(100);
            });

            $divDimCode.bind("mouseleave", function () {
                $("#div_show_code").hide();
            });
        });
    }
};