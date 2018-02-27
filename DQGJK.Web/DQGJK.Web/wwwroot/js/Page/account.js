;
var Account = Account || {};

Account.Widgets = {
    dialog: null,
    table: null,
    init: function () {
        this.dialog = new Account.Dialog();
        this.table = new Account.Table();

        return this;
    }
};

Account.Table = function () {
    this.keyInput = $("input[name=key]");
    this.container = $("#unseen");
    this.pageIndexBox = $("#pageIndexBox");

    return this;
};

Account.Table.prototype = {
    query: function (pi) {
        var _this = this;

        $.get("user/List", { key: _this.keyInput.val(), pi: pi }, function (r) {
            _this.container.html(r);
        });
    },
    delete: function (userID) {
        var _this = this;

        $.post("user/Delete", { userID: userID }, function (r) {
            alert(r.msg);

            if (r.code < 0) { return false; }

            _this.query(_this.pageIndexBox.val());
        });
    }
}

Account.Dialog = function () { };

Account.Dialog.prototype = {
    open: function (modal, userID) {
        var _this = this;

        $.post("user/Dialog", { userID: userID }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            _this._bind(modal);
        });
    },
    _bind: function (modal) {
        var _form = $("form", modal);

        _form.ajaxForm({
            beforeSubmit: function () {
                return _form.valid();
            },
            success: function (r) {
                alert(r.msg);

                if (r.code > 0) {
                    modal.modal('hide');
                    Account.Widgets.table.query();
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

        $("input._select", _form).on("change", function () {
            var _this = $(this);

            if (_this.val()) {
                _this.parents(".form-group:first").removeClass("has-error");
                _this.parents(".form-group:first").children("span").remove();
            }
            else {
                var html = '<span for="StationID" generated="true" class="help-block">{0}是必选项</span>';
                _this.parents(".form-group:first").addClass("has-error");
                _this.parent().after(html.format(_this.siblings("span").text()));
            }
        });

        $(".save", modal).click(function () {
            if ($(".has-error").length > 0) { return false; }
            _form.submit();
        });
    }
}

$(function () {
    var widgets = Account.Widgets.init();

    widgets.table.query();

    $(".query").click(function () {
        widgets.table.query();
    });

    $("#dlg_edit").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        widgets.dialog.open($(this), button.parent().data('id'));
    }).on('hidden.bs.modal', function () {
        $(".modal-dialog", $(this)).remove();
    });
});