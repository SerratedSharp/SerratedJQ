using SerratedSharp.SerratedJQ;
using System;
using Wasm;

namespace Tests.Wasm;

public class TreeTraversal_Children : JQTest
{
    public override void Run()
    {         
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        Assert(stubs.HasClass("a") && stubs.HasClass("e"));
        Assert(stubs.Length == 5);
    }
}

public class TreeTraversal_Children_Selector : JQTest
{
    public override void Run()
    {
        StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = tc.Children(".a,.e");
        Assert(result.HasClass("a") && result.HasClass("e"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_Closest : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQueryObject result = stubs.Closest(".tc");
        Assert(result.HasClass("tc"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Find : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = tc.Find(".a,.e");
        Assert(result.HasClass("a") && result.HasClass("e"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_Next : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".b").Next();
        Assert(result.HasClass("c"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Next_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs1 = StubHtmlIntoTestContainer(5);
        stubs1.Remove(".c");// a,b,d,e
        StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e
        StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e,a,b,c,d,e
        JQueryObject result = tc.Children().Filter(".b").Next(".c");
        Assert(result.HasClass("c"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_NextAll : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.NextAll();
        Assert(result.HasClass("b") && result.HasClass("c") && result.HasClass("d") && result.HasClass("e"));
        Assert(result.Length == 4);
    }
}

public class TreeTraversal_NextAll_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".c").NextAll("div");
        Assert( result.HasClass("d") && result.HasClass("e"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_NextUntil : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".b").NextUntil(".e");
        Assert(result.HasClass("c") && result.HasClass("d"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_NextUntil_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".b").NextUntil(".e", ".c");
        Assert(result.HasClass("c"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Parent : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQueryObject result = stubs.Parent();
        Assert(result.HasClass("tc"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Parent_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQueryObject result = stubs.Parent(".tc");
        Assert(result.HasClass("tc"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Parents : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQueryObject result = stubs.Parents();
        Assert(result.HasClass("tc"));
        Assert(result.Length > 2);
    }
}

public class TreeTraversal_Parents_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQueryObject result = stubs.Parents(".tc");
        Assert(result.HasClass("tc"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_ParentsUntil : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQueryObject result = stubs.ParentsUntil();
        Assert(result.HasClass("tc"));
        Assert(result.Length > 2);
    }
}

public class TreeTraversal_ParentsUntil_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(2);
        stubs.First().Append("<div class='x'></div>");
        JQueryObject result = tc.Find(".x").ParentsUntil(".tc");
        Console.WriteLine("ASDF: " + result.Parent().Html);
        Assert(result.HasClass("a"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Prev : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".d").Prev();
        Assert(result.HasClass("c"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Prev_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs1 = StubHtmlIntoTestContainer(5);
        stubs1.Remove(".c");// a,b,d,e
        StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e
        StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e,a,b,c,d,e
        JQueryObject result = tc.Children().Filter(".c").Prev(".b");
        Assert(result.HasClass("b"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_PrevAll : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.PrevAll();
        Assert(result.HasClass("a") && result.HasClass("b") && result.HasClass("c") && result.HasClass("d"));
        Assert(result.Length == 4);
    }
}

public class TreeTraversal_PrevAll_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".c").PrevAll("div");
        Assert(result.HasClass("b") && result.HasClass("a"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_PrevUntil : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
        JQueryObject result = stubs.Filter(".d").PrevUntil(".a");
        Assert(result.HasClass("c") && result.HasClass("b"));
        Assert(result.Length == 2);
    }
}

public class TreeTraversal_PrevUntil_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);
        JQueryObject result = stubs.Filter(".d").PrevUntil(".a", ".c");
        Assert(result.HasClass("c"));
        Assert(result.Length == 1);
    }
}

public class TreeTraversal_Siblings : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);
        JQueryObject result = stubs.Filter(".c").Siblings();
        Assert(result.HasClass("a") && result.HasClass("e"));
        Assert(result.Length == 4);
    }
}

public class TreeTraversal_Siblings_Selector : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(5);
        JQueryObject result = stubs.Filter(".c").Siblings(".b");
        Assert(result.HasClass("b"));
        Assert(result.Length == 1);
    }
}




