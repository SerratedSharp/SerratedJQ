using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    // Minimal DOM element wrapper to exercise GetJSProperty/SetJSProperty CallerMemberName helpers
    private class DomElementProxy : IJSObjectWrapper<DomElementProxy>
    {
        public JSObject JSObject { get; }
        public DomElementProxy(JSObject js) { JSObject = js; }
        static DomElementProxy IJSObjectWrapper<DomElementProxy>.WrapInstance(JSObject js) => new(js);

        public string Id
        {
            get => this.GetJSProperty<string>();
            set => this.SetJSProperty(value);
        }

        public string Title
        {
            get => this.GetJSProperty<string>();
            set => this.SetJSProperty(value);
        }

        public string TextContent
        {
            get => this.GetJSProperty<string>();
            set => this.SetJSProperty(value);
        }
    }

    public class Setters_OnDomElement : JQTest
    {
        public override void Run()
        {
            // Arrange: create a new <div> and append to body
            var document = (JSObject)JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var body = document.GetJSProperty<JSObject>("body");
            var div = document.CallJS<JSObject>(funcName: "createElement", "div");
            _ = body.CallJS<JSObject>(funcName: "appendChild", div);

            var el = new DomElementProxy(div);

            // Act: set via CallerMemberName-based setters
            el.Id = "setter-test-id";
            el.Title = "setter-test-title";
            el.TextContent = "setter-test-text";

            // Assert via getters and DOM queries
            Assert(el.Id == "setter-test-id", "Id was not set via setter");
            Assert(el.Title == "setter-test-title", "Title was not set via setter");
            Assert(el.TextContent == "setter-test-text", "TextContent was not set via setter");

            var byId = document.CallJS<JSObject>(funcName: "getElementById", "setter-test-id");
            Assert(byId != null, "getElementById returned null");
        }
    }
}
