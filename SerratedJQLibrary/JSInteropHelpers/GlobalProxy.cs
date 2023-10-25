using System.Runtime.InteropServices.JavaScript;
using System;

namespace SerratedSharp.JSInteropHelpers;


internal static partial class GlobalProxy
{

    private const string baseJSNamespace = "globalThis.Serrated.GlobalJS";

    internal static partial class Console
    {
        [JSImport("globalThis.console.log")]
        public static partial void
            Log([JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);


        // TODO: Open .NET runtime issue about lack of `params` support in JSImport. Results in error CS0758
        //[JSImport("globalThis.console.log")]
        //public static partial void
        //    Log([JSMarshalAs<JSType.Array<JSType.Any>>] params object[] parameters);

    }

    internal static partial class Helpers
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

