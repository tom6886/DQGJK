﻿;
var Home = Home || {};

Home.Widgets = {
    carousel: null,
    init: function () {
        this.carousel = new Home.Carousel();
        return this;
    }
}

Home.Carousel = function () {
    this.container = $("#unseen");
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
    this.stationCode = $("input[name=StationCode]");
}

Home.Carousel.prototype = {
    query: function (code) {
        var _this = this;

        $.get("Home/Carousel", {
            stationCode: code
        }, function (r) {
            _this.container.html(r);
            _this.bind();
        });
    },
    fresh: function () {
        this.query($("#stationCode").val());
    },
    bind: function () {
        var _entity = this;

        $(".carousel-select", _entity.container).each(function (i, v) {
            $(v).val($(v).siblings(".select-value").val());
        });

        $(".carousel-panel-edit", _entity.container).click(function () {
            var _this = $(this), _panel = _this.parents(".panel-info:eq(0)"), _editable = $(".carousel-save:eq(0)", _panel).is(":hidden");
            $(".carousel-input", _panel).attr("readonly", !_editable);
            $(".carousel-select", _panel).attr("disabled", !_editable);
            $(".carousel-save", _panel).toggle();
            _this.text(_editable ? "停止遥控" : "遥控");
        });

        $(".carousel-save", _entity.container).click(function () {
            var _this = $(this), _panel = _this.parents(".panel-info:eq(0)");
            if (_this.attr("data-code") === "B1") {
                _entity.command({
                    stationCode: $("#stationCode").val(),
                    functionCode: "B1",
                    DeviceCode: _panel.attr("data-code"),
                    RelayOne: $("select[name=Relay1]", _panel).val(),
                    RelayTwo: $("select[name=Relay2]", _panel).val(),
                    Dehumidify: $("select[name=Dehumidify]", _panel).val()
                });
            } else if (_this.attr("data-code") === "B2") {
                _entity.command({
                    stationCode: $("#stationCode").val(),
                    functionCode: "B2",
                    DeviceCode: _panel.attr("data-code"),
                    HumidityLimit: $("input[name=HumidityLimit]", _panel).val(),
                    TemperatureLimit: $("input[name=TemperatureLimit]", _panel).val()
                });
            }
        });

        $(".btn-measure", _entity.container).click(function () {
            _entity.measure($("#stationCode").val());
        });
    },
    command: function (params) {
        $.post("Home/Command", params, function (r) {
            alert(r.msg);
        });
    },
    measure: function (code) {
        $.post("Home/Measure", { stationCode: code }, function (r) {
            alert(r.msg);
        });
    }
}

$(function () {
    var widgets = Home.Widgets.init();

    widgets.carousel.query();

    $(".query").click(function () {
        widgets.carousel.query(widgets.carousel.stationCode.select2("val"));
    });

    setInterval(function () { widgets.carousel.fresh(); }, 60000);
});