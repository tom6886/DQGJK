;
$(function () {

    var userDevice = {};

    userDevice.openDialog = function (modal, relationID, userID) {
        $.post("userDevice/queryDialog", { id: relationID }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            userDevice.bindDialog(modal, userID);
        });
    };

    userDevice.bindDialog = function (modal, userID) {
        var _form = $("form", modal);

        $("input[name=userID]", _form).val(userID);
        _form.ajaxForm({
            beforeSubmit: function () {
                return _form.valid();
            },
            success: function (r) {
                alert(r.msg);

                if (r.code > 0) {
                    modal.modal('hide');
                    $("#userList tr.active").trigger("click");
                }
            }
        }).validate({
            rules: {
                userID: "required",
                devices: "required"
            },
            messages: {
                userID: "请选择用户",
                devices: "设备不能为空"
            }
        });

        $('select[name=Status]', _form).val($("#Status", _form).val());

        $(".save", modal).click(function () {
            var isNull = false;

            $("input._select", _form).each(function (i, v) {
                if (!$(v).val()) {
                    var html = '<label generated="true" class="error">{0}是必选项</label>';

                    $(".error", $(v).parents(".input-group:first")).remove();
                    $(v).after(html.format($(v).siblings("span").text()));

                    isNull = true;
                    return false;
                }
            });

            if (isNull) return false;
            _form.submit();
        });
    }

    userDevice.initPage = function () {

        $(".query").click(function () {
            $("#form_query").submit();
        });

        $("#dlg_edit").on('show.bs.modal', function (event) {
            if (!$("input[name=currentUser]").val()) {
                alert("请选择用户");
                return false;
            }

            var button = $(event.relatedTarget);
            userDevice.openDialog($(this), button.parent().data('id'), $("input[name=currentUser]").val());
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });

        $("._select", $("#form_query")).select_2();
    }();

});