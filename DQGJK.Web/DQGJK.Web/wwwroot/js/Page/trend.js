;
var Trend = Trend || {};

Trend.Widgets = {
    station: null,
    init: function () {
        this.station = new Trend.Station();
        return this;
    }
};

Trend.Station = function () {
    this.container = $("#unseen");
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
    this.stationCode = $("input[name=StationCode]");
};

Trend.Station.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("Trend/StationInfo", {
            stationCode: _this.stationCode.select2("val")
        }, function (r) {
            _this.container.html(r);
            //_this.bind();
        });
    }
};

Trend.Carousel = function () {

};

Trend.Carousel.prototype = {

};

$(function () {
    var widgets = Trend.Widgets.init();

    widgets.station.query();

    $(".query").click(function () {
        widgets.station.query();
    });
});