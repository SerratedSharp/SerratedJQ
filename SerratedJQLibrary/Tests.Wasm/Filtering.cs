using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{

    public class Filtering_First : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.First();
            Assert(result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class Filtering_Last : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Last();
            Assert(result.HasClass("e"));
            Assert(result.Length == 1);
        }
    }

    public class Filtering_Even : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Even();
            Assert(result.Length == 3);
            Assert(result.HasClass("a"));
            Assert(result.HasClass("c"));
            Assert(result.HasClass("e"));
        }
    }

    public class Filtering_Odd : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Odd();
            Assert(result.Length == 2);
            Assert(result.HasClass("b"));
            Assert(result.HasClass("d"));
        }
    }

    public class Filtering_Eq : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Eq(2);
            Assert(result.Length == 1);
            Assert(result.HasClass("c"));
        }
    }

    public class Filtering_Filter : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".c");
            Assert(result.Length == 1);
            Assert(result.HasClass("c"));
        }
    }

    public class Filtering_Not : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Not(".c");
            Assert(result.Length == 4);
            Assert(result.HasClass("a"));
            Assert(result.HasClass("b"));
            Assert(result.HasClass("d"));
            Assert(result.HasClass("e"));
        }
    }

    public class Filtering_Slice : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Slice(1, 3);
            Assert(result.Length == 2);
            Assert(result.HasClass("b"));
            Assert(result.HasClass("c"));
        }
    }

    public class Filtering_Is : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            Assert(stubs.Is(".c"));
        }
    }

    public class Filtering_Has : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            tc.Find(".c").Append("<div class='x'></div>");
            result = stubs.Has(".x");
            Assert(result.Length == 1);
        }
    }

}
