;
var Station = Station || {};

Station.Widgets = function () { }

Station.Widgets = {
    dialog: null,
    table: null,
    init: function () {
        this.dialog = new Station.Dialog();
        this.table = new Station.Table().init();

        return this;
    }
};

Station.Table = function () { };

Station.Table.prototype = {
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

        $.get("station/List", { key: _this.keyInput.val(), pi: pi }, function (r) {
            _this.container.html(r);
        });
    },
    delete: function (stationID) {
        var _this = this;

        $.post("station/Delete", { stationID: stationID }, function (r) {
            alert(r.msg);

            if (r.code < 0) { return false; }

            _this.query(_this.pageIndexBox.val());
        });
    }
}

Station.Dialog = function () { };

Station.Dialog.prototype = {
    open: function (modal, stationID) {
        var _this = this;

        $.post("station/Dialog", { stationID: stationID }, function (r) {
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
                    Station.Widgets.table.query();
                }
            }
        }).validate({
            rules: {
                Name: "required",
                Code: "required",
                Address: "required"
            },
            messages: {
                Name: "环网柜名称是必填项",
                Code: "环网柜编号是必填项",
                Address: "环网柜地址是必填项"
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

            var country = $("input[name=Country]", modal).select2("data");

            $("input[name=CityCode]", modal).val(country.id);

            _form.submit();
        });
    }
}

$(function () {
    var widgets = Station.Widgets.init();

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