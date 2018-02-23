;
(function ($) {
    var globe = { dw: null, bm: null };

    var area = {
        province: null, city: null, country: null, level: null, value: null,
        setValue: function () {
            var p = area.province.select2('data');
            var ci = area.city.select2('data');
            var co = area.country.select2('data');

            if (co) { area.level = 3; area.value = co.id; return; }
            if (ci) { area.level = 2; area.value = ci.id; return; }
            if (p) { area.level = 1; area.value = p.id; return; }

            area.level = null;
            area.value = null;
        }
    };

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
            globe.dw = priv.select({ elem: e, placeholder: "选择单位", url: "GetDept?deptType=0", dataParam: "id" });

            globe.dw.on('change', function (e) {
                if (globe.bm) {
                    globe.bm.select2('val', '');
                }
            });
        },
        bm: function (e) {
            globe.bm = priv.select({
                elem: e, placeholder: "选择部门", url: "GetDept?deptType=1", dataParam: "id", addQuery: function (query) {
                    if (globe.dw) {
                        var i = globe.dw.select2('data');
                        if (i) { query.pId = i.id; };
                    }
                }
            });
        },
        u: function (e) {
            globe.bm = priv.select({
                elem: e, placeholder: "选择用户", url: "GetUser", dataParam: "id", addQuery: function (query) {
                    if (globe.dw) {
                        var i = globe.bm.select2('data');
                        if (i) { query.pId = i.id; };
                    }
                }
            });
        },
        s: function (e) {
            priv.select({
                elem: e, placeholder: "选择环网柜", url: "GetStation", dataParam: "id", addQuery: function (query) {
                    if (area.level && area.value) {
                        query.areaType = area.level;
                        query.areaCode = area.value;
                    }
                }
            });
        },
        r: function (e) {
            priv.select({ elem: e, placeholder: "选择角色", url: "GetRoles", dataParam: "id" });
        },
        p: function (e) {
            area.province = priv.select({ elem: e, placeholder: "选择省", url: "GetProvince", dataParam: "name" });

            area.province.on('change', function (e) {
                if (area.city) {
                    area.city.select2('val', '');
                }

                if (area.country) {
                    area.country.select2('val', '');
                }

                area.setValue();
            });
        },
        ci: function (e) {
            area.city = priv.select({
                elem: e, placeholder: "选择市", url: "GetCity", dataParam: "name", addQuery: function (query) {
                    if (area.province) {
                        var i = area.province.select2('data');
                        if (i) { query.pId = i.id; };
                    }
                }
            });

            area.city.on('change', function (e) {
                var i = area.city.select2('data');

                if (area.province && i) {
                    area.province.select2('data', i.province);
                }

                if (area.country) {
                    area.country.select2('val', '');
                }

                area.setValue();
            });
        },
        co: function (e) {
            area.country = priv.select({
                elem: e, placeholder: "选择县", url: "GetCountry", dataParam: "name", addQuery: function (query) {
                    if (area.city) {
                        var i = area.city.select2('data');
                        if (i) { query.pId = i.id; };
                    }
                }
            });

            area.country.on('change', function (e) {
                var i = area.country.select2('data');

                if (!i) { return; }

                if (area.province) {
                    area.province.select2('data', i.province);
                }

                if (area.city) {
                    area.city.select2('data', i.city);
                }

                area.setValue();
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