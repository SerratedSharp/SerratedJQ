//using Uno.Extensions;
using Uno.Foundation;
//using Uno.Foundation.Interop;
using System.Runtime.InteropServices.JavaScript;
//using Uno.Foundation.Interop;

namespace SerratedSharp.SerratedJQ
{
    // TODO: Change to private
    // Proxy for javascript declaration in JQueryProxy.js
    public static partial class JQueryProxy //: IJSObject
    {
        private const string JSClassName = "InternalSerratedJQBox";
        private static int keep = 2;
        static JQueryProxy()
        {

            // Add javascript declaration that is used by WebAssembly but was declared in incorrect Uno Platform project.
            WebAssemblyRuntime.InvokeJS(_1.ManagedObjectJavascriptDispatcherDeclaration);

            WebAssemblyRuntime.InvokeJS(@$"     
                    window.{JSClassName} = window.{JSClassName} || {{}};
                    window.{JSClassName}.UnpinEventListener = Module.mono_bind_static_method('[SerratedSharp.SerratedJQ] SerratedSharp.SerratedJQ.JQueryBox:UnpinEventListener');                         
                ");

            WebAssemblyRuntime.InvokeJS(SerratedJQ.EmbeddedFiles.ObserveRemovedJs);
           // WebAssemblyRuntime.InvokeJS(SerratedJQ.EmbeddedFiles.JQueryProxy);

            if (keep == 1)// Prevent these methods from being removed by ILLinker at compile time. The condition being false prevents these from actually being called at runtime, but indeterministic so that linker doesn't remove them. 
            {
                var keeper = new JQueryBox();
                keeper.InternalClickCallback("", "");
                keeper.InternalEventCallback("", "");
                keeper.InternalInputCallback("", "");
            }

        }


        #region Static/Factory Methods

        private const string baseJSNamespace = "globalThis.Serrated.JQueryProxy";

        [JSImport(baseJSNamespace + ".Select")]
        public static partial JSObject Select(string selector);

        [JSImport(baseJSNamespace + ".ParseHTML")]
        public static partial JSObject ParseHTML(string html, bool keepScript);

        #endregion

        #region Instance Proxies


        //[JSImport(baseJSNamespace + ".FuncByNameToObject")]
        //public static partial 
        //    JSObject FuncByNameAsJSObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        //[JSImport(baseJSNamespace + ".FuncByNameToObject")]
        //public static partial
        //    string FuncByNameAsString(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        // Proxy for any instance methods taking any number of parameters
        [JSImport(baseJSNamespace + ".FuncByNameToObject")]
        [return: JSMarshalAs<JSType.Any>]
        public static partial
             object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        #endregion

    }



}

