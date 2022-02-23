# SerratedJQ

A C# WebAssembly wrapper for JQuery which provides the capability to read and manipulate the HTML DOM, subscribe to HTML DOM events, and hold references to elements from C# WASM.  Compatible with WebAssembly projects using Uno.Wasm.Bootstrap.  Please see Nuget package Release Notes for specific version compatibility information.

## Installation

Prerequisites

- Uno.Wasm.Bootstrap 3.2
- .NET 5 or 6

Add the package SerratedSharp.SerratedJQ.Lite to your WebAssembly project from the Nuget pacakge manager.  Currently you must check **Include prerelease** as only alpha versions are available.

![image](https://user-images.githubusercontent.com/97156524/155268895-cef3df20-0a1d-4cfb-beaf-4d85c21e1474.png)

## Example
```C#
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
