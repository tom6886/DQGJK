;
var Trend = Trend || {};

Trend.Widgets = {
    table: null,
    init: function () {
        this.table = new Trend.Table();
        return this;
    }
};

Trend.Table = function () {
    this.container = $("#unseen");
};

Trend.Table.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("trend/DeviceList", { pi: pi }, function (r) {
            _this.container.html(r);
        });
    }
};

$(function () {
    //var widgets = Trend.Widgets.init();

    //widgets.table.query();

    //$(".query").click(function () {
    //    widgets.table.query();
    //});
});




Trend.LineChart = function () {
    this.container = $("#chart");
    this.entity = null;
}

Trend.LineChart.prototype = {

}