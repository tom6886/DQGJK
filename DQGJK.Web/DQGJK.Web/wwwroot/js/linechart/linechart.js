;
(function ($) {
    var priv = {
        set: function ($this) {
            $this.each(function (i, v) {
                var _this = $(v);

                priv.chart(_this);
            });
        },
        chart: function (obj) {
            obj.highcharts({
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: obj.data("title")
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                xAxis: [{
                    categories: ['一月', '二月', '三月', '四月', '五月', '六月',
                        '七月', '八月', '九月', '十月', '十一月', '十二月'],
                    crosshair: true
                }],
                yAxis: [{ // Primary yAxis
                    labels: {
                        format: '{value}°C'
                    },
                    title: {
                        text: '温度'
                    }
                }, {
                    title: {
                        text: '湿度'
                    },
                    labels: {
                        format: '{value} RH'
                    },
                    opposite: true
                }],
                tooltip: {
                    shared: true
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 55,
                    floating: true
                },
                series: [{
                    name: '湿度',
                    type: 'spline',
                    yAxis: 1,
                    data: [59, 64, 51, 42, 59, 64, 59, 54, 59, 53, 53, 56],
                    tooltip: {
                        valueSuffix: ' mb'
                    }
                }, {
                    name: '温度',
                    type: 'spline',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6],
                    tooltip: {
                        valueSuffix: ' °C'
                    }
                }]
            });
        }
    };

    $.fn.linechart = function () {

        var $this = this;

        if ($this.length == 0) { return; }

        priv.set($this);

        return $this;
    };
})(jQuery);