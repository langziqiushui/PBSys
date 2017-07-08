//登录通用。
var login = {
    numVerCodeError: 0,
    init: function () {
        $(document).ready(function () {
            //divVerifyCodeStatus
            $("#divVerifyCodeStatus").bind("click", function () {
                $2("txtVerCode").value = "";
                $(this).hide();
            });

            //实时验证码验证。
            var txtcode = $("#txtVerCode");
            txtcode.bind("keyup blur", function () {
                if (txtcode.val().length != 4) {
                    $("#divVerifyCodeStatus").hide();
                    return;
                }

                jQuery.ajax({
                    type: "get",
                    url: "/API/MyHandler.ashx?method=ValidateVerCode&verType=AdminLogin&verCode=" + txtcode.val(),
                    success: function (data) {
                        if (data == "true") {
                            login.numVerCodeError = 0;
                            login.showVerifyCodeStatus(true);
                            login.beginLogin(true);
                        }
                        else {
                            login.showVerifyCodeStatus(false);

                            //错误次数加1，如果大于3次则重刷验证码。
                            login.numVerCodeError += 1;
                            if (login.numVerCodeError >= 3) {
                                verifyCode.loadImage();
                                login.numVerCodeError = 0;
                            }
                        }
                    }
                });
            });
        });
    },

    //显示验证码状态。
    showVerifyCodeStatus: function (isOk) {
        if (isOk)
            $2("divVerifyCodeStatus").className = "verifyCode_ok";
        else
            $2("divVerifyCodeStatus").className = "verifyCode_error";
        common.showPanel("txtVerCode", "divVerifyCodeStatus", 15, 14, -20, 9);
    },

    //隐藏验证码状态。
    hideVerifyCodeStatus: function () {
        var divVerifyCodeStatus = $2("divVerifyCodeStatus");
        if (null == divVerifyCodeStatus)
            return;
        divVerifyCodeStatus.style.display = "none";
    },

    //准备登录。
    beginLogin: function (autoPost) {
        var txtUserName = $2("txtUserName");
        if ($.trim(txtUserName.value).length == 0) {
            txtUserName.focus();
            return false;
        }

        var txtPassword = $2("txtPassword");
        if ($.trim(txtPassword.value).length == 0) {
            txtPassword.focus();
            return false;
        }
       
        if (autoPost) {
            //自动提交登录。
            $2("btLogin").click();
        }

        return true;
    },
    //忘记密码。
    retrievePassword: function () {
        var caption = "忘记密码";
        var url = "../Passport/RetrievePassword.aspx";
        common.openDialog_iframe(
            url,
            caption,
            570,
            400
        );
    }
};

//验证码。
var verifyCode = {
    loadImage: function () {
        var img = $2("imgVerifyCode");
        var r = common.createRandomChars()
        img.src = img.getAttribute("url").replace("&amp;", "&").replace("{0}", r);

        var txtVerCode = $2("txtVerCode");
        if (null != txtVerCode) {
            txtVerCode.value = "";
            txtVerCode.focus();
        }
    }
};

var _canGetCode = true; //全局变量，是否可以重新获取
//找回密码
var RetrievePassword = {
    Mobile: null, //修改密码绑定的手机号
    MCode: null, //修改密码手机的验证码
    //发送验证码
    SendCode: function () {        
        var _codeTimeLast = 10; //剩余10秒
        if (_canGetCode) {

            var _mobile = $.trim($("#txtMobile").val());
            if (_mobile == "") {
                common.alert("请输入手机号码");
                return;
            }
           
            var reg = /^\d{11}$/;
            if (!reg.test(_mobile)) {
                common.alert("手机号格式不正确！");
                return;
            }

            jQuery.ajax({
                type: "get",
                url: "/API/MyHandler.ashx?method=sendretrievepasscode&mobile=" + _mobile,
                success: function (data) {
                    
                }
            });
        }
    },
    //下一步 重设密码
    nexStep: function () {
        var _mobile = $("#txtMobile").val();
        if (_mobile == "") {
            common.alert("请输入手机号码");
            $("#txtMobile").focus();
            return;
        }
        var reg = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;

        if (!reg.test(_mobile)) {
            common.alert("手机号格式不正确！");
            $("#txtMobile").focus();
            return;
        }

        var $code = $("#txtcode");
        var reg = /^\d{4}$/;
        if ($code.val().trim() == "") {
            common.alert("请输入验证码！");
            $code.focus();
            return;
        }
        if (!reg.test($code.val())) {
            common.alert("验证码格式不正确！");
            $code.focus();
            return;
        }



        var sendData = { "method": "retrivepassvalidatecde", "mobile": _mobile, "code": $code.val(), "t": Math.random() * 10000 };
        $.ajax({
            url: "/API/MyHandler.ashx",
            dataType: 'json',
            data: sendData
        }).done(function (data) {
            if (data.r == "T") {               
                var caption = "忘记密码";
                var url = "SetNewPassword.aspx?mobile=" + _mobile + "&code=" + $code.val();
                common.openDialog_iframe(
            url,
            caption,
            570,
            400
        );
            }
            else {
                _canGetCode = true;
                common.alert(data.m);
                $("#a_sendcode").html("获取验证码");
            }
        });
    },
    funshow: function () {
        $("#divUsernametype").show();
    },
    //设置新密码
    updatePassword: function () {
        //用户名
        var username = "";

        //原发送验证码手机号码
        var mobile = RetrievePassword.Mobile;
        var usernamecount = $("#hid_usernamecount").val();

        //如果用户手机号码多个情况用户选择
        if (parseInt(usernamecount) > 1) {
            var rdusername = document.getElementsByName("username");
            for (var i = 0; i < rdusername.length; i++) {
                if (rdusername[i].checked) {
                    username = rdusername[i].value;
                }
            }

            //用户未选择的时候提示
            if (username.trim() == "") {
                common.alert("请选择您需要修改的用户名！");
                return;
            }
        } else {
            //默认用户选择原手机修改密码
            username = RetrievePassword.Mobile;
        }

        var $txtPassword = $("#txtPassword");
        var $txtComfirmPassword = $("#txtComfirmPassword");
        if ($txtPassword.val().trim() == "") {
            common.alert("请输入密码！");
            $txtPassword.focus();
            return;
        }
        if ($txtPassword.val().length < 6) {
            common.alert("密码最小长度为5位！");
            $txtPassword.focus();
            return;
        }
        if ($txtComfirmPassword.val().trim() == "") {
            common.alert("请输入确认密码！");
            $txtComfirmPassword.focus();
            return;
        }
        if ($txtComfirmPassword.val().length < 6) {
            common.alert("确认密码最小长度为5位！");
            $txtComfirmPassword.focus();
            return;
        }
        if ($txtComfirmPassword.val().trim() != $txtPassword.val().trim()) {
            common.alert("两次密码输入不一致！");
            $txtComfirmPassword.focus();
            return;
        }

        var sendData = { "method": "updatepassword", "mobile": mobile, "username": username, "code": RetrievePassword.MCode, "password": $txtPassword.val(), "comfirmpassword": $txtComfirmPassword.val(), "t": Math.random() * 10000 };
        $.ajax({
            url: "/API/MyHandler.ashx",
            dataType: 'json',
            data: sendData
        }).done(function (data) {
            if (data.r == "T") {
                common.alert("密码重置成功");
                setTimeout(function () {
                    window.location.href = "/passport/login";
                }, 1000);

            }
            else {
                _canGetCode = true;
                common.alert(data.m);
                $("#a_sendcode").html("获取验证码");
            }
        });
    }
}
