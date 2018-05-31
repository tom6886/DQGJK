;

var Main = Main || {};

Main.init = function () {
    Main.Interval.Start();
    new Main.PassWord().init();
    new Main.Alarms().init();
    Main.Lights.init();
};

Main.Interval = {
    Start: function () {
        var _this = this;
        _this.Online();
        setInterval(_this.Online, 60000);
    },
    Online: function () {
        $.post("Main/GetOnlineCount", function (r) {
            $("#online-span").text(r.online);
            Main.Lights.overtime.setState(r.overtime);
            Main.Lights.temperature.setState(r.temperature);
            Main.Lights.humidity.setState(r.humidity);
        });
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
};

Main.Alarms = function () {
    this.container = $("#dlg_alarms");
    this.overtime = new Main.BaseAlarm("Main/OverTime");
    this.temperature = new Main.BaseAlarm("Main/Temperature");
    this.humidity = new Main.BaseAlarm("Main/Humidity");
};

Main.Alarms.prototype = {
    init: function () {
        var _this = this;

        _this.container.on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            var fc = _this[button.data("fc")];
            if (!fc) { return; }
            fc.open($(this));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });
    }
}

Main.BaseAlarm = function (url) {
    this.url = url;
};

Main.BaseAlarm.prototype = {
    open: function (modal) {
        var _this = this;

        $.post(_this.url, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);
        });
    }
};

Main.Lights = {
    overtime: null,
    temperature: null,
    humidity: null,
    init: function () {
        this.overtime = new Main.BaseLight($(".a-normal:eq(0)"), $(".a-abnormal:eq(0)"));
        this.temperature = new Main.BaseLight($(".a-normal:eq(1)"), $(".a-abnormal:eq(1)"));
        this.humidity = new Main.BaseLight($(".a-normal:eq(2)"), $(".a-abnormal:eq(2)"));
        return this;
    }
}

Main.BaseLight = function (normal, abnormal) {
    this.normal = normal;
    this.abnormal = abnormal;
}

Main.BaseLight.prototype = {
    setState: function (state) {
        if (state == 0) {
            this.normal.show();
            this.abnormal.hide();
        } else {
            this.normal.hide();
            this.abnormal.show();
        }
    }
}

$(function () {
    Main.init();
});