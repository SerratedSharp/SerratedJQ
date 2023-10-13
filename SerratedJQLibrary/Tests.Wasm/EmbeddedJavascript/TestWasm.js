console.log("TestWasm.js loading");

globalThis.loadScript = function (url) {
    return new Promise(function (resolve, reject) {
        var script = document.createElement("script");
        script.onload = resolve;
        script.onerror = reject;
        script.src = url;
        document.getElementsByTagName("head")[0].appendChild(script);
    });
};

globalThis.loadjQuery = function () {
    if (window.jQuery) {
        // already loaded and ready to go
        return Promise.resolve();
    } else {
        return loadScript('jquery-3.7.1.js');
    }
};


// Usage:
//loadjQuery().then(function () {
//    // code here that uses jQuery
//}, function () {
//    // error loading jQuery
//});


