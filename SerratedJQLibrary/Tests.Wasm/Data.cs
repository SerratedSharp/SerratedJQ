using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    internal class TestModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    public class Data_Data_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            stubs.Data("one", "uno");
            
            string val = tc.Find(".a").Data<string>("one");

            Assert(val == "uno");
        }
    }

    public class Data_Data_JSObject : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            stubs.Data("one", "uno");

            JSObject obj = tc.Find(".a").DataAsJSObject();
            GlobalJS.Console.Log("Data() test: ", obj);
            string val = obj.GetPropertyAsString("one");

            Assert(val == "uno");
        }
    }

    //public class Data_Data_Json : JQTest
    //{
    //    public override void Run()
    //    {
    //        JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
    //        //var model = """
    //        //{ UserId = 1, Name = "uno" }
    //        //""";
    //        // TODO: create interop call that takes json string, JSON.parse into js object, returns JSObject that can be passed to data

    //        var model = new JSObject();
    //        System.Runtime.InteropServices.JavaScript.JSHost.GlobalThis.
    //        model.UserId = 1;
    //        model.Name = "uno";

    //        stubs.Data("one", model);
    //        dynamic val = tc.Find(".a").Data<dynamic>("one");

    //       // Assert(val.Name == "uno");
    //       // Assert(val.UserId == 1);
    //    }
    //}

    public class Data_Data_ManagedObject : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            var model = new TestModel { UserId = 1, Name = "uno" };
            
            stubs.Data("one", model);
            TestModel val = tc.Find(".a").Data<TestModel>("one");
        
            Assert(val.Name == "uno");
            Assert(val.UserId == 1);
        }
    }

    public class Data_Data_MemoryLoadTest : JQTest
    {
        public override void Run()
        {
            StoreData();

            foreach (var i in Enumerable.Range(0, 100000))
            {
                List<int> test = new List<int>(Enumerable.Range(0, 10));
                JQueryPlain.Select(".a");
            }
            GC.Collect();
            TestModel val = tc.Find(".a").Data<TestModel>("one");
            
            Assert(val.Name == "uno");
            Assert(val.UserId == 1);
        }

        private void StoreData()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            var model = new TestModel { UserId = 1, Name = "uno" };
            stubs.Data("one", model);
        }
    }

}
