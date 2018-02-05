;
$(function () {

    var dept = {};

    dept.openDialog = function (modal, deptID) {
        $.post("dept/queryDialog", { deptID: deptID }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            dept.bindDialog(modal);
        });
    };

    dept.bindDialog = function (modal) {
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

    dept.openNodes = function (modal, code) {
        $.post("dept/queryNodes", { code: code }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            modal.html(r);

            $("#addNode", modal).click(function () {
                dept.openNodeEdit($("#parent").val());
            });

            dept.getNodes(code);
        });
    };

    dept.getNodes = function (code) {
        $.post("dept/getNodes", { code: code }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            dept.bindNodes(r);
        });
    }

    dept.bindNodes = function (table) {
        $("#container_nodes").html(table);

        $(".editDict", $("#container_nodes")).click(function () {
            dept.openNodeEdit($("#parent").val(), $(this).parent().data("id"));
        });

        $(".delDept", $("#container_nodes")).click(function () {
            if (!confirm("您确定要删除此部门？")) { return false; }

            var _this = $(this);

            $.post("dept/deleteDept", { deptID: _this.parent().data('id') }, function (r) {
                alert(r.msg);

                if (r.code < 0) {
                    return false;
                }

                dept.getNodes($("#parent").val());
            });
        });
    }

    dept.openNodeEdit = function (parentID, id) {
        $.post("dept/queryNodeEdit", { parentID: parentID, id: id }, function (r) {
            if (r.code < 0) {
                alert(r.msg);
                return false;
            }

            $("#dlg_node_edit").modal("show").html(r);

            dept.bindNodeEdit($("#dlg_node_edit"));
        });
    }

    dept.bindNodeEdit = function (modal) {
        var _form = $("form", modal);

        _form.ajaxForm({
            success: function (r) {
                alert(r.msg);

                if (r.code > 0) {
                    modal.modal('hide');
                    dept.getNodes($("#parent").val());
                }
            }
        });

        $('select[name=Status]', _form).val($("#Status", _form).val());

        $(".save", modal).click(function () {
            _form.submit();
        });
    }

    dept.initPage = function () {
        $(".query").click(function () {
            $("#form_query").submit();
        });

        $("#dlg_edit").on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            dept.openDialog($(this), button.parent().data('id'));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });

        $("#dlg_nodes").on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            dept.openNodes($(this), button.parent().data('id'));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });

        $("#dlg_node_edit").on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });
    }();

});