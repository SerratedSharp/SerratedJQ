using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.JSInteropHelpers.Internal;

namespace SerratedSharp.JSInteropHelpers;

public static class JSObjectExtensions
{

    public static J GetPropertyOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _ = default(Breaker), [CallerMemberName] string propertyName = null)
        => JSImportInstanceHelpers.GetPropertyOfSameName<J>(wrapper.JSObject, _, propertyName);

    // extension method on JSObject Wrappers, that returns wrapped instance same as wrapper 
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, new object[0], _, funcName);

    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
    {
        //Console.WriteLine("CallJSOfSameNameAsWrapped: " + funcName);
        return JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, ToParams(param1), _, funcName);
    }

    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, object param2, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
    {
        //Console.WriteLine($"CallJSOfSameNameAsWrapped 2: {funcName}, {param1}, {param2}, {funcName}"  );


        return JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, ToParams(param1, param2), _, funcName);
    }

    // CONSIDER: Extension taking one Wrapper but returning different wrapper.  Such as JQuery method returning HtmlElement
    // <W,R>

    // extension method on JSObject, that returns wrapped instance same as wrapper 
    public static W CallJSOfSameNameAsWrapped<W>(this JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(jsObject, parameters, _, funcName);

    public static W CallJSOfSameNameAsWrapped<W>(this JSObject jsObject, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(jsObject, new object[0], _, funcName);
    
    public static W CallJSOfSameNameAsWrapped<W>(this JSObject jsObject, object param1, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(jsObject, ToParams(param1), _, funcName);
    
    public static W CallJSOfSameNameAsWrapped<W>(this JSObject jsObject, object param1, object param2, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(jsObject, ToParams(param1, param2), _, funcName);


    // extension method on Wrapper, that returns J, a primitive JS type or a JSObject. Use <object> for calling methods that return void
    // appropriate for instance methods that return a different type than the Wrapper's
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(wrapper.JSObject, parameters, _, funcName);

    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(wrapper.JSObject, new object[0], _, funcName);
    
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object param1, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(wrapper.JSObject, ToParams(param1), _, funcName);
    
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object param1, object param2, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(wrapper.JSObject, ToParams(param1, param2), _, funcName);


    // extension method on JSObject, that returns J, a primitive JS type or a JSObject.  Use <object> for calling methods that return void      
    public static J CallJSOfSameName<J>(this JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(jsObject, parameters, _, funcName);

    public static J CallJSOfSameName<J>(this JSObject jsObject, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(jsObject, new object[0], _, funcName);
            
    public static J CallJSOfSameName<J>(this JSObject jsObject, object param1, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(jsObject, ToParams(param1), _, funcName);
    
    public static J CallJSOfSameName<J>(this JSObject jsObject, object param1, object param2, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(jsObject, ToParams(param1, param2), _, funcName);


    public static T[] ToParams<T>(params T[] args)
    {
        return args;
    }
}


//public static class IJSObjectWrapperExtensions
//{

//    public 

//}

