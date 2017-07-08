//扩展$2为document.getElementById。
var $2 = function (id) { return document.getElementById(id); }

$().ready(function () {
    //双击隐藏所有浮动div。
    $(document).bind("dblclick", function () {
        common.hideAllPopupPanel();
    });

    //添加asp.net ajax事件。
    if ("undefined" != typeof (Sys) && "undefined" != typeof (Sys.WebForms) && "undefined" != typeof (Sys.WebForms.PageRequestManager)) {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(common.beginAjaxRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(common.endAjaxRequestHandler);
    }
});

var common = {
    divWarning_default: null, //页面警告提示DIV。
    divErrorPanels: new Array(), //保存页面错误提示DIV的集合。
    divContent_default: null, //内容提示DIV。
    popupSourceElementID: null, //弹出div的对象的源对象ID。
    popupElements: new Array(), //弹出div的ID。
    panelZIndex: 99999999,
    latestOverlayRelObject: null,   //最后一个弹出的overlay rel引用的jQuery对象。
    lastAjaxRequestElement: null,  //最后一次触发PostBack的按钮对象。

    divPopupContainer: null,  //弹出警告框jq对象。
    divPopupOK: null,
    divPopupCancel: null,
    divPopupCancelContainer: null,

    //当页面载入完毕后执行某个方法。
    onReady: function (fun) {
        $(document).ready(function () {
            if (null != fun && window.fun)
                fun();
        });
    },

    //当asp.net ajax请求开始时。
    beginAjaxRequestHandler: function (sender, args) {
        var $elem = $(args.get_postBackElement());
        common.lastAjaxRequestElement = $elem;

        $elem[0].disabled = "disabled";
        $elem.attr("_oValue", $elem.val());
        $elem.attr("_oBgColor", $elem.css("background-color"));
        $elem.css("background-color", "#b5b3b3");
        var requestText = $elem.attr("_requestText");
        $elem.val(null != requestText ? requestText : "处理中..");
    },

    //当asp.net ajax请求结束时。
    endAjaxRequestHandler: function (sender, args) {
        if (null == common.lastAjaxRequestElement)
            return;

        common.lastAjaxRequestElement.val(common.lastAjaxRequestElement.attr("_oValue"));
        common.lastAjaxRequestElement[0].disabled = "";
        common.lastAjaxRequestElement.css("background-color", common.lastAjaxRequestElement.attr("_oBgColor"));
    },

    //显示消息。
    //message 消息内容
    //title 标题
    //callback 触发方法
    info: function (message, title, callback) {
        common.__alert(message, title, "info", callback, false);
    },

    //弹出警告。
    alert: function (message, title, callback) {
        common.__alert(message, title, "warning", callback, false);
    },

    //弹出错误。
    error: function (message, title, callback) {
        common.__alert(message, title, "error", callback, false);
    },

    //确认对话框。
    //message 消息
    //okCallback 确定回发事件
    //cancelCallback 取消回发事件
    confirm: function (message, okCallback, cancelCallback) {
        $.dialog.confirm(message, function () {
            if (null != okCallback)
                okCallback();
        }, function () {
            if (null != cancelCallback)
                cancelCallback();
        });
    },

    //弹出警告框 。
    //message 消息内容
    //title 消息标题
    //type 消息类型(取值为info,warning,error,confirm)
    //callback 当点击确定按钮时将要执行的方法
    //isConfirm 是否为确认对话框。
    __alert: function (message, title, type, callback, isConfirm) {
        if (null == title || title.length == 0)
            title = "系统提示";

        var icon = "alert.gif";
        if (type == "error" || type == "confirm")
            icon = type + ".gif";

        $.dialog({
            zIndex: 999999,
            min: false,
            max: false,
            fixed: true,
            lock: true,
            resize: false,
            icon: icon,
            width: 350,
            title: title,
            content: "<div style=\"width:350px; text-align:left !important;\">" + message + "</div>",
            cancelVal: '关闭',
            cancel: true,
            close: function () {
                if (null != callback)
                    callback();
            }
        });
    },

    //打开一个带IFrame的模态窗口。
    //divID 包含iframe的div的ID.
    //src iframe的引用网址。
    //title 标题
    //iframeWidth 
    //iframeHeight
    lastLHDialog: null, //最近一次LH弹窗。
    openDialog_iframe: function (src, title, iframeWidth, iframeHeight) {
        common.lastLHDialog = $.dialog({
            id: "divIFrameDialog003893903",
            lock: true,
            resize: true,
            width: iframeWidth,
            height: iframeHeight,
            title: title,
            fixed: true,
            content: 'url:' + src
        });
    },

    //打开模态窗口。
    //divID  弹出的div的ID
    //htmlContent 包含在divID中的html内容。
    //title 标题
    openDialog: function (htmlContent, title, width, height) {
        common.lastLHDialog = $.dialog({
            id: "divDialog859965266",
            lock: true,
            resize: true,
            width: width,
            height: height,
            title: title,
            content: htmlContent,
            cancelVal: '关闭',
            fixed: true,
            cancel: true
        });
    },

    //关闭模态窗口。
    closeDialog: function (callback) {
        try {
            if (null != common.lastLHDialog)
                common.lastLHDialog.close();
        }
        catch (e) { }
    },

    //刷新父页面。
    refurParentPage: function (parentPageUrl) {
        if (null != window.parent.frames['fraMain'])
            window.parent.frames['fraMain'].location.reload();
        else
            window.parent.location.reload();
    },

    //父页面跳转至指定URL.
    parentPageLocationTo: function (href) {
        if (null != window.parent.frames['fraMain'])
            window.parent.frames['fraMain'].location.href = href;
        else
            window.parent.location.href = href;
    },

    //是否为子节点。
    isChildNode: function (parentID, oElement) {
        var _oElement = oElement;
        while (true) {
            if (null != _oElement.id && _oElement.id == parentID)
                return true;

            _oElement = _oElement.parentNode;
            if (null == _oElement)
                return false;
        }

        return false;
    },

    //显示错误信息。
    //olID：依赖于显示位置的控件ID。
    //msg：显示的消息(html)。
    //autoHidden 是否自动隐藏
    showPop: function (olID, msg, autoHidden) {
        var divPop = common.createPopPanel();
        if (null != msg)
            msg = common.formatMessage(msg);
        $2(divPop.id + "_content").innerHTML = msg;
        divPop.className = "pop_Wrap";
        var ol = $2(olID);
        ol.focus();
        common.showPanel(olID, divPop.id, 220, null, -ol.offsetWidth, ol.offsetHeight + 2);
        ol.focus();

        if (null != autoHidden && autoHidden) {
            setTimeout(function () {
                common.hidePanel(divPop.id);
            }, 3000);
        }
        return divPop.id;
    },

    //创建提示的面板。
    createPopPanel: function () {
        //先从错误提示的集合中查找可用的面板。      
        var id = "divPop956325644";
        var div = $2(id);
        if (null != div)
            return div;

        var html = "<div id=\"" + id + "\" class=\"pop_Wrap\"><p class=\"fa  fa-caret-up\"></p><ul id=\"" + id + "_content\"></ul></div>";
        $("body").append(html);
        return $2(id);
    },

    //隐藏Pop。
    hidePop: function () {
        var div = $2("divPop956325644");
        if (null != div)
            div.style.display = "none";
    },

    //显示某一提示消息对象。
    //olID 作为参照的对象ID。
    //panelID div面板ID。
    //width div面板宽。
    //height div面板高。
    //extraX：增加额外的x轴(left)坐标。
    //extraY：增加额外的Y轴(top)坐标。
    showPanel: function (olID, panelID, width, height, extraX, extraY, animate) {
        if (null == width)
            width = 250;
        if (null == height)
            height = 0;
        if (null == extraX)
            extraX = 0;
        if (null == extraY)
            extraY = 0;

        //获取指定对象的坐标。
        var ol = $("#" + olID);
        var divPanel = $("#" + panelID);
        divPanel[0].setAttribute("targetElement", olID);
        if (width > 0)
            divPanel.css("width", width + "px");
        if (height > 0)
            divPanel.css("height", height + "px");

        var _____pagesize12698 = common.getPageSize();

        var offset = ol.offset();
        var locationX = offset.left + ol[0].offsetWidth + extraX;
        var locationY = offset.top + extraY;

        common.panelZIndex += 1;
        divPanel.css("z-index", common.panelZIndex);
        divPanel.css("left", locationX + "px");
        divPanel.css("top", locationY + "px");

        if (null == animate) {
            divPanel.show();
        } else {
            switch (animate) {
                case "slideDown":
                    divPanel.slideDown(100);
                    break;
                case "fadeIn":
                    divPanel.fadeIn("fast");
                    break;
            }
        }

        //判断是否超出页面宽度。
        var divPanelWidth = divPanel[0].offsetWidth;
        var xw = locationX + divPanelWidth - _____pagesize12698[0];
        if (xw > 0) {
            //由于向下偏，所以换一个样式。
            if (divPanel.attr("class").indexOf("myHint_left") >= 0)
                divPanel.attr("class", "myHint_right");
            else if (divPanel.attr("class").indexOf("myWarning_left") >= 0)
                divPanel.attr("class", "myWarning_right");
            if (divPanel.attr("class").indexOf("myHint_right") >= 0 || divPanel.attr("class").indexOf("myWarning_right") >= 0) {
                locationX = offset.left - divPanel[0].offsetWidth - 6;
                locationY = offset.top + extraY;
            }
            else {
                locationX = locationX - xw - 5;
                locationY = offset.top + ol[0].offsetHeight + 2;
            }

            //重新设置坐标。
            divPanel.css("left", locationX + "px");
            divPanel.css("top", locationY + "px");
        }

        common.lastPopLocationY = locationY;
    },

    //隐藏警告信息。
    hidePanel: function (panelID) {
        if (null != panelID) {
            var p = $2(panelID);
            if (null != p)
                p.style.display = "none";
        }
    },

    //隐藏所有弹出信息面板。
    hideAllPopupPanel: function () {
        $("div[class='pop_Wrap']").css("display", "none");
    },

    //格式化消息。
    //msg 消息内容。
    formatMessage: function (msg) {
        var pattern = /(\n)|(\\n)/;
        var arr = msg.split(pattern);
        msg = "";
        for (var i = 0; i < arr.length; i++) {
            if ($.trim(arr[i]).length > 0 && $.trim(arr[i]) != "\\n")
                msg += "<li>• " + arr[i] + "</li>";
        }

        return msg;
    },

    //获取页面上部滚动条的高度。
    getPageScroll: function () {
        var yScroll;
        if (self.pageYOffset) {
            yScroll = self.pageYOffset;
        } else if (document.documentElement && document.documentElement.scrollTop) { // Explorer 6 Strict 
            yScroll = document.documentElement.scrollTop;
        } else if (document.body) {// all other Explorers 
            yScroll = document.body.scrollTop;
        }

        return yScroll;
    },

    //获取页面尺寸。
    getPageSize: function () {
        var xScroll, yScroll;
        if (window.innerHeight && window.scrollMaxY) {
            xScroll = window.innerWidth + window.scrollMaxX;
            yScroll = window.innerHeight + window.scrollMaxY;
        } else if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac
            xScroll = document.body.scrollWidth;
            yScroll = document.body.scrollHeight;
        } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
            xScroll = document.body.offsetWidth;
            yScroll = document.body.offsetHeight;
        }
        var windowWidth, windowHeight;
        if (self.innerHeight) {	// all except Explorer
            if (document.documentElement.clientWidth) {
                windowWidth = document.documentElement.clientWidth;
            } else {
                windowWidth = self.innerWidth;
            }
            windowHeight = self.innerHeight;
        } else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
            windowWidth = document.documentElement.clientWidth;
            windowHeight = document.documentElement.clientHeight;
        } else if (document.body) { // other Explorers
            windowWidth = document.body.clientWidth;
            windowHeight = document.body.clientHeight;
        }
        // for small pages with total height less then height of the viewport
        if (yScroll < windowHeight) {
            pageHeight = windowHeight;
        } else {
            pageHeight = yScroll;
        }
        // for small pages with total width less then width of the viewport
        if (xScroll < windowWidth) {
            pageWidth = xScroll;
        } else {
            pageWidth = windowWidth;
        }

        arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight);
        return arrayPageSize;
    },

    //生成随机码。
    createRandomChars: function () {
        return Math.round(Math.random() * 10000);
    },

    //返回 1970 年 1 月 1 日至今的毫秒数。
    getTicks: function () {
        return new Date().getTime();
    },

    //在FRAME中打开某个网址。
    openInFrame: function (frameID, url) {
        var n = false;
        try {
            var ev = window.event;
            if (ev.shiftKey)
                n = true;
        }
        catch (err) { }

        if (n)
            window.open(url, "newWindow");
        else
            $("#" + frameID).attr("src", url);

        return n;
    },

    //模拟生成GUID号。
    createGuid: function () {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                guid += "-";
        }
        return guid;
    },

    //获取某一对象的坐标。
    getLocation: function (oElementID) {
        var obj = new Object();
        var ol = $("#" + oElementID);

        var offset = ol.offset();
        obj.X = offset.left;
        obj.Y = offset.top;

        return obj;
    },

    //按回车键触发相关操作。
    //funOnEnter  当按回车键时将要执行的方法
    //funOthers  当按非回车键时将要执行的访求
    enterKeyDown: function (e, funOnEnter, funOthers) {
        var ev = window.event || e;
        if (ev.keyCode == 13) {
            funOnEnter();
            return false;
        }
        else if (null != funOthers) {
            funOthers();
        }

        return true;
    },

    //===================对字符串进行和asp.net Server.UrlEncode相同方式的编码=================================================================//
    //对字符串进行和asp.net Server.UrlEncode相同方式的编码。
    EncodeURI: function (unzipStr, isCusEncode) {
        if (isCusEncode) {
            var zipArray = new Array();
            var zipstr = "";
            var lens = new Array();
            for (var i = 0; i < unzipStr.length; i++) {
                var ac = unzipStr.charCodeAt(i);
                zipstr += ac;
                lens = lens.concat(ac.toString().length);
            }
            zipArray = zipArray.concat(zipstr);
            zipArray = zipArray.concat(lens.join("O"));
            return zipArray.join("N");
        } else {
            //return encodeURI(unzipStr);
            var zipstr = "";
            var strSpecial = "!\"#$%&'()*+,/:;<=>?[]^`{|}~%";
            var tt = "";

            for (var i = 0; i < unzipStr.length; i++) {
                var chr = unzipStr.charAt(i);
                var c = common.StringToAscii(chr);
                tt += chr + ":" + c + "n";
                if (parseInt("0x" + c) > 0x7f) {
                    zipstr += encodeURI(unzipStr.substr(i, 1));
                } else {
                    if (chr == " ")
                        zipstr += "+";
                    else if (strSpecial.indexOf(chr) != -1)
                        zipstr += "%" + c.toString(16);
                    else
                        zipstr += chr;
                }
            }
            return zipstr;
        }
    },
    //对字符串进行和asp.net Server.UrlDecode相同方式的解码。
    DecodeURI: function (zipStr, isCusEncode) {
        if (isCusEncode) {
            var zipArray = zipStr.split("N");
            var zipSrcStr = zipArray[0];
            var zipLens;
            if (zipArray[1]) {
                zipLens = zipArray[1].split("O");
            } else {
                zipLens.length = 0;
            }

            var uzipStr = "";

            for (var j = 0; j < zipLens.length; j++) {
                var charLen = parseInt(zipLens[j]);
                uzipStr += String.fromCharCode(zipSrcStr.substr(0, charLen));
                zipSrcStr = zipSrcStr.slice(charLen, zipSrcStr.length);
            }
            return uzipStr;
        } else {
            //return decodeURI(zipStr);
            var uzipStr = "";

            for (var i = 0; i < zipStr.length; i++) {
                var chr = zipStr.charAt(i);
                if (chr == "+") {
                    uzipStr += " ";
                } else if (chr == "%") {
                    var asc = zipStr.substring(i + 1, i + 3);
                    if (parseInt("0x" + asc) > 0x7f) {
                        uzipStr += decodeURI("%" + asc.toString() + zipStr.substring(i + 3, i + 9).toString()); ;
                        i += 8;
                    } else {
                        uzipStr += common.AsciiToString(parseInt("0x" + asc));
                        i += 2;
                    }
                } else {
                    uzipStr += chr;
                }
            }
            return uzipStr;
        }
    },
    StringToAscii: function (str) {
        return str.charCodeAt(0).toString(16);
    },
    AsciiToString: function (asccode) {
        return String.fromCharCode(asccode);
    },
    //===================end=================================================================//

    //重新设置图片大小。
    resizeImage: function (imgID, maxWidth, maxHeight) {
        common.resizeImage2($2(imgID), maxWidth, maxHeight);
    },

    //重新设置图片大小。
    resizeImage2: function (img, maxWidth, maxHeight) {
        if (null == img)
            return;

        var ow = img.offsetWidth;
        var oh = img.offsetHeight;
        var imageRate = img.offsetWidth / img.offsetHeight;

        if (img.offsetWidth > maxWidth) {
            ow = maxWidth;
            oh = maxWidth / imageRate;

            if (oh > maxHeight) {
                ow = maxHeight * imageRate;
                oh = maxHeight;
            }
        }

        if (img.offsetHeight > maxHeight) {
            ow = maxHeight * imageRate;
            oh = maxHeight;

            if (ow > maxWidth) {
                ow = maxWidth;
                oh = maxWidth / imageRate;
            }
        }

        img.style.width = ow + "px";
        img.style.Height = oh + "px";
    },

    //是否为本地网域。
    isLocal: function () {
        return window.location.href.toLowerCase().indexOf("localhost") >= 0;
    },

    //如果是本地则显示错误警告(e为捕捉到的错误， errorMsg为预先输入的错误提示)。
    alertOnLocal: function (e, errorMsg) {
        //        if (!common.isLocal())
        //            return;
        //        if (null == errorMsg)
        //            errorMsg = ""
        //        else
        //            errorMsg += "："

        //        if ($.browser.msie)
        //            errorMsg += e.description;
        //        else
        //            errorMsg += e;

        //        alert(errorMsg);
    },

    //===================注册客户端验证=================================================================//   

    //注册输入文本框。
    //olID  输入文本框的编号。
    regInput: function (olID) {
        $().ready(function () {
            var ol = $2(olID);

            if (ol.tagName.toLowerCase() == "textarea") {
                ol.className = "textarea";
                var parent = ol.parentNode;
                if (null == parent) {
                    alert("对不起，MultiLine的TextBox外面必须套一个服务器HtmlGenericControl对象。");
                    return;
                }

                var className = $.trim(ol.getAttribute("Message")).length == 0 ? "inputText550" : "inputTextTip550";
                parent.className = className;
                ol.setAttribute("__oc", className);
            }
            else {
                var n2 = "250";
                if (ol.className.indexOf("350") >= 0)
                    n2 = "350";
                ol.className = $.trim(ol.getAttribute("Message")).length == 0 ? "inputText" + n2 : "inputTextTip" + n2;
                ol.setAttribute("__oc", ol.className);
            }

            $(ol).bind("focus", function () {
                common.inputFocus(this);
            });
            $(ol).bind("blur", function () {
                common.inputBlur(this);
            });
        });
    },

    //注册按钮。
    //olID 按钮编号。
    regButton: function (olID) {
        $().ready(function () {
            var jqOL = $("#" + olID);
            jqOL[0].className = "inputButton";
            jqOL.bind("mouseover", function () {
                this.className = "focusInputButton";
            });
            jqOL.bind("mouseout", function () {
                this.className = "inputButton";
            });
        });
    },

    //当点击文本框触焦时。
    inputClick: function (ol) {
        //显示提示消息。
        var message = $.trim(ol.getAttribute("Message"));
        if (message.length > 0)
            common.showPop(ol.id, message);
        else
            common.hidePop();
    },

    //当输入文本框触焦时。
    inputFocus: function (ol) {
        //如果存在myTextBoxFocus这个函数，则调用。
        if (window.myInputFocus)
            myInputFocus(ol);
    },

    //当输入文本框失焦时。
    //isSubmit:是否为表单提交时验证。
    inputBlur: function (ol, isSubmit) {
        common.hidePop();
        //判断是否为输入文本框。
        var isInputText = (ol.tagName.toUpperCase() == "INPUT" && ol.type.toUpperCase() == "TEXT") || (ol.tagName.toUpperCase() == "INPUT" && ol.type.toUpperCase() == "PASSWORD") || ol.tagName.toUpperCase() == "TEXTAREA";

        //自动调用asp.net的验证。
        /*
        item：
        inputControlID 为控件(如TextBox)的ClientID
        rfvID 为必填验证控件的ClientID。
        cvID 为比较验证控件信息的ClientID。
        revIDs 为正则式验证控件信息的ClientID数组。
        var _____parms = {ControlID : "txtPassword", warningMessage : "密码为6位字符；不区分大小写", rfvID : "rfvPassword", cvID : "cvPassword", revIDs : ["revPassword1", "revPassword2"] };
        */
        var vadSuccess = true;
        if (window._____vad_parms) {
            $.each(_____vad_parms, function (i, item) {
                if (item.inputControlID == ol.id && !ol.disabled) {
                    vadSuccess = false;
                    //如果验证成功。
                    if (common.validateInput(item, isSubmit)) {
                        vadSuccess = true;

                        //如果存在myTextBoxBlur这个函数，则调用。
                        if (vadSuccess && window.myInputBlur)
                            vadSuccess = myInputBlur(ol);
                    }

                    //如果验证不成功，高亮显示错误的文本框。
                    if (!vadSuccess && isInputText)
                        common.setInputClass(ol, "e");
                }
            });
        }

        //设置为验证成功状态。
        if (vadSuccess && isInputText) {
            common.setInputClass(ol, "s");
        }

        return vadSuccess;
    },

    //验证用户输入。
    //parm参数结构同上。   
    //isSubmit：是否为表单提交时验证。
    validateInput: function (parm, isSubmit) {
        //获取当前处于焦点的文本框。
        var ol = $2(parm.inputControlID);
        if (null == ol)
            return true;

        //获取控件值。
        var _inputValue = "";
        var tagName = ol.tagName.toUpperCase();
        if (tagName == "INPUT" || tagName == "TEXTAREA") {
            //文本框。
            _inputValue = $.trim(ol.value);
        }
        else if (tagName == "SELECT") {
            //Select。
            if (ol.options.length > 0)
                _inputValue = $.trim(ol.options[ol.selectedIndex].value);
        }
        else if (tagName == "TABLE") {
            //RadioButtonList
            ol = $2(ol.id + "_0");
            if (null == ol)
                return true;
            _inputValue = $("input[name='" + ol.name + "']:checked").val();
            if (null == _inputValue)
                _inputValue = "";
        }

        //必填项判断。
        if ((null != isSubmit && isSubmit) && _inputValue.length == 0) {
            var rfv = null;
            if (null != parm.rfvID && $.trim(parm.rfvID).length > 0)
                rfv = $2(parm.rfvID);
            if (null != rfv) {
                //当验证失败，执行嵌入方法。
                if (typeof (onValidateInputFail98562) != "undefined")
                    onValidateInputFail98562(ol); common.showPop(parm.inputControlID, rfv.errormessage, true);
                return false;
            }
        }

        if (_inputValue.length > 0) {
            //验证正则式。  
            if (null != parm.revIDs && parm.revIDs.length > 0) {
                for (var i = 0; i < parm.revIDs.length; i++) {
                    var rev = $2(parm.revIDs[i]);
                    if (null == rev)
                        continue;

                    var r = new RegExp(rev.validationexpression);
                    if (!r.test(_inputValue)) {
                        //当验证失败，执行嵌入方法。
                        if (typeof (onValidateInputFail98562) != "undefined")
                            onValidateInputFail98562(ol); common.showPop(parm.inputControlID, rev.errormessage, true);
                        return false;
                    }
                }
            }

            //比较验证。
            if (null != parm.cvID && $.trim(parm.cvID).length > 0) {
                var cv = $2(parm.cvID);
                if (null != cv) {
                    if (_inputValue != $2(cv.controltocompare).value) {
                        //当验证失败，执行嵌入方法。
                        if (typeof (onValidateInputFail98562) != "undefined")
                            onValidateInputFail98562(ol); common.showPop(parm.inputControlID, cv.errormessage, true);
                        return false;
                    }
                }
            }

            //数据范围验证。
            if (null != parm.rvID && $.trim(parm.rvID).length > 0) {
                var rv = $2(parm.rvID);
                if (null != rv) {
                    var minValue = parseInt(rv.minimumvalue);
                    var maxValue = parseInt(rv.maximumvalue);
                    _inputValue = parseInt(_inputValue);
                    if (!(_inputValue >= minValue && _inputValue <= maxValue)) {
                        //当验证失败，执行嵌入方法。
                        if (typeof (onValidateInputFail98562) != "undefined")
                            onValidateInputFail98562(ol); common.showPop(parm.inputControlID, rv.errormessage, true);
                        return false;
                    }
                }
            }
        }

        return true;
    },

    //验证全部控件输入。
    validateAllInput: function () {
        if (!window._____vad_parms)
            return true;

        var r = true;

        try {
            common.hideAllPopupPanel();

            //验证控件验证。    
            for (var i = 0; i < _____vad_parms.length; i++) {
                //获取需要验证的对象。
                var ol = $2(_____vad_parms[i].inputControlID);

                //判断是否仅验证可见对象。
                var isValidateVisibleElement = false;
                if (typeof (isValidateVisibleElement985652) != "undefined")
                    isValidateVisibleElement = true;
                else
                    isValidateVisibleElement = null != ol && $(ol).is(":visible");

                if (null != ol && isValidateVisibleElement && !common.inputBlur(ol, true)) {
                    r = false;

                    //如果有lastPopLocationY(表示有弹出显示层)。
                    if (typeof (common.lastPopLocationY) != "undefined" && common.lastPopLocationY > 0) {
                        //如果当前弹出显示层的位置小于页面滚动的位置(表示弹出显示层将被隐藏需无法显示)，则滚动页面到指定(lastPopLocationY)位置。
                        if (common.lastPopLocationY < document.documentElement.scrollTop + document.body.scrollTop) {
                            window.scrollTo(0, common.lastPopLocationY - 10);
                        }
                    }
                    break;
                }
            }
        }
        catch (e) {
            common.alertOnLocal(e, "验证全部控件输入发生错误");
            return false;
        }

        return r;
    },

    //设置输入文本框的样式。
    setInputClass: function (ol, scene) {
        //特殊处理数据选择器(mySelectDialog)的样式。
        var oClass = ol.getAttribute("__oc");
        if (null == oClass)
            return oClass;

        //先重置CSS样式为默认。
        ol.className = oClass;
        var n = "";
        switch (scene) {
            case "e": //验证出错
                n = "error-control";
                break;
        }

        if (n.length > 0)
            $(ol).addClass(n);
        return n;
    },

    //===================end 注册客户端验证 end =================================================================//


    //获取文本框中选择的文本。
    getSelectedText: function (inputID) {
        var el = document.getElementById(inputID);
        var startPosition = 0; //所选文本的开始位置
        var endPosition = 0; //所选文本的结束位置
        if (document.selection) {
            //IE
            var range = document.selection.createRange(); //创建范围对象
            var drange = range.duplicate(); //克隆对象

            drange.moveToElementText(el);  //复制范围  
            drange.setEndPoint('EndToEnd', range);

            startPosition = drange.text.length - range.text.length;
            endPosition = startPosition + range.text.length;
        }
        else if (window.getSelection) {
            //Firefox,Chrome,Safari etc
            startPosition = el.selectionStart;
            endPosition = el.selectionEnd;
        }

        var text = el.value;
        return text.substr(startPosition, (endPosition - startPosition));
    },

    //向文本框光标处追加文本。
    //inputID 文本框编号。
    //txt 要追加的文本
    appendText: function (inputID, txt) {
        var obj = document.getElementById(inputID);
        selection = document.selection;
        obj.focus();
        if (typeof (obj.selectionStart) != 'undefined') {
            obj.value = obj.value.substr(0, obj.selectionStart) + txt + obj.value.substr(obj.selectionEnd);
        } else if (selection && selection.createRange) {
            var sel = selection.createRange();
            sel.text = txt;
            sel.moveStart('character', -txt.length);
        } else {
            obj.value += txt;
        }

        obj.focus();
    },

    //当某输入框按下回车键时触发某个按钮。
    kd: function (txtID, fun) {
        $("#" + txtID).bind("keydown", function (e) {
            return common.enterKeyDown(e, function () {
                if (null != fun)
                    fun();
            });
        });
    },


    //获取密码强度。
    gCheckPassword: function (password) {
        var _score = 0; // 初始化积分

        if (!password) {
            return 0;
        }

        /**
        * 密码长度
        *
        *  5 分: 小于等于 4 个字符
        * 10 分: 5 到 7 字符
        * 25 分: 大于等于 8 个字符
        */
        if (password.length <= 4) {
            _score += 5;
        } else if (password.length >= 5 && password.length <= 7) {
            _score += 10;
        } else if (password.length >= 8) {
            _score += 25;
        }

        /**
        * 字母
        *  0 分: 没有字母
        * 10 分: 全都是小（大）写字母
        * 20 分: 大小写混合字母
        */
        var _UpperCount = (password.match(/[A-Z]/g) || []).length;
        var _LowerCount = (password.match(/[a-z]/g) || []).length;
        var _LowerUpperCount = _UpperCount + _LowerCount;

        if (_UpperCount && _LowerCount) {
            _score += 20;
        } else if (_UpperCount || _LowerCount) {
            _score += 10;
        }

        /**
        * 数字
        *
        *  0 分: 没有数字
        * 10 分: 1 个数字
        * 20 分: 大于等于 3 个数字
        */
        var _NumberCount = (password.match(/[\d]/g, '') || []).length;
        if (_NumberCount > 0 && _NumberCount <= 2) {
            _score += 10;
        } else if (_NumberCount >= 3) {
            _score += 20;
        }

        /**
        * 符号
        *  0 分: 没有符号
        * 10 分: 1 个符号
        * 25 分: 大于 1 个符号
        */
        var _CharacterCount = (password.match(/[!@#$%^&*?_\.\-~]/g) || []).length;
        if (_CharacterCount == 1) {
            _score += 10;
        } else if (_CharacterCount > 1) {
            _score += 25;
        }


        /**
        * 奖励
        *
        * 2 分: 字母和数字
        * 3 分: 字母、数字和符号
        * 5 分: 大小写字母、数字和符号
        */
        if (_NumberCount && _LowerUpperCount) {
            _score += 2;
        } else if (_NumberCount && _LowerUpperCount && _CharacterCount) {
            _score += 3;
        } else if (_NumberCount && (_UpperCount && _LowerCount) && _CharacterCount) {
            _score += 5;
        }

        /**
        * 最后的评分标准
        *
        * >= 90: 非常安全
        * >= 80: 安全（Secure）
        * >= 70: 非常强
        * >= 60: 强（Strong）
        * >= 50: 一般（Average）
        * >= 25: 弱（Weak）
        * >= 0: 非常弱
        */
        return _score;
    },

    //根据密码强度得分输出相关描述。
    getResultDesp: function (score) {
        if (score <= 5) {
            return '太短';
        } else if (score > 5 && score <= 20) {
            return '弱';
        } else if (score > 20 && score < 60) {
            return '中';
        } else if (score >= 60) {
            return '强';
        } else {
            return '';
        }
    },

    //输出对象结构。
    printObject: function (obj) {
        var s = "";
        $.each(obj, function (i, item) {
            s += i + ":" + item + "\n";
        });

        return s;
    },

    //倒计时。
    //time 秒
    countDown: function (buttonID, time) {
        var t = time;
        var button = $2(buttonID);
        button.disabled = "disabled";
        var oldButtonValue = common.getControlValue(button);

        var st = setInterval(function () {
            common.setControlValue(button, "请等待" + t + "秒");
            if (t <= 0) {
                button.disabled = "";
                common.setControlValue(button, oldButtonValue);
                clearInterval(st);
            }

            t -= 1;
        }, 1000);
    },

    //设置控件值。
    setControlValue: function (ol, value) {
        var tagName = ol.tagName.toLowerCase();
        if (tagName == "input")
            ol.value = value;
        else
            ol.innerHTML = value;
    },

    //获取控件值。
    getControlValue: function (ol) {
        var tagName = ol.tagName.toLowerCase();
        if (tagName == "input")
            return ol.value;
        else
            return ol.innerHTML;
    },


    //根据key获取查询字符串。
    //aaa.htm?a=1&b=2&c=3
    getQueryString: function (key) {
        key = key.toLowerCase();
        var url = window.location.href;
        var lastIndex = url.lastIndexOf("?");
        if (lastIndex < 0)
            return null;
        var arrQS = url.substr(lastIndex + 1).split("&");

        for (var i = 0; i < arrQS.length; i++) {
            var index = arrQS[i].indexOf("=");
            if (arrQS[i].substr(0, index).toLowerCase() == key)
                return common.DecodeURI(arrQS[i].substr(index + 1));
        }

        return null;
    },

    //将指定秒数转换为分钟秒数。
    changeToMinute: function (totalSecond) {
        var t = "";

        //计算小时。
        var h = parseInt(totalSecond / 3600);
        if (h > 0)
            t += (h.toString().length == 1 ? "0" : "") + h + "时";

        //计算分钟。
        var m = parseInt((totalSecond - (h * 3600)) / 60);
        t += (m.toString().length == 1 ? "0" : "") + m + "分";

        //计算秒。
        var s = totalSecond - h * 3600 - m * 60;
        t += (s.toString().length == 1 ? "0" : "") + s + "秒";

        return t;
    },

    //从指定日期添加指定天数。
    addDay: function (dateText, numDay) {
        if (dateText.length == 0)
            return "";

        var d = new Date(dateText);
        d.setDate(d.getDate() + numDay);

        var month = (d.getMonth() + 1).toString();
        if (month.length == 1)
            month = "0" + month;

        var day = d.getDate().toString();
        if (day.length == 1)
            day = "0" + day;

        return d.getFullYear() + "-" + month + "-" + day;
    },

    //叠加或替换当前处于焦点状态的Tab栏参数。
    addOrReplaceQS: function (qs) {
        //获取当前处于焦点状态的Tab栏的网址。
        var divActivedTab = $("#divTabStatus li[class='active'] a");
        var url = divActivedTab.attr("href");
        if (null == url)
            return qs;

        //获取参数。
        var index = url.indexOf("?");
        if (index < 0)
            return qs;

        var arrOQS = url.toLowerCase().substr(index + 1).split("&");
        $.each(arrOQS, function (i, item) {
            var arr = item.split("=");
            var f = false;
            $.each(qs, function (i2, item2) {
                if (item2.indexOf(arr[0] + "=") == 0) {
                    f = true;
                    return false;
                }
            });
            if (!f)
                qs.push(item);
        });

        return qs;
    }
}