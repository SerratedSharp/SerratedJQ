
# SerratedJQ

A C# WebAssembly wrapper for jQuery, intended to enable implementation of client side logic in C# for a traditional web application such as ASP.NET MVC.  Provides the capability to read and manipulate the HTML DOM, create .NET event handlers subscribed to HTML DOM events, hold references to DOM elements from a .NET WebAssembly, and attach primitive data or managed object references to elements.  Leverages Uno.Wasm.Bootstrap for compilation to WebAssembly format, but does not require consumers to use the full Uno Platform.

## Demo

Video demo of the SerratedJQSample project which includes integration with a MVC project and API requests from the WASM client to MVC host, including a walkthrough of the code: https://youtu.be/l_G3_WYZorE

Code from Demo: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample

A demo is also published as a static site at https://serratedsharp.github.io/CSharpWasmJQueryDemo/.  The github.io hosted demo is a static site with no backing MVC host.  The .NET assemblies are downloaded to your browser as simple static files, the same way your browser would download *.js, *.css, or images, then run inside a WebAssembly sandbox.  This approach could easily be combined with any traditional web application such as MVC.  This makes this solution composable with existing architectures looking to provide greater agility in developing client side logic.

## Example

This example C# WebAssembly code shows how you might select an HTML element, subscribe to an HTML click event, and respond to the event by manipulating the DOM, such as appending an element to the page.

```C#
using SerratedSharp.SerratedJQ;
static void Main(string[] args)
{
  JQueryPlainObject clickMe = JQueryPlain.ParseHtmlAsJQuery("<span>Click Me</span>");
  JQueryPlain.Select("body").Append(clickMe);
  clickMe.OnClick += Test_OnClick;
}

void Test_OnClick(JQueryPlainObject sender, dynamic e)
{
  var newElement = JQueryPlain.ParseHtmlAsJQuery("<span>Clicked</span>");
  sender.Append(newElement);  
}
```

Having handles to DOM elements within client side C# opens the door for model driven DOM manipulation.  In this example from the SerratedJQSample ListDemo, we use C# models to reorder items, then reorder the corresponding HTML DOM elements:
```C#
private void SortByRep_OnClick(JQueryPlainObject sender, object e)
{
    Rows.OrderBy(r => r.Model.Rep.Name) // Order by backing model data
        .ToList().ForEach(a => Container.Append(a.JQBox)); //Reorder HTML elements in the DOM
}
```

JQuery event objects are converted to dynamic objects.  HtmlElement references at the first layer such as `.target` and `.currentTarget` are wrapped as JQueryPlain object references to support interaction through the JQueryPlain API.  The `sender` will typically be the same jQuery object you used to subscribe to the event from:
```C#
void Test_OnClick(JQueryPlainObject sender, dynamic e)
{
  Console.WriteLine(e); // Outputs full event structure to browser debug console
  string eventName = e.type;// If we know the structure of the event object we can access values through loosely typed dynamic
  Assert.Equal(eventName == "click");
  GlobalJS.Console.Log("Child_OnClick", sender, e, e.target, e.currentTarget);
}
```

Since both the client and server are .NET based, then client API requests can be made to the server using .NET API's, and responses deserialized using the shared class models used by both the client and server:
```C#
// In MVC(or alternatively WebAPI):
public class ListDemoController : Controller
{       
    public JsonResult GetSales()
    {
        List<ProductSalesModel> sales = Repo.GetProductSales();
        return Json(sales);
    }
}

// In WASM client:
var response = await client.GetAsync("GetSales");
var content = await response.Content.ReadAsStringAsync();
var prods = JsonSerializer.Deserialize<List<ProductSalesModel>>(content);
```

## Installation

### Prerequisites  
- SerratedSharp.SerratedJQ, Uno.Wasm.Bootstrap, Uno.Foundation.Runtime.WebAssembly, NewtonSoft.Json
- See Release Notes for specific dependency versions that have been validated.
- .NET Core 8

### Quick Start Guide

Video: https://www.youtube.com/watch?v=h7c05KnybrQ

- Create a Blank Solution. 
- Add new projects each targeting .NET 8:
  - .NET Console App
  - ASP.NET Core Web App (Model-View-Controller)
  - Class Library (to hold classes shared by the WASM client and MVC host).
- Build the MVC project
- Right click the Console project -> Edit Project File
- Add the following Nuget references and config:
```XML
<ItemGroup>
	<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
	<PackageReference Include="Uno.Foundation.Runtime.WebAssembly" Version="5.0.48" />
	<PackageReference Include="Uno.Wasm.Bootstrap" Version="8.0.4" />
	<PackageReference Include="Uno.Wasm.Bootstrap.DevServer" Version="8.0.4" />
	<PackageReference Include="SerratedSharp.JSInteropHelpers" Version="0.1.3" />
	<PackageReference Include="SerratedSharp.SerratedJQ" Version="0.1.3" />
</ItemGroup>
<PropertyGroup>	
	<WasmShellMode>BrowserEmbedded</WasmShellMode>
</PropertyGroup>
<PropertyGroup Condition="'$(Configuration)'=='Debug'">	
	<MonoRuntimeDebuggerEnabled>true</MonoRuntimeDebuggerEnabled>	
	<WasmShellILLinkerEnabled>false</WasmShellILLinkerEnabled>
</PropertyGroup>
```

Click Save All and you will likely be prompted to reload the project. (Be sure to Save All first or the project changes will be lost when reloading.)  

A launchSettings.json file should be generated under the console project's /Properties/.  Open the file and note the https base URL's port which will be the URL that serves the WASM static files.  We will refer to this as the WASM Base URL for use later:

![image](https://github.com/SerratedSharp/SerratedJQ/assets/97156524/30dfd58f-3d63-4366-90c2-bf04be013101)

Change the `"launchBrowser": true` setting to `false`, since we will only want one browser window launched from the MVC project, and not from the WASM project.

- Place the following in the MVC project's Views/Shared/_Layout.cshtml just above existing `RenderSectionAsync("Script"...)`.  It's assumed jquery.js is included above somewhere on the page.  **Replace the port 11111 with the port identified above from the WASM project's launchSettings.json base URL.**  This is the Uno Bootstrap script that will load our WASM module client side:  
```Razor
    <script src="https://localhost:11111/embedded.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
```

- In the MVC project's launchSettings.json, add the following line inside the "https" section which will support connecting the debugger from the browser to the WASM project, again **replace the port 11111 with the port identified from the WASM project**:
`,"inspectUri": "https://localhost:11111/_framework/debug/ws-proxy?browser={browserInspectUri}"`

- Place the following just after the ending `</header>` (this should be inside the <body> for a default MVC project):
```HTML
<div id="uno-body"></div>
```

- In the Console project Program.cs:        
  - Change `static void Main` to `static async Task Main` (supports awaitable methods wrapping JS promises)
  - Add the following to main which initializes scripts for interop, and waits for JQuery document ready:
```
SerratedSharp.SerratedJQ.JSDeclarations.LoadScripts();// declares javascript proxies needed for JSImport
await JQueryPlain.Ready(); // Wait for document Ready
JQueryPlain.Select("base").Remove();// Remove Uno's <base> element that can break relative URL's if embedded.js hosted remotely

JQueryPlainObject unoBody = JQueryPlain.Select("[id='uno-body'");            
unoBody.Html("<div style='display:none'></div>");// triggers uno observer that hides the loading bar/splash screen
```

- Build both projects, then right click the solution and choose "Configure Startup Projects..." and set both the the MVC and Console/WASM projects as startup projects:

![image](https://github.com/SerratedSharp/SerratedJQ/assets/97156524/39387cef-df21-4f66-80b2-f7d69ef41c2f)

- Start the solution in debug mode:

![image](https://github.com/SerratedSharp/SerratedJQ/assets/97156524/37d94a7e-b0e2-4fed-ba1c-3d21eaae9ac6)

Two console windows will start, one hosting the MVC app, the other hosting the WASM app, and a browser window should launch pointing to the MVC URL.

- If everything is working properly then you should see the Console.Writeline "Hello World" appear as message in the browser debug console, confirming your C# ran locally in the browser.

![image](https://github.com/SerratedSharp/SerratedJQ/assets/97156524/77248159-4866-44e9-a320-350ee72547c0)
 
> [!NOTE] 
> You must explicitly build the WasmClient when making changes so it rebuilds the package.  Because there is no project reference from the MVC project to the WasmClient project, then it is not automatically rebuilt. 

At this point you have a working setup and can write code in Program.cs Main() to interact with the HTML DOM.  Stop the debug solution session.



For a traditional multi-page web app, you will want a way to execute C# code specific to each page.  There are a variety of ways this could be supported, such as using `[JSExport]` and calling managed C# code from the page's javascript, but requires exporting and importing a module for each page.  

Another option is to use a simple enum declaration.
- Add project references from both the MVC and Console projects to the shared Class Library project.
- Add an enum to the Class Library:
```C#
public enum WasmPageScriptEnum
{
    None = 0,
    Index = 1,
    Privacy = 2,
    // Add additional pages here
}
```

- In the MVC project, add the following to the script section of each cshtml view, using a different enum value for the appropriate page.  For example, for Privacy.cshtml:
```
@section Scripts {
    <script type="text/javascript">
        globalThis.WasmPageScript = '@(WasmPageScriptEnum.Privacy.ToString())';
    </script>
}
```

- In Program.cs Main(), add the below to retrieve the declared globalThis.WasmPageScript enum name that was declared in the CSHTML:
```C#
var wasmPageScriptName = JSHost.GlobalThis.GetPropertyAsString("WasmPageScript");
WasmPageScriptEnum pageScript = Enum.Parse<WasmPageScriptEnum>(wasmPageScriptName);

switch (pageScript)
{
    case WasmPageScriptEnum.Index:
        IndexClient.Init();// start the page specific script
        break;
    case WasmPageScriptEnum.Privacy:
        PrivacyClient.Init();// start the page specific script
        break;
    default:
        break;
}
```

Each page specific script can be implemented in a separate class with an Init() entry point:
```C#
public class PrivacyClient
{
    public static void Init()
    {
        Console.WriteLine("Privacy Page WASM Executed.");
        JQueryPlain.Select("body").Append("<div>Hello from PrivacyClient</div>");
    }
}
```

See the code for the [GettingStarted](https://github.com/SerratedSharp/SerratedJQ/tree/main/GettingStarted) solution as an example of this setup.

### Overview
This setup will generate the WebAssembly when the Console project is compiled and copy it into the wwwroot of the ASP.NET project.  When the ASP.NET project is launched and a page loads in the browser, then Uno Bootstrap will download and run our WebAssembly in the browser.  The `#uno-body` div displays a loading progress bar when downloading/initializing the WASM.  Typically issues with this process as well as exceptions generated from your WebAssembly will appear in the browser console.

### Troubleshooting

- RequireJS is used by embedded.js, and this requires some scripts such as jQuery to be included before embedded.js or otherwise be included using require instead of a `<script>` block.
- Support for IL Linker Trimming is possible with appropriate configuration.  SerratedJQSample demonstrates a project where the Release build implements IL Linker trimming, along with configuration to demonstrate suppressing trimming where necessary such as for the API client models used in reflection based deserializers.  A more refined solution would use compile time serialization source generators to eliminate use of reflection based JSON deserializers.
- Support for AoT compilation has not been tested.

## Usage

Types suffixed with "Plain" seek to implement the jQuery API as-is.  Some liberties for security or consistency have been taken, such as not providing a `$()` equivalent, but rather providing separate `.Select` and `.ParseHtml` methods to ensure parameters are never interpreted as HTML when not intended, as this can be a security pitfall.  Separate `.ParseHtml` and `.ParseHtmlAsJQuery` methods disambiguate pitfalls where jQuery ParseHtml can sometimes return an HtmlElement instead of a jQuery object.

Opinionated non-Plain API's are planned for future implementation which would more closely align with a typical .NET framework API.

- The GettingStarted project demonstrates basic DOM manipulation and event subscription: [GettingStarted IndexClient.cs](https://github.com/SerratedSharp/SerratedJQ/blob/main/GettingStarted/GettingStarted.WasmClient/IndexClient.cs)
- The SerratedJQSample project includes more advanced examples as well as API requests to the MVC project from the WASM client: [SerratedJQSample](https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample)

# Security Considerations

The same security considerations when using JQuery apply when using this wrapper.  Some JQuery methods could be vulnerable to XSS where uncleaned data originating from different users is passed into library methods.  (This is not a unique risk to JQuery, and applies in some form to virtually all templating and UI frameworks where one might interpolate user data and content.)   See Security Considerations in https://api.jquery.com/jquery.parsehtml/ and https://cheatsheetseries.owasp.org/cheatsheets/DOM_based_XSS_Prevention_Cheat_Sheet.html to understand the contexts where different sanitization must occur.  Typically this means the appropriate encoding or escaping is applied to HTML or Javascript, depending on the context of where the user generated content is being interpolated.

## Release Notes

### 0.1.4
- Replaced `JQueryPlainObject.Data()` with `.DataAsJSObject()` to disambiguate return type.  Returns the entire data object as a JSObject reference.  Use JSObject.GetPropertyAs* methods to access properties or pass the returned reference to `GlobalJS.Console.Log(jqObj.DataAsJSObject())` to log entire object graph in browser console.  This method is generally useful for troubleshooting or discovering structure of the data object.  Typically you would use the existing strongly typed method `.Data<string>("one");` to access specific data properties of specific types.

This version has been tested with Uno.Wasm.Bootstrap 8.0.3, Uno.Foundation.Runtime.WebAssembly 5.0.19, and SerratedSharp.JSInteropHelpers 0.1.3 under .NET Core 8.

### 0.1.3
- Added missing `Microsoft.Windows.Compatibility` dependency required by Newtonsoft when using IL Linker trimming.

This version has been tested with Uno.Wasm.Bootstrap 8.0.3, Uno.Foundation.Runtime.WebAssembly 5.0.19, and SerratedSharp.JSInteropHelpers 0.1.3 under .NET Core 8.

### Documentation Update
Simplified Quick Start instructions, updated GettingStarted project, and adjusted setup to support debugging/breakpoints in the WASM client module.

### 0.1.2
- Added awaitable JQueryPlain.Ready().
- Updated SerratedJQSample, GettingStarted sample, and Quick Start instructions.

This version has been tested with Uno.Wasm.Bootstrap 8.0.3, Uno.Foundation.Runtime.WebAssembly 5.0.19, and SerratedSharp.JSInteropHelpers 0.1.2 under .NET Core 8.

### 0.1.0
Migration of the majority of underlying JS interop API from Uno WebAssemblyRuntime to .NET 7's `System.Runtime.InteropServices.JavaScript`.

- Going forward the underlying implementation is simplified, should perform better, and simplifies implementation of future capabilities.
- Moves toward the ability to provide interop helpers as a separate library to assist others creating .NET interop wrappers for javascript.
- Fixes for compatibility with IL Linker trimming, i.e. `<WasmShellILLinkerEnabled>true</...`.  (AOT has not yet been validated.)
- Event properties at the first layer of an event object which represent HTMLElement's, such as e.target and e.currentTarget, are now preserved as JQueryObject references across the interop layer when handling events.
- Support for delegated event handlers where a selector is passed to `.On(`: https://learn.jquery.com/events/event-delegation/#using-the-triggering-element.  See `Events_On_Click_Selector` in Tests.Wasm for a usage example.
- Expands available jQuery API methods and overloads.
- Includes breaking changes to the API interface.
    - With the goal of converging on a stable API sooner, the JQueryPlainObject seeks to implement the jQuery interface as-is to minimize the need to make opinionated choices about how to expose the API in a .NET flavored way. 
    - Sample projects have not been updated to use the new API, but unit tests within the `SerratedJQLibrary/Tests.Wasm` have been updated and can be used as a usage reference. 
    - `JQueryPlain` is a static class mirroring the global jQuery object. It now exposes the static methods `.Select(string selector)` and `.ParseHtmlAsJQuery(string html)` which generate instances of jQuery collection objects as `JQueryPlainObject` references.
    - `JQueryPlainObject` is an instance of a jQuery collection object, and exposes the majority of the jQuery API and event subscription capabilities.  Replaces `JQueryBox`.
    - `.Data` now supports types supported by .NET 7 interop as well as references to managed objects.
    - Experimental `ManagedObjectAttach()` is superseded by `JQueryPlainObject.Data("key", model)` which can store and retrieve references to .NET objects.  
- Limited methods such as .ParseHtml and .Append now also support the type HtmlElement, which allows references to native DOM elements which are not wrapped as a jQuery object. This can be necessary in certain usage scenarios where jQuery returns an HtmlElement rather than a jQuery object, or for compatibility with other interop libraries holding HtmlElement references (this can be achieved by using their .NET 7 native JSObject reference to create a SerratedSharp HtmlElement via the constructor taking a JSObject reference).  Overloads supporting this type will be expanded in future versions, but the type will be separated into a distinct library and refined before expanding usage within SerratedJQ.

This version has been tested with Uno.Wasm.Bootstrap 8.0.3 and Uno.Foundation.Runtime.WebAssembly 5.0.19 under .NET Core 8.

### 0.0.4
Nuget package metadata updates.

### 0.0.3
Updated to latest stable Uno.Wasm.Bootstrap package.

### 0.0.2
Appropriate encoding applied to ensure parameters used in the javascript interopt layer cannot break out of the parameter context.  This addresses remaining security concerns regarding javascript generated in the interopt layer.  

### 0.0.1-alpha.5

Implemented automatic management of pinning/unpinning event listeners to ensure managed listeners are made eligible for garbage collection when no unmanaged JS handles/event publishers reference them.  

### 0.0.1-alpha.4

The event object is now passed to handlers as a C# dynamic type allowing those who know the structure to navigate to desired values.
I would recommend favoring using the `JQueryBox sender` over the loosely typed `dynamic event` where there is overlap, such as retrieving the value on an input event.
Support is accomplished through a serialization/deserialization since references cannot be passed across the WASM boundary.  
This means properties such as `e.originalEvent.target` are an object ID rather than an object reference.

```C#
void Test_OnClick(JQueryBox sender, dynamic e)
{
  Console.WriteLine(e); // Outputs full event structure to browser debug console
  string eventName = e.type;// If we know the structure of the event object we can access values through loosely typed dynamic
  Assert.Equal(eventName == "click");
}
```


