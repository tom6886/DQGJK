;
var IMap = IMap || {};

IMap.Widgets = {
    table: null,
    map: null,
    init: function () {
        this.table = new IMap.Table();
        this.map = new IMap.Map("map");

        return this;
    }
}

IMap.Map = function (id) {
    this.entity = new BMap.Map(id);    // 创建Map实例
    this.entity.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    this.markers = [];

    return this;
}

IMap.Map.prototype = {
    setPoints: function (points) {
        var _this = this;

        for (var i = 0, length = points.length; i < length; i++) {
            var marker = new BMap.Marker(points[i]);  // 创建标注
            _this.entity.addOverlay(marker);
            _this.markers.push(marker);
        }

        this.entity.centerAndZoom(points[0], 12);  // 设置中心点坐标和地图级别
    },
    clearPoints: function () {
        var _this = this, _markers = _this.markers;

        for (var i = 0, length = _markers.length; i < length; i++) {
            _this.entity.removeOverlay(_markers[i]);
        }

        _markers.splice(0, length);
    }
}

IMap.Table = function () {
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
    this.station = $("input[name=StationCode]");
    this.container = $("#unseen");
    this.pageIndexBox = $("#pageIndexBox");

    return this;
}

IMap.Table.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("map/StationList", {
            province: _this.province.select2("val"),
            city: _this.city.select2("val"),
            country: _this.country.select2("val"),
            station: _this.station.select2("val"),
            pi: pi
        }, function (r) {
            _this.container.html(r);
            IMap.Widgets.map.clearPoints();
            IMap.Widgets.map.setPoints(_this.getPoints());
        });
    },
    getPoints: function () {
        var _points = [];

        $("tr", this.container).each(function (i, v) {
            _points.push(new BMap.Point($(v).attr("data-lng"), $(v).attr("data-lat")));
        });

        return _points;
    }
}

$(function () {
    var widgets = IMap.Widgets.init();

    widgets.table.query();

    $(".query").click(function () {
        widgets.table.query();
    });
});