using GettingStarted.WasmShared;
using SerratedSharp.SerratedJQ;
using SerratedSharp.SerratedJQ.Plain;
using System.Runtime.InteropServices.JavaScript;

namespace GettingStarted.WasmClient
{
    internal class Program
    {
        // Setup: Change `void Main` to `async Task Main` to support awaitable calls that wrap JS promises
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!"); // when launching the MVC project and WASM is loaded successful, this message will appear in Browser's debug console

            SerratedSharp.SerratedJQ.JSDeclarations.LoadScripts();// declares javascript proxies needed for JSImport
            await JQueryPlain.Ready(); // Wait for document Ready

            JQueryPlainObject unoBody = JQueryPlain.Select("[id='uno-body'");            
            unoBody.Html("<div style='display:none'></div>");// triggers uno observer that hides the loading bar/splash screen

            // We have a single WASM module for all pages, and dedicated classes that represent page specific scripts.
            // This Main will be called first for any page, then we determine what the current page is and start the appropriate script.
            
            // Get the globalThis.WasmPageScript enum name that was declared in the CSHTML: `globalThis.WasmPageScript = '@( WasmPageScriptEnum.ValidationDemo.ToString() )';`
            var wasmPageScriptName = JSHost.GlobalThis.GetPropertyAsString("WasmPageScript");
            WasmPageScriptEnum? pageScript = null;
            if ( Enum.TryParse<WasmPageScriptEnum>(wasmPageScriptName, out WasmPageScriptEnum parsed) ){
                pageScript = parsed;
            }

            switch (pageScript)
            {
                case WasmPageScriptEnum.Index:
                    IndexClient.Init();// start the page specific script
                    break;
                case WasmPageScriptEnum.Privacy:
                    PrivacyClient.Init();// start the page specific script
                    break;
                default:
                    Console.WriteLine($"No page script configured for WasmPageScriptEnum '{pageScript}'.");
                    break;
            }
        }

        
    }
}