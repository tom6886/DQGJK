;
var Board = Board || {};

Board.config = {
    thermograph: {
        //最低温度
        "min": -20,
        //最高温度
        "max": 200,

        "minorTickInterval": "auto",
        "minorTickWidth": 1,
        "minorTickLength": 10,
        "minorTickPosition": "inside",
        "minorTickColor": "#666",

        "tickPixelInterval": 30,
        "tickWidth": 2,
        "tickPosition": "inside",
        "tickLength": 10,
        "tickColor": "#666",
        "labels": {
            "step": 2,
            "rotation": "auto"
        },
        //表盘显示单位
        "title": {
            "text": "℃"
        },
        //表盘各区间配置
        "plotBands": [
            {
                "from": -20,
                "to": 120,
                "color": "#55BF3B" // green
            },
            {
                "from": 120,
                "to": 160,
                "color": "#DDDF0D" // yellow
            },
            {
                "from": 160,
                "to": 200,
                "color": "#DF5353" // red
            }
        ]
    },
    ygrometer: {
        //最低湿度
        "min": 0,
        //最高湿度
        "max": 100,

        "minorTickInterval": "auto",
        "minorTickWidth": 1,
        "minorTickLength": 10,
        "minorTickPosition": "inside",
        "minorTickColor": "#666",

        "tickPixelInterval": 30,
        "tickWidth": 2,
        "tickPosition": "inside",
        "tickLength": 10,
        "tickColor": "#666",
        "labels": {
            "step": 2,
            "rotation": "auto"
        },
        //表盘显示单位
        "title": {
            "text": "RH"
        },
        //表盘各区间配置
        "plotBands": [
            {
                "from": 0,
                "to": 30,
                "color": "#DDDF0D"
            },
            {
                "from": 30,
                "to": 70,
                "color": "#55BF3B"
            },
            {
                "from": 70,
                "to": 100,
                "color": "#DF5353"
            }
        ]
    }
}