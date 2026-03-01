using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class InstanceProperties_Length : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();
            Assert(result.Length == 3);
        }
    }

    public class InstanceProperties_JQueryVersion : JQTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();
            Assert(result.JQueryVersion == "3.7.1");
        }
    }

    // Updated test: use jQuery append, avoid calling appendChild on jQuery object
    public class InstanceProperties_AsWrapped_ParentElement : JQTest
    {
        public override void Run()
        {
            // Initialize empty test container
            StubHtmlIntoTestContainer(0);

            var document = (JSObject)JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var parentDiv = document.CallJS<JSObject>(funcName: "createElement", "div");
            var childDiv = document.CallJS<JSObject>(funcName: "createElement", "div");
            parentDiv.SetJSProperty("parent-id", "id");
            childDiv.SetJSProperty("child-id", "id");
            _ = parentDiv.CallJS<JSObject>(funcName: "appendChild", childDiv);

            // Append parentDiv into test container using jQuery .append() (valid method on tc.JSObject)
            _ = tc.JSObject.CallJS<JSObject>(funcName: "append", parentDiv);

            // Act: fetch parentElement wrapped
            var parentWrapped = childDiv.GetJSProperty<DomElementProxy>("ParentElement");

            // Assert
            Assert(parentWrapped != null, "Wrapped parent element is null");
            Assert(parentWrapped.Id == "parent-id", "parentElement did not wrap correctly or wrong element returned");
        }
    }

    // New test: exercise extension method GetJSProperty<W>
    public class InstanceProperties_Extension_AsWrapped_ParentElement : JQTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var document = (JSObject)JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var parentDiv = document.CallJS<JSObject>(funcName: "createElement", "div");
            var childDiv = document.CallJS<JSObject>(funcName: "createElement", "div");
            parentDiv.SetJSProperty("ext-parent-id", "id");
            childDiv.SetJSProperty("ext-child-id", "id");
            _ = parentDiv.CallJS<JSObject>(funcName: "appendChild", childDiv);
            _ = tc.JSObject.CallJS<JSObject>(funcName: "append", parentDiv);

            // Wrap childDom as proxy to call extension
            var childProxy = new DomElementProxy(childDiv);
            var parentWrapped = childProxy.GetJSProperty<DomElementProxy>("ParentElement");

            Assert(parentWrapped != null, "Extension wrapped parent element is null");
            Assert(parentWrapped.Id == "ext-parent-id", "Extension parentElement did not wrap correctly or wrong element returned");
        }
    }
}