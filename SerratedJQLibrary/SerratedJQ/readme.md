
A C# WASM JQuery wrapper appropriate for use in a WebAssembly using Uno.Wasm.Bootstrap. Provides the capability to read and manipulate the HTML DOM, subscribe to HTML DOM events, and hold references to elements from C# WASM.

## Demo
A demo is published as a static site at https://serratedsharp.github.io/CSharpWasmJQueryDemo/

Emphasis on "static".  There's no server side code in this demo.  The .NET assemblies are downloaded to your browser as simple static files, the same way your browser would download *.js, *.css, or images, and run inside a WebAssembly sandbox.  No .NET server side hosting is needed, but this approach could easily be combined with any traditional web application such as MVC.  This makes this solution composable with existing architectures looking to provide greater agility in developing client side logic. 

## Installation

Prerequisites:  
- Uno.Wasm.Bootstrap 3.3.1, Uno.Foundation.Runtime.WebASsembly 3.11.19
    - (See Release Notes tab for current version support info: https://www.nuget.org/packages/SerratedSharp.SerratedJQ.Lite/)
- .NET Core 5 or 6

Setup/Getting Started Video (includes Uno.Wasm.Bootstrap setup): https://www.youtube.com/watch?v=lyebV4v1T_A  
Code from Video: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQGetStarted

Sample/Demo Code: https://github.com/SerratedSharp/SerratedJQ/tree/main/SerratedJQSample
    - The SerratedJQSample is more comprehensive than the static demo site and GetStarted projects.  It includes client to server API calls, facotring of UI elements into self-contained component, and strongly typed event handling.

Add the package SerratedSharp.SerratedJQ.Lite to your WebAssembly project from the Nuget package manager.  Currently you must check **Include prerelease** as only alpha versions are available.  See above video for setting up Uno.Wasm.Bootstrap.

Note the Lite version now includes everything I had slated for a Pro version. Due to time constraints I haven't been able to get this package to a production ready state as quickly as I hoped, so I am fully open sourcing it and doing away with tiered access.

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