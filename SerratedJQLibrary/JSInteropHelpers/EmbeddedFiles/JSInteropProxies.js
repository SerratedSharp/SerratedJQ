// TODO: Rename file to HelpersProxy

// This javascript declaration is an embedded resource, and is emitted client side by C# startup code at runtime.
console.log("Declaring JSInteropProxies");

// makes JSExport from JQueryProxy available on globalThis.SerratedExports.SerratedSharp.SerratedJQ.JQueryProxy
//Module.getAssemblyExports("SerratedSharp.SerratedJQ")
//    .then(module => globalThis.SerratedExports = module);

// TODO: Consider object literal notation instead of IIFE, and export as module
var Serrated = globalThis.Serrated || {};
(function (Serrated) {

    //JQueryProxy.Init = function () {
    //    globalThis.SerratedExports = await Module.getAssemblyExports("SerratedSharp.SerratedJQ");
    //}

    var HelpersProxy = Serrated.HelpersProxy || {};// create child namespace

    HelpersProxy.GetArrayObjectItems = function (arrayObject) {
        return arrayObject.items;
    }

    HelpersProxy.LoadScript = function (relativeUrl) {
        return new Promise(function (resolve, reject) {
            var script = document.createElement("script");
            script.onload = resolve;
            script.onerror = reject;
            script.src = relativeUrl;
            document.getElementsByTagName("head")[0].appendChild(script);
        });
    };

    HelpersProxy.LoadjQuery = function (relativeUrl) {
        if (window.jQuery) {
            // already loaded and ready to go
            return Promise.resolve();
        } else {
            return HelpersProxy.LoadScript(relativeUrl);//'jquery-3.7.1.js');
        }
    };

    Serrated.HelpersProxy = HelpersProxy; // add to parent namespace

})(Serrated = globalThis.Serrated || (globalThis.Serrated = {}));

