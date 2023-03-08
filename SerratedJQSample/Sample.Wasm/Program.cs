using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sample.Wasm
{

	// Runtime Overview
	// 1) The Controller Action is called, the CSHTML rendered to HTML, and returned to the browser.
	// 2) The HTML contains references to the Uno.Wasm.Bootstrap javascript files which load
	//		and run, then begin loading the WASM assemblies. (i.e. this the "boostrapping" process, see )
	// 3) Program.Main() is called, which is running client side in the browser.
	// 4) Using CallbacksHelper.Export we expose C# client side methods to javascript.
	//		These methods will be available to perform setup/initialization for each specific page.
	// 5) In each individual CSHTML page, we have javascript in the $() document ready event,
	//		which calls the specific JS-to-C# method init for that page.  This ensures our C# code for the page isn't run until
	//		the JQuery is loaded, WASM is loaded, and the DOM/Page is loaded/ready.
	// 6) Once the JQuery doc ready event fires, our page specific Init() method does any setup,
	//		typically wiring up to page/UI events via the SerratedJQ wrapper or doing initial HTML/DOM manipulation.

	public class Program
    {

        // Once the page loads and our WASM Assembly is loaded, Program.Main entry point is called
		static void Main(string[] args)
        {
            Console.WriteLine("The main entry point is executed on page load once WASM is bootstrapped/loaded. This message should appear in the browser console confirming the WASM is loaded.");

            // Register C# functions that can be called from Javascript.  Primarily for setup/initialization for specific pages.
            CallbacksHelper.Export(jsMethodName: "InitValidationDemo", () => ValidationDemoPage.Init());
            CallbacksHelper.Export(jsMethodName: "InitListDemo", () => ListDemoPage.Init());
            CallbacksHelper.Export(jsMethodName: "InitAdvListDemo", () => AdvListDemoPage.Init());
            // For example calling InitListDemo() from javascript will call C# ListDemoPage.Init()

            // Now that callbacks are registered and available,
            // we signal to the page that WebAssembly is loaded.
            Uno.Foundation.WebAssemblyRuntime.InvokeJS("WasmReady()");
            
        }



	}
}
  