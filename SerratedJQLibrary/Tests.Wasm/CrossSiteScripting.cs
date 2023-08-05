using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wasm;
using Windows.UI.Xaml.Shapes;

namespace Tests.Wasm
{

    public class Xss_ScriptTag : JQTest
    {
        public override void Run()
        {
            var xssAttempt = JQueryBox.ParseHtml("<div class='w'></div><script>alert('Script run')</script>");
            tc.Append(xssAttempt);

            var child = tc.Find(".w");
        }
    }

    public class Xss_Select_PayloadFile : JQTest
    {
        public override void Run()
        {
            var xssPayloads = Tests.Wasm.EmbeddedTestFiles.XssPayloads1.Split(Environment.NewLine).ToList();

            foreach (var xssPayload in xssPayloads) {
                var xssAttempt = JQueryBox.Select(xssPayload);
                tc.Append(xssAttempt);
            }

            var child = tc.Find(".w");
        }

    }


    public class Xss_Select_DefeatSingleQuotes : JQTest
    {
        public Xss_Select_DefeatSingleQuotes() { }

        public override void Run()
        {
            var xssPayloads = new List<string>();

            //https://cheatsheetseries.owasp.org/cheatsheets/XSS_Filter_Evasion_Cheat_Sheet.html#escaping-javascript-escapes
            xssPayloads.Add(@"\';alert(\'XSS\');//");
            xssPayloads.Add(@"\';alert(""XSS"");//");
            xssPayloads.Add(@"\"";alert('XSS');//");
            xssPayloads.Add(@"');alert(""XSS""");
            xssPayloads.Add(@"\');alert(""XSS""");
            xssPayloads.Add(@"');alert(""XSS"");alert('test");

            foreach (var xssPayload in xssPayloads)
            {
                var xssAttempt = JQueryBox.Select(xssPayload);
                tc.Append(xssAttempt);
            }

            var child = tc.Find(".w");
        }

    }





}
