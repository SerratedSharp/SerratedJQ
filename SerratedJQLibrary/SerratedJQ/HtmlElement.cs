using System;
using System.Collections.Generic;
using static System.Console;
using System.Linq;
using System.Dynamic;
using Newtonsoft.Json;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.JSInteropHelpers;
using Params = SerratedSharp.JSInteropHelpers.ParamsHelpers;
using SerratedSharp.SerratedJQ.Plain;

namespace SerratedSharp.SerratedJQ;


// Wrapper that references a HTML Element instance object, i.e. an item returned fropm vailla JS DOM access or a JQuery  the collection returned from selectors/queries
public class HtmlElement : IJSObjectWrapper<HtmlElement>, IJQueryContentParameter
    {
        internal JSObject jsObject;// reference to the jQuery javascript interop object
        public JSObject JSObject { get { return jsObject; } }

        // instances can only be created thru factory methods like Select()/ParseHtml()
        internal HtmlElement() { }
        public HtmlElement(JSObject jsObject) { this.jsObject = jsObject; }
        // This static factory method defined by the IJSObjectWrapper enables generic code such as CallJSOfSameNameAsWrapped to automatically wrap JSObjectsWrap
        static HtmlElement IJSObjectWrapper<HtmlElement>.WrapInstance(JSObject jsObject)
        {
            return new HtmlElement(jsObject); 
        }



    }






