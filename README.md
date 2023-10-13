
# SerratedJQ

A C# WebAssembly wrapper for JQuery which provides the capability to read and manipulate the HTML DOM, create .NET event handlers for HTML DOM events, hold references to DOM elements from C# WASM, attach data or managed references to HTML DOM element datasets, and expose static .NET methods as javascript methods.  Leverages Uno.Wasm.Bootstrap for WebAssembly support, but does not require consumers to use full Uno Platform.  The intention is that this wrapper would be used by those building traditional web applications(e.g. ASP.NET MVC) but who wish to use a .NET language such as C# to implement client side UI logic rather than javascript.  Please see Nuget package Release Notes for specific version compatibility information.

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
  var clickMe = JQueryBox.FromHtml("<span>Click Me</span>");
  JQueryBox.Select("body").Append(clickMe);
  clickMe.OnClick += Test_OnClick;
}

void Test_OnClick(JQueryBox sender, dynamic e)
{
  var newElement = JQueryBox.FromHtml("<span>Clicked</span>");
  JQueryBox.Select("body").Append(newElement);
}
```

Having handles to DOM elements within client side C# opens the door for model driven DOM manipulation.  In this example from the SerratedJQSample ListDemo, we use C# models to reorder items, then reorder the corresponding HTML DOM elements:
```C#
private void SortByRep_OnClick(JQueryBox sender, object e)
{
    Rows.OrderBy(r => r.Model.Rep.Name) // Order by backing model data
        .ToList().ForEach(a => Container.Append(a.JQBox)); //Reorder HTML elements in the DOM
}
```

JQuery event objects are converted to dynamic objects, but keep in mind this only supports primitives thus there is no current support for complex objects embedded in the event.  The `sender` will typically be the same JQuery object you used to subscribe to the event from:
```C#
void Test_OnClick(JQueryBox sender, dynamic e)
{
  Console.WriteLine(e); // Outputs full event structure to browser debug console
  string eventName = e.type;// If we know the structure of the event object we can access values through loosely typed dynamic
  Assert.Equal(eventName == "click");
}
```

## Installation

### Prerequisites  
- SerratedSharp.SerratedJQ, Uno.Wasm.Bootstrap, Uno.Foundation.Runtime.WebAssembly
- .NET Core 7

### Quick Start Guide
- Create a Blank Solution, add new .NET Console App (.NET 7) and ASP.NET Core Web App (Model-View-Controller) projects.
- Build the MVC project
- Add Nuget references to **Uno.Wasm.Bootstrap**, **Uno.Foundation.Runtime.WebAssembly**, and **SerratedSharp.SerratedJQ** in the Console project.
![image](https://github.com/SerratedSharp/SerratedJQ/assets/97156524/9a40be28-b420-47d2-90be-e1035bcc7297)
- Add a copy of this Build.props file to the Console app: [Build.props](https://github.com/SerratedSharp/SerratedJQ/blob/main/GettingStarted/GettingStarted.WasmClient/Build.props)
- Update `<DestinationWebProjectName>` to match your MVC app's project folder name, then add `<Import Project=".\Build.props" />` inside the Console app's *.csproj just within the `</Project>` closing tag.
- Place the following in the MVC project's Views/Shared/_Layout.cshtml in the bottom of the `<head>` tag, adjusting the jquery URL as appropriate for your inclusion approach. Note the below includes loading the jQuery javascript library:
```Razor
    <script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>

    @inject Microsoft.AspNetCore.Hosting.IWebHostEnvironment WebHostEnvironment
    @{
        var directories = new System.IO.DirectoryInfo(WebHostEnvironment.WebRootPath).GetDirectories("package_*").OrderByDescending(d => d.CreationTimeUtc);
        string wasmPackageName = directories.First().Name;
        string wasmBaseUrl = $"{Url.Content("~/")}{wasmPackageName}";// Get most recently generated WASM package and reference it.
        // Note you must also set the web project's folder name in the build property <DestinationWebProjectName>. See Sample.Wasm/Build.props
    }
    <script type="text/javascript" src="@wasmBaseUrl/require.js"></script>    
    <script type="text/javascript" src="@wasmBaseUrl/uno-bootstrap.js"></script>    
    <link rel="stylesheet" type="text/css" href="@wasmBaseUrl/normalize.css" />
    <link rel="stylesheet" type="text/css" href="@wasmBaseUrl/uno-bootstrap.css" />
```

- Place the following just after the ending `</header>`
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

- In Startup.cs, preceding the existing `app.UseStaticFiles();` add:
```C#
var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".clr"] = "application/octet-stream";
provider.Mappings[".dat"] = "application/dat";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
```

- Build both projects, then launch the MVC project.
- If everything is working properly then you should see the Console.Writeline "Hello World" appear as message in the browser debug console, confirming your C# ran locally in the browser.

[!NOTE] 
You must explicitly build the WasmClient when making changes so it rebuilds the package.  Because there is no project reference from the MVC project to the WasmClient project, then it is not automatically rebuilt. 

### Overview
This setup will generate the WebAssembly when the Console project is compiled and copy it into the wwwroot of the ASP.NET project.  When the ASP.NET project is launched and a page loads in the browser, then Uno Bootstrap will download and run our WebAssembly in the browser.  The `#uno-body` div displays a loading progress bar when downloading/initializing the WASM.  Typically issues with this process as well as exceptions generated from your WebAssembly will appear in the browser console.

## Usage

- The GettingStarted project demonstrates basic DOM manipulation and event subscription: [GettingStarted IndexClient.cs](https://github.com/SerratedSharp/SerratedJQ/blob/main/GettingStarted/GettingStarted.WasmClient/IndexClient.cs)
- The SerratedJQSample project includes more advanced examples as well as API requests to the MVC project from the WASM client: [SerratedJQSample](https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample)
- To have page specific C# WASM code executed, and wait for both WASM and JQuery to be loaded, see Getting Started 
[Index.cshtml WasmReady()](https://github.com/SerratedSharp/SerratedJQ/blob/d6e39830de2c5255b32921e4115be36445df5c97/GettingStarted/GettingStarted.Mvc/Views/Home/Index.cshtml#L16) and [WasmClient Program.cs CallbacksHelper.Export()](https://github.com/SerratedSharp/SerratedJQ/blob/d6e39830de2c5255b32921e4115be36445df5c97/GettingStarted/GettingStarted.WasmClient/Program.cs#L12)
```C#
CallbacksHelper.Export(jsMethodName: "IndexPageReady", () => IndexClient.Init());// register a JS to C#WASM callback
Uno.Foundation.WebAssemblyRuntime.InvokeJS("WasmReady()");// signal WASM as loaded/ready
```
```Razor
// In page specific CSHTML, wait for both JQuery and WASM to load
@section Scripts {
    <script type="text/javascript">        
        function WasmReady() { // Wait for WASM to initialize and start Program.Main()
            $(function () { // Wait for JQuery to be ready
                Serrated.Callbacks.IndexPageReady(); // Initialize this page's script.
            });
        }
    </script>
}
```


# Warning
ManagedObjectAttach() is experimental and potentially generates memory leaks due to shortcuts taken to pin managed objects referenced from DOM or javascript.

# Security Considerations

The same security considerations when using JQuery apply when using this wrapper.  Some JQuery methods could be vulnerable to XSS where uncleaned data originating from different users is passed into library methods.  (This is not a unique risk to JQuery, and applies in some form to virtually all templating and UI frameworks where one might interpolate user data and content.)   See Security Considerations in https://api.jquery.com/jquery.parsehtml/ and https://cheatsheetseries.owasp.org/cheatsheets/DOM_based_XSS_Prevention_Cheat_Sheet.html to understand the contexts where different sanitization must occur.  Typically this means the appropriate encoding or escaping is applied to HTML or Javascript, depending on the context of where the user generated content is being interpolated.

## Release Notes

### 0.1.0
Migration of the majority of underlying JS interop API from Uno WebAssemblyRuntime to .NET 7's `System.Runtime.InteropServices.JavaScript`.

- Going forward the underlying implementation is simplified, should perform better, and simplifies implementation of future capabilities.
- Event properties which represent HTMLElement's, such as e.target and e.currentTarget, are now preserved as JQueryObject references across the interop layer when handling events.
- Expands available jQuery API methods and overloads.
- Includes some breaking changes to the API interface.
    - Sample projects have not been updated to use the new API, but unit tests within the `SerratedJQLibrary/Tests.Wasm` have been updated and can be used as a usage reference. 
    - `JQuery` is a static class mirroring the global JQuery object. It now exposes the static methods .Select() and .ParseHtml() which generate instances of jQuery collection objects.
    - `JQueryObject` is an instance of a jQuery collection object, and exposes the majority of the jQuery API and event subscription capabilities.  Replaces `JQueryBox`.
- Removes superfluous method chaining:
    - This refers to method chaining present in jQuery where the return from the method is always the same object reference, which is only used to facilitate chaining.
    - It's not really a common pattern in C# except for builder/fluent or unit of work APIs.
    - It creates ambiguity between methods where the return type is the result of the method operation, versus where the same object is being returned only to facilitate method chaining and doesn't need to be captured.  In these cases sometimes it's not clear if the operation mutated the original object reference, or if the original object is left as is and the return value is a new result of the operation.
    - Now it's clear if a modifying operation returns void, then you know the original object was mutated.  If the operation returns a different object, then you know the original object was not mutated and you must capture the return value for the result.
    - Eliminates unnecessary object allocations.
    - Chaining is still possible with methods such as .Find() and .Children() since each call returns a different JQuery object/collection, and allows iterative navigation/filtering of the DOM.



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


