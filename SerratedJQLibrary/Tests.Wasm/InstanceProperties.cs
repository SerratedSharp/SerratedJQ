using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class InstanceProperties_Length : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();
            Assert(result.Length == 3);
        }
    }

    public class InstanceProperties_JQueryVersion : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();
            Assert(result.JQueryVersion == "3.7.1");
        }
    }

}