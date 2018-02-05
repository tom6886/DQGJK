;
$(function () {

    var index = { markers: [] };

    index.deviceStateStr = '<i class="fa fa-circle deviceState device-level-{0}" style="margin-right: 10px;"></i>';

    window.freshState = function () {
        var codes = new Array();
        $("#deviceList tr").each(function (i, v) {
            if (!$(v).attr("data-code")) { return true; }

            codes.push($(v).attr("data-code"));
        });

        if (codes.length == 0) { return false; }

        $.ajax({
            type: "post",
            traditional: true,
            url: "Home/freshState",
            data: { codes: codes },
            success: function (r) {
                var devices = $.parseJSON(r);

                for (var i = 0, length = devices.length; i < length; i++) {
                    var _tr = $("tr[data-code=" + devices[i].Code + "]");
                    $(".deviceState", _tr).replaceWith(index.deviceStateStr.format(devices[i].State));
                }
            }
        });
    }

    index.clearPoints = function () {

        var length = index.markers.length;

        for (var i = 0; i < length; i++) {
            index.map.removeOverlay(index.markers[i]);
        }

        index.markers.splice(0, length);
    }

    index.setPoint = function () {

        var _tr = $("table tr:eq(0)"), _center = new BMap.Point(_tr.data("lng"), _tr.data("lat")), _points = new Array();

        //坐标纠偏，重新赋值到列表上
        $("table tr").each(function (i, v) {
            _points.push("{0},{1}".format($(v).data("lng"), $(v).data("lat")));
        });

        if (_points.length == 0) { return false; }

        $.ajax({
            url: "http://api.map.baidu.com/geoconv/v1/?coords={0}&from=1&to=5&ak=3ulqLPebKpPEGLx5VEHYMGY6".format(_points.join(";")),
            type: 'GET',
            dataType: 'JSONP',//here
            success: function (r) {
                if (r.status > 0) { alert("坐标纠错发生错误，错误码：{0}".format(r.status)); }

                var _result = r.result;
                for (var i = 0; i < _result.length; i++) {
                    $("table tr").eq(i).attr({ "data-lng": _result[i].x, "data-lat": _result[i].y })
                }

                index.clearPoints();

                index.map.centerAndZoom(_center, 11);

                $("table tr").each(function (i, v) {
                    var point = new BMap.Point($(v).attr("data-lng"), $(v).attr("data-lat"));

                    var marker = new BMap.Marker(point);  // 创建标注

                    index.map.addOverlay(marker);

                    var infoWindow = new BMap.InfoWindow($("td", $(v)).text());  // 创建信息窗口对象 

                    marker.addEventListener("click", function () {
                        index.map.openInfoWindow(infoWindow, point); //开启信息窗口
                    });

                    index.markers.push(marker);
                });
            }
        });
    };

    index.initPage = function () {

        $(".query").click(function () {
            $("#form_query").submit();
        });

        index.map = new BMap.Map("map");    // 创建Map实例

        index.map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放

        index.setPoint();

        window.setPoint = index.setPoint;

        window.map = index.map;

        $("._select").select_2().select2("val", "");
    }();

});