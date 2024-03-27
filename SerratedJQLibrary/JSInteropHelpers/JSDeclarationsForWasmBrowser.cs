using System.Buffers.Text;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;


namespace SerratedSharp.JSInteropHelpers
{
    internal static class JSDeclarationsForWasmBrowser
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// </summary>
        /// <param name="basePath">Base path if site is not rooted at domain.</param>
        public static async Task LoadScripts(string basePath = "")
        {
            await JSHost.ImportAsync("SerratedInteropHelpers", basePath.TrimEnd('/') + "/_content/SerratedSharp.JSInteropHelpers/SerratedInteropHelpers.js");

        }

    }



}

