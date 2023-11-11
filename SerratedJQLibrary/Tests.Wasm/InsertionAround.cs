using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{

    public class InsertionAround_Unwrap : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            stubs.Wrap("<div class='wrap1'></div>").Unwrap();
            result = tc.Children();
            Assert(result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class InsertionAround_Wrap : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            stubs.Wrap("<div class='wrap1'></div>");
            result = tc.Children();
            Assert(result.HasClass("wrap1"));
            Assert(result.Length == 1);
        }
    }

    public class InsertionAround_Wrap_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            stubs.Wrap(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 1);
        }
    }

    public class InsertionAround_WrapAll : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            stubs.WrapAll("<div class='wrap1'></div>");
            result = tc.Children();
            Assert(result.HasClass("wrap1"));
            Assert(result.Length == 1);
        }
    }

    public class InsertionAround_WrapAll_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            stubs.WrapAll(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 1);
        }
    }

    public class InsertionAround_WrapInner : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            stubs.WrapInner("<div class='wrap1'></div>");
            result = tc.Children();
            var inners = result.Children();
            Assert(inners.HasClass("wrap1"));
            Assert(inners.Length == 5);
            Assert(result.HasClass("a"));
            Assert(result.Length == 5);
        }
    }

    //public class InsertionAround_WrapInner_Object : JQTest
    //{
    //    public override void Run()
    //    {
    //        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
    //        stubs.WrapInner( JQuery.ParseHtml("<div class='wrap1'></div>") );
    //        result = tc.Children();
    //        var inners = result.Children();
    //        Assert(inners.HasClass("wrap1"));
    //        Assert(inners.Length == 5);
    //        Assert(result.HasClass("a"));
    //        Assert(result.Length == 5);
    //    }
    //}

}
