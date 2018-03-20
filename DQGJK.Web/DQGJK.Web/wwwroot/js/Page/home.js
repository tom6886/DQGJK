;
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
    this.stationCode = $("input[name=stationCode]");
}

Home.Carousel.prototype = {
    query: function () {
        var _this = this;

        $.get("Home/Carousel", {
            stationCode: null
        }, function (r) {
            _this.container.html(r);
        });
    },
}

$(function () {
    var widgets = Home.Widgets.init();

    widgets.carousel.query();

    $(".query").click(function () {
        widgets.carousel.query();
    });
});