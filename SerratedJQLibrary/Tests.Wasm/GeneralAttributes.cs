using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Linq;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{

    // General Attributes - https://api.jquery.com/category/attributes/general-attributes/

    public class GeneralAttributes_Attr_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Attr("id", "GeneralAttributes_Attr_Get");
            Assert(result.Attr("id") == "GeneralAttributes_Attr_Get");
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_Attr_Number : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Attr("id", 123);
            Assert(result.Attr("id") == "123");
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_Attr_Number2 : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Attr("id", 124d);
            Assert(result.Attr("id") == "124");
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_RemoveAttr : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Attr("id", "GeneralAttributes_Attr_Get");
            Assert(result.Attr("id") == "GeneralAttributes_Attr_Get");
            result.RemoveAttr("id");
            Assert(result.Attr("id") == null);
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_Val_String : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<input></input>");
            result.Val("GeneralAttributes_Val_Get");
            Assert(result.Val() == "GeneralAttributes_Val_Get");
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_Val_Number : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<input></input>");
            result.Val(123);
            Assert(result.Val() == "123");
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_Val_Number2 : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<input></input>");
            result.Val(124d);
            Assert(result.Val() == "124");
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_Val_StringArray : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<select multiple></select>").Children();
            result.Append("<option value='GeneralAttributes_Val_Get1'>GeneralAttributes_Val_Get1</option>");
            result.Append("<option value='GeneralAttributes_Val_Get2'>GeneralAttributes_Val_Get2</option>");
            result.Append("<option value='GeneralAttributes_Val_Get3'>GeneralAttributes_Val_Get3</option>");
            result.Val(new string[] { "GeneralAttributes_Val_Get1", "GeneralAttributes_Val_Get3" });
            //Console.WriteLine("List" + result.Val<string[]>());

            // TODO: Need to work on returning array of strings
            Assert(result.Val<string[]>().Contains("GeneralAttributes_Val_Get1"));

        }
    }

    public class GeneralAttributes_Prop_String : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<input></input>");
            result.Prop("value", "GeneralAttributes_Prop_Get");
            Assert(result.Prop<string>("value") == "GeneralAttributes_Prop_Get");
            Assert(result.Length == 1);
        }
    }

    //public class GeneralAttributes_Prop_Number : JQTest
    //{
    //    public override void Run()
    //    {
    //        result = tc.Append("<input></input>");
    //        result.Prop("value", 123);
    //        Assert(result.Prop<int>("value") == 123);
    //        Assert(result.Length == 1);
    //    }
    //}

    public class GeneralAttributes_Prop_Number2 : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<input></input>");
            result.Prop("value", 124d);
            Assert(result.Prop<double>("value") == 124d);
            Assert(result.Length == 1);
        }
    }

    public class GeneralAttributes_RemoveProp : JQTest
    {
        public override void Run()
        {
            result = tc.Append("<input></input>");
            result.Prop("value", "GeneralAttributes_Prop_Get");
            Assert(result.Prop<string>("value") == "GeneralAttributes_Prop_Get");
            result.RemoveProp("value");
            Assert(result.Prop<string>("value") == null);
            Assert(result.Length == 1);
        }
    }

}
