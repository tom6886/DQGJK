;
var Account = Account || {};

Account.Widgets = function () { }

Account.Widgets = {
    dialog: null,
    table: null,
    init: function () {
        this.dialog = new Account.Dialog();
        this.table = new Account.Table().init();

        return this;
    }
};

Account.Table = function () { };

Account.Table.prototype = {
    keyInput: null,
    container: null,
    pageIndexBox: null,
    init: function () {
        this.keyInput = $("input[name=key]");
        this.container = $("#unseen");
        this.pageIndexBox = $("#pageIndexBox");

        return this;
    },
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
}

$(function () {
    var widgets = Account.Widgets.init();

    widgets.table.query();

    $("#dlg_edit").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        widgets.dialog.open($(this), button.parent().data('id'));
    }).on('hidden.bs.modal', function () {
        $(".modal-dialog", $(this)).remove();
    });
});