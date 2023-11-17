using Uno.Foundation;

namespace SerratedSharp.SerratedJQ
{
    public static class JSDeclarations
    {
        public static void LoadScripts()
        {
            WebAssemblyRuntime.InvokeJS(SerratedSharp.JSInteropHelpers.EmbeddedFiles.JSInteropProxies);

            WebAssemblyRuntime.InvokeJS(SerratedSharp.JSInteropHelpers.EmbeddedFiles.JQueryProxy);
        }
    }



}

