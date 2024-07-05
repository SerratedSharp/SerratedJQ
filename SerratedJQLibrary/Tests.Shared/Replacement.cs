using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

// DOM Replacement - https://api.jquery.com/category/manipulation/dom-replacement/
public partial class TestsContainer
{
    public class Replacement_ReplaceWith : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            tc.Children().ReplaceWith("<div class='x'></div>");
            result = tc.Children(".x");
            Assert(result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }

    public class Replacement_ReplaceWith_Object : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            tc.Children().ReplaceWith(JQueryPlain.ParseHtmlAsJQuery("<div class='x'></div>"));
            result = tc.Children(".x");
            Assert(result.HasClass("x"));
            Assert(result.Length == 2);
        }
    }


}

