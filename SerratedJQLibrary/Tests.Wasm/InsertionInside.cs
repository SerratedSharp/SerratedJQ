using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;
public partial class TestsContainer
{
    public class InsertionInside_Append : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.Append("<div class='x'></div>");
            result = tc.Children();
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_Append_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.Append(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_Append_HtmlElement: JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            HtmlElement htmlElement = JQueryPlain.ParseHtml("<div class='x'></div>");
            tc.Append(htmlElement);
            result = tc.Children();
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_AppendTo : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.AddClass("InsertionInside_AppendTo");
            JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>").AppendTo(".InsertionInside_AppendTo");
            result = tc.Children();
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_AppendTo_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>").AppendTo(tc);
            result = tc.Children();
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_Prepend : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.Prepend("<div class='x'></div>");
            result = tc.Children();
            Assert(result.HasClass("x") && result.HasClass("a"));
            Assert(result.Length == 2);
        }
    }

    // TODO: Fix .Prepend taking an object.  Possibly an issue with ParseHtml wrapping
    public class InsertionInside_Prepend_HtmlObject : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.Prepend(JQueryPlain.ParseHtml("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("x") && result.HasClass("a"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_Prepend_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.Prepend(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("x") && result.HasClass("a"));
            Assert(result.Length == 2);
        }
    }


    public class InsertionInside_PrependTo : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            tc.AddClass("InsertionInside_PrependTo");
            JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>").PrependTo(".InsertionInside_PrependTo");
            result = tc.Children();
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class InsertionInside_PrependTo_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            tc.Append("<div class='InsertionInside_PrependTo_Object'></div>");
            JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>").PrependTo(tc.Find(".InsertionInside_PrependTo_Object"));
            result = tc.Find(".InsertionInside_PrependTo_Object").Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 1);
        }
    }

    public class InsertionInside_PrependTo_Object2 : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            JQueryPlainObject newObj = JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>");
            newObj.PrependTo(tc);
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 4);
        }
    }

}