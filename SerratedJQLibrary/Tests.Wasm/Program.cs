using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SerratedSharp.JSInteropHelpers;
using System.Runtime.InteropServices.JavaScript;

//using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using Tests.Wasm;

namespace Wasm;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Program))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SerratedSharp.SerratedJQ.Plain.JQueryPlainObject))]
    static async Task Main(string[] args)
    {
        Console.WriteLine("Beginning test...");

#if DEBUG
        Console.WriteLine("DEBUG DEFINED");
#endif

        Trace.Listeners.Add(new ThrowingTraceListener());

        await Begin();
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TestsContainer))]    
    public static async Task Begin()
    {
        Console.WriteLine("Begin()");
        //Console.WriteLine(JSHost.GlobalThis.GetPropertyAsJSObject("SerratedInteropHelpers"));
        //  WebAssemblyRuntime.InvokeAsync("globalThis.SerratedExports = await Module.getAssemblyExports(\"SerratedSharp.SerratedJQ\")");
        //GlobalJS.Console.Log("SerratedExports", JSHost.GlobalThis.GetPropertyAsJSObject("SerratedInteropHelpers"));
        //Thread.Sleep(2000);
        //Console.WriteLine("Slept()");
        //TestJS.Check();
        // Run javascript files that add proxy declarations
        //await SerratedSharp.SerratedJQ.JSDeclarations.LoadScripts();
        //await HelpersJS.LoadJQuery("jquery-3.7.1.js");
        await SerratedSharp.SerratedJQ.SerratedJQModule.LoadJQuery("https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");
        await JQueryPlain.Ready();
        //// Fixes scrolling for unit test output
        JQueryPlain.Select("body").Css("position", "static");
        JQueryPlain.Select("body").Css("overflow", "auto");

        JQueryPlainObject jqObject = JQueryPlain.Select("body");
        Console.WriteLine("HTML Test: " + jqObject.Html());
        JQueryPlainObject unoBody = jqObject.Find("#uno-body");

        unoBody.Html("<div></div>");// triggers uno observer that hides the loading bar/splash screen

        //AsyncContext.Run(() => MainAsync(args)) }, null, 5000, 5000);

        var orch = new TestOrchestrator();
        // TODO: I disabled the linker. Isntead disable a namespace just for tests: https://platform.uno/docs/articles/features/using-il-linker-webassembly.html

        // Iterate through all JQTest classes and add them to the Test Orchestrator
        var types = System.Reflection.Assembly.GetAssembly(typeof(TestOrchestrator)).GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(JQTest)))
            .ToList();
        Console.WriteLine("Types found: " + types.Count);

        foreach (Type type in types)
        {
            Console.WriteLine("Class " + type.Name);
            orch.Tests.Add((JQTest)Activator.CreateInstance(type));
        }
        // Run the Orchestrator, which will in turn run all tests
        orch.Run();

    }

}

public class TestOrchestrator
{
    public List<IJQTest> Tests = new List<IJQTest>();

   
    public void Run()
    {
        int i = 1;
        foreach(IJQTest test in Tests)
        {
            Exception exc = null; JQueryPlainObject status = null;
            try
            {
                test.TestNum = i;
                Console.WriteLine("Running test " + i + $" for type {test.GetType().Name}");

                test.BeginTest(out status);
                test.Run();
            }
            catch (Exception ex) {
                exc = ex; 
            }
            test.EndTest(status, exc);
            ++i;
        }
    }
}

public interface IJQTest
{
    int TestNum { get; set; }

    //bool IsModelTest { get; set; }

    void BeginTest(out JQueryPlainObject status);
    void EndTest(JQueryPlainObject status, Exception exc);
    void Run();
}

public abstract class JQTest : IJQTest
{
    JQueryPlainObject body = JQueryPlain.Select("body");
    public int TestNum { get; set; }
    //public bool IsModelTest { get; set; } = false;
    Exception exc = null; JQueryPlainObject status = null;
    protected JQueryPlainObject tc;// test container
    protected JQueryPlainObject result;
    //protected JQueryBoxV2<TestModel> tcm;// test container with model

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(JQTest))]

    public virtual void Run() { }


    public void BeginTest(out JQueryPlainObject status)
    {
        string htmlTemplate = $"<div id='t{TestNum}'><div class='status'>T{TestNum}:</div><div class='tc'></div></div>";
        
        body.Append(htmlTemplate);
        status = JQueryPlain.Select($"#t{TestNum} .status");
        
        // tc or tcm will hold reference to test container HTML that the test implementation should use as a sandbox for the text
        // For now let test case add it's own JqueryBox inside the test container.
        //if (IsModelTest)
        //    tcm = div.Find<TestModel>(".tc");
        //else
        //    tc = div.Find(".tc");

        tc = JQueryPlain.Select($"#t{TestNum} .tc");
    }

    public void EndTest(JQueryPlainObject status, Exception exc)
    {
        string testName = this.GetType().Name;
        if (exc == null)
        {
            status.Append($"<span style='color:green'>Valid - {testName}</span>");
        }
        else
        {
            Console.WriteLine(exc);
            
            status.Append($"<span style='color:red'>Failed - <b>{testName}</b>: {exc.ToString()}</span>");

            status.Append("<div class='excontext'></div>");
            
            status.Find(".excontext").Text("Test Container: " + tc.Html());
            status.Append("<div class='resultcontext'></div>");
            status.Find(".resultcontext").Text("Result: " + result.Html());
            GlobalJS.Console.Log(testName, tc, result);
            // if exc contains data key "html" then Append it to the test container
            //if (exc.Data.Contains("html"))
            //{
            //    tc.Append((string)exc.Data["html"]);
            //}
        }
    }

    public void Assert(bool condition, string message = null)
    {
        if (!condition)
            throw new Exception(message ?? "Assert Failure");
    }

    public virtual JQueryPlainObject StubHtmlIntoTestContainer(int numberOfElements = 1)
    {
        // for each element to insert, increment the class name by one letter, starting at a
        string html = "";
        for (int i = 0; i < numberOfElements; ++i)
        {
            html += $"<div class='{(char)((int)'a' + i)}'></div>";
        }
        tc.Append(html);
        return tc.Children();
    }
}


public class ThrowingTraceListener : TraceListener
{
    public override void Write(string msg)
    {
        Console.WriteLine(msg);
        throw new Exception(msg);
    }
    public override void WriteLine(string msg)
    {
        Console.WriteLine(msg);
        throw new Exception(msg);
    }
}
