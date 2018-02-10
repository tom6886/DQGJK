;
var Dept = Dept || {};

Dept.Widgets = function () { }

Dept.Widgets = {
    dialog: null,
    nodes: null,
    table: null,
    init: function () {
        this.dialog = new Dept.Dialog();
        this.nodes = new Dept.Nodes();
        this.table = new Dept.Table().init();

        return this;
    }
};

Dept.Table = function () { };

Dept.Table.prototype = {
    keyInput: null,
    container: null,
    pageIndexBox: null,
    init: function () {
        this.keyInput = $("input[name=key]");
        this.container = $("#unseen");
        this.pageIndexBox = $("#pageIndexBox");

        return this;
    },
    query: function () {
        var _this = this;

        $.get("department/List", { key: _this.keyInput.val(), pi: _this.pageIndexBox.val() }, function (r) {
            _this.container.html(r);
        });
    },
    delete: function (deptID) {
        var _this = this;

        $.post("department/Delete", { deptID: deptID }, function (r) {
            alert(r.msg);

            if (r.code < 0) { return false; }

            _this.query();
        });
    }
}

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
                    Dept.Widgets.table.query();
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

Dept.Nodes = function () { };

Dept.Nodes.prototype = {
    open: function (modal, code) {
        var _this = this;

        $.post("department/Nodes", { code: code }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            $("#addNode", modal).click(function () {
                dept.openNodeEdit($("#parent").val());
            });

            _this._get(code);
        });
    },
    _get: function (code) {
        var _this = this;

        $.post("department/NodesGet", { code: code }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            _this._bind(r);
        });
    },
    _bind: function (table) {
        //var _this = this;

        //$("#container_nodes").html(table);

        //$(".editDict", $("#container_nodes")).click(function () {
        //    dept.openNodeEdit($("#parent").val(), $(this).parent().data("id"));
        //});

        //$(".delDept", $("#container_nodes")).click(function () {
        //    if (!confirm("您确定要删除此部门？")) { return false; }

        //    var _this = $(this);

        //    $.post("dept/deleteDept", { deptID: _this.parent().data('id') }, function (r) {
        //        alert(r.msg);

        //        if (r.code < 0) {
        //            return false;
        //        }

        //        dept.getNodes($("#parent").val());
        //    });
        //});
    }
}

$(function () {
    var widgets = Dept.Widgets.init();

    widgets.table.query();

    $("#dlg_edit").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        widgets.dialog.open($(this), button.parent().data('id'));
    }).on('hidden.bs.modal', function () {
        $(".modal-dialog", $(this)).remove();
    });

    var nodes = new Dept.Nodes();

    $("#dlg_nodes").on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        widgets.nodes.open($(this), button.parent().data('id'));
    }).on('hidden.bs.modal', function () {
        $(".modal-dialog", $(this)).remove();
    });

    $("#dlg_node_edit").on('hidden.bs.modal', function () {
        $(".modal-dialog", $(this)).remove();
    });
});