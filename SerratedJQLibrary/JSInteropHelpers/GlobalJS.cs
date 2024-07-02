using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.JSInteropHelpers
{
    // CONSIDER: Implementing a selector builder fluent API for https://api.jquery.com/category/selectors/

    public static class GlobalJS
    {
        public static class Console
        {
            static Lazy<JSObject> _console = new(() => JSHost.GlobalThis.GetPropertyAsJSObject("console"));

            /// <summary>
            /// console.log, but note all params gets logged as a single array
            /// </summary>
            /// <param name="parameters">JSObjects or strings to log.</param>
            public static void Log(params object[] parameters)
            {   
                _console.Value.CallJSOfSameName<object>(parameters);
            }
        }
    }



}

