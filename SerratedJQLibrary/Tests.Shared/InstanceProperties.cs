using SerratedSharp.JSInteropHelpers;
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

            var document = JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var parentDiv = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            var childDiv = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            JSInstanceProxy.SetPropertyByName(parentDiv, "id", "parent-id");
            JSInstanceProxy.SetPropertyByName(childDiv, "id", "child-id");
            _ = JSInstanceProxy.FuncByNameAsObject(parentDiv, "appendChild", new object[] { childDiv });

            // Append parentDiv into test container using jQuery .append() (valid method on tc.JSObject)
            _ = JSInstanceProxy.FuncByNameAsObject(tc.JSObject, "append", new object[] { parentDiv });

            // Act: fetch parentElement wrapped
            var parentWrapped = JSImportInstanceHelpers.GetPropertyOfSameNameAsWrapped<DomElementProxy>(childDiv, propertyName: "ParentElement");

            // Assert
            Assert(parentWrapped != null, "Wrapped parent element is null");
            Assert(parentWrapped.Id == "parent-id", "parentElement did not wrap correctly or wrong element returned");
        }
    }
}