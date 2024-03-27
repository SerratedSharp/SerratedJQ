using Sample.Wasm.ClientSideModels;
using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace Sample.Wasm;

// Note: The Sample.Mvc project should be set as the startup project.
// The WASM project is not run directly, but compiled into a web assmebly compatible assembly and loaded in the browser.

// Runtime Overview
// 1) Request is made from browser to the MVC's home page.
// 2) The Controller Action is called, the CSHTML rendered to HTML, and returned to the browser.
// 3) The HTML(via shared Layout) contains references to the Uno.Wasm.Bootstrap javascript files which load
//		and run, then begin loading the WASM assemblies. (i.e. this is the "boostrapping" process)
// 4) WasmClient Program.Main() is called, which is running client side in the browser.
// 5) In each individual CSHTML page, we have javascript globalThis.wasmPageScript delcaration to indicate which page specific script to run.
// Note: In Main(), after awaiting JQueryPlain.Ready(), we can be sure both the document and WASM loaded successfully.  The latter is assumed because our Main method is not executed until WASM has loaded.  So we are ready to call page specific code that interacts with the DOM via jQuery.  
// 7) We select via switch/case which page specific script to run based on the javascript globalThis.wasmPageScript declaration in the CSHTML.
//      This is not a required approach.  Alternatively we could use JSExport to expose our .NET methods to the page to trigger initialization, and/or use an event to trigger initialization.

public class Program
{

    // Once the page loads and our WASM Assembly is loaded, Program.Main entry point is called.
    static async Task Main(string[] args)
    {

        Console.WriteLine("The main entry point is executed on page load once WASM is bootstrapped/loaded. This message should appear in the browser console confirming the WASM is loaded.");

        // If using .NET 8 wasmbrowser template instead of Uno.Wasm.Bootstrap, then this initialization call is necessary:
        // await JSDeclarations.LoadScriptsForWasmBrowser();

        // In this sample, jQuery is referenced from the Layout.cshtml.  Optionally load jQuery from a URL(this method creates a script tag and awaits the onload as a promise)
        // await JSDeclarations.LoadJQuery("https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");

        await JQueryPlain.Ready(); // Wait for document Ready
        JQueryPlain.Select("base").Remove();// Remove incorrect <base> element added by embedded.js that changes the base URL for entire site
        JQueryPlainObject unoBody = JQueryPlain.Select("[id='uno-body'");
        int unoBodyCount = (int)unoBody.Length;
        if (unoBodyCount != 1)
        {
            Console.WriteLine($"Warning: {unoBodyCount} #uno-body elements found, this may indicate a failure or incorrect initialization of Uno Bootstrap.");
        }
        unoBody.Html("<div></div>");// triggers uno observer that hides the loading bar/splash screen
        Console.WriteLine("uno body updated");
        // We have a single WASM module for all pages, and dedicated classes that represent page specific scripts.
        // This Main will be called first for any page, then we determine what the current page is and start the appropriate script.

        // Get the globalThis.WasmPageScript enum name that was declared in the CSHTML: `globalThis.WasmPageScript = '@( WasmPageScriptEnum.ValidationDemo.ToString() )';`
        var wasmPageScriptName = JSHost.GlobalThis.GetPropertyAsString("WasmPageScript");
        WasmPageScriptEnum pageScript = Enum.Parse<WasmPageScriptEnum>(wasmPageScriptName);

        switch (pageScript)
        {
            case WasmPageScriptEnum.ValidationDemo:
                ValidationDemoPage.Init();// start the page specific script
                break;
            case WasmPageScriptEnum.ListDemo:
                ListDemoPage.Init();// start the page specific script
                break;
            default:
                break;
        }

    }

	}
