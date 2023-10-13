//using System.Runtime.InteropServices.JavaScript;
//using System;

//namespace SerratedSharp.SerratedJQ
//{
//    // TODO: Change to private
//    // Proxy for javascript declaration in JQueryProxy.js
//    public static partial class JQueryProxy //: IJSObject
//    {
//        #region Static/Factory Methods
//        // TODO: Move these static JQuery methods and their corersponding javascript declarations to seperate files, and move into SerratedJQ project (also adjusting JS namespace)
//        private const string baseJSNamespace = "globalThis.Serrated.JQueryProxy";

//        [JSImport(baseJSNamespace + ".Select")]
//        public static partial JSObject Select(string selector);

//        [JSImport(baseJSNamespace + ".ParseHtml")]
//        [return: JSMarshalAs<JSType.Object>]
//        public static partial JSObject ParseHtml(string html, bool keepScript);

//        #endregion

//        #region Instance Proxies
//        #endregion
//    }


//    public static partial class JSInstanceProxy
//    {
//        private const string baseJSNamespace = "globalThis.Serrated.JQueryProxy";

//        #region Instance Method Proxies, takes a JSObject instance reference and access/calls the property or method on that object.

//        [JSImport(baseJSNamespace + ".PropertyByNameToObject")]
//        [return: JSMarshalAs<JSType.Any>]
//        public static partial
//            object PropertyByNameToObject(JSObject jqObject, string propertyName);

//        // Proxy for any instance methods taking any number of parameters and returning any type
//        [JSImport(baseJSNamespace + ".FuncByNameToObject")]
//        [return: JSMarshalAs<JSType.Any>]
//        public static partial
//            object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

//        #endregion
//        #region Listeners

//        [JSImport(baseJSNamespace + ".BindListener")]       
//        public static partial JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
//            [JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler);

//        [JSImport(baseJSNamespace + ".UnbindListener")]
//        public static partial void UnbindListener(JSObject jqObject, string events, JSObject handler);

//        #endregion
        
//    }

//    public partial class HelpersProxy
//    {
//        private const string baseJSNamespace = "globalThis.Serrated.HelpersProxy";

//        // Used for unpacking an ArrayObject into a JSObject[] array
//        [JSImport(baseJSNamespace + ".GetArrayObjectItems")]
//        [return: JSMarshalAs<JSType.Array<JSType.Object>>]
//        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);
//    }

//}

