//=============== 仿淘宝数据选择器 ============================================//
/*
-- 名称：实现自动完成功能的jQuery插件
-- 功能：1. 仿google搜索输入关键字即自动显示下拉框；2. 自动保存搜索历史。
-- 作者：HXW(http://mrhgw.cnblogs.com, mrhgw@sohu.com)
$('#txtKeywords').dataSelector({
ajaxData : null,
ajaxURL: '/utils/MyHandler.ashx?kw=',
width: null,  //设置显示宽度。
onDataSelected: null
});
        
提示：参数设置见 $.fn.dataSelector.defaults 方法。
*/

(function ($) {
    $.fn.extend({
        dataSelector: function (options) {
            //设置默认参数。
            options = $.extend({}, $.fn.dataSelector.defaults, {}, options);
            return this.each(function () {
                //创建对象实例并保存。
                var ins = new $.DataSelector($(this), options);
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
    $.DataSelector = function (___jqIns, ___options) {
        this.jqIns = ___jqIns;
        this.options = ___options;
        this.jqElementID = ___jqIns.attr("id");
        var _this = this;

        //表示当前控件的ID前缀。
        this.panelID = "divDataSelector_" + this.jqElementID;
        //顶级层。
        this.$divPanel = null;
        //tab层。
        this.divTab = null;
        //内容层。
        this.divContent = null;
        //数据源。
        this.dataSource = null;
        //数据源是否已经载入。
        this.isDataLoaded = false;
        //是否需要反向数据绑定。
        this.isNeedReverseBindData = false;
        this.tmFindData = null;

        //初始化下拉框div。
        var divContainerID = this.panelID + "Container";
        if (null == $2(divContainerID)) {
            $("body").append("<div id=\"" + divContainerID + "\" class=\"dataSelector9569\" oncontextmenu=\"return false;\"><ul id=\"" + this.panelID + "Tab\"></ul><div id=\"" + this.panelID + "Content\"><i>载入中...</i></div></div>");
        }

        //获取对象。
        this.$divPanel = $("#" + this.panelID + "Container");
        this.divTab = $2(this.panelID + "Tab");
        this.divContent = $2(this.panelID + "Content");

        var w = undefined == this.options.width ? this.jqIns[0].offsetWidth : this.options.width;
        this.$divPanel.css("width", w + "px");

        //显示弹层。
        this.show = function () {
            this.$divPanel.css("left", this.jqIns.offset().left + "px");
            this.$divPanel.css("top", this.jqIns.offset().top + this.jqIns[0].offsetHeight + 1 + "px");
            this.$divPanel.slideDown(100);
        };

        //隐藏弹层。
        this.hide = function () {
            this.$divPanel.slideUp(100);
        };

        //当父控件位置发生变化时。
        this.onParentPositionChanged = function () {
            if (this.$divPanel[0].style.display != "none")
                this.show();
        };

        //根据名称查询数据并反向绑定。
        this.findData = function (name) {
            name = name.replace("市", "");

            //采用定时器以确定数据源是否已经加载成功。
            var fun = function () {
                if (null == _this.dataSource)
                    return;

                $.each(_this.dataSource.contents, function (i, item) {
                    if (item.n.indexOf(name) >= 0) {
                        reverseBindContent(item.id);
                        return false;
                    }
                });
                clearInterval(_this.tmFindData);
            };
            _this.tmFindData = setInterval(fun, 300);
        };

        //页面载入时处理。
        $(document).ready(function () {
            //附加点击事件以自动关闭弹层。
            $(document).bind("click", function (e) {
                var ev = window.event || e;
                var srcElement = ev.srcElement ? ev.srcElement : ev.target;
                if (!(srcElement.id == _this.jqIns[0].id || common.isChildNode(_this.$divPanel[0].id, srcElement)))
                    _this.$divPanel.hide();
            });

            //附加事件。
            _this.jqIns.bind("click", function () {
                //显示弹层。
                _this.show();

                //从服务器载入数据。
                if (!_this.isDataLoaded)
                    loadData();
            });

            //判断是否需要反向绑定数据。
            _this.isNeedReverseBindData = null != _this.options.originalValue && _this.options.originalValue.length > 0;
            if (_this.isNeedReverseBindData) {
                //显示弹层。
                _this.show();
                //从服务器载入数据。
                loadData();
            }
        });

        //从服务器载入数据。
        function loadData() {
            $.ajax({
                type: "post",
                dataType: "json",
                url: _this.options.ajaxURL,
                data: _this.options.ajaxData,
                timeout: 20 * 1000,
                success: function (d) {
                    //如果请求失败。   
                    if (d.r != "T") {
                        alert(d.m);
                        return;
                    }

                    //请求成功。
                    _this.isDataLoaded = true;
                    _this.dataSource = d.d;

                    //绑定基础数据。
                    renderData();

                    //如果需要反向绑定数据。
                    if (_this.isNeedReverseBindData) {
                        reverseBindContent(_this.options.originalValue);
                        _this.isNeedReverseBindData = false;
                        _this.hide();
                    }
                }
            });
        }

        //渲染数据。
        function renderData() {
            //绑定Tab栏s。
            var tabHtml = "";
            var contentHtml = "";
            var w = _this.divTab.offsetWidth / _this.dataSource.tab.length;
            $.each(_this.dataSource.tab, function (i, tab) {
                tabHtml += "<li style=\"width:" + w + "px;\" tabIndex=\"" + i + "\">" + tab + "</li>";
                contentHtml += "<p id=\"" + _this.panelID + "TabContent" + i + "\"></p>";
            });
            _this.divTab.innerHTML = tabHtml;
            _this.divContent.innerHTML = contentHtml;

            //绑定数据。
            toTab(0, "", true);

            //附加事件。
            $.each($("#" + _this.$divPanel[0].id + " ul li"), function (i, tab) {
                $(tab).bind("click", function () {
                    toTab(this.getAttribute("tabIndex"), null, false);
                });
            });
        }

        //转到相应tab。
        function toTab(tabIndex, parentID, isRebindData) {
            //获取目标Tab。
            var currentTab = $("#" + _this.$divPanel[0].id + " li[tabIndex='" + tabIndex + "']")[0];
            if (null == currentTab)
                return;

            //隐藏所有。
            $.each($("#" + _this.$divPanel[0].id + " div p"), function (j, p) {
                p.style.display = "none";
            });

            //复员所有tab样式。
            $.each($("#" + _this.$divPanel[0].id + " ul li"), function (v, tab2) {
                tab2.className = "";
            });

            //设置当前样式。            
            currentTab.className = "current";

            //显示对应层。
            var divCurrentContent = $2(_this.panelID + "TabContent" + currentTab.getAttribute("tabIndex"));
            if (null != divCurrentContent)
                divCurrentContent.style.display = "block";

            //载入数据。
            if (isRebindData)
                bindContent(tabIndex, parentID);
        }

        //从分组数据中找到指定分类。
        function findGroupData(data, category) {
            var obj = null;
            $.each(data, function (i, item) {
                if (item.category == category) {
                    obj = item;
                    return false;
                }
            });

            return obj;
        }

        //绑定内容。
        //data 数据源。
        //tabIndex tab栏索引号
        //parentID 父级编号
        function bindContent(tabIndex, parentID) {
            var data = new Array();
            $.each(_this.dataSource.contents, function (i, item) {
                if (item.pid != parentID)
                    return;

                var obj = findGroupData(data, item.c);
                if (null == obj) {
                    obj = new Object();
                    obj.category = item.c;
                    obj.data = new Array();
                    data.push(obj);
                }
                obj.data.push(item);
            });

            //拼接html。
            var contentHtml = "";
            if (data.length == 1) {
                //如果没有或只有一个分组，则使用普通方式构建html。
                $.each(data[0].data, function (i, item) {
                    contentHtml += "<span tabIndex=\"" + tabIndex + "\" value=\"" + item.id + "\">" + item.n + "</span>";
                });
            }
            else {
                //分组构建。
                contentHtml += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;\">";
                $.each(data, function (i, obj) {
                    contentHtml += "<tr><td class=\"category\">" + obj.category + "</td><td>";
                    $.each(obj.data, function (i2, item) {
                        contentHtml += "<span tabIndex=\"" + tabIndex + "\" value=\"" + item.id + "\">" + item.n + "</span>";
                    });
                    contentHtml += "</td></tr>";
                });
                contentHtml += "</table>";
            }
            $2(_this.panelID + "TabContent" + tabIndex).innerHTML = contentHtml;

            //附加事件。
            var dataSpan = $("#" + _this.panelID + "TabContent" + tabIndex + " span");
            $.each(dataSpan, function (i, span) {
                $(span).bind("click", function () {
                    onSelected(this, tabIndex);
                });
            });

            //如果只有一条数据时则选择当前并直接触发选择事件。
            if (dataSpan.length == 1) {
                onSelected(dataSpan[0], tabIndex);
            }
        }

        //当城市被选择时。
        function onSelected(ol, tabIndex) {
            var dataSpan = $("#" + _this.panelID + "TabContent" + tabIndex + " span");
            $.each(dataSpan, function (i, span) {
                span.className = "";
            });
            ol.className = "current";

            //跳转到下一个tab。
            var currentTabIndex = parseInt(ol.getAttribute("tabIndex"));
            if (currentTabIndex >= _this.dataSource.tab.length - 1) {
                //如果已到尽头。
                //如果不是第一次反向绑定则隐藏弹层。
                if (!_this.isNeedReverseBindData) {
                    _this.$divPanel.hide();
                }

                //调用用户传入的回调方法。
                if (null != _this.options.onDataSelected)
                    _this.options.onDataSelected(ol.innerHTML, ol.getAttribute("value"), getFullName(ol.getAttribute("value")), _this.isNeedReverseBindData);
            }
            else {
                //清空所有子级tab的内容。
                emptyChildrenTab(currentTabIndex + 2);
                //跳转到指定tab。
                toTab(currentTabIndex + 1, ol.getAttribute("value"), true);
            }
        }

        //清空所有子级tab的内容。
        function emptyChildrenTab(tabIndex) {
            for (var i = tabIndex; i < _this.dataSource.tab.length; i++) {
                var divTabContent = $2(_this.panelID + "TabContent" + i);
                if (null != divTabContent)
                    divTabContent.innerHTML = "";
            }
        }

        //根据编号反向获取数据。
        function reverseGetData(id) {
            var obj = new Object();
            obj.dataID = new Array();
            obj.dataText = new Array();

            var isTop = false;
            for (var i = 0; i < 10; i++) {
                $.each(_this.dataSource.contents, function (j, item) {
                    if (item.id == id) {
                        obj.dataID.push(item.id);
                        obj.dataText.push(item.n);

                        if (item.pid.length == 0)
                            isTop = true;
                        else
                            id = item.pid;
                        return false;
                    }
                });

                //如果到了顶级则跳出循环。
                if (isTop)
                    break;
            }

            return obj;
        }

        //根据编号获取完整名称。
        function getFullName(id) {
            //根据编号反向获取数据。
            var obj = reverseGetData(id);

            //拼接字符串。
            var fullName = "";
            for (var i = obj.dataText.length - 1; i >= 0; i--) {
                fullName += obj.dataText[i] + "/";
            }

            //去除最后一个/号。
            if (fullName.length > 0)
                fullName = fullName.substr(0, fullName.length - 1);
            return fullName;
        }

        //反向绑定内容。
        function reverseBindContent(id) {
            //根据编号反向获取数据。
            var obj = reverseGetData(id);
            if (obj.dataID.length == 0)
                return;

            //添加顶级父级。
            obj.dataID.push("");

            //开始绑定。
            var tabIndex = 0;
            for (var i = obj.dataID.length - 1; i > 0; i--) {
                toTab(tabIndex, obj.dataID[i], true);
                var spanSelected = $("#" + _this.panelID + "TabContent" + tabIndex + " span[value='" + obj.dataID[i - 1] + "']")[0];
                onSelected(spanSelected, tabIndex);

                tabIndex += 1;
            }

            //发出事件。
            if (null != _this.options.onReverseBindData)
                _this.options.onReverseBindData();
        }
    }


    //默认参数设置。
    $.fn.dataSelector.defaults = {
        ajaxData: null, //ajax获取数据PostData
        ajaxURL: "", // ajax请求数据url
        width: null, // 下拉框的宽度，如果不设置或为null，则自动与输入文本框的宽度一致
        onDataSelected: null,   //当选择最终数据时触发，参数(t=选择文本, v=选择值, ft=完整文本, isReverseBindData=是否为编辑模式时反向绑定)。   
        originalValue: null,  //需要反向绑定的值   
        onReverseBindData: null //当反向绑定数据完成时，参数(ft=完整文本)。
    };

    //-------------------------------供外部供用的方法(end)------------------------------------//    

})(jQuery);
