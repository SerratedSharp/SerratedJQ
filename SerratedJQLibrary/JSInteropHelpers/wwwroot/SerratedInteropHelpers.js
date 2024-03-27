// TODO: Rename file to HelpersProxy

// This javascript declaration is an embedded resource, and is emitted client side by C# startup code at runtime.
console.log("Declaring SerratedInteropHelpers Shims with export");

// makes JSExport from JQueryProxy available on globalThis.SerratedExports.SerratedSharp.SerratedJQ.JQueryProxy
//Module.getAssemblyExports("SerratedSharp.SerratedJQ")
//    .then(module => globalThis.SerratedExports = module);

// TODO: Consider object literal notation instead of IIFE, and export as module
var SerratedInteropHelpers = globalThis.SerratedInteropHelpers || {};
(function (SerratedInteropHelpers) {

    //JQueryProxy.Init = function () {
    //    globalThis.SerratedExports = await Module.getAssemblyExports("SerratedSharp.SerratedJQ");
    //}

    var HelpersProxy = SerratedInteropHelpers.HelpersProxy || {};// create child namespace

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

    //HelpersProxy.LoadScriptWithContent = function (scriptContent) {
    //    return new Promise(function (resolve, reject) {
    //        var script = document.createElement("script");
    //        script.onload = resolve;
    //        script.onerror = reject;
    //        script.text = scriptContent;
    //        document.getElementsByTagName("head")[0].appendChild(script);
    //    });
    //};

    HelpersProxy.LoadjQuery = function (relativeUrl) {
        if (window.jQuery) {
            // already loaded and ready to go
            return Promise.resolve();
        } else {
            return HelpersProxy.LoadScript(relativeUrl);//'jquery-3.7.1.js');
        }
    };

    HelpersProxy.FuncByNameToObject = function (jsObject, funcName, params) {
        //console.log('FuncByNameToObject proxy: ', jsObject, funcName, params);
        const rtn = jsObject[funcName].apply(jsObject, params);
        //console.log('Return proxy: ', rtn);
        return rtn;
    };

    HelpersProxy.PropertyByNameToObject = function (jsObject, propertyName) {
        //console.log('PropertyByNameToObject proxy: ', jsObject, propertyName);
        const rtn = jsObject[propertyName];
        //console.log('Return proxy: ', rtn);
        return rtn;
    };

    SerratedInteropHelpers.HelpersProxy = HelpersProxy; // add to parent namespace
    


})(SerratedInteropHelpers = globalThis.SerratedInteropHelpers || (globalThis.SerratedInteropHelpers = {}));

export { SerratedInteropHelpers };
