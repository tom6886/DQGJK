;
(function ($) {
    var priv = {
        set: function ($this, data) {
            $this.each(function (i, v) {
                var _this = $(v);

                priv.chart(_this, data);
            });
        },
        chart: function (obj, data) {
            obj.highcharts({
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: "#" + data.code + "柜"
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                xAxis: [{
                    categories: data.xAxis,
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
                    data: data.humidity,
                    tooltip: {
                        valueSuffix: ' RH'
                    }
                }, {
                    name: '温度',
                    type: 'spline',
                    data: data.temperature,
                    tooltip: {
                        valueSuffix: ' °C'
                    }
                }]
            });
        }
    };

    $.fn.linechart = function (data) {

        var $this = this;

        if ($this.length == 0) { return; }

        priv.set($this, data);

        return $this;
    };
})(jQuery);