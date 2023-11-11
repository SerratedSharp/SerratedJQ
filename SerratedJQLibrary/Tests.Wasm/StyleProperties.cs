using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;
public partial class TestsContainer
{
    // Style Properties - https://api.jquery.com/category/manipulation/style-properties/
    public class StyleProperties_Height : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Height(100);
            Assert(result.Height() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_Height_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Height("100px");
            Assert(result.Height() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_Width : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Width(100);
            Assert(result.Width() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_Width_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.Width("100px");
            Assert(result.Width() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_InnerHeight : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.InnerHeight(100);
            Assert(result.InnerHeight() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_InnerHeight_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.InnerHeight("100px");
            Assert(result.InnerHeight() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_InnerWidth : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.InnerWidth(100);
            Assert(result.InnerWidth() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_InnerWidth_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.InnerWidth("100px");
            Assert(result.InnerWidth() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_OuterHeight : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.OuterHeight(100);
            Assert(result.OuterHeight() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_OuterHeight_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.OuterHeight("100px");
            Assert(result.OuterHeight() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_OuterWidth : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.OuterWidth(100);
            Assert(result.OuterWidth() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_OuterWidth_String : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.OuterWidth("100px");
            Assert(result.OuterWidth() == 100);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_ScrollLeft : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            // make a small container
            var scrollable = stubs.First().Height(100).Width(100).Css("overflow", "scroll");
            // larger item inside to overflow and make scrollbar
            scrollable.Append("<div style='height:200px;width:200px'>Content</div>");
            result = scrollable.ScrollLeft(50);// scroll
                                               //check scroll position
            Assert(Convert.ToInt32(result.ScrollLeft()) == 50);
            Assert(result.Length == 1);
        }
    }

    public class StyleProperties_ScrollTop : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            // make a small container
            var scrollable = stubs.First().Height(100).Width(100).Css("overflow", "scroll");
            // larger item inside to overflow and make scrollbar
            scrollable.Append("<div style='height:200px;width:200px'>Content</div>");
            result = scrollable.ScrollTop(50);// scroll
                                               //check scroll position
            Assert(Convert.ToInt32(result.ScrollTop()) == 50);
            Assert(result.Length == 1);
        }
    }

    //public class StyleProperties_ScrollTop : JQTest
    //{
    //    public override void Run()
    //    {
    //        result = JQueryPlain.Select("body").ScrollTop(300);
    //        Assert(Convert.ToInt32(result.ScrollTop()) == 300);
    //        //Assert(result.Length == 1);
    //    }
    //}

}





