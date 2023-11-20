
# SerratedJQ

A C# WebAssembly wrapper for jQuery, intended to enable implementation of client side logic in C# for a traditional web application such as ASP.NET MVC.  Provides the capability to read and manipulate the HTML DOM, create .NET event handlers subscribed to HTML DOM events, hold references to DOM elements from a .NET WebAssembly, and attach primitive data or managed object references to elements.  Leverages Uno.Wasm.Bootstrap for compilation to WebAssembly format, but does not require consumers to use the full Uno Platform.

## Demo
A demo is published as a static site at https://serratedsharp.github.io/CSharpWasmJQueryDemo/

Emphasis on "static".  There's no server side code in this demo.  The .NET assemblies are downloaded to your browser as simple static files, the same way your browser would download *.js, *.css, or images, and run inside a WebAssembly sandbox.  No .NET server side hosting is needed, but this approach could easily be combined with any traditional web application such as MVC.  This makes this solution composable with existing architectures looking to provide greater agility in developing client side logic.

A more extensive demo including integration with a MVC project and API requests from the WASM client to MVC host, including a walkthrough of the code:
https://www.youtube.com/watch?v=0BrGf99K6CU

Code from Demo: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample

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

## Installation

### Prerequisites  
- SerratedSharp.SerratedJQ, Uno.Wasm.Bootstrap, Uno.Foundation.Runtime.WebAssembly, NewtonSoft.Json
- See Release Notes for specific dependency versions that have been validated.
- .NET Core 8

### Quick Start Guide
- Create a Blank Solution. 
- Add new projects each targetting .NET 8:
  - .NET Console App
  - ASP.NET Core Web App (Model-View-Controller)
  - Class Library (to hold classes shared by the WASM client and MVC host).
- Build the MVC project
- Right click the Console project -> Edit Project File
- Add the following Nuget referenes:
```XML
<ItemGroup>
	<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
	<PackageReference Include="Uno.Foundation.Runtime.WebAssembly" Version="5.0.19" />
	<PackageReference Include="Uno.Wasm.Bootstrap" Version="8.0.3" />
	<PackageReference Include="SerratedSharp.JSInteropHelpers" Version="0.1.2" />
	<PackageReference Include="SerratedSharp.SerratedJQ" Version="0.1.2" />
</ItemGroup>
```
- Add a copy of this Build.props file to the Console app: [Build.props](https://github.com/SerratedSharp/SerratedJQ/blob/main/GettingStarted/GettingStarted.WasmClient/Build.props)
- Update `<DestinationWebProjectName>` to match your MVC app's project folder name.  You may need to adjust the release value of `<WasmShellWebAppBasePath` at a future time depending on the base path your app is hosted at.  The default debug value should work for the default configuration of a new MVC project.
- Add `<Import Project=".\Build.props" />` inside the Console app's *.csproj just within the `</Project>` closing tag.
- Place the following in the MVC project's Views/Shared/_Layout.cshtml in the bottom of the `<head>` tag, adjusting the jquery URL as appropriate for your inclusion approach. Note the below includes loading the jQuery javascript library:
```Razor
<script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
<!-- Move other non-RequireJS scripts here -->

<!-- Setup: WASM Bootstrap -->
@inject Microsoft.AspNetCore.Hosting.IWebHostEnvironment WebHostEnvironment
@{
    // Get most recently generated WASM package and reference it.      
    var directories = new System.IO.DirectoryInfo(WebHostEnvironment.WebRootPath).GetDirectories("package_*").OrderByDescending(d => d.CreationTimeUtc);
    string wasmPackageName = directories.First().Name;
    string wasmBaseUrl = $"{Url.Content("~/")}{wasmPackageName}";          
}
<script type="text/javascript" src="@wasmBaseUrl/require.js"></script>
<script type="module" src="@wasmBaseUrl/uno-bootstrap.js"></script>
<link rel="stylesheet" type="text/css" href="@wasmBaseUrl/normalize.css">
<link rel="stylesheet" type="text/css" href="@wasmBaseUrl/uno-bootstrap.css">
<link rel="prefetch" href="@wasmBaseUrl/uno-config.js">
<link rel="prefetch" href="@wasmBaseUrl/dotnet.js">
<link rel="prefetch" href="@wasmBaseUrl/mono-config.json">
<link rel="prefetch" href="@wasmBaseUrl/dotnet.native.wasm">
<link rel="prefetch" href="@wasmBaseUrl/dotnet.native.js">
<link rel="prefetch" href="@wasmBaseUrl/dotnet.runtime.js">
```

The default MVC template places jquery, bootstrap, amd site.js script references at the bottom of `_Layout.cshtml`, but these will now fail due to use of RequireJS.  These can be converted to use require, but we will move them above RequireJS for now.  Move the following from the bottom and insert it just above `<!-- Setup: WASM Bootstrap -->` and commenting out one of the jquery references of your choice.  
```
<script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
@* <script src="~/lib/jquery/dist/jquery.min.js"></script> *@
<script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script src="~/js/site.js" asp-append-version="true"></script>

<!-- Setup: WASM Bootstrap -->
```

- Place the following just after the ending `</header>` (this should be inside the <body> for a default MVC project)
```HTML
<div id="uno-body" class="container-fluid uno-body">
    <div class="uno-loader"
         loading-position="bottom"
         loading-alert="none">

        <!-- Logo: change src to customize the logo -->
        <img class="logo"
             src=""
             title="Uno is loading your application" />

        <progress></progress>
        <span class="alert"></span>
    </div>
</div>
<noscript>
    <p>This application requires Javascript and WebAssembly to be enabled.</p>
</noscript>
```

- In Startup.cs or in Program.cs for more current MVC project templates, add this code preceding the existing `app.UseStaticFiles();` to support serving static WASM files:
```C#
var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".clr"] = "application/octet-stream";
provider.Mappings[".dat"] = "application/dat";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
app.UseStaticFiles();// existing default static files config
```

- In the Console project Program.cs:        
  - Change `static void Main` to `static async Task Main` (supports awaitable methods wrapping JS promises)
  - Add the following to main which initializes scripts for interop, and waits for JQuery document ready:
```
SerratedSharp.SerratedJQ.JSDeclarations.LoadScripts();// declares javascript proxies needed for JSImport
await JQueryPlain.Ready(); // Wait for document Ready

JQueryPlainObject unoBody = JQueryPlain.Select("[id='uno-body'");            
unoBody.Html("<div style='display:none'></div>");// triggers uno observer that hides the loading bar/splash screen
```

- Build both projects, set the MVC Project as the startup project, then run the MVC project.
- If everything is working properly then you should see the Console.Writeline "Hello World" appear as message in the browser debug console, confirming your C# ran locally in the browser.
![image](https://github.com/SerratedSharp/SerratedJQ/assets/97156524/77248159-4866-44e9-a320-350ee72547c0)
 
> [!NOTE] 
> You must explicitly build the WasmClient when making changes so it rebuilds the package.  Because there is no project reference from the MVC project to the WasmClient project, then it is not automatically rebuilt. 

Rebuild the solution then launch the MVC project.  Verify you recieve no errors in browser console.

At this point you have a working setup and can interact with the DOM from Program.cs Main().  For a traditional multi-page web app, you will want a way to execute C# code specific to each page.  There are a variety of ways this could be supported, such as using `[JSExport]` and calling managed C# code from the page's javascript, but requires exporting and importing a module for each page.  

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

## Usage

Types suffixed with "Plain" seek to implement the jQuery API as-is.  Some liberties for security or consistency have been taken, such as not providing a `$()` equivalent, but rather providing separate `.Select` and `.ParseHtml` methods to ensure parameters are never interpreted as HTML when not intended, as this can be a security pitfall.  Seperate `.ParseHtml` and `.ParseHtmlAsJQuery` methods disambiguate pitfalls where jQuery ParseHtml can sometimes return an HtmlElement instead of a jQuery object.

Opinionated non-Plain API's are planned for future implementation which would more closely align with a typical .NET framework API.

- The GettingStarted project demonstrates basic DOM manipulation and event subscription: [GettingStarted IndexClient.cs](https://github.com/SerratedSharp/SerratedJQ/blob/main/GettingStarted/GettingStarted.WasmClient/IndexClient.cs)
- The SerratedJQSample project includes more advanced examples as well as API requests to the MVC project from the WASM client: [SerratedJQSample](https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample)

# Security Considerations

The same security considerations when using JQuery apply when using this wrapper.  Some JQuery methods could be vulnerable to XSS where uncleaned data originating from different users is passed into library methods.  (This is not a unique risk to JQuery, and applies in some form to virtually all templating and UI frameworks where one might interpolate user data and content.)   See Security Considerations in https://api.jquery.com/jquery.parsehtml/ and https://cheatsheetseries.owasp.org/cheatsheets/DOM_based_XSS_Prevention_Cheat_Sheet.html to understand the contexts where different sanitization must occur.  Typically this means the appropriate encoding or escaping is applied to HTML or Javascript, depending on the context of where the user generated content is being interpolated.

## Release Notes

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


