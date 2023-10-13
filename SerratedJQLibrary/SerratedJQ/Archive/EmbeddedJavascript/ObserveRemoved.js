// This javascript declaration is an embedded resource, and is emitted client side by C# startup code at runtime.
console.log("Observe declared");
//var targetNodes = $(document.documentElement); // $('.myclass');
var MutationObserver = window.MutationObserver || window.WebKitMutationObserver;
var observer = new MutationObserver(mutationHandler);
var obsConfig = {
    childList: true,
    characterData: true,
    attributes: true,
    subtree: true
};

//--- Add a target node to the observer. Can only add one node at a time.
//console.log('register observer...');
observer.observe($(document.documentElement)[0], obsConfig); // add observer to document root
//targetNodes.each(function() {
//  myObserver.observe(this, obsConfig);
//});
globalThis.nodePtrs = new WeakMap();

function mutationHandler(mutationRecords) {
    //console.log('mutationHandler:');

    mutationRecords.forEach(function (mutation) {
        //console.log(mutation.type);

        if (typeof mutation.removedNodes == 'object') {
            //debugger;
            //var jq = $(mutation.removedNodes[0]);
            mutation.removedNodes.forEach(
                function (node) {
                    //console.log('The removed node', node);
                    //console.log('Node from map', nodePtrs.get(node));
                    //var jq2 = $(node);
                    //console.log('Remove Observed2');
                    //console.log(jq2);
                    //console.log(jq2.data());
                    //console.log("nodePtrs", nodePtrs);
                    //console.log("nodePtrs.get(node)", nodePtrs.get(node));
                    //console.log("nodePtrs.get(node)", nodePtrs.get($(node)));
                    //console.log('Remove Observed for pin ID: ' + );
                    //console.log(jq);
                    if (globalThis.nodePtrs.get(node) !== undefined) {
                        //console.log('Pntr Found in weak map');
                        //console.log('Pntr Found in weak map ' + nodePtrs.get(node));
                        InternalSerratedJQBox.UnpinEventListener(globalThis.nodePtrs.get(node));
                    }
                    //if (jq2.data('SerratedJQBPntr') !== undefined) {
                    //    console.log('Pntr Found');
                    //    console.log('Ptr ' + jq2.data('SerratedJQBPntr'));
                    //}

                }
            );

            //console.log('Remove Observed');
            //console.log(jq);
            //console.log(jq.data());

            ////console.log('Remove Observed for pin ID: ' + );
            ////console.log(jq);
            //if (jq.data('SerratedJQBPntr') !== undefined) {
            //    console.log('Pntr Found');
            //    console.log('Ptr ' + jq.data('SerratedJQBPntr'));
            //}
            //InternalSerratedJQBox.UnpinEventListener(jq.data('SerratedJQBPntr') ?? 9999);
            //console.log(jq.is('span.myclass2'));
            //console.log(jq.find('span'));
        }
    });
}