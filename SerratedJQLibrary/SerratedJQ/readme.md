
# SerratedJQ

A C# WebAssembly wrapper for JQuery which provides the capability to read and manipulate the HTML DOM, create .NET event handlers for HTML DOM events, hold references to DOM elements from C# WASM, attach data or managed references to HTML DOM element datasets, and expose static .NET methods as javascript methods.  Leverages Uno.Wasm.Bootstrap for WebAssembly support, but does not require consumers to use full Uno Platform.  The intention is that this wrapper would be used by those building traditional web applications(e.g. ASP.NET MVC) but who wish to use a .NET language such as C# to implement client side UI logic rather than javascript.  Please see Nuget package Release Notes for specific version compatibility information.

## Demo
A demo is published as a static site at https://serratedsharp.github.io/CSharpWasmJQueryDemo/

Emphasis on "static".  There's no server side code in this demo.  The .NET assemblies are downloaded to your browser as simple static files, the same way your browser would download *.js, *.css, or images, and run inside a WebAssembly sandbox.  No .NET server side hosting is needed, but this approach could easily be combined with any traditional web application such as MVC.  This makes this solution composable with existing architectures looking to provide greater agility in developing client side logic. 

A more extensive demo including integration with a MVC project and API requests from the WASM client to MVC host, including a walkthru of the code:
https://www.youtube.com/watch?v=0BrGf99K6CU

Code from Demo: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample

## Example
This example WebAssembly code shows how you might subscribe to an HTML click event, and respond to the event by manipulating the DOM, such as appending an element to the page.

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

## Installation

Prerequisites:  
- Uno.Wasm.Bootstrap 3.3.1, Uno.Foundation.Runtime.WebASsembly 3.11.19
    - (See Release Notes tab for current version support info: https://www.nuget.org/packages/SerratedSharp.SerratedJQ.Lite/)
- .NET Core 5 or 6

Setup/Getting Started Video (includes Uno.Wasm.Bootstrap setup): https://www.youtube.com/watch?v=lyebV4v1T_A  
Code from Video: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQGetStarted

Add the package SerratedSharp.SerratedJQ.Lite to your WebAssembly project from the Nuget package manager.  Currently you must check **Include prerelease** as only alpha versions are available.  See above video for setting up Uno.Wasm.Bootstrap.

![image](https://user-images.githubusercontent.com/97156524/155268895-cef3df20-0a1d-4cfb-beaf-4d85c21e1474.png)


## Warning
This is an experimental proof of concept and not appropriate for production use.

~~Event handlers~~(Fixed in 0.0.2) and ManagedObjectAttach() potentially generate memory leaks due to shortcuts taken to pin managed objects referenced from DOM or javascript.    

Some methods could be vulnerable to XSS where uncleaned data originating from users is passed into library methods.  Many methods internally generate and execute javascript or manipulate the DOM, and hardening has not been done to ensure parameters embedded in JS or HTML is appropriately cleaned/escaped.  This would be equivalent to the risk posed by use of JS eval().

## Release Notes

### 0.0.1-alpha.5

Implemented automatic management of pinning/unpinning event listeners to ensure managed listeners are made elgible for garbage collection when no unmanaged JS handles/event publishers reference them.  

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

Note the Lite version now includes everything I had slated for a Pro version. Due to time constraints I haven't been able to get this package to a production ready state as quickly as I hoped, so I am fully open sourcing it and doing away with tiered access.


