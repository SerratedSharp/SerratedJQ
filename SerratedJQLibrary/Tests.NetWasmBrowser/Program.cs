//using SerratedSharp.SerratedJQ.Plain;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Wasm;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Browser!");

        await SerratedSharp.SerratedJQ.JSDeclarations.LoadScriptsForWasmBrowser();
        await SerratedSharp.SerratedJQ.JSDeclarations.LoadJQuery("https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");
        await JQueryPlain.Ready();
        Console.WriteLine("JQuery Document Ready!");

        // Do something with JQuery
        JQueryPlain.Select("#out").Append("<b>Appended</b>");
        
        // Run Suite of Tests
        await TestOrchestrator.Begin();
    }
}

public partial class MyClass
{
    [JSExport]
    internal static string Greeting()
    {
        var text = $"Hello, World! Greetings from {GetHRef()}";
        Console.WriteLine(text);
        return text;
    }

    [JSImport("window.location.href", "main.js")]
    internal static partial string GetHRef();
}
