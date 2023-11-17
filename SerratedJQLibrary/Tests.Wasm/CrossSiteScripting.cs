using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{

    public class Xss_ScriptTag : JQTest
    {
        public override void Run()
        {
            var xssAttempt = JQueryPlain.ParseHtml("<div class='w'></div><script>alert('Script run')</script>");
            tc.Append(xssAttempt);

            var child = tc.Find(".w");
        }
    }

    //public class Xss_Select_PayloadFile : JQTest
    //{
    //    public override void Run()
    //    {
    //        var xssPayloads = Tests.Wasm.EmbeddedTestFiles.XssPayloads1.Split(Environment.NewLine).ToList();

    //        foreach (var xssPayload in xssPayloads) {
    //            var xssAttempt = JQueryBox.Select(xssPayload);
    //            tc.Append(xssAttempt);
    //        }

    //        var child = tc.Find(".w");
    //    }

    //}

    // TODO: Review
    //public class Xss_Select_DefeatSingleQuotes : JQTest
    //{
    //    public Xss_Select_DefeatSingleQuotes() { }

    //    public override void Run()
    //    {
    //        var xssPayloads = new List<string>();

    //        //https://cheatsheetseries.owasp.org/cheatsheets/XSS_Filter_Evasion_Cheat_Sheet.html#escaping-javascript-escapes

    //        xssPayloads.Add(@"\';alert(\'XSS\');//");
    //        xssPayloads.Add(@"\';alert(""XSS"");//");
    //        xssPayloads.Add(@"\"";alert('XSS');//");
    //        xssPayloads.Add(@"');alert(""XSS""");
    //        xssPayloads.Add(@"\');alert(""XSS""");
    //        xssPayloads.Add(@"');alert(""XSS"");alert('test");

    //        foreach (var xssPayload in xssPayloads)
    //        {
    //            var xssAttempt = AssertTest.AssertException<JQueryBox>(delegate ()
    //            {
    //                return JQueryBox.Select(xssPayload);// TODO: handle, we expect exceptions here because the payload is not a valid selector, AssertException needs to communicate to test running we got expected result

    //            });

    //            if (xssAttempt != null)
    //            {
    //                tc.Append(xssAttempt);
    //            }
    //            else
    //            {
    //                tc.Text = "Expected Failure";
    //            }
    //        }

    //        var child = tc.Find(".w");
    //    }
    //}

    //public class AssertTest
    //{

    //    public delegate T Func<T>();

    //    public static T AssertException<T>(Func<T> retryThis)
    //    {

    //        bool failed = false;

    //        try
    //        {
    //            return retryThis();
    //        }
    //        catch (Exception ex)
    //        {
    //            failed = true;
    //            return default(T);
    //        }

    //    }

    //}





}
