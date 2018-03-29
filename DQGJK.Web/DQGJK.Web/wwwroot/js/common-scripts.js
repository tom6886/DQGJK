;

var Main = Main || {};

Main.init = function () {
    Main.Interval.Start();
    new Main.PassWord().init();
};

Main.Interval = {
    Start: function () {
        var _this = this;
        setInterval(_this.Online, 6000);
    },
    Online: function () {
        $.post("Common/GetOnlineCount", function (r) { $("#online-span").text(r); });
    }
};

Main.PassWord = function () {
    var _this = this;
    _this.form = $("#form_password");
    _this.modal = $("#dlg_password");
    _this.button = $("#btn_password", _this.form);
    _this.NewPassWord = $("input[name=newPassWord]", _this.form);
    _this.CheckPassWord = $("input[name=checkPassWord]", _this.form);
};

Main.PassWord.prototype = {
    check: function () {
        var _this = this;
        if (_this.NewPassWord.val() !== _this.CheckPassWord.val()) {
            alert("两次输入的密码不一致，请重新输入");
            return false;
        }
        return true;
    },
    init: function () {
        var _this = this;

        _this.form.ajaxForm({
            beforeSubmit: function () {
                return _this.form.valid();
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

        _this.button.click(function () {
            if (!_this.check()) { return; }
            _this.form.submit();
        });
    }
}

$(function () {
    Main.init();
});