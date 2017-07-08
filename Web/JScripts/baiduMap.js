//百度地图相关封装。
/*
options = {
    divMapID : "divMap",                 //承载地图控件编号
    originalLocation : {X:100, Y:200},   //百度地图初始坐标
    txtXID : "txtX",                     //填写X坐标的文件框ID
    txtYID : "txtY",                     //填写Y坐标的文件框ID
    onGetMyCity : null                   //当百度地图成功获取到当前城市名称时触发
};
*/
var baiduMap = {
    //相关参数。
    options: null,
    //地图初始化。
    //options 相关参数，详见上面。
    initMap: function (options) {
        baiduMap.options = options;

        // 百度地图API功能
        baiduMap.map = new BMap.Map(options.divMapID);
        // 创建地址解析器实例
        baiduMap.myGeo = new BMap.Geocoder();
        baiduMap.map.addEventListener("click", function (e) {
            if (null != options.txtXID)
                $2(options.txtXID).value = e.point.lng;
            if (null != options.txtYID)
                $2(options.txtYID).value = e.point.lat;
        });

        if (null == options.originalLocation) {
            //当处于添加模式下。
            //定位到当前城市
            baiduMap.getMyCity();
        }
        else {
            //当处于编辑模式下。
            baiduMap.goPoint(options.originalLocation.X, options.originalLocation.Y);
        }

        return true;
    },

    //定位到当前城市。
    getMyCity: function () {
        var myCity = new BMap.LocalCity();
        myCity.get(function (result) {
            baiduMap.goAddress(result.name);
            if (null != baiduMap.options.onGetMyCity) {
                baiduMap.options.onGetMyCity(result.name);
            }
        });
    },

    //根据地址跳转到坐标点。
    goAddress: function (cityAddress, funOnGoAddress) {
        //清除所有坐标点。
        baiduMap.map.clearOverlays();  

        // 将地址解析结果显示在地图上,并调整地图视野
        baiduMap.myGeo.getPoint(cityAddress, function (point) {
            if (point) {
                baiduMap.map.centerAndZoom(point, 15);
                baiduMap.map.addOverlay(new BMap.Marker(point));

                if (null != funOnGoAddress)
                    funOnGoAddress(point);
            }
        }, "");
    },
    

    //跳转到指定坐标。
    goPoint: function (x, y) {
        //清除所有坐标点。
        baiduMap.map.clearOverlays();  

        //创建标注。
        var pt = new BMap.Point(x, y);
        //var myIcon = new BMap.Icon("http://developer.baidu.com/map/jsdemo/img/fox.gif", new BMap.Size(300,157));
        var marker2 = new BMap.Marker(pt, { icon: null });
        baiduMap.map.addOverlay(marker2);

        baiduMap.map.centerAndZoom(new BMap.Point(x, y), 15);
        baiduMap.map.enableScrollWheelZoom(true);
    }
};