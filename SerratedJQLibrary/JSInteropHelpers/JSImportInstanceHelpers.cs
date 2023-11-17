namespace SerratedSharp.JSInteropHelpers;

using SerratedSharp.JSInteropHelpers;
using SerratedSharp.JSInteropHelpers.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

public static class JSImportInstanceHelpers
{


    // J should be a JSObject or other prmitiive JS type
    public static J GetPropertyOfSameName<J>(JSObject jsObject, Breaker _ = default(Breaker), [CallerMemberName] string propertyName = null)
    {
        // convert C# PascalCase func name to JS lowerCamelCase
        return GetProperty<J>(jsObject, propertyName); //CallJSFuncAsJSObject(jsObject, funcName, parameters);
    }

    // J should be a JSObject or other prmitiive JS type
    public static J GetProperty<J>(JSObject jsObject, string propertyName)
    {
        object genericObject = JSInstanceProxy.PropertyByNameToObject(jsObject, ToJSCasing(propertyName));
        // Console.WriteLine($"GetProperty: {propertyName}, {genericObject}, {genericObject?.GetType()?.Name}");
        return (J)genericObject;
    }





    // TODO: Move static helpers to separete library

    // This call automatically wraps a JSObject using type W's WrapInstance interface
    public static W CallJSOfSameNameAsWrapped<W>(JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string funcName = null)
        where W : IJSObjectWrapper<W>
    {        
        JSObject jsObjectRtn = CallJSOfSameName<JSObject>(jsObject, parameters, _, funcName);
        return W.WrapInstance(jsObjectRtn); // wrap JSObject with W's factory create method
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
        object[] objs = UnwrapJSObjectParams(parameters);
        object genericObject;
        Type type = typeof(J);
        if (type.IsArray)
        {
            //genericObject = JSInstanceProxy.FuncByNameAsArray(jsObject, ToJSCasing(funcName), objs);
            //object[] objectArray = (object[])genericObject;
            //object typedArray = objectArray.Select( a => Convert.ChangeType(a, type.GetElementType() ) ).ToArray< type.GetElementType() > ();
            //return (J)typedArray;

            switch(type.GetElementType())
            {
                case Type t when t == typeof(string): 
                    genericObject = JSInstanceProxy.FuncByNameAsStringArray(jsObject, ToJSCasing(funcName), objs);
                    return (J)genericObject;
                case Type t when t == typeof(double): 
                    genericObject = JSInstanceProxy.FuncByNameAsDoubleArray(jsObject, ToJSCasing(funcName), objs);
                    return (J)genericObject;
                // TODO: Implement other primitive types supported as marshalled arrays
                default:
                    throw new NotImplementedException($"CallJSFunc: Returning array of {type.GetElementType()} not implemented");
            }

        }
        else
            genericObject = JSInstanceProxy.FuncByNameAsObject(jsObject, ToJSCasing(funcName), objs);
                
        return (J)genericObject;
    }

    public static object[] UnwrapJSObjectParams(object[] parameters)
    {
        object[] objs = null;
        for (int i = 0; i < parameters.Length; i++)
        {
            object param = parameters[i];
            //if(funcName == "Append")
            //    Console.WriteLine($"CallJSFunc: {param}");
            // if the param is a JSObject wrapper, then unwrap Wrappers to it's JSObject handle
            if (param is IJSObjectWrapper wrapper)
            {
                if (objs == null) // if first time we converting a type, 
                    objs = TypedArrayToObjectArray(parameters, i);// then need to create an untyped object array to allow reassignment of this element

                objs[i] = wrapper.JSObject;
            }

            //parameters[i] = (param as IJSObjectWrapper)?.JSObject ?? param;

            //if (funcName == "Append")
            //    Console.WriteLine($"CallJSFunc 2: {param}");
        }
        if (objs == null) // if no wrappers were found, then use original parameters array
            objs = parameters;
        return objs;
    }

    private static object[] TypedArrayToObjectArray(object[] objs, int index)
    {
        if(objs.GetType().GetElementType() == typeof(object))
        {
            return objs;
        }
        else
        {
            object[] objs2 = new object[objs.Length];
            Array.Copy(objs, objs2, index+1);
            //for(int i = 0; i < objs.Length; i++)
            //{
            //    objs2[i] = objs[i] as IJSObjectWrapper)?.JSObject ?? objs[i];
            //}
            return objs2;
        }



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

