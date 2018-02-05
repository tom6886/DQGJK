;
$(function () {

    var device = {};

    device.deviceStateStr = '<i class="fa fa-circle deviceState device-level-{0}" style="margin-right: 10px;"></i>';

    window.freshState = function () {
        var codes = new Array();
        $("table tr").each(function (i, v) {
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
                    $(".deviceState", _tr).replaceWith(device.deviceStateStr.format(devices[i].State));
                }
            }
        });
    }

    device.openDialog = function (modal, deviceID) {
        $.post("device/queryDialog", { deviceID: deviceID }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            device.bindDialog(modal);
        });
    };

    device.bindDialog = function (modal) {
        var _form = $("form", modal);

        _form.ajaxForm({
            beforeSubmit: function () {
                return _form.valid();
            },
            success: function (r) {
                alert(r.msg);

                if (r.code > 0) {
                    modal.modal('hide');
                    $("#form_query").submit();
                }
            }
        }).validate({
            rules: {
                Name: "required",
                Code: "required",
                Tower: "required",
                Lng: {
                    required: true,
                    number: true,
                    lng: true
                },
                Lat: {
                    required: true,
                    number: true,
                    lat: true
                },
                Interval: {
                    required: true,
                    number: true
                }
            },
            messages: {
                Name: "设备名称是必填项",
                Code: "设备编号是必填项",
                Tower: "杆塔编号是必填项",
                Lng: {
                    required: "经度是必填项",
                    number: "经度只能输入数字"
                },
                Lat: {
                    required: "纬度是必填项",
                    number: "纬度只能输入数字"
                },
                Interval: {
                    required: "拍摄间隔是必填项",
                    number: "拍摄间隔只能输入整数"
                }
            }
        });

        $("._select", _form).select_2();

        $('select[name=Important]', _form).val($("#Important", _form).val());
        $('select[name=IsDanger]', _form).val($("#IsDanger", _form).val());
        $('select[name=AutoPost]', _form).val($("#AutoPost", _form).val());
        $('select[name=Status]', _form).val($("#Status", _form).val());

        $(".save", modal).click(function () {
            _form.submit();
        });
    }

    device.initPage = function () {

        $(".query").click(function () {
            $("#form_query").submit();
        });

        $("#dlg_edit").on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            device.openDialog($(this), button.parent().data('id'));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });

        $("._select", $("#form_query")).select_2();
    }();

});