//using Uno.Extensions;
//using Uno.Foundation.Interop;
//using Uno.Foundation.Interop;

using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.JSInteropHelpers
{
    // CONSIDER: Implementing a selector builder fluent API for https://api.jquery.com/category/selectors/

    public static class HelpersJS
    {

        public static async Task LoadScript(string relativeUrl)
        {
            await GlobalProxy.Helpers.LoadScript( relativeUrl );
        }

        public static async Task LoadJQuery(string relativeUrl)
        {
            await GlobalProxy.Helpers.LoadJQuery(relativeUrl);
        }

        public static JSObject[] GetArrayObjectItems(JSObject jqObject)
        {
            return GlobalProxy.Helpers.GetArrayObjectItems(jqObject);
        }

    }



}

