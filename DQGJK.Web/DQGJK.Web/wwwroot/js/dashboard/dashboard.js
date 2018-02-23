;
(function ($) {
    var globe = { ty: null, yy: null };

    var priv = {
        set: function ($this) {
            $this.each(function (i, v) {
                var _this = $(v), _config = Board.config[_this.data("board")];

                priv.board({
                    id: _this.attr("id"),
                    title: _this.data("title"),
                    config: _config,
                    data: 60
                });
            });
        },
        board: function (obj) {
            Highcharts.chart(obj.id, {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                title: {
                    text: obj.title,
                    margin: 0,
                    style: {
                        fontSize: '12px'
                    }
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                        // default background
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },

                // the value axis
                yAxis: {
                    min: obj.config.min,
                    max: obj.config.max,

                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',

                    tickPixelInterval: 30,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    title: {
                        text: obj.config.title
                    },
                    plotBands: obj.config.plotBands
                },

                series: [{
                    name: obj.title,
                    data: [obj.data],
                    tooltip: {
                        valueSuffix: ' ℃'
                    }
                }]

            });
        }
    };

    $.fn.dashboard = function () {

        var $this = this;

        if ($this.length == 0) { return; }

        priv.set($this);

        return $this;
    };
})(jQuery);