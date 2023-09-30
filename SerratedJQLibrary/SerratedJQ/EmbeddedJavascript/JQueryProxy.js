// This javascript declaration is an embedded resource, and is emitted client side by C# startup code at runtime.
console.log("Declaring JQuery Proxy");

var Serrated = globalThis.Serrated || {};
(function (Serrated) {
    //var Callbacks = Serrated.Callbacks || {};
    //Callbacks.UnpinEventListener = function () {
    //    InternalSJQ.Listener('UnpinEventListener');
    //};
    //Serrated.Callbacks = Callbacks;

    var JQueryProxy = Serrated.JQueryProxy || {};// create child namespace
    JQueryProxy.Select = function (selector) {
        console.log("Select called");
        return jQuery(document).find(selector);
    };
    JQueryProxy.ParseHtml = function (html, keepScripts) {
        return jQuery( jQuery.parseHTML( html, undefined, keepScripts) );
    };
    JQueryProxy.FuncByNameToObject = function (jqObject, funcName, params) {
        return jqObject[funcName].apply(jqObject, params);
    };



    Serrated.JQueryProxy = JQueryProxy; // add to parent namespace

})(Serrated = globalThis.Serrated || (globalThis.Serrated = {}));

