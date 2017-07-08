//=============== 实现自动完成功能的jQuery插件 ============================================//
/*
-- 名称：实现自动完成功能的jQuery插件
-- 功能：1. 仿google搜索输入关键字即自动显示下拉框；2. 自动保存搜索历史。
-- 作者：HXW(http://mrhgw.cnblogs.com, mrhgw@sohu.com)
-- 使用：在$().ready(function() {  //在此处加入以下代码初始化  }); 
$('#txtKeywords').myAutocomplete({
ajaxURL: '/utils/MyHandler.ashx?kw=',
width: null,  //设置显示宽度。
panelClass: "myAutocomplete"
});
        
提示：参数设置见 $.fn.myAutocomplete.defaults 方法。
*/

(function ($) {
    $.fn.extend({
        myAutocomplete: function (options) {
            //设置默认参数。
            options = $.extend({}, $.fn.myAutocomplete.defaults, {}, options);

            return this.each(function () {
                //创建对象实例并保存。
                var ins = new $.MyAutocomplete($(this), options);
                $(this).data("ac-" + this.id, ins);

                /*
                获取实例对象：
                假如使用此jq插件的文本框id为Text1，则为：
                var api = $("#Text1").data("ac-Text1");
                */
            });
        }
    });


    //创建一个自动完成的实例。
    //___jqIns jq实例对象
    //___options jq配置参数
    $.MyAutocomplete = function (___jqIns, ___options) {
        this.jqIns = ___jqIns;
        this.options = ___options;
        this.jqElementID = ___jqIns.attr("id");
        var _this = this;

        //下拉层jq对象。
        this.divPanel = null;
        //取消输入的层。
        this.divCancel = null;
        this.divCancelWidth = 0;
        this.divCancelHeight = 0;
        //鼠标是否位于自动完成的层上。
        this.mouseInPanel = false;
        //最后一次请求的网址。
        this.lastRequestUrl = null;
        //表示当前处于焦点状态的自动完成某一项索引号。
        this.currentIndex = -1;
        //获取的自动完成数据项长度。
        this.dataLength = 0;
        //当前用户输入的关键词。
        this.inputValue = "";
        //当前是否处于keydown事件中。
        this.inKeydownEevent = true;
        //ajax数据是否正在请求中(和lastAjaxUrl一起，为了用户输入过快以至服务器响应无法跟上)。
        this.isBusy = false;
        //最后一次请求url。
        this.lastAjaxUrl = null;

        //初始化下拉框div。
        var panelID = "div_myAutocomplete_" + this.jqElementID;
        var cancelID = "div_myAutocomplete_" + this.jqElementID + "_cancel";

        $("body").append("<div id='" + panelID + "' oncontextmenu='return false;' class='" + this.options.panelClass + "'></div>");
        this.divPanel = $("#" + panelID);

        if (this.options.isDisplayCancelButton) {
            $("body").append("<div id='" + cancelID + "' title='清除搜索关键词' class='" + this.options.cancelClass + "'></div>");
            this.divCancel = $("#" + cancelID);
            this.divCancel.show();
            this.divCancelWidth = this.divCancel[0].offsetWidth;
            this.divCancelHeight = this.divCancel[0].offsetHeight;
            this.divCancel.hide();
            this.divCancel.bind("click", function () {
                _this.jqIns.val("");
                _this.jqIns[0].focus();
                this.style.display = "none";
            });
            this.divCancel.bind("mouseover", function () {
                this.className = _this.options.cancelClassOver;
            });
            this.divCancel.bind("mouseout", function () {
                this.className = _this.options.cancelClass;
            });
        }

        var w = undefined == this.options.width ? this.jqIns[0].offsetWidth : this.options.width;
        this.divPanel.css("width", w - 8);

        this.divPanel.bind("mouseover", function () {
            _this.mouseInPanel = true;
        });
        this.divPanel.bind("mouseout", function () {
            _this.mouseInPanel = false;
        });

        //显示自动完成panel。
        this.show = function () {
            this.divPanel.css("left", this.jqIns.offset().left + "px");
            this.divPanel.css("top", this.jqIns.offset().top + this.jqIns[0].offsetHeight + 1 + "px");
            this.divPanel.slideDown(100);
        };

        //显示或隐藏清除搜索关键词的面板。
        this.displayOrHideCancel = function () {
            if (!this.options.isDisplayCancelButton)
                return;

            if ($.trim(this.jqIns.val()).length == 0) {
                this.divCancel.fadeOut(100);
                return;
            }

            var h = ((this.jqIns[0].offsetHeight - this.divCancelHeight) / 2);
            this.divCancel.css("left", this.jqIns.offset().left + this.jqIns[0].offsetWidth - this.divCancelWidth - 3 + "px");
            this.divCancel.css("top", this.jqIns.offset().top + h + "px");
            this.divCancel.fadeIn(100);
        };

        //当父控件位置发生变化时。
        this.onParentPositionChanged = function () {
            if (this.divPanel[0].style.display != "none")
                this.show();

            this.displayOrHideCancel();
        };

        //保存搜索历史。
        this.saveHistory = function (keywords) {
            saveHistory(keywords);
        };

        $(document).ready(function () {
            _this.jqIns[0].autocomplete = "off";

            //绑定文本框的按键事件。
            _this.jqIns.bind("keydown", function (e) {
                var ev = window.event || e;
                if (ev.keyCode == 13) {
                    //如果是回车键触发。

                    //如果在参数中设置了回车键触发方法则执行。
                    if (null != _this.options.onEnterkeyDown) {
                        try {
                            _this.options.onEnterkeyDown();
                        }
                        catch (e) { }
                    }

                    //如果需要保存搜索历史记录。
                    if (_this.options.autoSaveHistory) {
                        try {
                            saveHistory(_this.jqIns.val());
                        }
                        catch (e) { }
                    }

                    return false;
                }

                return true;
            });

            if (/msie/.test(navigator.userAgent.toLowerCase())) //ie浏览器 
            {
                _this.jqIns[0].onpropertychange = onInputChange;
            }
            else {//非ie浏览器，比如Firefox
                _this.jqIns[0].addEventListener("input", onInputChange, false);
            }

            $(document).bind("keydown", function (e) {
                var ev = window.event || e;
                if (ev.keyCode == 38 || ev.keyCode == 40) {
                    ALkeyAction(e);
                }
                else {
                    _this.inKeydownEevent = false;
                }
            });

            $(document).bind("click", function () {
                _this.divPanel.hide();
            });

            _this.jqIns.bind("click", function () {
                _this.inKeydownEevent = false;
                onInputChange();
            });
        });

        //当输入框值发生变化时。
        function onInputChange() {
            //显示或隐藏取消按钮。
            _this.displayOrHideCancel();

            //如果当前是由于上下光标键移动而引起的文本框属性改变的则退出。
            if (_this.inKeydownEevent)
                return;

            //判断当前输入框是否有焦点。
            if (null == document.activeElement || document.activeElement.id != _this.jqElementID)
                return;

            //判断关键词是否为空。
            _this.inputValue = _this.jqIns.val();
            if (($.trim(_this.jqIns[0].title).length > 0 && _this.inputValue == _this.jqIns[0].title)) {
                //置空最后请求的网址。
                _this.lastAjaxUrl = null;
                //载入搜索历史。
                setTimeout(function () {
                    _this.divPanel.hide();
                    //载入输入历史记录。
                    var data = getHistory();
                    if (null != data) {
                        bindData(data);
                    }
                }, 100);

                return;
            }

            //如果未设置ajax请求数据的url则直接返回。
            if (null == _this.options.ajaxURL || $.trim(_this.options.ajaxURL).length == 0) {
                _this.divPanel.hide();
                return;
            }

            //设置最后一次请求的网址。
            _this.lastAjaxUrl = _this.options.ajaxURL + common.EncodeURI(_this.inputValue);
            //如果上一次请求未结束则直接返回。
            if (_this.isBusy)
                return;

            //通过ajax从服务器上获取数据。
            getData(_this.lastAjaxUrl);
        }

        //从服务器上获取JSON格式的数据。
        function getData(ajaxUrl) {
            if (null != _this.options.onBeforeRequest) {
                if (!_this.options.onBeforeRequest())
                    return;
            }

            _this.isBusy = true;
            $.ajax({
                type: "GET",
                url: ajaxUrl,
                cache: true,
                error: function (e) {
                    alert("出错了：" + e);
                    _this.isBusy = false;
                },
                success: function (data) {
                    //取消繁忙状态。
                    _this.isBusy = false;
                    //如果_this.lastAjaxUrl为NULL,表示输入框中已经没有关键词，无须继续显示数据，返回。
                    if (null == _this.lastAjaxUrl)
                        return;
                    //判断最后一次请求的网址和当前是否一致，表示用户输入过快产生了其它关键词，马上请求最后一个形成的关键词。
                    if (null != _this.lastAjaxUrl && _this.lastAjaxUrl != ajaxUrl) {
                        getData(_this.lastAjaxUrl);
                        return;
                    }

                    //置空最后请求的网址。
                    _this.lastAjaxUrl = null;
                    //绑定数据。
                    bindData(eval("(" + data + ")"));
                }
            });
        }

        //绑定数据。
        function bindData(data) {
            //如果没有数据，退出。
            _this.divPanel.html("");

            if (null == data || data.length == 0) {
                _this.divPanel.hide();
                _this.dataLength = 0;
                return;
            }
            _this.dataLength = data.length;

            //绘制html。
            var html = "";
            var kw = $.trim(_this.inputValue);
            var ks = kw.split(" ");
            $.each(data, function (i, item) {
                var _text = item.n.replace("'", "&apos;");
                var dv = _text;
                $.each(ks, function (i, item) {
                    dv = dv.replace(item, "<strong>" + item + "</strong>");
                });
                html += "<p myText='" + _text + "' myVal='" + item.v + "' myVal2='" + (null != item.v2 ? item.v2 : "") + "' id='" + _this.divPanel[0].id + "_item_" + i + "' myIndex='" + i + "'><b>" + dv + "</b><i>" + item.v + "</i></p>";
            });

            _this.currentIndex = -1;
            _this.divPanel.append(html);
            _this.show();

            $.each($("#" + _this.divPanel[0].id + " p"), function (i, item) {
                var jqItem = $(item);
                jqItem.bind("mouseover", function () {
                    $(this).addClass("ap-item-mo");
                });
                jqItem.bind("mouseout", function () {
                    $(this).removeClass("ap-item-mo");
                });
                jqItem.bind("click", function () {
                    var keywords = this.getAttribute("myText");
                    var value = this.getAttribute("myVal");
                    var value2 = this.getAttribute("myVal2");

                    _this.jqIns.val(keywords);
                    _this.divPanel.hide();
                    //自动保存搜索历史。
                    if (_this.options.autoSaveHistory)
                        saveHistory(keywords);
                    if (null != _this.options.onPanelItemClick)
                        _this.options.onPanelItemClick(keywords, value, value2);
                });
            });
        }

        //键盘方向键处理。
        function ALkeyAction(e) {
            //如果div已隐藏则退出。
            if (_this.divPanel[0].style.display == "none" || _this.divPanel[0].innerHTML.length == 0)
                return;
            var ev = window.event || e;

            _this.inKeydownEevent = true;
            if (ev.keyCode == 38 || ev.keyCode == 40) {
                var c = ev.keyCode == 38 ? -1 : 1;
                _this.currentIndex += c * 1;
                if (_this.currentIndex < -1 || _this.currentIndex >= _this.dataLength)
                    _this.currentIndex = -1;

                _this.jqIns.val(_this.inputValue);
                $.each($("#" + _this.divPanel[0].id + " p"), function (i, item) {
                    if (this.getAttribute("myIndex") == _this.currentIndex) {
                        $(this).addClass("ap-item-mo");
                        _this.jqIns.val(this.getAttribute("myText"));
                    }
                    else {
                        $(this).removeClass("ap-item-mo");
                    }
                });
            }
        }

        //获取并显示搜索历史记录。
        function getHistory() {
            try {
                var splitChars = "@%$-123-$%@";
                var cookieName = "inputHistory_MyAutocomplete" + _this.options.appType;
                var historyData = $.cookie(cookieName);
                if (null == historyData)
                    return null;

                //[{n:'文员',v:'500个结果'},{n:'c#',v:'1000个结果'}]
                var arr = historyData.split(splitChars);
                var h = false;
                var data = "[";
                $.each(arr, function (i, item) {
                    if (item.length > 0) {
                        h = true;
                        data += "{n:'" + item.replace("'", "&apos;") + "',v:''},";
                    }
                });
                if (h)
                    data = data.substring(0, data.length - 1);
                data += "]";

                return eval("(" + data + ")");
            }
            catch (e) {
                return null;
            }
        }

        //保存搜索历史记录。
        function saveHistory(keywords) {
            try {
                if (keywords.length < 2)
                    return;

                var cookieName = "inputHistory_MyAutocomplete" + _this.options.appType;
                var splitChars = "@%$-123-$%@";

                var historyData = $.cookie(cookieName);
                if (null == historyData)
                    historyData = "";

                //删除存在的历史记录。
                var exists = false;
                var arr = historyData.split(splitChars);
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].toLowerCase() == keywords.toLowerCase()) {
                        arr[i] = null;
                        break;
                    }
                }

                //加入新的。
                arr.unshift(keywords);

                //重新拼接。
                var index = 0;
                historyData = "";
                for (var i = 0; i < arr.length; i++) {
                    if (null != arr[i] && arr[i].length > 1) {
                        historyData += arr[i] + splitChars;
                        index += 1;
                        if (index >= 10)
                            break;
                    }
                }

                //保存cookie。
                $.cookie(cookieName, historyData, { expires: 1000, path: '/' });
            }
            catch (e) { }
        }
    }


    //默认参数设置。
    $.fn.myAutocomplete.defaults = {
        ajaxURL: "", //ajax请求数据url(提示：如果ajaxURL为空则不进行ajax请求，仅保存和显示搜索历史记录)(1.url最后参数必须为关键词参数且以=与结尾，如：utils.ashx?kw=)；2.ajax数据格式为JSON，格式为：[{n:'文员',v:'500个结果'},{n:'c#',v:'1000个结果'}])
        width: null, //下拉框的宽度，如果不设置或为null，则自动与输入文本框的宽度一致
        onEnterkeyDown: null,   //当键盘的回车键(Enter键)按下时触发的事件。
        onPanelItemClick: null,    //当自动显示的某一项被单击时。
        onBeforeRequest: null,      //当准备向服务器端发送请求前执行的方法，如果返回false则立即终止请求
        panelClass: "myAutocomplete",          //自动完成下拉框样式。
        cancelClass: "myAutocomplete-cancel",         //取消输入的层样式。
        cancelClassOver: "myAutocomplete-cancel-over", //当鼠标置于取消输入的层之上的样式
        autoSaveHistory: false,   //自动保存搜索历史(需要引用jquery.cookie.js)
        appType: "",             //应用程序类别(将根据此值来获取搜索历史记录)。
        isDisplayCancelButton: true    //是否显示清除按钮
    };

    //-------------------------------供外部供用的方法(end)------------------------------------//    

})(jQuery);
