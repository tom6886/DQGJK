;
$(function () {

    var images = {};

    images.openDialog = function (modal, id) {
        $.post("images/queryDialog", { id: id }, function (r) {
            modal.html(r);
            images.bindDialog(modal);
        });
    };

    images.bindDialog = function (modal) {
        var _form = $("form", modal);

        _form.ajaxForm({
            beforeSubmit: function () {
                return _form.valid();
            },
            success: function (r) {
                alert(r.msg);
                modal.modal('hide');

                if (r.code > 0) {
                    $("#form_query").submit();
                }
            }
        }).validate({
            rules: {
                title: "required"
            },
            messages: {
                title: "标题是必填项"
            }
        });

        $(".save", modal).click(function () {
            _form.submit();
        });
    }

    images.initPage = function () {

        $('.form_date').datetimepicker({
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });

        var date = new Date();
        var start = date.getFullYear() + "/" + (date.getMonth() + 1) + "/01";
        var end = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();

        $("input[name=StartDate]").val(start);
        $("input[name=EndDate]").val(end);

        $(".query").click(function () {
            $("#form_query").submit();
        });

        $("#dlg_push").on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            images.openDialog($(this), button.parent().data('id'));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });

        $("._select").select_2();
    }();

});