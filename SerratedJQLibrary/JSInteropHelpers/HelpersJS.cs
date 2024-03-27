
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.JSInteropHelpers
{

    public static class HelpersJS
    {

        /// <summary>
        /// Gets a value indicating whether being used from Uno.Wasm.Bootstrap rather than .NET8 wasmbrowser.
        /// </summary>
        /// <remarks>
        /// This property uses lazy evaluation to check whether the assembly is loaded.
        /// </remarks>
        internal static bool IsUnoWasmBootstrapLoaded => isUnoWasmBootstrapLoaded.Value;
        private static Lazy<bool> isUnoWasmBootstrapLoaded = new Lazy<bool>(() =>
        {
            bool isUnoPresent = false;
            if (JSHost.GlobalThis.HasProperty("IsFromUno"))
            {
                if (JSHost.GlobalThis.GetPropertyAsBoolean("IsFromUno"))
                    isUnoPresent = true;
            }

            return isUnoPresent;
        });

        /// <summary>
        /// Loads script or library from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadScript(string relativeUrl)
        {
            if (IsUnoWasmBootstrapLoaded)
                await GlobalProxy.HelpersProxyForUno.LoadScript(relativeUrl);
            else
                await GlobalProxy.HelpersProxy.LoadScript(relativeUrl);
        }

        //public static async Task LoadScriptWithContent(string scriptContent)
        //{
        //    if (IsUnoWasmBootstrapLoaded)
        //        await GlobalProxy.HelpersProxyForUno.LoadScriptWithContent(scriptContent);
        //    else
        //        await GlobalProxy.HelpersProxy.LoadScriptWithContent(scriptContent);
        //}

        /// <summary>
        /// Loads jQuery from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag, or resolves immediately if `window.jQuery` is already valid.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadJQuery(string url)
        {
            LogUnoCheck();
            if (IsUnoWasmBootstrapLoaded)
                await GlobalProxy.HelpersProxyForUno.LoadJQuery(url);
            else
                await GlobalProxy.HelpersProxy.LoadJQuery(url);
        }

        [Conditional("DEBUG")]
        public static void LogUnoCheck()
        {
            if (IsUnoWasmBootstrapLoaded)
                Console.WriteLine("Uno Detected based on presence of Uno.Wasm.MetadataUpdater assembly.");
            else
                Console.WriteLine("Uno not detected, based on lack of Uno.Wasm.MetadataUpdater assembly.");
        }

        /// <summary>
        /// Takes a reference to a JSObject with a `.items` property of type array, and returns an array of the .items.
        /// Used as a workaround where some interop mappings cannot return arrays.
        /// </summary>
        /// <param name="jqObject">A reference to a JS type with a `.items` of type array.</param>
        /// <returns>An array of JSObject references contained in the `.items` property.</returns>
        public static JSObject[] GetArrayObjectItems(JSObject jqObject)
        {
            if (IsUnoWasmBootstrapLoaded)
                return GlobalProxy.HelpersProxyForUno.GetArrayObjectItems(jqObject);
            else
                return GlobalProxy.HelpersProxy.GetArrayObjectItems(jqObject);
            
        }

    }



}

