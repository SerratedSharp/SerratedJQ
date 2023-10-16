//using Uno.Extensions;
//using Uno.Foundation.Interop;
//using Uno.Foundation.Interop;

namespace SerratedSharp.JSInteropHelpers
{
    // CONSIDER: Implementing a selector builder fluent API for https://api.jquery.com/category/selectors/

    public static class GlobalJS
    {
        public static class Console
        {

            /// <summary>
            /// console.log, but note all params gets logged as a single array
            /// </summary>
            /// <param name="parameters">JSObjects or strings to log.</param>
            public static void Log(params object[] parameters)
            {
                GlobalProxy.Console.Log( JSImportInstanceHelpers.UnwrapJSObjectParams(parameters) );
            }
        }
    }



}

