# SerratedJQ

A C# WebAssembly wrapper for JQuery which provides the capability to read and manipulate the HTML DOM, create .NET event handlers for HTML DOM events, hold references to DOM elements from C# WASM, attach data or managed references to HTML DOM element datasets, and expose static .NET methods as javascript methods.  Leverages Uno.Wasm.Bootstrap for WebAssembly support, but does not require consumers to use full Uno Platform.  The intention is that this wrapper would be used by those building traditional web applications(e.g. ASP.NET MVC) but who wish to use a .NET language such as C# to implement client side UI logic rather than javascript.  Please see Nuget package Release Notes for specific version compatibility information.

## Demo
A demo is published as a static site at https://serratedsharp.github.io/CSharpWasmJQueryDemo/

Emphasis on "static".  There's no server side code in this demo.  The .NET assemblies are downloaded to your browser as simple static files, the same way your browser would download *.js, *.css, or images, and run inside a WebAssembly sandbox.  No .NET server side hosting is needed, but this approach could easily be combined with any traditional web application such as MVC.  This makes this solution composable with existing architectures looking to provide greater agility in developing client side logic using C# rather than javascript. 

The sample project implements additional scenarios not demonstrated in the static site, such as making API calls from the client side .NET module to a .NET WebAPI.

## Installation

Prerequisites:  
- Uno.Wasm.Bootstrap 3.2 
    - (See Nuget Release Notes tab for current version support info: https://www.nuget.org/packages/SerratedSharp.SerratedJQ.Lite/)
- .NET 6

Setup/Getting Started Video (includes Uno.Wasm.Bootstrap setup): https://www.youtube.com/watch?v=lyebV4v1T_A  
Code from Video: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQGetStarted

Add the package SerratedSharp.SerratedJQ.Lite to your WebAssembly project from the Nuget package manager.  Currently you must check **Include prerelease** as only alpha versions are available.  See above video for setting up Uno.Wasm.Bootstrap.

![image](https://user-images.githubusercontent.com/97156524/155268895-cef3df20-0a1d-4cfb-beaf-4d85c21e1474.png)

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

void Test_OnClick(JQueryBox sender, object e)
{
  var newElement = JQueryBox.FromHtml("<span>Clicked</span>");
  JQueryBox.Select("body").Append(newElement);
}
```

## Warning
This is an experimental proof of concept and not appropriate for production use.

Event handlers and ManagedObjectAttach() potentially generate memory leaks due to shortcuts taken to pin managed objects referenced from DOM or javascript.    

Some methods could potentially be exploited by XSS where uncleaned data originating from users is passed into library methods.  Many methods internally generate and execute javascript or manipulate the DOM, and hardening has not been done to ensure parameters embedded in JS or HTML is appropriately cleaned/escaped.  This would be equivalent to the risk posed by use of JS eval().