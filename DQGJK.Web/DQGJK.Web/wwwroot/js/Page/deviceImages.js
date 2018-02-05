;
$(function () {

    var deviceImage = {};

    deviceImage.deviceStateStr = '<i class="fa fa-circle deviceState device-level-{0}" style="margin-right: 10px;"></i>';
    deviceImage.takePicStr = '<i class="fa fa-camera takePic" style="margin: 10px; float: right;"></i>';

    window.freshState = function () {
        var codes = new Array();
        $("#deviceList tr").each(function (i, v) {
            if (!$(v).attr("data-code")) { return true; }

            codes.push($(v).attr("data-code"));
        });

        if (codes.length == 0) { return false; }

        $.ajax({
            type: "post",
            traditional: true,
            url: "Home/freshState",
            data: { codes: codes },
            success: function (r) {
                var devices = $.parseJSON(r);

                for (var i = 0, length = devices.length; i < length; i++) {
                    var _tr = $("tr[data-code=" + devices[i].Code + "]");
                    $(".deviceState", _tr).replaceWith(deviceImage.deviceStateStr.format(devices[i].State));
                }

                $(".takePic").remove();

                //$(".device-level-0").parent().append(deviceImage.takePicStr);

                $(".takePic").click(function () {
                    takePic($(this).parents("tr").attr("data-code"));
                });
            }
        });
    }

    deviceImage.initPage = function () {
        $(".query").click(function () {
            $("#form_query").submit();
        });

        $("._select").select_2().select2("val", "");

        setInterval(loadImage, 60000);

        $("#deviceList tr:eq(0)").trigger("click");
    }();

});