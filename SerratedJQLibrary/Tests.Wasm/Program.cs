using System;
using System.Collections.Generic;
//using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using SerratedSharp.SerratedJQ;
using Uno.Foundation;


namespace Wasm
{
    public static class Program
    {
        static void Main(string[] args)
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
            WebAssemblyRuntime.InvokeJS(@"
                var isReady = false;
                var script = document.createElement('script');
                script.onload = function () {                    
                    var beginTests = Module.mono_bind_static_method('[Tests.Wasm] Wasm.Program:Begin');
                    beginTests();
                };
                script.src = 'https://code.jquery.com/jquery-3.6.0.js';
                document.head.appendChild(script);
            ");

            if(new Random().Next(2) > 3 ) { // force compile not to optimize away this method
                Begin(); // Never called here, will be called from JS above
            }
        }

        public static async System.Threading.Tasks.Task MainAsync()
        {

            
            //var innerHandler = new Uno.UI.Wasm.WasmHttpHandler();
            //var client = new HttpClient(innerHandler);
            //var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, new Uri("https://localhost:5001/")));
            //var content = await response.Content.ReadAsStringAsync();
            
            //Console.WriteLine(content);
        }


        public static void Begin()
        {

            var timer = new System.Threading.Timer(async (e) =>
            {
                await MainAsync();
                Console.WriteLine("Tick");
                
            }, null, 5000, 15000);


            JSDeclarations.LoadScripts();

            Console.WriteLine("After sleep test...");
            //JQueryBox.Select("html").Styles["position"] = "static";
            //JQueryBox.Select("html").Styles["overflow"] = "auto";
            //JQueryBox.Select("body").Styles["position"] = "static";
            //JQueryBox.Select("body").Styles["overflow"] = "auto";

            //WebAssemblyRuntime.InvokeJS("Serrated.JQueryProxy.Select('div')");

            JQueryObject jqObject = JQuery.Select("body");
            JQueryObject children = jqObject.Find("*");


            Console.WriteLine("***** Testing Properties ******");
            //var body = JQueryProxy.Select("body");
            //var items = JQueryProxy.FuncByNameToObject(body, "find", new string[] { "*" });
            Console.WriteLine(children.Html);
            children.Html = "<div>Test</div>";
            Console.WriteLine(children.Html);

            jqObject.After("<div>After</div>");
            jqObject.After("<div>After1</div>", "<div>After2</div>", "<div>After3</div>");

            ////AsyncContext.Run(() => MainAsync(args));

            //// }, null, 5000, 5000);

            //var orch = new TestOrchestrator();
            //// TODO: I disabled the linker. Isntead disable a namespace jsut for tests: https://platform.uno/docs/articles/features/using-il-linker-webassembly.html
            //var types = System.Reflection.Assembly.GetAssembly(typeof(TestOrchestrator)).GetTypes()
            //    .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(JQTest)))
            //    .ToList();
            //Console.WriteLine("Types found: " + types.Count);

            //foreach (Type type in types)
            //{
            //    Console.WriteLine("Class");
            //    orch.Tests.Add((JQTest)Activator.CreateInstance(type));
            //}

            //orch.Run();

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
                Exception exc = null; JQueryBox status = null;
                try
                {
                    test.TestNum = i;
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

        void BeginTest(out JQueryBox status);
        void EndTest(JQueryBox status, Exception exc);
        void Run();
    }


    public abstract class JQTest : IJQTest
    {
        JQueryBox body = JQueryBox.Select("body");
        public int TestNum { get; set; }
        public bool IsModelTest { get; set; } = false;
        Exception exc = null; JQueryBox status = null;
        protected JQueryBox tc;// test container
        //protected JQueryBoxV2<TestModel> tcm;// test container with model



        public virtual void Run()
        {
        }

        public void BeginTest(out JQueryBox status)
        {
            string htmlTemplate = $"<div id='t{TestNum}'><div class='status'>T{TestNum}:</div><div class='tc'></div></div>";
            var div = JQueryBox.FromHtml(htmlTemplate);

      
            body.Append(div);
            status = div.Find(".status");

            // tc or tcm will hold reference to test container HTML that the test implementation should use as a sandbox for the text
            // **Nevermind: Seems like it's better for the test case to add it's on JqueryBox<SomeModel> inside the test container.
            //if (IsModelTest)
            //    tcm = div.Find<TestModel>(".tc");
            //else
                tc = div.Find(".tc");


        }

        public void EndTest(JQueryBox status, Exception exc)
        {
            if (exc == null)
            {
                status.Append(JQueryBox.FromHtml($"<span style='color:green'>Valid - {this.GetType().Name}</span>"));
            }
            else
            {
                Console.WriteLine(exc);
                status.Append(JQueryBox.FromHtml($"<span style='color:red'>Failed - {this.GetType().Name}: {exc.ToString()}</span>"));
            }
        }

        public void Assert(bool condition, string message = null)
        {
            if (!condition)
                throw new Exception(message);
        }

        //public void Test2()
        //{
        //    var div = JQueryBox.FromHtml("<div id='t2'>T2</div>");
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




}
