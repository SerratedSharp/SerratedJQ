using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.JSInteropHelpers;


internal static partial class GlobalProxy
{

    // TODO: Why did I put this inside GlobalProxy?
    internal static partial class HelpersProxy
    {

        private const string baseJSNamespace = "globalThis.Serrated.HelpersProxy";

        [JSImport(baseJSNamespace + ".LoadjQuery")]
        public static partial Task
            LoadJQuery(string relativeUrl);

        [JSImport(baseJSNamespace + ".LoadScript")]
        public static partial Task
            LoadScript(string relativeUrl);

        // Used for unpacking an ArrayObject into a JSObject[] array
        [JSImport(baseJSNamespace + ".GetArrayObjectItems")]
        [return: JSMarshalAs<JSType.Array<JSType.Object>>]
        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);
    }




}

