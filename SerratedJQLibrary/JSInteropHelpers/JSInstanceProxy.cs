using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.JSInteropHelpers;

public static class JSInstanceProxy
{

    public static object PropertyByNameToObject(JSObject jqObject, string propertyName)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JSInstanceProxyForUno.PropertyByNameToObject(jqObject, propertyName);
        else
            return JSInstanceProxyForDotNet.PropertyByNameToObject(jqObject, propertyName);
    }


    public static object FuncByNameAsObject(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JSInstanceProxyForUno.FuncByNameAsObject(jqObject, funcName, parameters);
        else
            return JSInstanceProxyForDotNet.FuncByNameAsObject(jqObject, funcName, parameters);
    }

    // The difficulty with using this proxy is a new array must be created for each call,
    // for example to cast a object[] to string[] requires iterating the array
    public static object[] FuncByNameAsArray(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JSInstanceProxyForUno.FuncByNameAsArray(jqObject, funcName, parameters);
        else
            return JSInstanceProxyForDotNet.FuncByNameAsArray(jqObject, funcName, parameters);
    }

    public static string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JSInstanceProxyForUno.FuncByNameAsStringArray(jqObject, funcName, parameters);
        else
            return JSInstanceProxyForDotNet.FuncByNameAsStringArray(jqObject, funcName, parameters);
    }

    public static double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJS.IsUnoWasmBootstrapLoaded)
            return JSInstanceProxyForUno.FuncByNameAsDoubleArray(jqObject, funcName, parameters);
        else
            return JSInstanceProxyForDotNet.FuncByNameAsDoubleArray(jqObject, funcName, parameters);
    }
        
}


public static partial class JSInstanceProxyForDotNet
{
    private const string baseJSNamespace = "SerratedInteropHelpers.HelpersProxy";
    private const string moduleName = "SerratedInteropHelpers";

    #region Instance Proxies

    [JSImport(baseJSNamespace + ".PropertyByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object PropertyByNameToObject(JSObject jqObject, string propertyName);


    // Proxy for any instance methods taking any number of parameters and returning any type
    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    // The difficulty with using this proxy is a new array must be created for each call,
    // for example to cast a object[] to string[] requires iterating the array
    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Array<JSType.Any>>]
    public static partial
        object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);


    #endregion

}



public static partial class JSInstanceProxyForUno
{
    private const string baseJSNamespace = "globalThis.SerratedInteropHelpers.HelpersProxy";
    private const string moduleName = "";

    #region Instance Proxies

    [JSImport(baseJSNamespace + ".PropertyByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object PropertyByNameToObject(JSObject jqObject, string propertyName);


    // Proxy for any instance methods taking any number of parameters and returning any type
    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    // The difficulty with using this proxy is a new array must be created for each call,
    // for example to cast a object[] to string[] requires iterating the array
    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Array<JSType.Any>>]
    public static partial
        object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);


    #endregion

}
