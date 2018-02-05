;
$(function () {

    var imageLoop = {};

    imageLoop.list = function (pi) {

        var _mode = $("#mode").val();

        if (_mode == 1) { imageLoop.listByStation(pi); }
        else { imageLoop.listByDevice(pi) }
    }

    imageLoop.listByStation = function (pi) {
        $.post("imageLoop/ListByStation", {
            stationID: $("input[name=stationID]").val(), important: $("input[name=important]").val(),
            isDanger: $("input[name=isDanger]").val(), state: $("input[name=state]").val(), pi: pi, ps: $("#showNum").val()
        }, function (r) {
            $("#grid").html(r);

            $(".pagination a").click(function () {
                imageLoop.list($(this).data("pageindex"));
                return false;
            });

            $("#goToBtn").click(function () {
                if (Number($("#pageIndexBox").val()) > Number($("#pageCount").val())) { alert("页索引超出范围"); return false; }
                imageLoop.list($("#pageIndexBox").val());
                return false;
            });
        });
    }

    imageLoop.listByDevice = function (pi) {
        $.post("imageLoop/ListByDevice", { pi: pi, ps: $("#showNum").val() }, function (r) {
            $("#grid").html(r);

            $(".pagination a").click(function () {
                imageLoop.list($(this).data("pageindex"));
                return false;
            });

            $("#goToBtn").click(function () {
                if (Number($("#pageIndexBox").val()) > Number($("#pageCount").val())) { alert("页索引超出范围"); return false; }
                imageLoop.list($("#pageIndexBox").val());
                return false;
            });
        });
    }

    imageLoop.startLoop = function (timer) {
        clearInterval(imageLoop.timer);

        imageLoop.list(1);

        imageLoop.timer = setInterval(function () {
            var _page = $(".nextPage").hasClass("disabled") ? 1 : $(".nextPage").data("pageindex");

            imageLoop.list(_page);
        }, timer * 1000);
    }

    imageLoop.openDialog = function (modal) {
        $.post("imageLoop/queryDialog", function (r) {
            modal.html(r);

            imageLoop.bindDialog(modal);
        });
    }

    imageLoop.bindDialog = function (modal) {
        $("._select", modal).select_2();

        $(".modal-query").click(function () {
            imageLoop.deviceAddList($("input[name=modal-stationID]").val(), $("select[name=modal-important]").val(),
                    $("select[name=modal-isDanger]").val(), $("select[name=modal-state]").val(), $(this).data("pageindex"));
        });

        $(".save", modal).click(function () {
            imageLoop.deviceAdd(modal);
        });

        imageLoop.deviceAddList();
    }

    imageLoop.deviceAdd = function (modal) {
        var _devices = new Array();

        $("button", "#panel-devices").each(function (i, v) {
            _devices.push($(v).attr("data-code"));
        });

        $.ajax({
            type: "post",
            traditional: true,
            url: "imageLoop/deviceAdd",
            data: { codes: _devices },
            success: function (r) {
                alert(r.msg);

                modal.modal('hide');

                $("#form_query").submit();
            }
        });
    }

    imageLoop.deviceAddList = function (stationID, important, isDanger, state, pi) {
        $.post("imageLoop/DeviceAddList", { stationID: stationID, important: important, isDanger: isDanger, state: state, pi: pi }, function (r) {
            $("#ModalList").html(r);

            $(".pagination a", $("#ModalList")).click(function () {
                imageLoop.deviceAddList($("input[name=modal-stationID]").val(), $("select[name=modal-important]").val(),
                    $("select[name=modal-isDanger]").val(), $("select[name=modal-state]").val(), $(this).data("pageindex"));
                return false;
            });

            $("#goToBtn", $("#ModalList")).click(function () {
                if (Number($("#pageIndexBox").val()) > Number($("#pageCount").val())) { alert("页索引超出范围"); return false; }

                imageLoop.deviceAddList($("input[name=modal-stationID]").val(), $("select[name=modal-important]").val(),
                    $("select[name=modal-isDanger]").val(), $("select[name=modal-state]").val(), $(this).data("pageindex"));
                return false;
            });
        });
    }

    imageLoop.initPage = function () {
        $("#mode").change(function () {
            var _this = $(this);

            $(".loop-mode").hide();
            if (_this.val() == 1) {
                $("#allLoop").show();
                $(".btn-addDevice").hide();
            } else {
                $("#deviceLoop").show();
                $(".btn-addDevice").show();
            }
        });

        $("._select").select_2();

        $(".startLoop").click(function () {
            var _timer = $("#timer").val();

            if (isNaN(_timer)) { alert("间隔时间请输入正确数字"); return false; }

            launchIntoFullscreen(document.getElementById("grid"));
        });

        $("#dlg_edit").on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget);
            imageLoop.openDialog($(this));
        }).on('hidden.bs.modal', function () {
            $(".modal-dialog", $(this)).remove();
        });

        // Events
        document.addEventListener("fullscreenchange", function (e) {
            fullScreenChange($("#grid").is(":hidden"));
        });
        document.addEventListener("mozfullscreenchange", function (e) {
            fullScreenChange($("#grid").is(":hidden"));
        });
        document.addEventListener("webkitfullscreenchange", function (e) {
            fullScreenChange($("#grid").is(":hidden"));
        });
        document.addEventListener("msfullscreenchange", function (e) {
            fullScreenChange($("#grid").is(":hidden"));
        });
    }();

    function fullScreenChange(isFull) {
        if (isFull) {
            imageLoop.startLoop($("#timer").val());

            $("#grid").css({ "display": "block" });
        } else {
            clearInterval(imageLoop.timer);
            $("#grid").css({ "display": "none" });
        }
    }

    function launchIntoFullscreen(element) {
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        }
    }

    function exitFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    }
});