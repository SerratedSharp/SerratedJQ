using System.Runtime.InteropServices.JavaScript;
using System;
using System.Threading.Tasks;

namespace SerratedSharp.JSInteropHelpers;

// Proxy for javascript declaration in JQueryProxy.js
//internal static partial class JQueryProxy //: IJSObject
//{

//    private const string baseJSNamespace = "SerratedJQ.JQueryProxy";
//    private const string moduleName = "SerratedJQ";

//    #region Static/Factory Methods

//    [JSImport(baseJSNamespace + ".Ready", moduleName)]
//    public static partial Task Ready();

//    [JSImport(baseJSNamespace + ".Select", moduleName)]
//    public static partial JSObject Select(string selector);

//    [JSImport(baseJSNamespace + ".ParseHtml", moduleName)]
//    [return: JSMarshalAs<JSType.Object>]
//    public static partial JSObject ParseHtml(string html, bool keepScript);

//    #endregion

//    // TODO: Move to SerratedJQ, depends on JQuery .on/.off
//    #region Listeners

//[JSImport(baseJSNamespace + ".BindListener", moduleName)]
//public static partial JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
//[JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler, string selector);//, object data);


//[JSImport(baseJSNamespace + ".BindDelegatedListener", "Serrated")]
//public static partial JSObject BindDelegatedListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
//[JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler, string selector);//, object data);

//[JSImport(baseJSNamespace + ".UnbindListener", moduleName)]
//public static partial void UnbindListener(JSObject jqObject, string events, JSObject handler, string selector);

//    #endregion
//}

internal static class JQueryProxy //: IJSObject
{

    public static Task Ready()
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JQueryProxyForUno.Ready();
        else
            return JQueryProxyForDotNet.Ready();
    }

    public static  JSObject Select(string selector)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JQueryProxyForUno.Select(selector);
        else
            return JQueryProxyForDotNet.Select(selector);
    }

    public static  JSObject ParseHtml(string html, bool keepScript)
        {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JQueryProxyForUno.ParseHtml(html, keepScript);
        else
            return JQueryProxyForDotNet.ParseHtml(html, keepScript);
    }

    public static JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
     Action<string, string, JSObject> handler, string selector)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JQueryProxyForUno.BindListener(jqObject, events, shouldConvertHtmlElement, handler, selector);
        else
            return JQueryProxyForDotNet.BindListener(jqObject, events, shouldConvertHtmlElement, handler, selector);
    }

    public static void UnbindListener(JSObject jqObject, string events, JSObject handler, string selector)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            JQueryProxyForUno.UnbindListener(jqObject, events, handler, selector);
        else
            JQueryProxyForDotNet.UnbindListener(jqObject, events, handler, selector);
    }

}


// JQueryProxyForDotNet
internal static partial class JQueryProxyForDotNet //: IJSObject
{

    private const string baseJSNamespace = "SerratedJQ.JQueryProxy";
    private const string moduleName = "SerratedJQ";

    [JSImport(baseJSNamespace + ".Ready", moduleName)]
    public static partial Task Ready();

    [JSImport(baseJSNamespace + ".Select", moduleName)]
    public static partial JSObject Select(string selector);

    [JSImport(baseJSNamespace + ".ParseHtml", moduleName)]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject ParseHtml(string html, bool keepScript);

    [JSImport(baseJSNamespace + ".BindListener", moduleName)]
    public static partial JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
        [JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler, string selector);//, object data);

    [JSImport(baseJSNamespace + ".UnbindListener", moduleName)]
    public static partial void UnbindListener(JSObject jqObject, string events, JSObject handler, string selector);


}

// JQueryProxyForUno
internal static partial class JQueryProxyForUno //: IJSObject
{
    private const string baseJSNamespace = "globalThis." + "SerratedJQ.JQueryProxy";

    [JSImport(baseJSNamespace + ".Ready")]
    public static partial Task Ready();

    [JSImport(baseJSNamespace + ".Select")]
    public static partial JSObject Select(string selector);

    [JSImport(baseJSNamespace + ".ParseHtml")]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject ParseHtml(string html, bool keepScript);

    [JSImport(baseJSNamespace + ".BindListener")]
    public static partial JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
        [JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler, string selector);//, object data);

    [JSImport(baseJSNamespace + ".UnbindListener")]
    public static partial void UnbindListener(JSObject jqObject, string events, JSObject handler, string selector);

}
