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
};

History.Table.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("history/List", { pi: pi }, function (r) {
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