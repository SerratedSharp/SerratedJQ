//using SerratedSharp.SerratedJQ;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Wasm;

//namespace Tests.Wasm
//{

//    public class ObjectProperties_Length : JQTest
//    {
//        public override void Run()
//        {
//            //tc.Append(JQuery.ParseHtml("<div class='w'></div><div class='x'></div><div class='y'></div><div class='y'></div><div class='z'></div>"));

//            tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='y'></div><div class='z'></div>");
//            var ys = tc.Find(".y");
//            var length = ys.Length;
//            Console.WriteLine($"Length in test: {length}");


//            Assert(length == 2);
//        }
//    }

//    public class ObjectProperties_Version : JQTest
//    {
//        public override void Run()
//        {
//            Assert(tc.JQueryVersion.StartsWith("3."));
//        }
//    }


//}
