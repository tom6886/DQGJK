;
(function ($) {
    var globe = { ty: null, yy: null };

    var priv = {
        set: function ($this) {
            $this.each(function (i, v) {
                var func = fc[$(v).data('board')];

                if (!func) { return true; }

                func($(v));
            });
        }
    };

    var fc = {
        t: function (e) {

        },
        y: function (e) {

        }
    }

    $.fn.dashboard = function () {

        var $this = this;

        if ($this.length == 0) { return; }

        priv.set($this);

        return $this;
    };
})(jQuery);