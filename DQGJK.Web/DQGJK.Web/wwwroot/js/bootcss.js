/*
css3-mediaqueries.js - CSS Helper and CSS3 Media Queries Enabler

author: Wouter van der Graaf <wouter at dynora nl>
version: 1.0 (20110330)
license: MIT
website: http://code.google.com/p/css3-mediaqueries-js/

W3C spec: http://www.w3.org/TR/css3-mediaqueries/

Note: use of embedded <style> is not recommended when using media queries, because IE  has no way of returning the raw literal css text from a <style> element.
*/


// true prototypal inheritance (http://javascript.crockford.com/prototypal.html)
if (typeof Object.create !== 'function') {
    Object.create = function (o) {
        function F() { }
        F.prototype = o;
        return new F();
    };
}


// user agent sniffing shortcuts
var ua = {
    toString: function () {
        return navigator.userAgent;
    },
    test: function (s) {
        return this.toString().toLowerCase().indexOf(s.toLowerCase()) > -1;
    }
};
ua.version = (ua.toString().toLowerCase().match(/[\s\S]+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1];
ua.webkit = ua.test('webkit');
ua.gecko = ua.test('gecko') && !ua.webkit;
ua.opera = ua.test('opera');
ua.ie = ua.test('msie') && !ua.opera;
ua.ie6 = ua.ie && document.compatMode && typeof document.documentElement.style.maxHeight === 'undefined';
ua.ie7 = ua.ie && document.documentElement && typeof document.documentElement.style.maxHeight !== 'undefined' && typeof XDomainRequest === 'undefined';
ua.ie8 = ua.ie && typeof XDomainRequest !== 'undefined';



// initialize when DOM content is loaded
var domReady = function () {
    var fns = [];
    var init = function () {
        if (!arguments.callee.done) { // run init functions once
            arguments.callee.done = true;
            for (var i = 0; i < fns.length; i++) {
                fns[i]();
            }
        }
    };

    // listeners for different browsers
    if (document.addEventListener) {
        document.addEventListener('DOMContentLoaded', init, false);
    }
    if (ua.ie) {
        (function () {
            try {
                // throws errors until after ondocumentready
                document.documentElement.doScroll('left');

                // If we are in an iframe, the above does not work properly.
                // Trying to access the length attribute of document.body, however,
                // does throw an error until ondocumentready, fixing this issue.
                document.body.length;
            }
            catch (e) {
                setTimeout(arguments.callee, 50);
                return;
            }
            // no errors, fire
            init();
        })();
        // trying to always fire before onload
        document.onreadystatechange = function () {
            if (document.readyState === 'complete') {
                document.onreadystatechange = null;
                init();
            }
        };
    }
    if (ua.webkit && document.readyState) {
        (function () {
            if (document.readyState !== 'loading') {
                init();
            }
            else {
                setTimeout(arguments.callee, 10);
            }
        })();
    }
    window.onload = init; // fallback

    return function (fn) { // add fn to init functions
        if (typeof fn === 'function') {
            // If DOM ready has already been fired, fire the function
            // right away.
            if (init.done) {
                fn();
            } else {
                // Add to the queue
                fns[fns.length] = fn;
            }
        }
        return fn;
    };
}();


// helper library for parsing css to objects
var cssHelper = function () {

    var regExp = {
        BLOCKS: /[^\s{;][^{;]*\{(?:[^{}]*\{[^{}]*\}[^{}]*|[^{}]*)*\}/g,
        BLOCKS_INSIDE: /[^\s{][^{]*\{[^{}]*\}/g,
        DECLARATIONS: /[a-zA-Z\-]+[^;]*:[^;]+;/g,
        RELATIVE_URLS: /url\(['"]?([^\/\)'"][^:\)'"]+)['"]?\)/g,
        // strip whitespace and comments, @import is evil
        REDUNDANT_COMPONENTS: /(?:\/\*([^*\\\\]|\*(?!\/))+\*\/|@import[^;]+;|@-moz-document\s*url-prefix\(\)\s*{(([^{}])+{([^{}])+}([^{}])+)+})/g,
        REDUNDANT_WHITESPACE: /\s*(,|:|;|\{|\})\s*/g,
        WHITESPACE_IN_PARENTHESES: /\(\s*(\S*)\s*\)/g,
        MORE_WHITESPACE: /\s{2,}/g,
        FINAL_SEMICOLONS: /;\}/g,
        NOT_WHITESPACE: /\S+/g
    };

    var parsed, parsing = false;

    var waiting = [];
    var wait = function (fn) {
        if (typeof fn === 'function') {
            waiting[waiting.length] = fn;
        }
    };
    var ready = function () {
        for (var i = 0; i < waiting.length; i++) {
            waiting[i](parsed);
        }
    };
    var events = {};
    var broadcast = function (n, v) {
        if (events[n]) {
            var listeners = events[n].listeners;
            if (listeners) {
                for (var i = 0; i < listeners.length; i++) {
                    listeners[i](v);
                }
            }
        }
    };

    var requestText = function (url, fnSuccess, fnFailure) {
        if (ua.ie && !window.XMLHttpRequest) {
            window.XMLHttpRequest = function () {
                return new ActiveXObject('Microsoft.XMLHTTP');
            };
        }
        if (!XMLHttpRequest) {
            return '';
        }
        var r = new XMLHttpRequest();
        try {
            r.open('get', url, true);
            r.setRequestHeader('X_REQUESTED_WITH', 'XMLHttpRequest');
        }
        catch (e) {
            fnFailure();
            return;
        }
        var done = false;
        setTimeout(function () {
            done = true;
        }, 5000);
        document.documentElement.style.cursor = 'progress';
        r.onreadystatechange = function () {
            if (r.readyState === 4 && !done) {
                if (!r.status && location.protocol === 'file:' ||
						(r.status >= 200 && r.status < 300) ||
						r.status === 304 ||
						navigator.userAgent.indexOf('Safari') > -1 && typeof r.status === 'undefined') {
                    fnSuccess(r.responseText);
                }
                else {
                    fnFailure();
                }
                document.documentElement.style.cursor = '';
                r = null; // avoid memory leaks
            }
        };
        r.send('');
    };

    var sanitize = function (text) {
        text = text.replace(regExp.REDUNDANT_COMPONENTS, '');
        text = text.replace(regExp.REDUNDANT_WHITESPACE, '$1');
        text = text.replace(regExp.WHITESPACE_IN_PARENTHESES, '($1)');
        text = text.replace(regExp.MORE_WHITESPACE, ' ');
        text = text.replace(regExp.FINAL_SEMICOLONS, '}'); // optional final semicolons
        return text;
    };

    var objects = {
        stylesheet: function (el) {
            var o = {};
            var amqs = [], mqls = [], rs = [], rsw = [];
            var s = el.cssHelperText;

            // add attribute media queries
            var attr = el.getAttribute('media');
            if (attr) {
                var qts = attr.toLowerCase().split(',')
            }
            else {
                var qts = ['all'] // imply 'all'
            }
            for (var i = 0; i < qts.length; i++) {
                amqs[amqs.length] = objects.mediaQuery(qts[i], o);
            }

            // add media query lists and rules (top down order)
            var blocks = s.match(regExp.BLOCKS); // @charset is not a block
            if (blocks !== null) {
                for (var i = 0; i < blocks.length; i++) {
                    if (blocks[i].substring(0, 7) === '@media ') { // media query (list)
                        var mql = objects.mediaQueryList(blocks[i], o);
                        rs = rs.concat(mql.getRules());
                        mqls[mqls.length] = mql;
                    }
                    else { // regular rule set, page context (@page) or font description (@font-face)
                        rs[rs.length] = rsw[rsw.length] = objects.rule(blocks[i], o, null);
                    }
                }
            }

            o.element = el;
            o.getCssText = function () {
                return s;
            };
            o.getAttrMediaQueries = function () {
                return amqs;
            };
            o.getMediaQueryLists = function () {
                return mqls;
            };
            o.getRules = function () {
                return rs;
            };
            o.getRulesWithoutMQ = function () {
                return rsw;
            };
            return o;
        },

        mediaQueryList: function (s, stsh) {
            var o = {};
            var idx = s.indexOf('{');
            var lt = s.substring(0, idx);
            s = s.substring(idx + 1, s.length - 1);
            var mqs = [], rs = [];

            // add media queries
            var qts = lt.toLowerCase().substring(7).split(',');
            for (var i = 0; i < qts.length; i++) { // parse each media query
                mqs[mqs.length] = objects.mediaQuery(qts[i], o);
            }

            // add rule sets
            var rts = s.match(regExp.BLOCKS_INSIDE);
            if (rts !== null) {
                for (i = 0; i < rts.length; i++) {
                    rs[rs.length] = objects.rule(rts[i], stsh, o);
                }
            }

            o.type = 'mediaQueryList';
            o.getMediaQueries = function () {
                return mqs;
            };
            o.getRules = function () {
                return rs;
            };
            o.getListText = function () {
                return lt;
            };
            o.getCssText = function () {
                return s;
            };
            return o;
        },

        mediaQuery: function (s, listOrSheet) {
            s = s || '';
            var mql, stsh;
            if (listOrSheet.type === 'mediaQueryList') {
                mql = listOrSheet;
            }
            else {
                stsh = listOrSheet;
            }
            var not = false, type;
            var expr = [];
            var valid = true;
            var tokens = s.match(regExp.NOT_WHITESPACE);



            for (var i = 0; i < tokens.length; i++) {
                var token = tokens[i];
                if (!type && (token === 'not' || token === 'only')) { // 'not' and 'only' keywords
                    // keyword 'only' does nothing, as if it was not present
                    if (token === 'not') {
                        not = true;
                    }
                }
                else if (!type) { // media type
                    type = token;
                }
                else if (token.charAt(0) === '(') { // media feature expression
                    var pair = token.substring(1, token.length - 1).split(':');
                    expr[expr.length] = {
                        mediaFeature: pair[0],
                        value: pair[1] || null
                    };
                }
            }

            return {
                getQueryText: function () {
                    return s;
                },
                getAttrStyleSheet: function () {
                    return stsh || null;
                },
                getList: function () {
                    return mql || null;
                },
                getValid: function () {
                    return valid;
                },
                getNot: function () {
                    return not;
                },
                getMediaType: function () {
                    return type;
                },
                getExpressions: function () {
                    return expr;
                }
            };
        },

        rule: function (s, stsh, mql) {
            var o = {};
            var idx = s.indexOf('{');
            var st = s.substring(0, idx);
            var ss = st.split(',');
            var ds = [];
            var dts = s.substring(idx + 1, s.length - 1).split(';');
            for (var i = 0; i < dts.length; i++) {
                ds[ds.length] = objects.declaration(dts[i], o);
            }

            o.getStylesheet = function () {
                return stsh || null;
            };
            o.getMediaQueryList = function () {
                return mql || null;
            };
            o.getSelectors = function () {
                return ss;
            };
            o.getSelectorText = function () {
                return st;
            };
            o.getDeclarations = function () {
                return ds;
            };
            o.getPropertyValue = function (n) {
                for (var i = 0; i < ds.length; i++) {
                    if (ds[i].getProperty() === n) {
                        return ds[i].getValue();
                    }
                }
                return null;
            };
            return o;
        },

        declaration: function (s, r) {
            var idx = s.indexOf(':');
            var p = s.substring(0, idx);
            var v = s.substring(idx + 1);
            return {
                getRule: function () {
                    return r || null;
                },
                getProperty: function () {
                    return p;
                },
                getValue: function () {
                    return v;
                }
            };
        }
    };

    var parseText = function (el) {
        if (typeof el.cssHelperText !== 'string') {
            return;
        }
        var o = {
            stylesheet: null,
            mediaQueryLists: [],
            rules: [],
            selectors: {},
            declarations: [],
            properties: {}
        };

        // build stylesheet object
        var stsh = o.stylesheet = objects.stylesheet(el);

        // collect media query lists
        var mqls = o.mediaQueryLists = stsh.getMediaQueryLists();

        // collect all rules
        var ors = o.rules = stsh.getRules();

        // collect all selectors
        var oss = o.selectors;
        var collectSelectors = function (r) {
            var ss = r.getSelectors();
            for (var i = 0; i < ss.length; i++) {
                var n = ss[i];
                if (!oss[n]) {
                    oss[n] = [];
                }
                oss[n][oss[n].length] = r;
            }
        };
        for (i = 0; i < ors.length; i++) {
            collectSelectors(ors[i]);
        }

        // collect all declarations
        var ods = o.declarations;
        for (i = 0; i < ors.length; i++) {
            ods = o.declarations = ods.concat(ors[i].getDeclarations());
        }

        // collect all properties
        var ops = o.properties;
        for (i = 0; i < ods.length; i++) {
            var n = ods[i].getProperty();
            if (!ops[n]) {
                ops[n] = [];
            }
            ops[n][ops[n].length] = ods[i];
        }

        el.cssHelperParsed = o;
        parsed[parsed.length] = el;
        return o;
    };

    var parseEmbedded = function (el, s) {
        return;
        // This function doesn't work because of a bug in IE, where innerHTML gives us parsed css instead of raw literal.
        el.cssHelperText = sanitize(s || el.innerHTML);
        return parseText(el);
    };

    var parse = function () {
        parsing = true;
        parsed = [];
        var linked = [];
        var finish = function () {
            for (var i = 0; i < linked.length; i++) {
                parseText(linked[i]);
            }
            var styles = document.getElementsByTagName('style');
            for (i = 0; i < styles.length; i++) {
                parseEmbedded(styles[i]);
            }
            parsing = false;
            ready();
        };
        var links = document.getElementsByTagName('link');
        for (var i = 0; i < links.length; i++) {
            var link = links[i];
            if (link.getAttribute('rel').indexOf('style') > -1 && link.href && link.href.length !== 0 && !link.disabled) {
                linked[linked.length] = link;
            }
        }
        if (linked.length > 0) {
            var c = 0;
            var checkForFinish = function () {
                c++;
                if (c === linked.length) { // parse in right order, so after last link is read
                    finish();
                }
            };
            var processLink = function (link) {
                var href = link.href;
                requestText(href, function (text) {
                    // fix url's
                    text = sanitize(text).replace(regExp.RELATIVE_URLS, 'url(' + href.substring(0, href.lastIndexOf('/')) + '/$1)');
                    link.cssHelperText = text;
                    checkForFinish();
                }, checkForFinish);
            };
            for (i = 0; i < linked.length; i++) {
                processLink(linked[i]);
            }
        }
        else {
            finish();
        }
    };

    var types = {
        stylesheets: 'array',
        mediaQueryLists: 'array',
        rules: 'array',
        selectors: 'object',
        declarations: 'array',
        properties: 'object'
    };

    var collections = {
        stylesheets: null,
        mediaQueryLists: null,
        rules: null,
        selectors: null,
        declarations: null,
        properties: null
    };

    var addToCollection = function (name, v) {
        if (collections[name] !== null) {
            if (types[name] === 'array') {
                return (collections[name] = collections[name].concat(v));
            }
            else {
                var c = collections[name];
                for (var n in v) {
                    if (v.hasOwnProperty(n)) {
                        if (!c[n]) {
                            c[n] = v[n];
                        }
                        else {
                            c[n] = c[n].concat(v[n]);
                        }
                    }
                }
                return c;
            }
        }
    };

    var collect = function (name) {
        collections[name] = (types[name] === 'array') ? [] : {};
        for (var i = 0; i < parsed.length; i++) {
            var pname = name === 'stylesheets' ? 'stylesheet' : name; // the exception
            addToCollection(name, parsed[i].cssHelperParsed[pname]);
        }
        return collections[name];
    };

    // viewport size
    var getViewportSize = function (d) {
        if (typeof window.innerWidth != 'undefined') {
            return window['inner' + d];
        }
        else if (typeof document.documentElement !== 'undefined'
				&& typeof document.documentElement.clientWidth !== 'undefined'
				&& document.documentElement.clientWidth != 0) {
            return document.documentElement['client' + d];
        }
    };

    // public static functions
    return {
        addStyle: function (s, mediaTypes, process) {
            var el;
            var styleElId = 'css-mediaqueries-js';
            var styleMedia = '';

            var styleEl = document.getElementById(styleElId);

            if (mediaTypes && mediaTypes.length > 0) {
                styleMedia = mediaTypes.join(',');
                styleElId += styleMedia;
            }

            if (null !== styleEl) {
                el = styleEl;
            }
            else {
                el = document.createElement('style');
                el.setAttribute('type', 'text/css');
                el.setAttribute('id', styleElId);
                el.setAttribute('media', styleMedia);
                document.getElementsByTagName('head')[0].appendChild(el);
            }

            if (el.styleSheet) { // IE
                el.styleSheet.cssText += s;
            }
            else {
                el.appendChild(document.createTextNode(s));
            }

            el.addedWithCssHelper = true;

            if (typeof process === 'undefined' || process === true) {
                cssHelper.parsed(function (parsed) {
                    var o = parseEmbedded(el, s);
                    for (var n in o) {
                        if (o.hasOwnProperty(n)) {
                            addToCollection(n, o[n]);
                        }
                    }
                    broadcast('newStyleParsed', el);
                });
            }
            else {
                el.parsingDisallowed = true;
            }
            return el;
        },

        removeStyle: function (el) {
            if (el.parentNode)
                return el.parentNode.removeChild(el);
        },

        parsed: function (fn) {
            if (parsing) {
                wait(fn);
            }
            else {
                if (typeof parsed !== 'undefined') {
                    if (typeof fn === 'function') {
                        fn(parsed);
                    }
                }
                else {
                    wait(fn);
                    parse();
                }
            }
        },

        stylesheets: function (fn) {
            cssHelper.parsed(function (parsed) {
                fn(collections.stylesheets || collect('stylesheets'));
            });
        },

        mediaQueryLists: function (fn) {
            cssHelper.parsed(function (parsed) {
                fn(collections.mediaQueryLists || collect('mediaQueryLists'));
            });
        },

        rules: function (fn) {
            cssHelper.parsed(function (parsed) {
                fn(collections.rules || collect('rules'));
            });
        },

        selectors: function (fn) {
            cssHelper.parsed(function (parsed) {
                fn(collections.selectors || collect('selectors'));
            });
        },

        declarations: function (fn) {
            cssHelper.parsed(function (parsed) {
                fn(collections.declarations || collect('declarations'));
            });
        },

        properties: function (fn) {
            cssHelper.parsed(function (parsed) {
                fn(collections.properties || collect('properties'));
            });
        },

        broadcast: broadcast,

        addListener: function (n, fn) { // in case n is 'styleadd': added function is called everytime style is added and parsed
            if (typeof fn === 'function') {
                if (!events[n]) {
                    events[n] = {
                        listeners: []
                    };
                }
                events[n].listeners[events[n].listeners.length] = fn;
            }
        },

        removeListener: function (n, fn) {
            if (typeof fn === 'function' && events[n]) {
                var ls = events[n].listeners;
                for (var i = 0; i < ls.length; i++) {
                    if (ls[i] === fn) {
                        ls.splice(i, 1);
                        i -= 1;
                    }
                }
            }
        },

        getViewportWidth: function () {
            return getViewportSize('Width');
        },

        getViewportHeight: function () {
            return getViewportSize('Height');
        }
    };
}();



// function to test and apply parsed media queries against browser capabilities
domReady(function enableCssMediaQueries() {
    var meter;

    var regExp = {
        LENGTH_UNIT: /[0-9]+(em|ex|px|in|cm|mm|pt|pc)$/,
        RESOLUTION_UNIT: /[0-9]+(dpi|dpcm)$/,
        ASPECT_RATIO: /^[0-9]+\/[0-9]+$/,
        ABSOLUTE_VALUE: /^[0-9]*(\.[0-9]+)*$/
    };

    var styles = [];

    var nativeSupport = function () {
        // check support for media queries
        var id = 'css3-mediaqueries-test';
        var el = document.createElement('div');
        el.id = id;
        var style = cssHelper.addStyle('@media all and (width) { #' + id +
			' { width: 1px !important; } }', [], false); // false means don't parse this temp style
        document.body.appendChild(el);
        var ret = el.offsetWidth === 1;
        style.parentNode.removeChild(style);
        el.parentNode.removeChild(el);
        nativeSupport = function () {
            return ret;
        };
        return ret;
    };

    var createMeter = function () { // create measuring element
        meter = document.createElement('div');
        meter.style.cssText = 'position:absolute;top:-9999em;left:-9999em;' +
			'margin:0;border:none;padding:0;width:1em;font-size:1em;'; // cssText is needed for IE, works for the others
        document.body.appendChild(meter);
        // meter must have browser default font size of 16px
        if (meter.offsetWidth !== 16) {
            meter.style.fontSize = 16 / meter.offsetWidth + 'em';
        }
        meter.style.width = '';
    };

    var measure = function (value) {
        meter.style.width = value;
        var amount = meter.offsetWidth;
        meter.style.width = '';
        return amount;
    };

    var testMediaFeature = function (feature, value) {
        // non-testable features: monochrome|min-monochrome|max-monochrome|scan|grid
        var l = feature.length;
        var min = (feature.substring(0, 4) === 'min-');
        var max = (!min && feature.substring(0, 4) === 'max-');

        if (value !== null) { // determine value type and parse to usable amount
            var valueType;
            var amount;
            if (regExp.LENGTH_UNIT.exec(value)) {
                valueType = 'length';
                amount = measure(value);
            }
            else if (regExp.RESOLUTION_UNIT.exec(value)) {
                valueType = 'resolution';
                amount = parseInt(value, 10);
                var unit = value.substring((amount + '').length);
            }
            else if (regExp.ASPECT_RATIO.exec(value)) {
                valueType = 'aspect-ratio';
                amount = value.split('/');
            }
            else if (regExp.ABSOLUTE_VALUE) {
                valueType = 'absolute';
                amount = value;
            }
            else {
                valueType = 'unknown';
            }
        }

        var width, height;
        if ('device-width' === feature.substring(l - 12, l)) { // screen width
            width = screen.width;
            if (value !== null) {
                if (valueType === 'length') {
                    return ((min && width >= amount) || (max && width < amount) || (!min && !max && width === amount));
                }
                else {
                    return false;
                }
            }
            else { // test width without value
                return width > 0;
            }
        }
        else if ('device-height' === feature.substring(l - 13, l)) { // screen height
            height = screen.height;
            if (value !== null) {
                if (valueType === 'length') {
                    return ((min && height >= amount) || (max && height < amount) || (!min && !max && height === amount));
                }
                else {
                    return false;
                }
            }
            else { // test height without value
                return height > 0;
            }
        }
        else if ('width' === feature.substring(l - 5, l)) { // viewport width
            width = document.documentElement.clientWidth || document.body.clientWidth; // the latter for IE quirks mode
            if (value !== null) {
                if (valueType === 'length') {
                    return ((min && width >= amount) || (max && width < amount) || (!min && !max && width === amount));
                }
                else {
                    return false;
                }
            }
            else { // test width without value
                return width > 0;
            }
        }
        else if ('height' === feature.substring(l - 6, l)) { // viewport height
            height = document.documentElement.clientHeight || document.body.clientHeight; // the latter for IE quirks mode
            if (value !== null) {
                if (valueType === 'length') {
                    return ((min && height >= amount) || (max && height < amount) || (!min && !max && height === amount));
                }
                else {
                    return false;
                }
            }
            else { // test height without value
                return height > 0;
            }
        }
        else if ('orientation' === feature.substring(l - 11, l)) { // orientation

            width = document.documentElement.clientWidth || document.body.clientWidth; // the latter for IE quirks mode
            height = document.documentElement.clientHeight || document.body.clientHeight; // the latter for IE quirks mode

            if (valueType === 'absolute') {
                return (amount === 'portrait') ? (width <= height) : (width > height);
            }
            else {
                return false;
            }
        }
        else if ('aspect-ratio' === feature.substring(l - 12, l)) { // window aspect ratio
            width = document.documentElement.clientWidth || document.body.clientWidth; // the latter for IE quirks mode
            height = document.documentElement.clientHeight || document.body.clientHeight; // the latter for IE quirks mode

            var curRatio = width / height;
            var ratio = amount[1] / amount[0];

            if (valueType === 'aspect-ratio') {
                return ((min && curRatio >= ratio) || (max && curRatio < ratio) || (!min && !max && curRatio === ratio));
            }
            else {
                return false;
            }
        }
        else if ('device-aspect-ratio' === feature.substring(l - 19, l)) { // screen aspect ratio
            return valueType === 'aspect-ratio' && screen.width * amount[1] === screen.height * amount[0];
        }
        else if ('color-index' === feature.substring(l - 11, l)) { // number of colors
            var colors = Math.pow(2, screen.colorDepth);
            if (value !== null) {
                if (valueType === 'absolute') {
                    return ((min && colors >= amount) || (max && colors < amount) || (!min && !max && colors === amount));
                }
                else {
                    return false;
                }
            }
            else { // test height without value
                return colors > 0;
            }
        }
        else if ('color' === feature.substring(l - 5, l)) { // bits per color component
            var color = screen.colorDepth;
            if (value !== null) {
                if (valueType === 'absolute') {
                    return ((min && color >= amount) || (max && color < amount) || (!min && !max && color === amount));
                }
                else {
                    return false;
                }
            }
            else { // test height without value
                return color > 0;
            }
        }
        else if ('resolution' === feature.substring(l - 10, l)) {
            var res;
            if (unit === 'dpcm') {
                res = measure('1cm');
            }
            else {
                res = measure('1in');
            }
            if (value !== null) {
                if (valueType === 'resolution') {
                    return ((min && res >= amount) || (max && res < amount) || (!min && !max && res === amount));
                }
                else {
                    return false;
                }
            }
            else { // test height without value
                return res > 0;
            }
        }
        else {
            return false;
        }
    };

    var testMediaQuery = function (mq) {
        var test = mq.getValid();
        var expressions = mq.getExpressions();
        var l = expressions.length;
        if (l > 0) {
            for (var i = 0; i < l && test; i++) {
                test = testMediaFeature(expressions[i].mediaFeature, expressions[i].value);
            }
            var not = mq.getNot();
            return (test && !not || not && !test);
        }
        return test;
    };

    var testMediaQueryList = function (mql, ts) {
        // ts is null or an array with any media type but 'all'.
        var mqs = mql.getMediaQueries();
        var t = {};
        for (var i = 0; i < mqs.length; i++) {
            var type = mqs[i].getMediaType();
            if (mqs[i].getExpressions().length === 0) {
                continue;
                // TODO: Browser check! Assuming old browsers do apply the bare media types, even in a list with media queries.
            }
            var typeAllowed = true;
            if (type !== 'all' && ts && ts.length > 0) {
                typeAllowed = false;
                for (var j = 0; j < ts.length; j++) {
                    if (ts[j] === type) {
                        typeAllowed = true;
                    }
                }
            }
            if (typeAllowed && testMediaQuery(mqs[i])) {
                t[type] = true;
            }
        }
        var s = [], c = 0;
        for (var n in t) {
            if (t.hasOwnProperty(n)) {
                if (c > 0) {
                    s[c++] = ',';
                }
                s[c++] = n;
            }
        }
        if (s.length > 0) {
            styles[styles.length] = cssHelper.addStyle('@media ' + s.join('') + '{' + mql.getCssText() + '}', ts, false);
        }
    };

    var testMediaQueryLists = function (mqls, ts) {
        for (var i = 0; i < mqls.length; i++) {
            testMediaQueryList(mqls[i], ts);
        }
    };

    var testStylesheet = function (stsh) {
        var amqs = stsh.getAttrMediaQueries();
        var allPassed = false;
        var t = {};
        for (var i = 0; i < amqs.length; i++) {
            if (testMediaQuery(amqs[i])) {
                t[amqs[i].getMediaType()] = amqs[i].getExpressions().length > 0;
            }
        }
        var ts = [], tswe = [];
        for (var n in t) {
            if (t.hasOwnProperty(n)) {
                ts[ts.length] = n;
                if (t[n]) {
                    tswe[tswe.length] = n
                }
                if (n === 'all') {
                    allPassed = true;
                }
            }
        }
        if (tswe.length > 0) { // types with query expressions that passed the test
            styles[styles.length] = cssHelper.addStyle(stsh.getCssText(), tswe, false);
        }
        var mqls = stsh.getMediaQueryLists();
        if (allPassed) {
            // If 'all' in media attribute passed the test, then test all @media types in linked CSS and create style with those types.
            testMediaQueryLists(mqls);
        }
        else {
            // Or else, test only media attribute types that passed the test and also 'all'.
            // For positive '@media all', create style with attribute types that passed their test.
            testMediaQueryLists(mqls, ts);
        }
    };

    var testStylesheets = function (stshs) {
        for (var i = 0; i < stshs.length; i++) {
            testStylesheet(stshs[i]);
        }
        if (ua.ie) {
            // force repaint in IE
            document.documentElement.style.display = 'block';
            setTimeout(function () {
                document.documentElement.style.display = '';
            }, 0);
            // delay broadcast somewhat for IE
            setTimeout(function () {
                cssHelper.broadcast('cssMediaQueriesTested');
            }, 100);
        }
        else {
            cssHelper.broadcast('cssMediaQueriesTested');
        }
    };

    var test = function () {
        for (var i = 0; i < styles.length; i++) {
            cssHelper.removeStyle(styles[i]);
        }
        styles = [];
        cssHelper.stylesheets(testStylesheets);
    };

    var scrollbarWidth = 0;
    var checkForResize = function () {
        var cvpw = cssHelper.getViewportWidth();
        var cvph = cssHelper.getViewportHeight();

        // determine scrollbar width in IE, see resizeHandler
        if (ua.ie) {
            var el = document.createElement('div');
            el.style.position = 'absolute';
            el.style.top = '-9999em';
            el.style.overflow = 'scroll';
            document.body.appendChild(el);
            scrollbarWidth = el.offsetWidth - el.clientWidth;
            document.body.removeChild(el);
        }

        var timer;
        var resizeHandler = function () {
            var vpw = cssHelper.getViewportWidth();
            var vph = cssHelper.getViewportHeight();
            // check whether vp size has really changed, because IE also triggers resize event when body size changes
            // 20px allowance to accomodate short appearance of scrollbars in IE in some cases
            if (Math.abs(vpw - cvpw) > scrollbarWidth || Math.abs(vph - cvph) > scrollbarWidth) {
                cvpw = vpw;
                cvph = vph;
                clearTimeout(timer);
                timer = setTimeout(function () {
                    if (!nativeSupport()) {
                        test();
                    }
                    else {
                        cssHelper.broadcast('cssMediaQueriesTested');
                    }
                }, 500);
            }
        };

        window.onresize = function () {
            var x = window.onresize || function () { }; // save original
            return function () {
                x();
                resizeHandler();
            };
        }();
    };

    // prevent jumping of layout by hiding everything before painting <body>
    var docEl = document.documentElement;
    docEl.style.marginLeft = '-32767px';

    // make sure it comes back after a while
    setTimeout(function () {
        docEl.style.marginLeft = '';
    }, 5000);

    return function () {
        if (!nativeSupport()) { // if browser doesn't support media queries
            cssHelper.addListener('newStyleParsed', function (el) {
                testStylesheet(el.cssHelperParsed.stylesheet);
            });
            // return visibility after media queries are tested
            cssHelper.addListener('cssMediaQueriesTested', function () {
                // force repaint in IE by changing width
                if (ua.ie) {
                    docEl.style.width = '1px';
                }
                setTimeout(function () {
                    docEl.style.width = ''; // undo width
                    docEl.style.marginLeft = ''; // undo hide
                }, 0);
                // remove this listener to prevent following execution
                cssHelper.removeListener('cssMediaQueriesTested', arguments.callee);
            });
            createMeter();
            test();
        }
        else {
            docEl.style.marginLeft = ''; // undo visibility hidden
        }
        checkForResize();
    };
}());


// bonus: hotfix for IE6 SP1 (bug KB823727)
try {
    document.execCommand('BackgroundImageCache', false, true);
} catch (e) { }
/*! HTML5 Shiv pre3.5 | @afarkas @jdalton @jon_neal @rem | MIT/GPL2 Licensed
  Uncompressed source: https://github.com/aFarkas/html5shiv  */
(function (a, b) { function h(a, b) { var c = a.createElement("p"), d = a.getElementsByTagName("head")[0] || a.documentElement; return c.innerHTML = "x<style>" + b + "</style>", d.insertBefore(c.lastChild, d.firstChild) } function i() { var a = l.elements; return typeof a == "string" ? a.split(" ") : a } function j(a) { var b = {}, c = a.createElement, f = a.createDocumentFragment, g = f(); a.createElement = function (a) { l.shivMethods || c(a); var f; return b[a] ? f = b[a].cloneNode() : e.test(a) ? f = (b[a] = c(a)).cloneNode() : f = c(a), f.canHaveChildren && !d.test(a) ? g.appendChild(f) : f }, a.createDocumentFragment = Function("h,f", "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" + i().join().replace(/\w+/g, function (a) { return b[a] = c(a), g.createElement(a), 'c("' + a + '")' }) + ");return n}")(l, g) } function k(a) { var b; return a.documentShived ? a : (l.shivCSS && !f && (b = !!h(a, "article,aside,details,figcaption,figure,footer,header,hgroup,nav,section{display:block}audio{display:none}canvas,video{display:inline-block;*display:inline;*zoom:1}[hidden]{display:none}audio[controls]{display:inline-block;*display:inline;*zoom:1}mark{background:#FF0;color:#000}")), g || (b = !j(a)), b && (a.documentShived = b), a) } function p(a) { var b, c = a.getElementsByTagName("*"), d = c.length, e = RegExp("^(?:" + i().join("|") + ")$", "i"), f = []; while (d--) b = c[d], e.test(b.nodeName) && f.push(b.applyElement(q(b))); return f } function q(a) { var b, c = a.attributes, d = c.length, e = a.ownerDocument.createElement(n + ":" + a.nodeName); while (d--) b = c[d], b.specified && e.setAttribute(b.nodeName, b.nodeValue); return e.style.cssText = a.style.cssText, e } function r(a) { var b, c = a.split("{"), d = c.length, e = RegExp("(^|[\\s,>+~])(" + i().join("|") + ")(?=[[\\s,>+~#.:]|$)", "gi"), f = "$1" + n + "\\:$2"; while (d--) b = c[d] = c[d].split("}"), b[b.length - 1] = b[b.length - 1].replace(e, f), c[d] = b.join("}"); return c.join("{") } function s(a) { var b = a.length; while (b--) a[b].removeNode() } function t(a) { var b, c, d = a.namespaces, e = a.parentWindow; return !o || a.printShived ? a : (typeof d[n] == "undefined" && d.add(n), e.attachEvent("onbeforeprint", function () { var d, e, f, g = a.styleSheets, i = [], j = g.length, k = Array(j); while (j--) k[j] = g[j]; while (f = k.pop()) if (!f.disabled && m.test(f.media)) { for (d = f.imports, j = 0, e = d.length; j < e; j++) k.push(d[j]); try { i.push(f.cssText) } catch (l) { } } i = r(i.reverse().join("")), c = p(a), b = h(a, i) }), e.attachEvent("onafterprint", function () { s(c), b.removeNode(!0) }), a.printShived = !0, a) } var c = a.html5 || {}, d = /^<|^(?:button|form|map|select|textarea|object|iframe)$/i, e = /^<|^(?:a|b|button|code|div|fieldset|form|h1|h2|h3|h4|h5|h6|i|iframe|img|input|label|li|link|ol|option|p|param|q|script|select|span|strong|style|table|tbody|td|textarea|tfoot|th|thead|tr|ul)$/i, f, g; (function () { var c = b.createElement("a"); c.innerHTML = "<xyz></xyz>", f = "hidden" in c, f && typeof injectElementWithStyles == "function" && injectElementWithStyles("#modernizr{}", function (b) { b.hidden = !0, f = (a.getComputedStyle ? getComputedStyle(b, null) : b.currentStyle).display == "none" }), g = c.childNodes.length == 1 || function () { try { b.createElement("a") } catch (a) { return !0 } var c = b.createDocumentFragment(); return typeof c.cloneNode == "undefined" || typeof c.createDocumentFragment == "undefined" || typeof c.createElement == "undefined" }() })(); var l = { elements: c.elements || "abbr article aside audio bdi canvas data datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video", shivCSS: c.shivCSS !== !1, shivMethods: c.shivMethods !== !1, type: "default", shivDocument: k }; a.html5 = l, k(b); var m = /^$|\b(?:all|print)\b/, n = "html5shiv", o = !g && function () { var c = b.documentElement; return typeof b.namespaces != "undefined" && typeof b.parentWindow != "undefined" && typeof c.applyElement != "undefined" && typeof c.removeNode != "undefined" && typeof a.attachEvent != "undefined" }(); l.type += " print", l.shivPrint = t, t(b) })(this, document)
/*! matchMedia() polyfill - Test a CSS media type/query in JS. Authors & copyright (c) 2012: Scott Jehl, Paul Irish, Nicholas Zakas. Dual MIT/BSD license */
/*! NOTE: If you're already including a window.matchMedia polyfill via Modernizr or otherwise, you don't need this part */
(function (w) {
    "use strict";
    w.matchMedia = w.matchMedia || function (doc, undefined) {
        var bool, docElem = doc.documentElement, refNode = docElem.firstElementChild || docElem.firstChild, fakeBody = doc.createElement("body"), div = doc.createElement("div");
        div.id = "mq-test-1";
        div.style.cssText = "position:absolute;top:-100em";
        fakeBody.style.background = "none";
        fakeBody.appendChild(div);
        return function (q) {
            div.innerHTML = '&shy;<style media="' + q + '"> #mq-test-1 { width: 42px; }</style>';
            docElem.insertBefore(fakeBody, refNode);
            bool = div.offsetWidth === 42;
            docElem.removeChild(fakeBody);
            return {
                matches: bool,
                media: q
            };
        };
    }(w.document);
})(this);

/*! Respond.js v1.4.0: min/max-width media query polyfill. (c) Scott Jehl. MIT Lic. j.mp/respondjs  */
(function (w) {
    "use strict";
    var respond = {};
    w.respond = respond;
    respond.update = function () { };
    var requestQueue = [], xmlHttp = function () {
        var xmlhttpmethod = false;
        try {
            xmlhttpmethod = new w.XMLHttpRequest();
        } catch (e) {
            xmlhttpmethod = new w.ActiveXObject("Microsoft.XMLHTTP");
        }
        return function () {
            return xmlhttpmethod;
        };
    }(), ajax = function (url, callback) {
        var req = xmlHttp();
        if (!req) {
            return;
        }
        req.open("GET", url, true);
        req.onreadystatechange = function () {
            if (req.readyState !== 4 || req.status !== 200 && req.status !== 304) {
                return;
            }
            callback(req.responseText);
        };
        if (req.readyState === 4) {
            return;
        }
        req.send(null);
    };
    respond.ajax = ajax;
    respond.queue = requestQueue;
    respond.regex = {
        media: /@media[^\{]+\{([^\{\}]*\{[^\}\{]*\})+/gi,
        keyframes: /@.*keyframes[^\{]+\{(?:[^\{\}]*\{[^\}\{]*\})+[^\}]+\}/gi,
        urls: /(url\()['"]?([^\/\)'"][^:\)'"]+)['"]?(\))/g,
        findStyles: /@media *([^\{]+)\{([\S\s]+?)$/,
        only: /(only\s+)?([a-zA-Z]+)\s?/,
        minw: /\([\s]*min\-width\s*:[\s]*([\s]*[0-9\.]+)(px|em)[\s]*\)/,
        maxw: /\([\s]*max\-width\s*:[\s]*([\s]*[0-9\.]+)(px|em)[\s]*\)/
    };
    respond.mediaQueriesSupported = w.matchMedia && w.matchMedia("only all") !== null && w.matchMedia("only all").matches;
    if (respond.mediaQueriesSupported) {
        return;
    }
    var doc = w.document, docElem = doc.documentElement, mediastyles = [], rules = [], appendedEls = [], parsedSheets = {}, resizeThrottle = 30, head = doc.getElementsByTagName("head")[0] || docElem, base = doc.getElementsByTagName("base")[0], links = head.getElementsByTagName("link"), lastCall, resizeDefer, eminpx, getEmValue = function () {
        var ret, div = doc.createElement("div"), body = doc.body, originalHTMLFontSize = docElem.style.fontSize, originalBodyFontSize = body && body.style.fontSize, fakeUsed = false;
        div.style.cssText = "position:absolute;font-size:1em;width:1em";
        if (!body) {
            body = fakeUsed = doc.createElement("body");
            body.style.background = "none";
        }
        docElem.style.fontSize = "100%";
        body.style.fontSize = "100%";
        body.appendChild(div);
        if (fakeUsed) {
            docElem.insertBefore(body, docElem.firstChild);
        }
        ret = div.offsetWidth;
        if (fakeUsed) {
            docElem.removeChild(body);
        } else {
            body.removeChild(div);
        }
        docElem.style.fontSize = originalHTMLFontSize;
        if (originalBodyFontSize) {
            body.style.fontSize = originalBodyFontSize;
        }
        ret = eminpx = parseFloat(ret);
        return ret;
    }, applyMedia = function (fromResize) {
        var name = "clientWidth", docElemProp = docElem[name], currWidth = doc.compatMode === "CSS1Compat" && docElemProp || doc.body[name] || docElemProp, styleBlocks = {}, lastLink = links[links.length - 1], now = new Date().getTime();
        if (fromResize && lastCall && now - lastCall < resizeThrottle) {
            w.clearTimeout(resizeDefer);
            resizeDefer = w.setTimeout(applyMedia, resizeThrottle);
            return;
        } else {
            lastCall = now;
        }
        for (var i in mediastyles) {
            if (mediastyles.hasOwnProperty(i)) {
                var thisstyle = mediastyles[i], min = thisstyle.minw, max = thisstyle.maxw, minnull = min === null, maxnull = max === null, em = "em";
                if (!!min) {
                    min = parseFloat(min) * (min.indexOf(em) > -1 ? eminpx || getEmValue() : 1);
                }
                if (!!max) {
                    max = parseFloat(max) * (max.indexOf(em) > -1 ? eminpx || getEmValue() : 1);
                }
                if (!thisstyle.hasquery || (!minnull || !maxnull) && (minnull || currWidth >= min) && (maxnull || currWidth <= max)) {
                    if (!styleBlocks[thisstyle.media]) {
                        styleBlocks[thisstyle.media] = [];
                    }
                    styleBlocks[thisstyle.media].push(rules[thisstyle.rules]);
                }
            }
        }
        for (var j in appendedEls) {
            if (appendedEls.hasOwnProperty(j)) {
                if (appendedEls[j] && appendedEls[j].parentNode === head) {
                    head.removeChild(appendedEls[j]);
                }
            }
        }
        for (var k in styleBlocks) {
            if (styleBlocks.hasOwnProperty(k)) {
                var ss = doc.createElement("style"), css = styleBlocks[k].join("\n");
                ss.type = "text/css";
                ss.media = k;
                head.insertBefore(ss, lastLink.nextSibling);
                if (ss.styleSheet) {
                    ss.styleSheet.cssText = css;
                } else {
                    ss.appendChild(doc.createTextNode(css));
                }
                appendedEls.push(ss);
            }
        }
    }, translate = function (styles, href, media) {
        var qs = styles.replace(respond.regex.keyframes, "").match(respond.regex.media), ql = qs && qs.length || 0;
        href = href.substring(0, href.lastIndexOf("/"));
        var repUrls = function (css) {
            return css.replace(respond.regex.urls, "$1" + href + "$2$3");
        }, useMedia = !ql && media;
        if (href.length) {
            href += "/";
        }
        if (useMedia) {
            ql = 1;
        }
        for (var i = 0; i < ql; i++) {
            var fullq, thisq, eachq, eql;
            if (useMedia) {
                fullq = media;
                rules.push(repUrls(styles));
            } else {
                fullq = qs[i].match(respond.regex.findStyles) && RegExp.$1;
                rules.push(RegExp.$2 && repUrls(RegExp.$2));
            }
            eachq = fullq.split(",");
            eql = eachq.length;
            for (var j = 0; j < eql; j++) {
                thisq = eachq[j];
                mediastyles.push({
                    media: thisq.split("(")[0].match(respond.regex.only) && RegExp.$2 || "all",
                    rules: rules.length - 1,
                    hasquery: thisq.indexOf("(") > -1,
                    minw: thisq.match(respond.regex.minw) && parseFloat(RegExp.$1) + (RegExp.$2 || ""),
                    maxw: thisq.match(respond.regex.maxw) && parseFloat(RegExp.$1) + (RegExp.$2 || "")
                });
            }
        }
        applyMedia();
    }, makeRequests = function () {
        if (requestQueue.length) {
            var thisRequest = requestQueue.shift();
            ajax(thisRequest.href, function (styles) {
                translate(styles, thisRequest.href, thisRequest.media);
                parsedSheets[thisRequest.href] = true;
                w.setTimeout(function () {
                    makeRequests();
                }, 0);
            });
        }
    }, ripCSS = function () {
        for (var i = 0; i < links.length; i++) {
            var sheet = links[i], href = sheet.href, media = sheet.media, isCSS = sheet.rel && sheet.rel.toLowerCase() === "stylesheet";
            if (!!href && isCSS && !parsedSheets[href]) {
                if (sheet.styleSheet && sheet.styleSheet.rawCssText) {
                    translate(sheet.styleSheet.rawCssText, href, media);
                    parsedSheets[href] = true;
                } else {
                    if (!/^([a-zA-Z:]*\/\/)/.test(href) && !base || href.replace(RegExp.$1, "").split("/")[0] === w.location.host) {
                        if (href.substring(0, 2) === "//") {
                            href = w.location.protocol + href;
                        }
                        requestQueue.push({
                            href: href,
                            media: media
                        });
                    }
                }
            }
        }
        makeRequests();
    };
    ripCSS();
    respond.update = ripCSS;
    respond.getEmValue = getEmValue;
    function callMedia() {
        applyMedia(true);
    }
    if (w.addEventListener) {
        w.addEventListener("resize", callMedia, false);
    } else if (w.attachEvent) {
        w.attachEvent("onresize", callMedia);
    }
})(this);