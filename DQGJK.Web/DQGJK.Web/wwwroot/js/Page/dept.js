;
var Dept = Dept || {};

Dept.Data = {
    query: function (key, pi) {
        $.get("department/List", { key: key, pi: pi }, function (r) {
            $("#unseen").html(r);
        });
    },
    delete: function (deptID) {
        $.post("department/Delete", { deptID: deptID }, function (r) {
            alert(r.msg);

            if (r.code < 0) {
                return false;
            }

            Dept.Data.query($("input[name=key]").val(), $("#pageIndexBox").val());
        });
    }
};

Dept.Dialog = function () { };

Dept.Dialog.prototype = {
    open: function (modal, deptID) {
        var _this = this;

        $.post("department/Dialog", { deptID: deptID }, function (r) {
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
                    Dept.Data.query($("input[name=key]").val(), 1);
                }
            }
        }).validate({
            rules: {
                Name: "required",
                Code: "required"
            },
            messages: {
                Name: "组织机构名称是必填项",
                Code: "组织机构编号是必填项"
            }
        });

        $('select[name=Status]', _form).val($("#Status", _form).val());

        $(".save", modal).click(function () {
            _form.submit();
        });
    }
}

$(function () {

    var dialog = new Dept.Dialog();

    Dept.Data.query();

    $("#dlg_edit").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        dialog.open($(this), button.parent().data('id'));
    }).on('hidden.bs.modal', function () {
        $(".modal-dialog", $(this)).remove();
    });
});