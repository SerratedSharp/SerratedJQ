using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ;
using System;
using Wasm;

namespace Tests.Wasm;

public class InsertionInside_Append : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.Append("<div class='x'></div>");
        result = tc.Children();
        Assert(result.HasClass("a") && result.HasClass("x"));
        Assert(result.Length == 2);
    }
}

public class InsertionInside_Append_Object : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.Append( JQuery.ParseHtmlAsJQuery("<div class='x'></div>") );
        result = tc.Children();
        Assert(result.HasClass("a") && result.HasClass("x"));
        Assert(result.Length == 2);
    }
}

public class InsertionInside_AppendTo : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.AddClass("InsertionInside_AppendTo");
        JQuery.ParseHtmlAsJQuery("<div class='x'></div>").AppendTo(".InsertionInside_AppendTo");
        result = tc.Children();
        Assert(result.HasClass("a") && result.HasClass("x"));
        Assert(result.Length == 2);
    }
}

public class InsertionInside_AppendTo_Object : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        JQuery.ParseHtmlAsJQuery("<div class='x'></div>").AppendTo(tc);
        result = tc.Children();
        Assert(result.HasClass("a") && result.HasClass("x"));
        Assert(result.Length == 2);
    }
}

public class InsertionInside_Prepend : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.Prepend("<div class='x'></div>");
        result = tc.Children();
        Assert(result.HasClass("x") && result.HasClass("a"));
        Assert(result.Length == 2);
    }
}

// TODO: Fix .Prepend taking an object.  Possibly an issue with ParseHtml wrapping
public class InsertionInside_Prepend_HtmlObject : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.Prepend(JQuery.ParseHtml("<div class='x'></div>"));
        result = tc.Children();
        Assert(result.HasClass("x") && result.HasClass("a"));
        Assert(result.Length == 2);
    }
}

public class InsertionInside_Prepend_Object : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.Prepend(JQuery.ParseHtmlAsJQuery("<div class='x'></div>"));
        result = tc.Children();
        Assert(result.HasClass("x") && result.HasClass("a"));
        Assert(result.Length == 2);
    }
}


public class InsertionInside_PrependTo : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(1);
        tc.AddClass("InsertionInside_PrependTo");
        JQuery.ParseHtmlAsJQuery("<div class='x'></div>").PrependTo(".InsertionInside_PrependTo");
        result = tc.Children();
        Assert(result.HasClass("a") && result.HasClass("x"));
        Assert(result.Length == 2);
    }
}

public class InsertionInside_PrependTo_Object : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(3);
        tc.Append("<div class='InsertionInside_PrependTo_Object'></div>");
        JQuery.ParseHtmlAsJQuery("<div class='x'></div>").PrependTo(tc.Find(".InsertionInside_PrependTo_Object"));
        result = tc.Find(".InsertionInside_PrependTo_Object").Children();
        Assert(result.HasClass("x"));
        Assert(result.Length == 1);
    }
}

public class InsertionInside_PrependTo_Object2 : JQTest
{
    public override void Run()
    {
        JQueryObject stubs = StubHtmlIntoTestContainer(3);
        JQueryObject newObj = JQuery.ParseHtmlAsJQuery("<div class='x'></div>");
        newObj.PrependTo(tc);
        result = tc.Children();
        Assert(result.HasClass("x"));
        Assert(result.Length == 4);
    }
}
