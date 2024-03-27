
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;


namespace SerratedSharp.SerratedJQ
{
    public static class JSDeclarations
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on. 
        /// Leverages embedded RCL Static Web Assets embedded in this library, 
        /// which are emitted into the consuming application during publish.
        /// Loaded with JSHost.ImportAsync()
        /// </summary>
        /// <param name="basePath">Base path if site, if not rooted at domain.</param>
        /// <returns></returns>
        public static async Task LoadScriptsForWasmBrowser(string basePath = "")
        {
            await SerratedSharp.JSInteropHelpers.JSDeclarationsForWasmBrowser.LoadScripts(basePath);
            await JSHost.ImportAsync("SerratedJQ", basePath.TrimEnd('/') + "/_content/SerratedSharp.SerratedJQ/SerratedJQ.js");
        }

        /// <summary>
        /// Loads jQuery from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag, or resolves immediately if `window.jQuery` is already valid.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadJQuery(string url)
        {
            await SerratedSharp.JSInteropHelpers.HelpersJS.LoadJQuery(url);
        }
    }



}

