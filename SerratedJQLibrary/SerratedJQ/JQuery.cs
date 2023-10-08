//using Uno.Extensions;
//using Uno.Foundation.Interop;
//using Uno.Foundation.Interop;

namespace SerratedSharp.SerratedJQ
{
    // MEthods exposed by global jQuery object, typically methods that generate JQuery object isntances
    public static class JQuery
    {
        /// <summary>
        /// Calls $(document).find('selector'), equivilant of $('selector') but in a way that eliminates accidental passing of HTML for more secure usage.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns>A JQueryBox wrapping the JQuery collection returned by .find()</returns>
        public static JQueryObject Select(string selector)
        {
            var managedObj = new JQueryObject();
            managedObj.jsObject = JQueryProxy.Select(selector);
            return managedObj;
        }

        // TODO: implement overload or optional params for "JQ context": jQuery.parseHTML( data [, JQ context ] [, bool keepScripts ] )
        /// <summary>
        /// Generates detached JQuery object from HTML fragment. Exposes option to keep script tags(defaults to false).  This doesn't cover all XSS scenarios such as sanatizing attribute embedded scripts.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keepScripts"></param>
        /// <returns></returns>     
        public static JQueryObject ParseHTML(string html, bool keepScript = false)
        {
            var managedObj = new JQueryObject();
            managedObj.jsObject = JQueryProxy.ParseHTML(html, keepScript);
            return managedObj;
        }

        #region Static Properties - https://api.jquery.com/category/properties/global-jquery-object-properties/
        // TODO: Global JQUery object properties as static properties
        #endregion

    }

    // CONSIDER: Implementing a selector builder fluent API for https://api.jquery.com/category/selectors/





}

