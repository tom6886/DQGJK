$(function () {
    var _form = $("#form_password");

    _form.ajaxForm({
        beforeSubmit: function () {
            return _form.valid();
        },
        success: function (r) {
            alert(r.msg);

            if (r.code > 0) {
                $("#dlg_password").modal('hide');
            }
        }
    }).validate({
        rules: {
            OldPassWord: "required",
            NewPassWord: "required",
            CheckPassWord: "required"
        },
        messages: {
            OldPassWord: "请填入当前登录密码",
            NewPassWord: "请输入新密码",
            CheckPassWord: "请再次输入新密码"
        }
    });

    $("#btn_password", _form).click(function () {
        if ($("input[name=newPassWord]", _form).val() !== $("input[name=checkPassWord]", _form).val()) { alert("两次输入的密码不一致，请重新输入"); return false; }

        _form.submit();
    });
});