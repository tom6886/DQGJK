﻿(function ($) {
    $.extend({
        autoInterception: function (text, cut_length, end) {
            if (text.length == 0 || cut_length > $.getLength(text)) return $.htmlencode(text);

            var result = '';
            var j = 0;

            var str_array = text.split('');

            for (var i = 0; i < str_array.length; i++) {
                var c = str_array[i];

                if (text.charCodeAt(i) >= 255) {
                    j = j + 2;
                } else {
                    j = j + 1;
                }

                result = result + c;

                if (j >= cut_length) {
                    if (end) return $.htmlencode(result + end); else return $.htmlencode(result + '...');
                }
            }
        },
        getLength: function (text) {
            var length = 0, c = '';

            for (var i = 0; i < text.length; i++) {
                c = text.charCodeAt(i);
                if (c >= 255) {
                    length = length + 2;
                } else {
                    length = length + 1;
                }
            }
            return length;
        },
        htmlencode: function (text) {
            return $('<div />').text(text).html();
        },
        htmldecode: function (text) {
            return $('<div />').html(text).text();
        },
        dynamic_dialog: function (selector) {
            if ($('#' + selector).length == 0) {
                $('body').append($('<div id="' + selector + '" />'));
            }
            return $('#' + selector);
        }
    });
})(jQuery);

$(function () {
    $.pnotify.defaults.styling = "jqueryui";
    $.pnotify.defaults.history = false;
    $.pnotify.defaults.animation = 'none';
    $.pnotify.defaults.sticker = false;

    window.alert = function (message, type) {
        $.pnotify({
            title: message,
            addclass: 'stack-bar-top',
            width: '30%',
            delay: 5000,
            sticker: false,
            stack: { "dir1": "left", "dir2": "up", "push": "top", "spacing1": 45, "spacing2": 45 },
            type: type || 'info'
        });
    };

    // override jquery validate plugin defaults
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    jQuery.validator.addMethod("cardnum", function (value, element) {
        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        return this.optional(element) || (reg.test(value));
    }, "请正确填写身份证号");

    jQuery.validator.addMethod("phone", function (value, element) {
        var reg = /(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        return this.optional(element) || (reg.test(value));
    }, "请正确填写手机号");

    jQuery.validator.addMethod("lng", function (value, element) {
        var lngRe = /^[-]?(\d|([1-9]\d)|(1[0-7]\d)|(180))(\.\d*)?$/g;
        return this.optional(element) || (lngRe.test(value));
    }, "请正确填写经度");

    jQuery.validator.addMethod("lat", function (value, element) {
        var latRe = /^[-]?(\d|([1-8]\d)|(90))(\.\d*)?$/g;
        return this.optional(element) || (latRe.test(value));
    }, "请正确填写纬度");
});

//字符串format
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
};

//阿拉伯数字转换为中文
String.prototype.IntTozhCNNum = function () {
    var result = this, isInTen = (parseInt(result) > 9 && parseInt(result) < 20), unit = "千百十亿千百十万千百十元", str = "";
    unit = unit.substr(unit.length - result.length);
    for (var i = 0; i < result.length; i++)
        str += '零一二三四五六七八九'.charAt(result.charAt(i)) + unit.charAt(i);
    result = str.replace(/零(千|百|十|角)/g, "零").replace(/(零)+/g, "零").replace(/零(万|亿|元)/g, "$1").replace(/(亿)万|一(十)/g, "$1$2").replace(/元$/g, "");
    if (isInTen) result.substring(0, 1);
    return result;
};

// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//js继承
function extend(subClass, superClass) {
    var F = function () { };
    F.prototype = superClass.prototype;
    subClass.prototype = new F();
    subClass.prototype.constructor = subClass;
    subClass.superclass = superClass.prototype; //加多了个属性指向父类本身以便调用父类函数
    if (superClass.prototype.constructor == Object.prototype.constructor) {
        superClass.prototype.constructor = superClass;
    }
};