using System.Runtime.InteropServices.JavaScript;
using System;
using System.Threading.Tasks;

namespace SerratedSharp.JSInteropHelpers;

// TODO: Change to private
// TODO: Make generic instead of being JQuery specific, move any Jquery specific proxies back into SerratedJQ, splitting appropriate JS file
// TODO: should be in SerratedJQ instead of JSInteropHelpers
// Proxy for javascript declaration in JQueryProxy.js
internal static partial class JQueryProxy //: IJSObject
{

    private const string baseJSNamespace = "globalThis.Serrated.JQueryProxy";

    #region Static/Factory Methods

    [JSImport(baseJSNamespace + ".Ready")]
    public static partial Task Ready();

    [JSImport(baseJSNamespace + ".Select")]
    public static partial JSObject Select(string selector);

    [JSImport(baseJSNamespace + ".ParseHtml")]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject ParseHtml(string html, bool keepScript);

    #endregion
}

