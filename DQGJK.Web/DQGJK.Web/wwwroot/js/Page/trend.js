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
    this.carousel = null;
};

Trend.Station.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("Trend/StationInfo", {
            stationCode: _this.stationCode.select2("val")
        }, function (r) {
            _this.container.html(r);
            _this.carousel = new Trend.Carousel();
            _this.carousel.create();
        });
    }
};

Trend.Carousel = function () {
    this.container = $(".carousel-inner");
    this.code = $("#stationCode");
    this.start = $("input[name=StartDate]");
    this.end = $("input[name=EndDate]");
};

Trend.Carousel.prototype = {
    create: function () {
        var _this = this;

        $.post("Trend/Cabinets", {
            stationCode: _this.code.val(),
            start: _this.start.val(),
            end: _this.end.val()
        }, function (r) {
            if (r.code < 0) { alert(r.msg); return false; }

            for (var i = 0, length = r.data.length; i < length; i++) {
                var _id = "chart" + i;
                _this.appendHtml(i, _id);
                $("#" + _id, _this.container).linechart(r.data[i]);
            }
        });
    },
    appendHtml: function (sort, id) {
        var _template = Trend.Template.carouseItem;

        if (sort == 0) { this.container.append('<div class="item active"><div class="row"></div></div>'); }
        else if (sort % 2 == 0) { this.container.append('<div class="item"><div class="row"></div></div>'); }

        $(".item:last .row").append(_template.format(id));
    }
};

Trend.Template = {
    carouseItem: '<div class="col-md-6"><div class="panel panel-info">' +
    '<div class="panel-body">' +
    '<div id="{0}" class="_line" style="min-width:400px;height:320px"></div>' +
    '</div></div></div>'
}

$(function () {
    debugger
    var widgets = Trend.Widgets.init();

    widgets.station.query();

    $(".query").click(function () {
        widgets.station.query();
    });
});