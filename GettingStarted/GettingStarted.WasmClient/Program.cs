using SerratedSharp.SerratedJQ;

namespace GettingStarted.WasmClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!"); // when launching the MVC project and loaded succesfully, this message will appear in Browser's debug console

            // Register C# functions that can be called from Javascript.  Primarily for setup/initialization for specific pages.
            CallbacksHelper.Export(jsMethodName: "IndexPageReady", () => IndexClient.Init());
            CallbacksHelper.Export(jsMethodName: "PrivacyPageReady", () => PrivacyClient.Init());
            // For example calling IndexPageReady() from javascript will call C# IndexClient.Init()

            // Signal that WASM is loaded, calling the javascript WasmReady declared at the bottom of the *.cshtml
            Uno.Foundation.WebAssemblyRuntime.InvokeJS("WasmReady()");
        }

        
    }
}