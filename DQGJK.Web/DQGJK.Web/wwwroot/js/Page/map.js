;
var IMap = IMap || {};

IMap.Entity = function (id) {
    this.map = new BMap.Map(id);    // 创建Map实例
    this.map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    this.map.centerAndZoom(new BMap.Point(116.404, 39.915), 11);  // 初始化地图,设置中心点坐标和地图级别
}


$(function () {
    var map = new IMap.Entity("map");
});