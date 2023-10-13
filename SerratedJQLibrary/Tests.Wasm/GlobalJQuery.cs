using SerratedSharp.SerratedJQ;
using Wasm;



namespace Tests.Wasm
{
    // Tests for statis JQuery methods mapped from the global JQuery object
    public class GlobalJQuery_Select : JQTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer();
            JQueryObject jq = JQuery.Select($"#t{base.TestNum}");
            Assert(jq.Length == 1);
        }
    }

    public class GlobalJQuery_ParseHtml: JQTest
    {
        public override void Run()
        {
            JQueryObject jq = JQuery.ParseHtml("<div class='w'></div>");
            tc.Append(jq);
            Assert(tc.Find(".w").Length == 1);
        }
    }



}

