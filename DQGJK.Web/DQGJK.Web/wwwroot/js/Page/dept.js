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
    query: function (pi) {
        var _this = this;

        $.get("department/List", { key: _this.keyInput.val(), pi: pi }, function (r) {
            _this.container.html(r);
        });
    },
    delete: function (deptID) {
        var _this = this;

        $.post("department/Delete", { deptID: deptID }, function (r) {
            alert(r.msg);

            if (r.code < 0) { return false; }

            _this.query(_this.pageIndexBox.val());
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

Dept.NodesTable = function () { };

Dept.NodesTable.prototype = {
    container: null,
    init: function () {
        this.container = $("#container_nodes");

        return this;
    },
    query: function () {
        var _this = this;

        $.get("department/NodesList", { code: Dept.Widgets.nodes.code }, function (r) {
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

Dept.NodesDialog = function () { };

Dept.NodesDialog.prototype = {
    open: function (id) {
        var _this = this;

        $.post("department/NodesDialog", { parentID: Dept.Widgets.nodes.code, id: id }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            $("#dlg_node_edit").modal("show").html(r);

            _this._bind($("#dlg_node_edit"));
        });
    },
    _bind: function (modal) {
        var _form = $("form", modal);

        _form.ajaxForm({
            success: function (r) {
                alert(r.msg);

                if (r.code > 0) {
                    modal.modal('hide');
                    Dept.Widgets.nodes.table.query();
                }
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
    table: null,
    dialog: null,
    code: null,
    open: function (modal, code) {
        var _this = this;
        _this.code = code;

        $.post("department/Nodes", { code: code }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            _this.dialog = new Dept.NodesDialog();

            $("#addNode", modal).click(function () {
                _this.dialog.open();
            });

            _this.table = new Dept.NodesTable().init(code);

            _this.table.query();
        });
    }
}

$(function () {
    var widgets = Dept.Widgets.init();

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