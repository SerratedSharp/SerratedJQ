//using Uno.Extensions;
using Uno.Foundation;
//using Uno.Foundation.Interop;
using System.Runtime.InteropServices.JavaScript;
using System;

namespace SerratedSharp.SerratedJQ
{
    // TODO: Change to private
    // Proxy for javascript declaration in JQueryProxy.js
    public static partial class JQueryProxy //: IJSObject
    {

        static JQueryProxy() {            

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

        // Proxy for any instance methods taking any number of parameters and returning any type
        [JSImport(baseJSNamespace + ".FuncByNameToObject")]
        [return: JSMarshalAs<JSType.Any>]
        public static partial
            object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        #endregion

        #region Listeners

        [JSImport(baseJSNamespace + ".BindListener")]       
        public static partial JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
            [JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler);

        [JSImport(baseJSNamespace + ".UnbindListener")]
        public static partial void UnbindListener(JSObject jqObject, string events, JSObject handler);

        #endregion
        
    }

    public partial class HelpersProxy
    {
        private const string baseJSNamespace = "globalThis.Serrated.HelpersProxy";

        // Used for unpacking an ArrayObject into a JSObject[] array
        [JSImport(baseJSNamespace + ".GetArrayObjectItems")]
        [return: JSMarshalAs<JSType.Array<JSType.Object>>]
        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);
    }

}

