;
$(function () {

    var account = {};

    account.openDialog = function (modal, accountID) {
        $.post("user/queryDialog", { accountID: accountID }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            account.bindDialog(modal);
        });
    };

    account.bindDialog = function (modal) {
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
                Account: "required",
                PassWord: "required"
            },
            messages: {
                Account: "用户名称是必填项",
                PassWord: "密码是必填项"
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

    account.initPage = function () {

        $(".query").click(function () {
            $("#form_query").submit();
        });

        $("#dlg_edit").on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            account.openDialog($(this), button.parent().data('id'));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });
    }();

});