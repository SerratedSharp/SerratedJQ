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




}

