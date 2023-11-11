using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

// DOM Removal - https://api.jquery.com/category/manipulation/dom-removal/
public partial class TestsContainer
{
    public class Removal_Detach : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            result = tc.Children();
            JQueryPlainObject obj = result.Detach(".a");
            Assert(obj.HasClass("a"));
            Assert(result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

    public class Removal_Empty : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            tc.Empty();
            result = tc.Children();
            Assert(result.Length == 0);
        }
    }

    public class Removal_Remove : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            tc.Children().Remove(".a");
            result = tc.Children();
            Assert(result.HasClass("b"));
            Assert(result.Length == 1);
        }
    }
}

