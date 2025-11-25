using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.JSInteropHelpers.Internal;

namespace SerratedSharp.JSInteropHelpers;

public static class JSObjectExtensions
{

    public static J GetPropertyOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string propertyName = null)
        => JSImportInstanceHelpers.GetPropertyOfSameName<J>(wrapper.JSObject, _, propertyName);

    public static void SetPropertyOfSameName(this IJSObjectWrapper wrapper, object value, Breaker _ = default, [CallerMemberName] string propertyName = null)
        => JSImportInstanceHelpers.SetPropertyOfSameName(wrapper.JSObject, value, _, propertyName);

    // From old to fix array issue 
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
    where W : IJSObjectWrapper<W>
    => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, Breaker _ = default, [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal(wrapper, funcName, _);

    // this one getting called with single array object with a nested string array class names
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, Breaker _ = default, [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal(wrapper, funcName, _, param1);

    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal(wrapper, funcName, _, param1, param2);

    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, Breaker _, string funcName, params object[] parameters)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal(wrapper, funcName, _, parameters);

    private static W CallJSOfSameNameAsWrappedInternal<W>(W wrapper, string funcName, Breaker _, params object[] parameters)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string funcName = null)
        => CallJSOfSameNameInternal<J>(wrapper, funcName, _);

    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object param1, Breaker _ = default, [CallerMemberName] string funcName = null)
        => CallJSOfSameNameInternal<J>(wrapper, funcName, _, param1);

    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string funcName = null)
        => CallJSOfSameNameInternal<J>(wrapper, funcName, _, param1, param2);

    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _, string funcName, params object[] parameters)
        => CallJSOfSameNameInternal<J>(wrapper, funcName, _, parameters);

    private static J CallJSOfSameNameInternal<J>(IJSObjectWrapper wrapper, string funcName, Breaker _, params object[] parameters)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(wrapper.JSObject, parameters, _, funcName);
}


