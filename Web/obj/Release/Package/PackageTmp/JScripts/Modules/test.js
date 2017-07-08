

$.ajax({
    type: "post",
    dataType: "json",
    url: "/API/MyHandler.ashx",
    ajaxData: {
        "method": "GetPBConfig",
        "id": "33690",
        "pt": "soft",
        "rand": Math.random() * 10000
    },
    //data: _this.options.ajaxData,
    timeout: 20 * 1000,
    success: function (d) {
        //{"r":"F","m":"非屏蔽"}
        //{"r":"T","m":"屏蔽"}
        //如果请求失败。   
        if (d.r != "T") {
            alert(d.m);
            return;
        }

        //请求成功。
      
        //绑定基础数据。
    }
});