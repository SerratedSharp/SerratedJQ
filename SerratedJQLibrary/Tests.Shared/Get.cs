using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    // Tests for JQueryPlainObject.Get() / Get(int) – mirrors jQuery .get()

    public class Get_AllElements_AsJSObjects : JQTest
    {
        public override void Run()
        {
            // Arrange: stub three elements into the shared test container
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();

            // Act: retrieve the underlying native elements as JSObject[]
            JSObject[] elements = result.Get();

            // Assert: three elements with classes "a", "b", "c" in order
            Assert(elements.Length == 3, "Get() should return three elements for three stubs");
            Assert(elements[0].GetJSProperty<string>("className") == "a", "First element should have class 'a'");
            Assert(elements[1].GetJSProperty<string>("className") == "b", "Second element should have class 'b'");
            Assert(elements[2].GetJSProperty<string>("className") == "c", "Third element should have class 'c'");
        }
    }

    public class Get_ByIndex_AsJSObject : JQTest
    {
        public override void Run()
        {
            // Arrange: stub three elements into the shared test container
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();

            // Act: retrieve individual underlying native elements by index
            JSObject first = result.Get(0);
            JSObject second = result.Get(1);
            JSObject third = result.Get(2);

            // Assert: index-based access matches expected classes "a", "b", "c"
            Assert(first.GetJSProperty<string>("className") == "a", "Get(0) should return element with class 'a'");
            Assert(second.GetJSProperty<string>("className") == "b", "Get(1) should return element with class 'b'");
            Assert(third.GetJSProperty<string>("className") == "c", "Get(2) should return element with class 'c'");
        }
    }
}

