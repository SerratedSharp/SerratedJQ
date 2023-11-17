using SerratedSharp.SerratedJQ.Plain;
using Wasm;



namespace Tests.Wasm;

public partial class TestsContainer
{
    // Tests for statis JQuery methods mapped from the global JQuery object
    public class GlobalJQuery_Select : JQTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer();
            JQueryPlainObject jq = JQueryPlain.Select($"#t{base.TestNum}");
            Assert(jq.Length == 1);
        }
    }

    public class GlobalJQuery_ParseHtml : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject jq = JQueryPlain.ParseHtmlAsJQuery("<div class='w'></div>");
            tc.Append(jq);
            Assert(tc.Find(".w").Length == 1);
        }
    }
}

