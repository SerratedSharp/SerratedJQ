
# SerratedSharp.JSInteropHelpers

A class library to simplify creating .NET WASM wrappers on JS instances.  This library is leveraged by SerratedJQ, but is designed to be agnostic.  Otherwise it has not been thoroughly refined, but may be of use to others.

## Example

This demonstrates how easy it is to create a C# interface for a large API surface area for a JS instance. In this case various methods for a jQuery object:

```C#
public class JQueryPlainObject : IJSObjectWrapper<JQueryPlainObject>
{
    //...
    public JQueryPlainObject First() => this.CallJSOfSameNameAsWrapped();
    public bool Is(string selector) => this.CallJSOfSameName<bool>(selector);
    public JQueryPlainObject Eq(int index) => this.CallJSOfSameNameAsWrapped(index);
    public JQueryPlainObject Slice(int start, int end) => this.CallJSOfSameNameAsWrapped(start, end);
    public JQueryPlainObject Filter(string selector) => this.CallJSOfSameNameAsWrapped(selector);        
    public JQueryPlainObject Odd() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject NextUntil(string stopAtSelector = null, string filterResultsSelector = null)
    public JQueryPlainObject Add(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryPlainObject Add(string selector, JQueryPlainObject context) => this.CallJSOfSameNameAsWrapped(selector, context);
    public JQueryPlainObject After(string html, params string[] htmls) 
        => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryPlainObject After(JQueryPlainObject jqObject, params JQueryPlainObject[] jqObjects) 
        => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
    public JQueryPlainObject Append(IJQueryContentParameter contentObject, params IJQueryContentParameter[] contentObjects) 
        => this.CallJSOfSameNameAsWrapped(Params.PrependToArray(contentObject, ref contentObjects));
    
}
```

`CallJSOFSameName*` methods leverage `[CallerMemberName]` to determine name of containing C# function, convert it to lower camel case, then call a method of that name on `this`'s containing `JSObject`.  The instance wrapper should implement IJSObjectWrapper<W> where W is the wrapping type, and holds a reference to the `JSObject` instance that it wraps. A C# call such as `instance.Filter(index)` would be translated to the javascript `jsObject["filter"].apply(jsObject, index);` which is a generic approach equivilant to `jsObject.filter(index);`.

Note in the above examples, use of `Params.Merge` and `Params.PrependToArray` which is necesary in some cases where the C# params and Javascript params differ in regards to repeating params.

A wrapper would contain a reference to a JSObject, which is the .NET handle for the javascript object it wraps:

```C#
public class JQueryPlainObject : IJSObjectWrapper<JQueryPlainObject>
{
    internal JSObject jsObject;// reference to the jQuery javascript interop object
    
    /// <summary>
    /// Handle to the underlying javascript jQuery object
    /// </summary>
    public JSObject JSObject { get { return jsObject; } } // required by IJSObjectWrapper and is the handle used by CallJSofSameName* methods

    // Not required, but often JS libraries return instances through specific static calls(not shown), and therefore we do not provide a public default constructor.
    // It is internal so our other static methods that capture instance to be wrapped can call this constructor.
    internal JQueryPlainObject() { }

    // Instances can only be created thru factory methods like Select()/ParseHtml() or .WrapInstance() used when an interop *AsWrapped call returns a new JSObject.
    
    public JQueryPlainObject(JSObject jsObject) { this.jsObject = jsObject; }

    // This static factory method defined by the IJSObjectWrapper enables generic code such as CallJSOfSameNameAsWrapped to automatically wrap JSObjects
    static JQueryPlainObject IJSObjectWrapper<JQueryPlainObject>.WrapInstance(JSObject jsObject)
    {
        return new JQueryPlainObject(jsObject);
    }
    //...
```

The above helpers assist with **instance** method mapping without needing any javascript shims or manual JSImport mapping.

.NET's JSImport is still used for **static** methods.

For example, a static javascript method that returns new instances could be called like so to wrap them in a new C# wrapper:

```C#
   YourWrapperObject newInstance = YourWrapperObject.Select("#bob");
```

Calling the internal constructor and assigning the JSObject instance it wraps:
```C#
public static YourWrapperObject Select(string selector)
{
    var managedObj = new YourWrapperObject();
    managedObj.jsObject = YourWrapper.GetSomething(selector); // a method that returns an instance of the JS type you're wrapping
    return managedObj;
}

//...
[JSImport(baseJSNamespace + ".GetSomething", moduleName)]
public static partial JSObject GetSomething(string selector);
```

```js
JQueryProxy.GetSomething = function (selector) {
    return someJSlibrary().getSomething(selector);
};
```

As a more concrete example, SerratedJQ captures jQuery object instances from static .Select calls like so to wrap them:

```C#
// Return new wrapped instance from the JSObject instance returned by the static JQueryProxy.Select
public static JQueryPlainObject Select(string selector)
{
    var managedObj = new JQueryPlainObject();// use internal constructor
    managedObj.jsObject = JQueryProxy.Select(selector); // assign returned JS object reference to its jsObject field
    return managedObj; // return newly wrapped instance
}
```

```C#
// Static proxy
[JSImport(baseJSNamespace + ".Select", moduleName)]
public static partial JSObject Select(string selector);
```

```js
// JS Shim for static call
JQueryProxy.Select = function (selector) {
    return jQuery(document).find(selector);
};
```

## Prerequisites  
- This can be leveraged where the final downstream consuming assembly will be either a .NET 8 WASMBrowser assembly or Uno.Wasm.Bootstrap assembly.
- .NET 8 Core

## Quick Start

This describes two different approaches to setting up a WASM project.  Both approaches include a self hosted HTTP dev server for delivering the WASM package to the browser to test locally. 

#### .NET 8 wasmbrowser Projects

- Create a new project using the "WebAssembly Browser App" template.
  - For more information about adding this template: https://learn.microsoft.com/en-us/aspnet/core/client-side/dotnet-interop?view=aspnetcore-8.0
- Add Nuget reference to SerratedSharp.JSInteropHelpers
- Add the following call to Program.Main() to load the JS module:

```
await SerratedSharp.JSInteropHelpers.JSInteropHelpersModule.ImportAsync("..");
```

The ".." base URL is typically used because the WASM module is loaded from a one level deep subpath, and the JS module is relative to the root of the app.  This could be adjusted to include your full base URL/domain or additional subpath segments if needed.

#### Uno.Wasm.Bootstrap Project

- Create a new Console App project.
- Add a Nuget reference to Uno.Wasm.Bootstrap and Uno.Wasm.Bootstrap.DevServer.
- Add a Nuget reference to SerratedSharp.JSInteropHelpers

The JS module is imported automatically by Uno.Wasm.Bootstrap.
