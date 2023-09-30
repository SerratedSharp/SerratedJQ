using System;
using System.Runtime.CompilerServices;
//using Uno.Extensions;
//using Uno.Foundation.Interop;
using System.Runtime.InteropServices.JavaScript;
//using Uno.Foundation.Interop;


namespace SerratedSharp.SerratedJQ
{
    public static class JSImportInstanceHelpers
    {

        // TODO: Move static helpers to separete library

        // This call automatically wraps a JSObject using type W's WrapInstance interface
        public static W CallJSOfSameNameAsWrapped<W>(JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
            where W : IJSObjectWrapper<W>
        {
            JSObject jSObject = CallJSOfSameName<JSObject>(jsObject, parameters, _, funcName);
            return W.WrapInstance(jsObject); // wrap JSObject with W's factory create method
        }

        // J should be a JSObject or other prmitiive JS type
        public static J CallJSOfSameName<J>(JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        {
            // convert C# PascalCase func name to JS lowerCamelCase
            return CallJSFunc<J>(jsObject, funcName, parameters); //CallJSFuncAsJSObject(jsObject, funcName, parameters);
        }

        // J should be a JSObject or other prmitiive JS type
        public static J CallJSFunc<J>(JSObject jsObject, string funcName, params object[] parameters)
        {
            object genericObject = JQueryProxy.FuncByNameAsObject(jsObject, ToJSCasing(funcName), parameters);
            return (J)genericObject;
        }

        // lower cases first character
        public static string ToJSCasing(string identifier)
            => Char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);


        //public JQueryObject CallJSOfSameNameAsJQueryObject(object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        //{
        //    return CallJSOfSameNameAsWrapped<JQueryObject>(this.JSObject, parameters, _, funcName);
        //}

        //public string CallJSOfSameNameAsString(object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        //{
        //    return CallJSOfSameName<string>(this.JSObject, parameters, _, funcName);
        //}

        //public R CallJSOfSameName<R>(object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        //{
        //    return CallJSOfSameName<R>(this.JSObject, parameters, _, funcName);
        //}


    }

    public static class JSObjectExtensions
    {
        // extension method on JSObject Wrappers, that returns wrapped instance same as wrapper 
        public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
            where W : IJSObjectWrapper<W>
            => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

        public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
            where W : IJSObjectWrapper<W>
            => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, new object[0], _, funcName);

        public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
            where W : IJSObjectWrapper<W>
            => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, ToParams(param1), _, funcName);

        public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, object param2, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
            where W : IJSObjectWrapper<W>
            => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, ToParams(param1, param2), _, funcName);

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

    // TODO: Move inide another class so not polluting intellisense?
    public struct Breaker { } // Prevents incorrect overload being used. See https://stackoverflow.com/a/26784846/84206


    //public static class IJSObjectWrapperExtensions
    //{

    //    public 

    //}



}

