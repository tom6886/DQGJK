;
var Trend = Trend || {};

Trend.Widgets = {
    station: null,
    querys: null,
    init: function () {
        this.station = new Trend.Station();
        this.querys = new Trend.Querys();
        return this;
    }
};

Trend.Querys = function () {
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
    this.stationCode = $("input[name=StationCode]");
    this.type = $("select[name=Type]");
    this.startDate = $("input[name=StartDate]");
    this.endDate = $("input[name=EndDate]");
    this.startMonth = $("input[name=StartMonth]");
    this.endMonth = $("input[name=EndMonth]");

    this.type.on('change', function () {
        var _type = $(this).val() === "0";
        if (_type) {
            $(".div-by-day").show();
            $(".div-by-month").hide();
            Trend.Widgets.station.fresh();
        }
        else {
            $(".div-by-day").hide();
            $(".div-by-month").show();
            Trend.Widgets.station.fresh();
        }
    });
}

Trend.Querys.prototype = {
    getStation: function () {
        return this.stationCode.select2("val")
    },
    getCondition: function () {
        var _this = this, _type = _this.type.val();
        if (_type === "0") {
            return { start: _this.startDate.val(), end: _this.endDate.val(), type: _type };
        }
        return { start: _this.startMonth.val(), end: _this.endMonth.val(), type: _type };
    }
}

Trend.Station = function () {
    this.container = $("#unseen");
    this.carousel = null;
};

Trend.Station.prototype = {
    query: function (pi) {
        var _this = this, _code = Trend.Widgets.querys.getStation();

        $.get("Trend/StationInfo", {
            stationCode: _code
        }, function (r) {
            _this.container.html(r);
            _this.carousel = new Trend.Carousel();
            _this.carousel.create();
        });
    },
    fresh: function () {
        this.carousel.container.html('');
        this.carousel.create();
    }
};

Trend.Carousel = function () {
    this.container = $(".carousel-inner");
    this.code = $("#stationCode");
};

Trend.Carousel.prototype = {
    create: function () {
        var _this = this, _condition = Trend.Widgets.querys.getCondition();

        $.post("Trend/Cabinets", {
            stationCode: _this.code.val(),
            start: _condition.start,
            end: _condition.end,
            type: _condition.type
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

    var widgets = Trend.Widgets.init();

    widgets.station.query();

    $(".query").click(function () {
        widgets.station.query();
    });
});