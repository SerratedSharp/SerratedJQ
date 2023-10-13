﻿using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wasm;



namespace Tests.Wasm
{

    public class ElementManipulation_AddClass_HasClass : JQTest
    {
        public override void Run()
        {
            Console.WriteLine("Before...");
            //tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='z'></div>");
            var blh = JQuery.ParseHtml("<div class='w'></div>");

            //Console.WriteLine("blh.HTML: " + blh.Length + "dfgd " + blh.Html) ;
            //tc.Append(JQuery.ParseHtml("<div class='w'>"));
            //tc.Append(JQuery.ParseHtml("<div class='w'></div><div class='x'></div><div class='y'></div><div class='z'></div>"));
            tc.Append(blh);

            var t = tc.Find(".w");
            Console.WriteLine("blh.HTML: " + tc.Html);
            Console.WriteLine("After...");
            t.AddClass("a");
            //bool hasClass = t.HasClass("a");
            //bool hasClass2 = t.HasClass("w");
            //Logger.Log(new { hasClass, hasClass2 });

            Assert(t.HasClass("a") && t.HasClass("w"));
        }
    }


    public class MoveElsewhere_Append_TwoJQ : JQTest
    {
        public override void Run()
        {
            JQueryObject jq = JQuery.ParseHtml("<div class='w'></div>");
            JQueryObject jq2 = JQuery.ParseHtml("<div class='w'></div>");
            tc.Append(jq, jq2);
            Assert(tc.Find(".w").Length == 2);
        }
    }

    public class MoveElsewhere_Append_ThreeJQ : JQTest
    {
        public override void Run()
        {
            JQueryObject jq = JQuery.ParseHtml("<div class='w'></div>");
            JQueryObject jq2 = JQuery.ParseHtml("<div class='w'></div>");
            JQueryObject jq3 = JQuery.ParseHtml("<div class='w'></div>");
            tc.Append(jq, jq2, jq3);
            Assert(tc.Find(".w").Length == 3);
        }
    }



    //public class ElementManipulation_Attributes1 : JQTest
    //{
    //    public override void Run()
    //    {
    //        tc.Append("<div class='w'></div><div class='x'></div><div class='y'></div><div class='z'></div>");
    //        var t = tc.Find(".w");
    //        //tc.Append(JQueryBox.FromHtml( $"<div>Class: {t.Attributes["class"]}</div>"));
    //        //Assert(t.Attributes["class"] == "w");
    //    }
    //}

    //    public class ElementManipulation_Attributes2 : JQTest
    //    {
    //        public override void Run()
    //        {
    //            tc.Append("<div  class='w'></div><div class='x'></div><div class='y'></div><div class='z'></div>"));
    //            var t = tc.Find(".w");
    //            t.Attributes["class"] = "q";
    //            Assert(tc.Find(".q").Length == 1);
    //            Assert(tc.Find(".w").Length == 0);
    //            //tc.Append(JQueryBox.FromHtml( $"<div>Class: {t.Attributes["class"]}</div>"));
    //            Assert(t.Attributes["class"] == "q");
    //        }
    //    }

    //    public class ElementManipulation_Styles1 : JQTest
    //    {
    //        public override void Run()
    //        {
    //            var child = JQueryBox.FromHtml("<div class='w' style='color:blue'>X</div>");
    //            tc.Append(child);
    //            Assert(child.Styles["color"] == "rgb(0, 0, 255)");
    //        }
    //    }

    //    public class ElementManipulation_Styles2 : JQTest
    //    {
    //        public override void Run()
    //        {
    //            var child = JQueryBox.FromHtml("<div class='w' style='color:red'>X</div>");
    //            tc.Append(child);
    //            child.Styles["color"] = "blue";
    //            Assert(child.Styles["color"] == "rgb(0, 0, 255)");
    //        }
    //    }

    //    public class ElementManipulation_Properties1 : JQTest
    //    {
    //        public override void Run()
    //        {
    //            var child = JQueryBox.FromHtml("<input type='checkbox' checked='checked'>");
    //            tc.Append(child);
    //            Assert(child.PropertyGet<bool>("checked") == true);
    //        }
    //    }

    //    public class ElementManipulation_Properties2 : JQTest
    //    {
    //        public override void Run()
    //        {
    //            var child = JQueryBox.FromHtml("<input type='checkbox' checked='checked'>");
    //            tc.Append(child);
    //            child.PropertySet("checked", false);
    //            Assert(child.PropertyGet<bool>("checked") == false);
    //        }
    //    }

    //    public class ElementManipulation_Properties3 : JQTest
    //    {
    //        public override void Run()
    //        {
    //            var child = JQueryBox.FromHtml("<input type='checkbox' checked='checked'>");
    //            tc.Append(child);
    //            child.PropertySet<bool?>("checked", null);
    //            Assert(child.PropertyGet<bool?>("checked") == false);
    //        }
    //    }

}

