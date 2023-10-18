using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using SerratedSharp.JSInteropHelpers;

using SerratedSharp.SerratedJQ.Plain;
using Uno.Foundation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wasm;

public static class Logger
{
    public static void Log(object obj)
    {    

        foreach (var property in obj.GetType().GetProperties())
            Console.WriteLine(property.Name + ": " + property.GetValue(obj, null).ToString());
    }
}



public static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Beginning test...");

#if DEBUG
        Console.WriteLine("DEBUG DEFINED");
#endif


        
        //CallbacksHelper.Export("blah", () => Console.WriteLine("asdas"));
        //CallbacksHelper.Export("blah", () => Console.WriteLine("override"), true);
        //CallbacksHelper.Export("blah2", () => Console.WriteLine("second"));
        //CallbacksHelper.Export("Blah", () => Console.WriteLine("Upper"));

        Trace.Listeners.Add(new ThrowingTraceListener());
        //WebAssemblyRuntime.InvokeJS(@"
        //    var isReady = false;
        //    var script = document.createElement('script');
        //    script.onload = function () {                    
        //        var beginTests = Module.mono_bind_static_method('[Tests.Wasm] Wasm.Program:Begin');
        //        beginTests();
        //    };
        //    script.src = 'https://code.jquery.com/jquery-3.6.0.js';
        //    document.head.appendChild(script);
        //");

        // Loads JQuery 
        WebAssemblyRuntime.InvokeJS(Tests.Wasm.EmbeddedTestFiles.TestWasm);

        await RequireAndAwaitDependencyLoadingMultiline();
        await Begin();


        if (new Random().Next(2) > 3 ) { // force compile not to optimize away this method
            Begin(); // Never called here, will be called from JS above
        }
    }

    //public static async System.Threading.Tasks.Task MainAsync()
    //{

        
    //    //var innerHandler = new Uno.UI.Wasm.WasmHttpHandler();
    //    //var client = new HttpClient(innerHandler);
    //    //var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, new Uri("https://localhost:5001/")));
    //    //var content = await response.Content.ReadAsStringAsync();
        
    //    //Console.WriteLine(content);
    //}

    public static async Task<string> RequireAndAwaitDependencyLoadingMultiline()
    {
        // Calling InvokeAsync with mult-line statements requires enclosing braces { } and a `return` statement.            
        //await WebAssemblyRuntime.InvokeAsync("""  
        //    {
        //        requirejs.config({
        //        paths: {
        //            jquery: 'jquery-3.6.0'
        //            }
        //        });
        //        return new Promise((resolve, reject) => {

        //            //require(["jquery"], (jquery) => {
        //            //    console.log("loading jquery");
        //            //    globalThis.jQuery = jquery
        //            //    resolve();// allow caller to await resolution of dependency
        //            //});
        //            define(["jquery"], function(jquery) {

        //                console.log("loading jquery");
        //                //console.log( jquery.browser );

        //                globalThis.jQuery = jquery
        //                resolve();// allow caller to await resolution of dependency
        //            });
        //        });
        //    }
        //""");



        return await WebAssemblyRuntime.InvokeAsync("""
                globalThis.loadjQuery();
            """);
    }


    public static async Task Begin()
    {

        //var timer = new System.Threading.Timer(async (e) =>
        //{
        //    await MainAsync();
        //    Console.WriteLine("Tick");

        //}, null, 5000, 15000);

        //  WebAssemblyRuntime.InvokeAsync("globalThis.SerratedExports = await Module.getAssemblyExports(\"SerratedSharp.SerratedJQ\")");



        SerratedSharp.SerratedJQ.JSDeclarations.LoadScripts();




        Console.WriteLine("After sleep test...");
        //JQuery.Select("html").Styles["position"] = "static";
        //JQuery.Select("html").Styles["overflow"] = "auto";
        //JQuery.Select("body").Styles["position"] = "static";
        //JQuery.Select("body").Styles["overflow"] = "auto";

        //WebAssemblyRuntime.InvokeJS("Serrated.JQueryProxy.Select('div')");


        JQueryPlainObject jqObject = JQueryPlain.Select("body");
        Console.WriteLine("HTML Test: " + jqObject.Html());
        JQueryPlainObject children = jqObject.Find("*");


        Console.WriteLine("***** Testing Properties ******");
        //var body = JQueryProxy.Select("body");
        //var items = JQueryProxy.FuncByNameToObject(body, "find", new string[] { "*" });
       // Console.WriteLine(children.Html);
        children.Html("<div id='x'>Test</div>");
       // Console.WriteLine(children.Html);

        //jqObject.After("<div>After</div>");
        //jqObject.After("<div>After1</div>", "<div>After2</div>", "<div>After3</div>");

        //jqObject.Find("div").InnerOn("click");
        var divs = jqObject.Find("#x");
        JQueryPlainObject.JQueryEventHandler<JQueryPlainObject, dynamic> clickListener = (sender, e) =>
        {
            Console.WriteLine("Clicked");
            Console.WriteLine(e.target);
            Console.WriteLine(e.currentTarget);
            Console.WriteLine(e);
        };
        
        
        JQueryPlainObject.JQueryEventHandler<JQueryPlainObject, dynamic> click2 = (sender, e) => Console.WriteLine("Clicked 2");
        JQueryPlainObject.JQueryEventHandler<JQueryPlainObject, dynamic> click3 = (sender, e) => Console.WriteLine("Clicked 3");


        divs.OnClick += clickListener;
        divs.OnClick += click2;
        divs.OnClick -= click2;
        //divs.OnClick += click3;
        //divs.OnClick -= clickListener;
        //            divs.OnClick -= click3;
        //divs.On("click", clickListener);
        //divs.On("click", click2);
        //divs.Off("click", click2);
        //divs.On("click", click3);

        // basic bind/unbind example
        //var divs = jqObject.Find("div");
        //divs.OnClick += (sender, e) => Console.WriteLine("Clicked" + e);
        //var handler = (string eventEncoded, string eventType) =>
        //{
        //    Console.WriteLine(eventEncoded);
        //    //eventCollection?.Invoke(this, eventData);
        //};
        //var handler2 = JQueryProxy.BindListener(divs.JSObject, "click", handler);
        //JQueryProxy.UnbindListener(divs.JSObject, "click", handler2);

        ////AsyncContext.Run(() => MainAsync(args));

        //// }, null, 5000, 5000);

        var orch = new TestOrchestrator();
        // TODO: I disabled the linker. Isntead disable a namespace just for tests: https://platform.uno/docs/articles/features/using-il-linker-webassembly.html
        var types = System.Reflection.Assembly.GetAssembly(typeof(TestOrchestrator)).GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(JQTest)))
            .ToList();
        Console.WriteLine("Types found: " + types.Count);

        foreach (Type type in types)
        {
            Console.WriteLine("Class" + type.Name);
            orch.Tests.Add((JQTest)Activator.CreateInstance(type));
        }

        orch.Run();

        ////var test = new Traversal_TreeTraversal_Children1();
        ////test.Test1();

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
                //var tc = 
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

    bool IsModelTest { get; set; }

    void BeginTest(out JQueryPlainObject status);
    void EndTest(JQueryPlainObject status, Exception exc);
    void Run();
}


public abstract class JQTest : IJQTest
{
    JQueryPlainObject body = JQueryPlain.Select("body");
    public int TestNum { get; set; }
    public bool IsModelTest { get; set; } = false;
    Exception exc = null; JQueryPlainObject status = null;
    protected JQueryPlainObject tc;// test container
    protected JQueryPlainObject result;
    //protected JQueryBoxV2<TestModel> tcm;// test container with model



    public virtual void Run()
    {
    }

    public void BeginTest(out JQueryPlainObject status)
    {
        string htmlTemplate = $"<div id='t{TestNum}'><div class='status'>T{TestNum}:</div><div class='tc'></div></div>";
        //var div = JQuery.ParseHtml(htmlTemplate);
        //body.Append(div);



        body.Append(htmlTemplate);
        status = JQueryPlain.Select($"#t{TestNum} .status");
        //div.Find(".status");

        // tc or tcm will hold reference to test container HTML that the test implementation should use as a sandbox for the text
        // **Nevermind: Seems like it's better for the test case to add it's on JqueryBox<SomeModel> inside the test container.
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
            //status.Append(JQuery.ParseHtml($"<span style='color:green'>Valid - {this.GetType().Name}</span>"));
            status.Append($"<span style='color:green'>Valid - {testName}</span>");
        }
        else
        {
            Console.WriteLine(exc);
            //status.Append(JQuery.ParseHtml($"<span style='color:red'>Failed - {this.GetType().Name}: {exc.ToString()}</span>"));
            status.Append($"<span style='color:red'>Failed - <b>{testName}</b>: {exc.ToString()}</span>");

            status.Append("<div class='excontext'></div>");
            // TODO: Implement OuterHtml and change to use that
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
            throw new Exception(message);
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


    //public void Test2()
    //{
    //    var div = JQuery.ParseHtml("<div id='t2'>T2</div>");
    //    body.PrependTo(div);

    //    // Verify
    //    var v = body.Find("#t1");
    //    Debug.Assert(v.Length() == 1);
    //    Debug.Assert(v.Text() == "T1");

    //    v.Append(":Valid");

    //}



}


public class ThrowingTraceListener : TraceListener
{
    public override void Write(string msg)
    {
        throw new Exception(msg);
    }
    public override void WriteLine(string msg)
    {
        throw new Exception(msg);
    }
}
