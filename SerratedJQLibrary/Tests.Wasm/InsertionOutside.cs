using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Diagnostics.CodeAnalysis;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    // DOM Insertion, Outside - https://api.jquery.com/category/manipulation/dom-insertion-outside/
    public class InsertionOutside_InsertAfter : JQTest
    {

        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);// a,b,c
            stubs.Filter(".a").InsertAfter(".c");
            result = tc.Children().Last();
            Assert(result.HasClass("a"));
            Assert(tc.Children().Length == 3);
        }
    }


    public class InsertionOutside_InsertAfter_Object : JQTest
    {

        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);// a,b,c
            stubs.Filter(".a").InsertAfter(stubs.Filter(".c"));
            result = tc.Children().Last();
            Assert(result.HasClass("a"));
            Assert(tc.Children().Length == 3);
        }
    }

    public class InsertionOutside_InsertBefore : JQTest
    {

        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);// a,b,c
            stubs.Filter(".b").InsertBefore(".a");
            result = tc.Children().First();
            Assert(result.HasClass("b"));
            Assert(tc.Children().Length == 3);
        }
    }

    public class InsertionOutside_InsertBefore_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);// a,b,c
            stubs.Filter(".b").InsertBefore(stubs.Filter(".a"));
            result = tc.Children().First();
            Assert(result.HasClass("b"));
            Assert(tc.Children().Length == 3);
        }
    }

    public class InsertionOutside_After : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            stubs.Filter(".a").After("<div class='x'></div>");
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 6);
        }
    }

    public class InsertionOutside_After_Strings2 : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            stubs.Filter(".a").After("<div class='x'></div>", "<div class='y'></div>");
            result = tc.Children();
            Assert(result.HasClass("x") && result.HasClass("y"));
            Assert(result.Length == 7);
        }
    }

    public class InsertionOutside_After_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            stubs.Filter(".a").After(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 6);
        }
    }

    public class InsertionOutside_Before : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            stubs.Filter(".a").Before("<div class='x'></div>");
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 6);
        }
    }

    public class InsertionOutside_Before_TwoString : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            stubs.Filter(".a").Before("<div class='x'></div>", "<div class='y'></div>");
            result = tc.Children();
            Assert(result.HasClass("x") && result.HasClass("y"));
            Assert(result.Length == 7);
        }
    }

    public class InsertionOutside_Before_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            stubs.Filter(".a").Before(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children();
            Assert(result.HasClass("x"));
            Assert(result.Length == 6);
        }
    }

}
