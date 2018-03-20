;
var History = History || {};

History.Widgets = {
    table: null,
    init: function () {
        this.table = new History.Table();
        return this;
    }
};

History.Table = function () {
    this.container = $("#unseen");
    this.province = $("input[name=Province]");
    this.city = $("input[name=City]");
    this.country = $("input[name=Country]");
    this.stationID = $("input[name=stationID]");
    this.startDate = $("input[name=StartDate]");
    this.endDate = $("input[name=EndDate]");
};

History.Table.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("history/List", {
            province: _this.province.select2("val"),
            city: _this.city.select2("val"),
            country: _this.country.select2("val"),
            stationID: _this.stationID.select2("val"),
            startDate: _this.startDate.val(),
            endDate: _this.endDate.val(),
            pi: pi
        }, function (r) {
            _this.container.html(r);
        });
    }
}

$(function () {
    var widgets = History.Widgets.init();

    widgets.table.query();

    $(".query").click(function () {
        widgets.table.query();
    });
});