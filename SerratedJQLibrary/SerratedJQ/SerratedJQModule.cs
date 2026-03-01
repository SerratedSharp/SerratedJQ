using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedJQ
{
    [Obsolete("This class is obsolete. Use SerratedJQModule.ImportAsync() instead.")]
    public static class JSDeclarations
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// Leverages embedded RCL Static Web Assets embedded in this library,
        /// which are emitted into the consuming application during publish.
        /// Loaded with JSHost.ImportAsync()
        /// </summary>
        /// <param name="basePath">Base path if site, if not rooted at domain.</param>
        public static async Task LoadScriptsForWasmBrowser(string basePath = "")
        {
            await SerratedJQModule.ImportAsync(basePath);
        }

        /// <summary>
        /// Loads jQuery from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag, or resolves immediately if `window.jQuery` is already valid.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadJQuery(string url)
        {
            await SerratedJQModule.LoadJQuery(url);
        }
    }

    public static class SerratedJQModule
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// Loads SerratedJSInterop (dependency) first, then SerratedJQ.js.
        /// Loaded with JSHost.ImportAsync()
        /// This is typically needed for a WASM Browser project, but is not needed for a Uno.Wasm.Bootstrap project.
        /// </summary>
        /// <param name="basePath">Base path if site, if not rooted at domain.</param>
        /// <param name="subPath">Sub path.  Default is "/_content/SerratedSharp.SerratedJQ/SerratedJQ.js" for loading Static Web Asset from Razor Class Library.  Provide alternate subpath to concatenate with basePath if loading from custom location.</param>
        public static async Task ImportAsync(string basePath = "", string subPath = null)
        {
            await SerratedSharp.SerratedJSInterop.SerratedJSInteropModule.ImportAsync(basePath);

            string path = basePath.TrimEnd('/') +
                (subPath ?? "/_content/SerratedSharp.SerratedJQ/SerratedJQ.js");
            await JSHost.ImportAsync("SerratedJQ", path);
        }

        /// <summary>
        /// Loads jQuery from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag, or resolves immediately if `window.jQuery` is already valid.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadJQuery(string url)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                await LoadJQueryForUno.LoadJQuery(url);
            else
                await LoadJQueryForDotNet.LoadJQuery(url);
        }
    }

    internal static partial class LoadJQueryForDotNet
    {
        private const string baseJSNamespace = "SerratedJQ.JQueryProxy";
        private const string moduleName = "SerratedJQ";

        [JSImport(baseJSNamespace + ".LoadJQuery", moduleName)] 
        public static partial Task LoadJQuery(string url);
    }

    internal static partial class LoadJQueryForUno
    {
        private const string baseJSNamespace = "globalThis." + "SerratedJQ.JQueryProxy";

        [JSImport(baseJSNamespace + ".LoadJQuery")]
        public static partial Task LoadJQuery(string url);
    }
}

