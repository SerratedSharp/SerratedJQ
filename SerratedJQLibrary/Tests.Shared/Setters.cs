using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    // Minimal DOM element wrapper to exercise GetProperty/SetProperty CallerMemberName helpers
    private class DomElementProxy : IJSObjectWrapper<DomElementProxy>
    {
        public JSObject JSObject { get; }
        public DomElementProxy(JSObject js) { JSObject = js; }
        static DomElementProxy IJSObjectWrapper<DomElementProxy>.WrapInstance(JSObject js) => new(js);

        public string Id
        {
            get => JSImportInstanceHelpers.GetPropertyOfSameName<string>(JSObject);
            set => JSImportInstanceHelpers.SetPropertyOfSameName(JSObject, value);
        }

        public string Title
        {
            get => JSImportInstanceHelpers.GetPropertyOfSameName<string>(JSObject);
            set => JSImportInstanceHelpers.SetPropertyOfSameName(JSObject, value);
        }

        public string TextContent
        {
            get => JSImportInstanceHelpers.GetPropertyOfSameName<string>(JSObject);
            set => JSImportInstanceHelpers.SetPropertyOfSameName(JSObject, value);
        }
    }

    public class Setters_OnDomElement : JQTest
    {
        public override void Run()
        {
            // Arrange: create a new <div> and append to body
            var document = JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var body = (JSObject)JSInstanceProxy.PropertyByNameToObject(document, "body");
            var div = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            _ = JSInstanceProxy.FuncByNameAsObject(body, "appendChild", new object[] { div });

            var el = new DomElementProxy(div);

            // Act: set via CallerMemberName-based setters
            el.Id = "setter-test-id";
            el.Title = "setter-test-title";
            el.TextContent = "setter-test-text";

            // Assert via getters and DOM queries
            Assert(el.Id == "setter-test-id", "Id was not set via setter");
            Assert(el.Title == "setter-test-title", "Title was not set via setter");
            Assert(el.TextContent == "setter-test-text", "TextContent was not set via setter");

            var byId = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "getElementById", new object[] { "setter-test-id" });
            Assert(byId != null, "getElementById returned null");
        }
    }
}
