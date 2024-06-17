using System.Buffers.Text;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;


namespace SerratedSharp.JSInteropHelpers
{
    [Obsolete("This class is obsolete. Use JSInteropHelpersModule.ImportAsync instead.")]
    public static class JSDeclarationsForWasmBrowser
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// </summary>
        /// <param name="basePath">Base path if site is not rooted at domain.</param>
        public static async Task LoadScripts(string basePath = "")
        {
            await JSInteropHelpersModule.ImportAsync(basePath);
        }

    }



    public static class JSInteropHelpersModule
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// Leverages embedded RCL Static Web Assets embedded in this library, 
        /// which are emitted into the consuming application during publish.
        /// Loaded with JSHost.ImportAsync()
        /// This is typically needed for a WASM Browser project, but is not needed for a Uno.Wasm.Bootstrap project.
        /// </summary>
        /// <param name="basePath">Base path, if site is not rooted at domain.</param>
        /// <param name="subPath">Sub path.  Default is "/_content/SerratedSharp.JSInteropHelpers/SerratedInteropHelpers.js" for loading Static Web Asset from Razor Class Library.  Provide alternate subpath to concatenate with basePath if loading from custom location.</param>
        public static async Task ImportAsync(string basePath = "", string subPath = null)
        {
            string path = basePath.TrimEnd('/') + 
                (subPath ?? "/_content/SerratedSharp.JSInteropHelpers/SerratedInteropHelpers.js");
            await JSHost.ImportAsync("SerratedInteropHelpers", path);

        }

    }



}

