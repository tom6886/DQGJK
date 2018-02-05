;
(function ($) {
    var globe = { dw: null, bm: null };

    var priv = {
        set: function ($this) {
            $this.each(function (i, v) {
                var func = fc[$(v).data('select')];

                if (!func) { return true; }

                func($(v));
            });
        },
        select: function (obj) {
            var org = obj.elem.select2({
                allowClear: true,
                placeholder: obj.placeholder,
                ajax: {
                    url: "common/" + obj.url,
                    dataType: 'json',
                    quietMillis: 100,
                    data: function (term, page) {
                        var query = { key: term, page: page };

                        if (typeof (obj.addQuery) == "function") {
                            obj.addQuery(query);
                        };

                        return query;
                    },
                    results: function (data, page) {
                        return { results: data.results, more: (page * data.pageSize) < data.total };
                    }
                }, id: function (data) {
                    return data[obj.dataParam];
                }, formatResult: function (data) {
                    return '<span class="select2-match" class="selector_subject_options" ></span>' + data.name;
                }, formatSelection: function (data) {
                    return data.name;
                }, initSelection: function (e, callback) {
                    callback({ id: e.data('id'), name: e.val() });
                }
            });
            if (obj.elem.data('value')) { obj.elem.select2('val', [obj.elem.data('value')]); }

            return org;
        }
    };

    var fc = {
        dw: function (e) {
            globe.dw = priv.select({ elem: e, placeholder: "选择单位", url: "getDept?deptType=0", dataParam: "id" });

            globe.dw.on('change', function (e) {
                if (globe.bm) {
                    globe.bm.select2('val', '');
                }
            });
        },
        bm: function (e) {
            globe.bm = priv.select({
                elem: e, placeholder: "选择部门", url: "getDept?deptType=1", dataParam: "id", addQuery: function (query) {
                    if (globe.dw) {
                        var i = globe.dw.select2('data');
                        if (i) { query.pId = i.id; };
                    }
                }
            });
        },
        u: function (e) {
            globe.bm = priv.select({
                elem: e, placeholder: "选择用户", url: "getUser", dataParam: "id", addQuery: function (query) {
                    if (globe.dw) {
                        var i = globe.bm.select2('data');
                        if (i) { query.pId = i.id; };
                    }
                }
            });
        },
        s: function (e) {
            priv.select({ elem: e, placeholder: "选择站点", url: "getStation", dataParam: "id" });
        },
        r: function (e) {
            priv.select({ elem: e, placeholder: "选择角色", url: "getRoles", dataParam: "id" });
        },
        qbm: function (e) {
            priv.select({
                elem: e, placeholder: "选择部门", url: "getQyDept", dataParam: "id"
            });
        }
    }

    $.fn.select_2 = function () {

        var $this = this;

        if ($this.length == 0) { return; }

        priv.set($this);

        return $this;
    };
})(jQuery);