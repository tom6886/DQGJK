;
var LogInfo = LogInfo || {};

LogInfo.Widgets = {
    table: null,
    init: function () {
        this.table = new LogInfo.Table();
        return this;
    }
};

LogInfo.Table = function () {
    this.container = $("#unseen");
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
    this.stationCode = $("input[name=stationCode]");
    this.startDate = $("input[name=StartDate]");
    this.endDate = $("input[name=EndDate]");
};

LogInfo.Table.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("loginfo/List", {
            province: _this.province.select2("val"),
            city: _this.city.select2("val"),
            country: _this.country.select2("val"),
            stationCode: _this.stationCode.select2("val"),
            startDate: _this.startDate.val(),
            endDate: _this.endDate.val(),
            pi: pi
        }, function (r) {
            _this.container.html(r);
        });
    }
}

$(function () {
    var widgets = LogInfo.Widgets.init();

    widgets.table.query();

    $(".query").click(function () {
        widgets.table.query();
    });
});