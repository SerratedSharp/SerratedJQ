// TODO: Rename file to HelpersProxy

// This javascript declaration is an embedded resource, and is emitted client side by C# startup code at runtime.
console.log("Declaring SerratedJQ Shims");

// makes JSExport from JQueryProxy available on globalThis.SerratedExports.SerratedSharp.SerratedJQ.JQueryProxy
//Module.getAssemblyExports("SerratedSharp.SerratedJQ")
//    .then(module => globalThis.SerratedExports = module);

// TODO: Consider object literal notation instead of IIFE, and export as module
var SerratedJQ = globalThis.SerratedJQ || {};
(function (SerratedJQ) {

    //JQueryProxy.Init = function () {
    //    globalThis.SerratedExports = await Module.getAssemblyExports("SerratedSharp.SerratedJQ");
    //}
    
    var JQueryProxy = SerratedJQ.JQueryProxy || {};// create child namespace

    JQueryProxy.Ready = function () {
        return new Promise(function (resolve, reject) {
            jQuery(function () {
                resolve();
            });
        });
    }

    JQueryProxy.Select = function (selector) {
        return jQuery(document).find(selector);
    };
    JQueryProxy.ParseHtml = function (html, keepScripts) {
        //console.log('ParseHtml proxy: ', html, keepScripts);
        const tst = jQuery(jQuery.parseHTML(html, undefined, keepScripts));
        //console.log("Rtn tst: ", tst);
        return tst;
    };

    JQueryProxy.BindListener = function (jsObject, events, shouldConvertHtmlElement, action, selector) {
        let handler = function (e) {
            //console.log('On Fired');
            // declare a javascript array called replacers, and implement JSON.stringify with a replacer that adds items which are of type HTMLElements to the array
            var replacements = [];
            var eEncoded = JSON.stringify(e, function (key, value) {
                if (this[key] instanceof HTMLElement) { // getType(value) === "[object HTMLElement]") {

                    if (shouldConvertHtmlElement) {
                        replacements.push(jQuery(value));
                    }
                    else {
                        replacements.push(value);
                    }
                    // replace with placeholder and the index of where the object is stored in replacers array
                    return { serratedPlaceholder: replacements.length - 1 };
                }
                return value;
            });
            // TODO: If we don't find any replacements then send null and supress the unpack GetArrayObjectItems call on C# side(unnecesary interop call).
            action(eEncoded, e.type, new ArrayObject(replacements));
        }.bind(jsObject);

        // TODO: Implment variation that takes selector for live/late binding
        //jsObject.on(events, selector, handler);)
        jsObject.on(events, selector, handler);
        return handler; // return reference to the handler so it can be passed later for .off
    }

    JQueryProxy.UnbindListener = function (jsObject, events, handler,   selector) {
        // TODO: If we implement the selector overload of .on, we need to pass the selector here too (must match exactly, see JQ docs)
        jsObject.off(events, selector, handler);// https://api.jquery.com/off/
    }

    JQueryProxy.Check = function () { console.log("check"); }

    //JQueryProxy.Init = function () {
    //    globalThis.SerratedExports = await Module.getAssemblyExports("SerratedSharp.SerratedJQ");
    //}

    class ArrayObject {
        constructor(items) {
            this.items = items;
        }
    }

    SerratedJQ.JQueryProxy = JQueryProxy; // add to parent namespace

})(SerratedJQ = globalThis.SerratedJQ || (globalThis.SerratedJQ = {}));

export { SerratedJQ };
