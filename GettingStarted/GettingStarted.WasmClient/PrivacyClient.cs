using SerratedSharp.SerratedJQ;
using SerratedSharp.SerratedJQ.Plain;

namespace GettingStarted.WasmClient
{
    public class PrivacyClient
    {
        public static void Init()
        {
            Console.WriteLine("Privacy Page WASM Executed.");
            JQueryPlain.Select("body").Append("<div>Hello from PrivacyClient</div>");
        }
    }
}
