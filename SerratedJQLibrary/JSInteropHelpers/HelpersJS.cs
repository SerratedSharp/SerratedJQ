//using Uno.Extensions;
//using Uno.Foundation.Interop;
//using Uno.Foundation.Interop;

using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.JSInteropHelpers
{
    
    public static class HelpersJS
    {

        /// <summary>
        /// Loads script or library from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadScript(string relativeUrl)
        {
            await GlobalProxy.HelpersProxy.LoadScript( relativeUrl );
        }

        /// <summary>
        /// Loads jQuery from a relative or absolute URL by adding script tag with src="{url}" (caller must ensure file is available at given URL)
        /// Returns awaitable task for JS promise of the onload event of the script tag, or resolves immediately if `window.jQuery` is already valid.
        /// </summary>
        /// <param name="url">Relative or absolute URL of jQuery library, such as "jquery-3.7.1.js" if it was in the root of the application.</param>
        public static async Task LoadJQuery(string url)
        {
            await GlobalProxy.HelpersProxy.LoadJQuery(url);
        }

        /// <summary>
        /// Takes a reference to a JSObject with a `.items` property of type array, and returns an array of the .items.
        /// Used as a workaround where some interop mappings cannot return arrays.
        /// </summary>
        /// <param name="jqObject">A reference to a JS type with a `.items` of type array.</param>
        /// <returns>An array of JSObject references contained in the `.items` property.</returns>
        public static JSObject[] GetArrayObjectItems(JSObject jqObject)
        {
            return GlobalProxy.HelpersProxy.GetArrayObjectItems(jqObject);
        }

    }



}

