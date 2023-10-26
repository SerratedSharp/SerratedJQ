using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

public class TestHandler
{
    private readonly JQTest test;

    public TestHandler(JQTest test)
    {
        this.test = test;
    }

    public void EventListener(JQueryPlainObject sender, dynamic e)
    {
        Console.WriteLine($"e.target, e.currentTarget {e.target}, {e.currentTarget}");
        GlobalJS.Console.Log("Child_OnClick", sender, e, e.target, e.currentTarget);
        string eventName = e.type;
        sender.AddClass("eventhandled");// add class sop unit test can Assert/verify was clicked
        sender.Attr("data-eventname", eventName);
    }

    public JQueryPlainObject.JQueryEventHandler<JQueryPlainObject, dynamic> clickListener = (sender, e) => {
        Console.WriteLine($"e.target, e.currentTarget {e.target}, {e.currentTarget}");
        GlobalJS.Console.Log("Clicked", sender, e, e.target, e.currentTarget);
        //Console.WriteLine(e.target);
        //Console.WriteLine(e.currentTarget);
        //Console.WriteLine(e);
    };
}


public class Events_Click : JQTest
{
    public override void Run()
    {
        tc.Append("<div class='w'>&emsp;Click This</div>");
        var child = tc.Find(".w");
        child.OnClick += new TestHandler(this).EventListener;
        child.Trigger("click");
        Assert(child.HasClass("eventhandled"));
        Assert(child.Attr("data-eventname") == "click" );
        //child.OnClick += Child_OnClick2;        
    }



    //private void Child_OnClick2(JQueryBox sender, dynamic e)
    //{
    //    string eventName = e.type;
    //    Assert(eventName == "click");

    //    tc.Append(JQueryBox.FromHtml($@"<br/>&emsp;&emsp;<span class='unsub2'>Click to unsubscribe ""Click This""</span>"));

    //    tc.Find(".unsub2").OnClick += Child_UnSub2;
    //}

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

