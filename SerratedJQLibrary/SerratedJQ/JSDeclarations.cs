//using Uno.Extensions;
using Uno.Foundation;
//using Uno.Foundation.Interop;
//using Uno.Foundation.Interop;

namespace SerratedSharp.SerratedJQ
{
    public static class JSDeclarations
    {
        public static void LoadScripts()
        {
            WebAssemblyRuntime.InvokeJS(SerratedJQ.EmbeddedFiles.JQueryProxy);
        }
    }



}

