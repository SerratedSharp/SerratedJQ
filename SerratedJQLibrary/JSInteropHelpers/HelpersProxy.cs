using System.Runtime.InteropServices.JavaScript;
using static SerratedSharp.JSInteropHelpers.GlobalProxy;

namespace SerratedSharp.JSInteropHelpers;

//public interface IHelpersProxy
//{
//    static abstract Task LoadJQuery(string relativeUrl);
//    static abstract Task LoadScript(string relativeUrl);
//    static abstract Task LoadScriptWithContent(string scriptContent);
//    static abstract JSObject[] GetArrayObjectItems(JSObject jqObject);
//}


internal static partial class GlobalProxy
{


    internal partial class HelpersProxy //: IHelpersProxy
    {
        private const string baseJSNamespace = "SerratedInteropHelpers.HelpersProxy";
        private const string moduleName = "SerratedInteropHelpers";


        [JSImport(baseJSNamespace + ".LoadjQuery", moduleName)]
        public static partial Task
            LoadJQuery(string relativeUrl);

        [JSImport(baseJSNamespace + ".LoadScript", moduleName)]
        public static partial Task
            LoadScript(string relativeUrl);

        //[JSImport(baseJSNamespace + ".LoadScriptWithContent", moduleName)]
        //public static partial Task
        //    LoadScriptWithContent(string scriptContent);


        // Used for unpacking an ArrayObject into a JSObject[] array
        [JSImport(baseJSNamespace + ".GetArrayObjectItems", moduleName)]
        [return: JSMarshalAs<JSType.Array<JSType.Object>>]
        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);
    }

    internal partial class HelpersProxyForUno //: IHelpersProxy
    {
        private const string baseJSNamespace = "globalThis." + "SerratedInteropHelpers.HelpersProxy";
        
        [JSImport(baseJSNamespace + ".LoadjQuery")]
        public static partial Task
            LoadJQuery(string relativeUrl);

        [JSImport(baseJSNamespace + ".LoadScript")]
        public static partial Task
            LoadScript(string relativeUrl);

        //[JSImport(baseJSNamespace + ".LoadScriptWithContent")]
        //public static partial Task
        //    LoadScriptWithContent(string scriptContent);

        // Used for unpacking an ArrayObject into a JSObject[] array
        [JSImport(baseJSNamespace + ".GetArrayObjectItems")]
        [return: JSMarshalAs<JSType.Array<JSType.Object>>]
        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);
    }
}

