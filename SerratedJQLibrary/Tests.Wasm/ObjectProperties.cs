using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wasm;

namespace Tests.Wasm
{

    public class ObjectProperties_Length : JQTest
    {
        public override void Run()
        {
            tc.Append(JQueryBox.FromHtml("<div class='w'></div><div class='x'></div><div class='y'></div><div class='y'></div><div class='z'></div>"));

            Assert(tc.Find(".y").Length == 2);
        }
    }

    public class ObjectProperties_Version : JQTest
    {
        public override void Run()
        {
            Assert(tc.Version.StartsWith("3."));
        }
    }


}
