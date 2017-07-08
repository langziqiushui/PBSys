//区域选择器。
var areaSelector = {
    //当前配置选择的区域(数组)。
    checkedArea: null,
    //其他配置选择的区域(数组)。
    otherCheckedArea: null,
    //当确定选择区域时调用的方法([t,v]参数t为区域名称集以、号分隔；参数v为区域编号集，以|号分隔)。
    funOnConfirmArea: null,

    //初始化。
    init: function () {
        //打开或关闭子级城市。
        $.each($("em[name='provinceTrigger']"), function (i, em) {
            $(em).bind("click", areaSelector.openOrCloseChildCity);
        });

        //区域、省市连动选择。
        //区域选择或取消连动省份。
        $.each($("#divDeliveryAreaContent input[name='chkArea']"), function (i, chk) {
            $(chk).bind("click", function () {
                areaSelector.onAreaClick(chk);
            });
        });

        //省份单击事件处理。
        $.each($("#divDeliveryAreaContent input[name='chkProvince']"), function (i, chk) {
            $(chk).bind("click", function () {
                areaSelector.onProvinceClick(chk);
            });
        });

        //取消所有区域。
        $("#btCancelAllCity").bind("click", function () {
            areaSelector.resetArea();
        });

        //关闭区域选择。
        $("[name='divCloseArea']").bind("click", areaSelector.closeDeliveryArea);

        //关闭子级城市对话框。
        $("#divCloseChildCity").bind("click", areaSelector.closeChildCity);

        //确定选择区域。
        $("#btConfirmArea").bind("click", areaSelector.confirmArea);
    },

    //选择省市区域。
    //checkedArea 选择的区域编号(多个以|号分隔)。
    //otherCheckedArea 其他区域编号(多个以|号分隔)。
    //funOnConfirmArea 当确定选择区域时调用的方法([t,v]参数t为区域名称集以、号分隔；参数v为区域编号集，以|号分隔)。
    openDialog: function (checkedArea, otherCheckedArea, funOnConfirmArea) {
        if (null == checkedArea)
            checkedArea = "";
        if (null == otherCheckedArea)
            otherCheckedArea = "";

        areaSelector.checkedArea = checkedArea.split("|");
        areaSelector.otherCheckedArea = otherCheckedArea.split("|");
        areaSelector.funOnConfirmArea = funOnConfirmArea;

        //重置区域选择状态。
        areaSelector.resetArea();
        //初始化配置区域。
        areaSelector.initArea();
    },

    //关闭区域选择。
    closeDeliveryArea: function () {
        areaSelector.otherCheckedArea = null;
        areaSelector.checkedArea = null;
        areaSelector.funOnConfirmArea = null;

        $2("divChildCity").style.display = "none";
        $2("divDeliveryArea").style.display = "none";
    },

    //重置区域选择。
    resetArea: function () {
        $.each($("#divDeliveryAreaContent input[name='chkArea']"), function (i, chk) {
            chk.checked = false;
            areaSelector.onAreaClick(chk);
        });

        //重置城市选择状态。
        $2("divChildCityContent").innerHTML = "";
    },

    //初始化省市区域的选择状态。
    initArea: function () {        
        //排除已经被其他配置占用的省份。        
        $.each($("input[name='chkProvince']"), function (i, chkProvince) {
            chkProvince.parentNode.className = "";
            chkProvince.disabled = false;

            if ($.inArray(chkProvince.value, areaSelector.otherCheckedArea) >= 0) {
                //如果省份被其他配置占用。
                chkProvince.disabled = true;
                chkProvince.parentNode.className = "mall-Delivery-area-item-gray";
            }
            else {
                //如果省份没有被其他配置占用，则检查当前省份下的城市是否占用。
                var numDisabledCity = 0;
                var dataCity = chkProvince.getAttribute("data").split("|");
                $.each(dataCity, function (i2, item) {
                    var arr = item.split("@");
                    if ($.inArray(arr[0], areaSelector.otherCheckedArea) >= 0) {
                        numDisabledCity += 1;
                    }
                });

                //如果当前省份下全部城市被占用则设置省份的占用状态。
                if (dataCity.length == numDisabledCity) {
                    chkProvince.disabled = true;
                    chkProvince.parentNode.className = "mall-Delivery-area-item-gray";
                }
            }
        });

        //隐藏其他配置选择的区域。
        if (null == areaSelector.checkedArea)
            return;

        //绑定省份并提现父级省份。
        var checkedCityValue = "";
        var arrGroup = new Array();
        $.each(areaSelector.checkedArea, function (i, cityIDOrProvinceID) {
            if ($.trim(cityIDOrProvinceID).length == 0)
                return;

            var chkProvince = $2("chkProvince_" + cityIDOrProvinceID);
            if (null != chkProvince) {
                //表示为省份。
                chkProvince.checked = true;
                areaSelector.onProvinceClick(chkProvince, false);
            }
            else {
                //子级省市。 
                //循环所有省份复选框，从data数据中找到当前子城市的父级省份，加入数组。             
                $.each($("#divDeliveryAreaContent input[name='chkProvince']"), function (i2, chkProvince2) {
                    var f = false;
                    $.each(chkProvince2.getAttribute("data").split("|"), function (i3, item3) {
                        var arr3 = item3.split("@");
                        if (arr3[0] == cityIDOrProvinceID) {
                            f = true;
                            checkedCityValue += chkProvince2.value + "@" + cityIDOrProvinceID + "|";
                            if ($.inArray(chkProvince2.value, arrGroup) < 0)
                                arrGroup.push(chkProvince2.value);
                            return false;
                        }
                    });

                    if (f)
                        return false;
                });
            }
        });

        //绑定子级城市。
        var arrCity = checkedCityValue.split("|");
        $.each(arrGroup, function (i, provinceID) {
            var checkedCity = "";
            var num = 0;

            $.each(arrCity, function (i, item) {
                var arr = item.split("@");
                if (arr[0] == provinceID) {
                    num += 1;
                    checkedCity += arr[1] + "|";
                }
            });

            $2("chkProvince_" + provinceID).setAttribute("checkedCity", checkedCity);
            var originalProvinceName = $2("chkProvince_" + provinceID).getAttribute("provinceName");
            $2("lblProvince_" + provinceID).innerHTML = originalProvinceName + "<b>(" + num + ")</b>";
        });
    },

    //当区域触发单击事件时。
    //chk 区域复选框。
    onAreaClick: function (chkArea) {
        var data = $("#divDeliveryAreaContent input[name='chkProvince'][area='" + chkArea.value + "']");
        $.each(data, function (i, chkProvince) {
            if (chkProvince.disabled)
                return;

            chkProvince.checked = chkArea.checked;
            areaSelector.onProvinceClick(chkProvince, true);
        });

        //判断当前区域下所有省市是否全被选中。
        if (chkArea.checked) {
            if ($("#divDeliveryAreaContent input[name='chkProvince'][area='" + chkArea.value + "']:checked").length != data.length)
                chkArea.checked = false;
        }
    },

    //当省份触发单击事件时(主动触发)。
    //chkProvince 省份复选框。
    //isAreaProvider 是否为区域复选框触发。
    onProvinceClick: function (chkProvince, isAreaProvider) {
        if (chkProvince.disabled)
            return;

        //清除以前的子级城市勾选。
        chkProvince.setAttribute("checkedCity", "");

        //统计并绘制城市数量。        
        areaSelector.rendCityNum(chkProvince);

        //当省份的选择状态发生变化时。
        if (!isAreaProvider)
            areaSelector.onProvinceChange(chkProvince);

        //关闭子级城市窗口。
        areaSelector.closeChildCity();
    },

    //统计并绘制城市数量。
    rendCityNum: function (chkProvince) {
        if (chkProvince.disabled)
            return;

        var provinceID = chkProvince.value;
        var lblProvince = $2("lblProvince_" + provinceID);
        var originalProvinceName = chkProvince.getAttribute("provinceName");
        var numChecked = 0;

        if (chkProvince.checked) {
            var dataCity = chkProvince.getAttribute("data").split("|");
            var checkedCity = "";
            $.each(dataCity, function (i, item) {
                var arr = item.split("@");
                if ($.inArray(arr[0], areaSelector.otherCheckedArea) < 0) {
                    numChecked += 1;
                    checkedCity += arr[0] + "|";
                }
            });

            if (numChecked != dataCity.length) {
                chkProvince.checked = false;
                chkProvince.setAttribute("checkedCity", checkedCity);
            }
        }

        if (numChecked > 0)
            lblProvince.innerHTML = originalProvinceName + "<b>(" + numChecked + ")</b>";
        else
            lblProvince.innerHTML = originalProvinceName;
    },

    //当省份的选择状态发生变化时(被动触发)。
    onProvinceChange: function (chk) {
        if (chk.disabled)
            return;

        var area = chk.getAttribute("area");
        var numChecked = 0;
        var numUnChecked = 0;
        var data = $("#divDeliveryAreaContent input[name='chkProvince'][area='" + area + "']");
        $.each(data, function (i2, chk2) {
            if (chk2.checked)
                numChecked += 1;
            else
                numUnChecked += 1;
        });

        var chkArea = $2("chkArea_" + area);
        if (numChecked == data.length)
            chkArea.checked = true;
        else
            chkArea.checked = false;
    },

    //准备打开子级城市。
    openOrCloseChildCity: function () {
        var provinceID = this.getAttribute("provinceID");
        var chkProvince = $2("chkProvince_" + provinceID);
        if (chkProvince.disabled)
            return;

        var divChildCity = $2("divChildCity");
        var tar = divChildCity.getAttribute("tar");
        if (!(null == tar || tar != provinceID)) {
            areaSelector.closeChildCity();
            return;
        }

        //取消之前的背景色。
        $.each($("em[name='provinceTrigger']"), function (i2, em2) {
            $(this.parentNode).removeClass("mall-Delivery-current");
        });

        //获取上次选择的子级城市。
        var arrCheckedCity = null;
        var checkedCity = chkProvince.getAttribute("checkedCity");
        if (null != checkedCity && checkedCity.length > 0)
            arrCheckedCity = checkedCity.split("|");

        //循环渲染子级城市。
        var $divChildCityContent = $("#divChildCityContent");
        $divChildCityContent.html("");
        $.each(chkProvince.getAttribute("data").split("|"), function (j, item) {
            if ($.trim(item).length == 0)
                return;

            //判断当前复选框是否需要被选中。
            var isNeedChecked = false;
            var arr = item.split("@");

            //判断当前城市是否已经被其他配置占用。
            var isCityUsed = $.inArray(arr[0], areaSelector.otherCheckedArea) >= 0;

            if (!isCityUsed) {
                if (chkProvince.checked) {
                    isNeedChecked = true;
                }
                else {
                    if (null != arrCheckedCity) {
                        $.each(arrCheckedCity, function (k, cityID) {
                            if ($.trim(cityID).length == 0)
                                return;

                            if (cityID == arr[0]) {
                                isNeedChecked = true;
                                return false;
                            }
                        });
                    }
                }
            }

            var chkCityID = "chkCCity_" + arr[0];
            var checkedText = isNeedChecked ? " checked=\"checked\" " : "";
            var html = "<span><input " + checkedText + " id=\"" + chkCityID + "\" type=\"checkbox\" value=\"" + arr[0] + "\" cityName=\"" + arr[1] + "\" provinceID=\"" + provinceID + "\"><label for=\"chkCCity_" + arr[0] + "\">" + arr[1] + "</label></span>";
            $divChildCityContent.append(html);

            //设置已经被其他配置占用的城市样式和状态。
            if (isCityUsed) {
                var chkCity = $2(chkCityID);
                chkCity.disabled = true;
                $(chkCity.parentNode).addClass("mall-Delivery-childCity-gray");
            }
        });

        //显示弹窗。
        common.showPanel(chkProvince.id, "divChildCity", 280, null, -18, 20);
        divChildCity.setAttribute("tar", provinceID);
        $(this.parentNode).addClass("mall-Delivery-current");

        //附加事件。
        $.each($("#divChildCityContent input"), function (i3, chkChild) {
            if (chkChild.disabled)
                return;

            $(chkChild).bind("change", function () {
                var provinceID = chkChild.getAttribute("provinceID");
                var chkProvince = $2("chkProvince_" + provinceID);
                var lblProvince = $2("lblProvince_" + provinceID);

                //统计处于选择状态的城市并获取值。
                var checkedCity = "";
                var numChecked = 0;
                var numTotal = 0;
                $.each($("#divChildCityContent input"), function (i2, chkChild2) {
                    numTotal += 1;
                    if (!chkChild2.disabled && chkChild2.checked) {
                        numChecked += 1;
                        checkedCity += chkChild2.value + "|";
                    }
                });
                chkProvince.setAttribute("checkedCity", checkedCity);

                //统计并绘制城市数量。        
                var provinceName = chkProvince.getAttribute("provincename");
                lblProvince.innerHTML = numChecked > 0 ? provinceName + "<b>(" + numChecked + ")</b>" : provinceName;

                //判断是否需要改变省级复选框的选择状态。
                var originalParentChecked = chkProvince.checked;
                if (numChecked == numTotal)
                    chkProvince.checked = true;
                else
                    chkProvince.checked = false;

                //如果选择状态发生变化。
                if (originalParentChecked != chkProvince.checked)
                    areaSelector.onProvinceChange(chkProvince);
            });
        });
    },

    //关闭子级城市窗口。
    closeChildCity: function () {
        $.each($("em[name='provinceTrigger']"), function (i2, em2) {
            $(this.parentNode).removeClass("mall-Delivery-current");
        });

        var divChildCity = $2("divChildCity");
        divChildCity.setAttribute("tar", null);
        divChildCity.style.display = "none";
    },

    //根据城市编号获取城市名称。
    //cityID 城市编号。
    //chkProvince 省份复选框。
    getCityName: function (cityID, chkProvince) {
        var cityName = "";
        $.each(chkProvince.getAttribute("data").split("|"), function (i, item) {
            if ($.trim(item).length > 0) {
                var arr = item.split("@");
                if (arr[0] == cityID) {
                    cityName = arr[1];
                    return false;
                }
            }
        });

        return cityName;
    },

    //确定选择区域。
    confirmArea: function () {
        //根据选择的区域构建字符串，将产生两种字符串：
        //1. 用于编辑状态的字符串，格式为：父级省份@省份或城市编号@省份或城市名称|父级省份@省份或城市编号@省份或城市名称
        var checkedArea = "";
        var t = "";
        $.each($("#divDeliveryAreaContent input[name='chkProvince']"), function (i, chk) {
            if (chk.checked) {
                //如果省份被选择。
                t += chk.getAttribute("provinceName") + "、";
                checkedArea += chk.value + "|";
            }
            else {
                //获取选择的当前省份下选择的城市。
                var checkedCity = chk.getAttribute("checkedCity");
                if (null != checkedCity && checkedCity.length > 0) {
                    checkedArea += checkedCity;
                    $.each(checkedCity.split("|"), function (i2, cityID) {
                        if (cityID.length > 0) {
                            t += areaSelector.getCityName(cityID, chk) + "、";
                        }
                    });
                }
            }
        });

        if (checkedArea.length == 0) {
            alert("请选择区域！");
            return;
        }
        t = t.substr(0, t.length - 1);

        //通过回调函数通知。
        if (null != areaSelector.funOnConfirmArea)
            areaSelector.funOnConfirmArea(t, checkedArea);

        //关闭对话框。
        areaSelector.closeDeliveryArea();
    }
};
