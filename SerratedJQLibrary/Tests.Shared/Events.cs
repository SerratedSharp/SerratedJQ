using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class TestHandler
    {
        // private readonly JQTest test;

        public TestHandler(JQTest test)
        {
            //this.test = test;
        }

        public void EventListener(JQueryPlainObject sender, dynamic e)
        {
            Console.WriteLine($"e.target, e.currentTarget {e.target}, {e.currentTarget}");
            GlobalJS.Console.Log("Child_OnClick", sender, e, e.target, e.currentTarget);
            string eventName = e.type;
            sender.AddClass("eventhandled");// add class sop unit test can Assert/verify was clicked
            sender.Attr("data-eventname", eventName);
            e.target.AddClass("eventtarget");
            e.target.Attr("data-targeteventname", eventName);
        }

        //public JQueryPlainObject.JQueryEventHandler<JQueryPlainObject, dynamic> clickListener = (sender, e) =>
        //{
        //    Console.WriteLine($"e.target, e.currentTarget {e.target}, {e.currentTarget}");
        //    GlobalJS.Console.Log("Clicked", sender, e, e.target, e.currentTarget);
        //    //Console.WriteLine(e.target);
        //    //Console.WriteLine(e.currentTarget);
        //    //Console.WriteLine(e);
        //};
    }

    public class Events_Click : JQTest
    {
        public override void Run()
        {
            tc.Append("<div class='w'>&emsp;Click This</div>");
            var child = tc.Find(".w");
            var testHandler = new TestHandler(this);
            //child.On("click", Child_OnClick);
            //child.OnClick += Child_OnClick;
            child.OnClick += testHandler.EventListener;
            child.Trigger("click");
            Assert(child.HasClass("eventhandled"));
            Assert(child.Attr("data-eventname") == "click");
            //child.OnClick += Child_OnClick2;        
        }

        private void Child_OnClick(JQueryPlainObject sender, dynamic e)
        {
            Console.WriteLine($"e.target,");
        }
    }

    public class Events_Click2 : JQTest
    {
        public override void Run()
        {
            tc.Append("<div class='w'>&emsp;Click This</div>");
            var child = tc.Find(".w");
            var handler = new TestHandler(this);
            child.OnClick += handler.EventListener;
            child.OnClick -= handler.EventListener;
            child.OnClick += new TestHandler(this).EventListener;
            child.Trigger("click");
            Assert(child.HasClass("eventhandled"));
            Assert(child.Attr("data-eventname") == "click");

        }
    }

    public class Events_Click_Remove : JQTest
    {
        public override void Run()
        {
            tc.Append("<div class='w'>&emsp;Click This</div>");
            var child = tc.Find(".w");
            var handler = new TestHandler(this);
            child.OnClick += handler.EventListener;
            child.OnClick -= handler.EventListener;
            child.Trigger("click");
            Assert(child.HasClass("eventhandled") == false);
        }
    }

    public class Events_Input : JQTest
    {
        public override void Run()
        {
            tc.Append("<input class='w' value='Edit This'/>");
            var child = tc.Find(".w");
            var handler = new TestHandler(this);
            child.OnInput += handler.EventListener;
            child.Trigger("input");
            Assert(child.HasClass("eventhandled"));
            Assert(child.Attr("data-eventname") == "input");
        }
    }

    public class Events_Change : JQTest
    {
        public override void Run()
        {
            tc.Append("<input class='w' value='Edit This'/>");
            var child = tc.Find(".w");
            var handler = new TestHandler(this);
            child.OnChange += handler.EventListener;
            child.Trigger("change");
            Assert(child.HasClass("eventhandled"));
            Assert(child.Attr("data-eventname") == "change");
        }
    }

    public class Events_On_Click : JQTest
    {
        public override void Run()
        {
            tc.Append("<div class='w'></div>");
            var child = tc.Find(".w");
            var handler = new TestHandler(this);
            child.On("click", handler.EventListener);
            child.Trigger("click");
            Assert(child.HasClass("eventhandled"));
            Assert(child.Attr("data-eventname") == "click");
        }
    }

    public class Events_On_Click_Selector : JQTest
    {
        public override void Run()
        {
            tc.Append("""
            <div class='w'><div class='y'>
                <span class='x1'></span>
            </div></div>
            """);
            var child = tc.Find(".w");

            var handler = new TestHandler(this);
            // Seperate delegated subscribe to each
            child.On("click", ".x1", handler.EventListener);
            child.On("click", ".x2", handler.EventListener);
            tc.Find(".y").Append("""<span class='x2'></span>""");
            var child2 = tc.Find(".x2");
            // trigger click on only one of the delegated elements
            child2.Trigger("click");
            Assert(child.HasClass("eventhandled"));
            Assert(child.Attr("data-eventname") == "click");
            Assert(child2.HasClass("eventtarget"));
            Assert(child2.Attr("data-targeteventname") == "click");

        }
    }
}