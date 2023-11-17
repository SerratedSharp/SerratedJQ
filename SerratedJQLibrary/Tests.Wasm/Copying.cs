using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;
//  Copying - https://api.jquery.com/category/manipulation/copying/


public partial class TestsContainer
{

    public class Copying_Clone : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            result = tc.Children().Clone();
            Assert(result.HasClass("a"));
            Assert(result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

}