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
    this.entity.centerAndZoom(new BMap.Point(116.404, 39.915), 11);  // 初始化地图,设置中心点坐标和地图级别

    return this;
}

IMap.Table = function () {
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
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
            pi: pi
        }, function (r) {
            _this.container.html(r);
        });
    }
}

$(function () {
    var widgets = IMap.Widgets.init();

    widgets.table.query();
});