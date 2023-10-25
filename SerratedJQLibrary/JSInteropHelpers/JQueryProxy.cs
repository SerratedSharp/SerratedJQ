﻿using System.Runtime.InteropServices.JavaScript;
using System;

namespace SerratedSharp.JSInteropHelpers;

// TODO: Change to private
// TODO: Make generic instead of being JQuery specific, move any Jquery specific proxies back into SerratedJQ, splitting appropriate JS file
// Proxy for javascript declaration in JQueryProxy.js
public static partial class JQueryProxy //: IJSObject
{

    private const string baseJSNamespace = "globalThis.Serrated.JQueryProxy";

    #region Static/Factory Methods

    [JSImport(baseJSNamespace + ".Select")]
    public static partial JSObject Select(string selector);

    [JSImport(baseJSNamespace + ".ParseHtml")]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject ParseHtml(string html, bool keepScript);

    #endregion
}


public static partial class JSInstanceProxy
{
    private const string baseJSNamespace = "globalThis.Serrated.JQueryProxy";

    #region Instance Proxies

    [JSImport(baseJSNamespace + ".PropertyByNameToObject")]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object PropertyByNameToObject(JSObject jqObject, string propertyName);


    // Proxy for any instance methods taking any number of parameters and returning any type
    [JSImport(baseJSNamespace + ".FuncByNameToObject")]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    // The difficulty with using this proxy is a new array must be created for each call,
    // for example to cast a object[] to string[] requires iterating the array
    [JSImport(baseJSNamespace + ".FuncByNameToObject")]
    [return: JSMarshalAs<JSType.Array<JSType.Any>>]
    public static partial
        object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject")]    
    public static partial
        string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject")]
    public static partial
        double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);


    #endregion

    #region Listeners

    [JSImport(baseJSNamespace + ".BindListener")]       
    public static partial JSObject BindListener(JSObject jqObject, string events, bool shouldConvertHtmlElement,
        [JSMarshalAs<JSType.Function<JSType.String, JSType.String, JSType.Object>>] Action<string, string, JSObject> handler);

    [JSImport(baseJSNamespace + ".UnbindListener")]
    public static partial void UnbindListener(JSObject jqObject, string events, JSObject handler);

    #endregion
    
}

