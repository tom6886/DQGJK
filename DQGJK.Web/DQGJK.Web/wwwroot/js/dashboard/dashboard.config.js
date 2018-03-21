;
var Board = Board || {};

Board.config = {};

$(function () {
    $.get("common/GetBoardConfigs", function (r) {
        for (var i = 0, length = r.length; i < length; i++) {
            Board.config[r[i].name] = r[i].config;
        }

        //$("._board").dashboard();
    });
});