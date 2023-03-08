﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Uno.Extensions;
using Uno.Foundation;
using Uno.Foundation.Interop;
using static System.Console;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace SerratedSharp.SerratedJQ
{

    public class JQueryBox : IJSObject
    {
        private static int keep = 2;
        static JQueryBox()
        {
            // Add javascript declaration that is used by WebAssembly but was declared in incorrect Uno Platform project.
            WebAssemblyRuntime.InvokeJS(_1.ManagedObjectJavascriptDispatcherDeclaration);

            //(function (Uno) {
            //    var Http;
            //    (function (Http) {
            //        class HttpClient {
            //            static async send(config) {
            //                const params = {
            //                    method: config.method,
            //                    cache: config.cacheMode || 'default',
            //                    headers: new Headers(config.headers)
            //                };
            //                if (config.payload) {
            //                    params.body = await this.blobFromBase64(config.payload, config.payloadType);
            //                }
            //                try {
            //                    const response = await fetch(config.url, params);
            //                    let responseHeaders = '';
            //                    response.headers.forEach((v, k) => responseHeaders += `${k}:${v}\n`);
            //                    const responseBlob = await response.blob();
            //                    const responsePayload = responseBlob ? await this.base64FromBlob(responseBlob) : '';
            //                    this.dispatchResponse(config.id, response.status, responseHeaders, responsePayload);
            //                }
            //                catch (error) {
            //                    this.dispatchError(config.id, `${error.message || error}`);
            //                    console.error(error);
            //                }
            //            }
            //            static async blobFromBase64(base64, contentType) {
            //                contentType = contentType || 'application/octet-stream';
            //                const url = `data:${contentType};base64,${base64}`;
            //                return await (await fetch(url)).blob();
            //            }
            //            static base64FromBlob(blob) {
            //                return new Promise(resolve => {
            //                    const reader = new FileReader();
            //                    reader.onloadend = () => {
            //                        const dataUrl = reader.result;
            //                        const base64 = dataUrl.split(',', 2)[1];
            //                        resolve(base64);
            //                    };
            //                    reader.readAsDataURL(blob);
            //                });
            //            }
            //            static dispatchResponse(requestId, status, headers, payload) {
            //                this.initMethods();
            //                const requestIdStr = MonoRuntime.mono_string(requestId);
            //                const statusStr = MonoRuntime.mono_string('' + status);
            //                const headersStr = MonoRuntime.mono_string(headers);
            //                const payloadStr = MonoRuntime.mono_string(payload);
            //                MonoRuntime.call_method(this.dispatchResponseMethod, null, [requestIdStr, statusStr, headersStr, payloadStr]);
            //            }
            //            static dispatchError(requestId, error) {
            //                this.initMethods();
            //                const requestIdStr = MonoRuntime.mono_string(requestId);
            //                const errorStr = MonoRuntime.mono_string(error);
            //                MonoRuntime.call_method(this.dispatchErrorMethod, null, [requestIdStr, errorStr]);
            //            }
            //            static initMethods() {
            //                if (this.dispatchResponseMethod) {
            //                    return; // already initialized.
            //                }
            //                const asm = MonoRuntime.assembly_load('Uno.UI.Runtime.WebAssembly');
            //                const httpClass = MonoRuntime.find_class(asm, 'Uno.UI.Wasm', 'WasmHttpHandler');
            //                this.dispatchResponseMethod = MonoRuntime.find_method(httpClass, 'DispatchResponse', -1);
            //                this.dispatchErrorMethod = MonoRuntime.find_method(httpClass, 'DispatchError', -1);
            //            }
            //        }
            //        Http.HttpClient = HttpClient;
            //    })(Http = Uno.Http || (Uno.Http = {}));
            //})(Uno || (Uno = {}));

            //            ");

            if (keep == 1)// Prevent these methods from being removed by ILLinker at compile time. The condition being false prevents these from actually being called at runtime, but indeterministic so that linker doesn't remove them. 
            {
                var keeper = new JQueryBox();
                keeper.InternalClickCallback("", "");
                keeper.InternalEventCallback("", "");
                keeper.InternalInputCallback("", "");
            }

        }

        // This constructor is internal because callers need to use factory methods such as .Select or FromHtml so that we can properly initialize handles.
        internal JQueryBox()
        {
            handle = JSObjectHandle.Create(this);
            styles = new JQIndexer(this, "css");
            //properties = new JQIndexer(this, "prop");
            attributes = new JQIndexer(this, "attr");

            // How to manually add a JS->C# callback
            //         WebAssemblyRuntime.InvokeJS(@"
            //   SerratedSharp.SerratedJQ.JQueryBox.prototype.ClickCallback = function(eEncoded, eventType) {
            //var parameters = {'eEncoded': eEncoded, 'eventType': eventType};

            //         var serializedParameters = JSON.stringify(parameters);
            // method name passed to .dispatch must be a public instance method (cannot be an explicit interface)
            //         Uno.Foundation.Interop.ManagedObject.dispatch(this.__managedHandle, 'ClickCallback', serializedParameters);
            //     };
            //         ");

        }

        public JQueryBox(JQueryBox jQueryBox)
        {
            // See these for how this works:
            // https://github.com/unoplatform/uno/blob/ab2ee2a26e160fb73db358d84ce780a895dae413/src/Uno.Foundation.Runtime.WebAssembly/Interop/JSObjectMetadataProvider.wasm.cs
            // https://github.com/unoplatform/uno/blob/768d3ef773cad95a9fb0d1d27ba0ce45e8c75267/src/Uno.Foundation.Runtime.WebAssembly/Interop/Runtime.wasm.cs

            handle = JSObjectHandle.Create(this);

            //var _managedGcHandle = GCHandle.Alloc(handle, GCHandleType.Weak);
            //var _managedHandle = GCHandle.ToIntPtr(_managedGcHandle);

            styles = new JQIndexer(this, "css");
            attributes = new JQIndexer(this, "attr");
        }

        internal JSObjectHandle handle;
        JSObjectHandle IJSObject.Handle { get => handle; }

        // this is effectively a factory method since it and other static methods are the main way to generate new JQBox
        public static JQueryBox Select(string selector)
        {
            var box = new JQueryBox();

            // Call {_1._2}(selector) and add the return to the new JS JQueryBox.{_1.jqbj}
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{box}.{_1.jqbj} = {_1.jQueryRef}('{selector}');"
                );

            return box;
        }

        public static JQueryBox FromHtml(string html)
        {
            // Escape single quotes and remove breaking control characters
            html = html.Replace("'", "\\'").Replace("\t", "").Replace("\r", "").Replace("\n", "");
            var box = new JQueryBox();
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{box}.{_1.jqbj} = {_1.jQueryRef}('{html}');"
                );

            return box;
        }


        #region Events



        // TODO: Implement static .ready?

        // Strongly typed event handler signature 
        public delegate void JQueryEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
            where TSender : JQueryBox;

        private JQueryEventHandler<JQueryBox, object> onClick;
        // We use explicit event so that we can only create the JQuery listener when necessary        
        public event JQueryEventHandler<JQueryBox, object> OnClick
        {
            add
            {
                if (onClick == null)// if first event subscriber
                    this.InnerOn("click", this, nameof(InternalClickCallback));// then add JQuery listener

                onClick += value;
            }
            remove
            {
                onClick -= value;
                if (onClick == null) // if last subscriber removed, then remove JQuery listener
                    this.InnerOff("click");
            }
        }

        private JQueryEventHandler<JQueryBox, object> onInput;
        // We use explicit event so that we can only create the JQuery listener when necessary        
        public event JQueryEventHandler<JQueryBox, object> OnInput
        {
            add
            {
                if (onInput == null)// if first event subscriber
                    this.InnerOn("input", this, nameof(InternalInputCallback));// then add JQuery listener

                onInput += value;
            }
            remove
            {
                onInput -= value;
                if (onInput == null) // if last subscriber removed, then remove JQuery listener
                    this.InnerOff("input");
            }
        }

        //private Dictionary<string, string> liteOnlyEvents = new Dictionary<string, string>
        //{
        //    {_1.click,"" },
        //    {_1.input,"" }
        //    // Review events and determine which ones to support in Lite
        //};

        // Handles the JQuery event and fires the C# event
        [Obsolete("For internal use only.")]
        public void InternalClickCallback(string eEncoded, string eventType)
        {
            onClick?.Invoke(this, null);
        }

        [Obsolete("For internal use only.")]
        public void InternalInputCallback(string eEncoded, string eventType)
        {
            onInput?.Invoke(this, null);
        }

        // generic event susbcription
        private Dictionary<string, JQueryEventHandler<JQueryBox, object>> onEvent = new Dictionary<string, JQueryEventHandler<JQueryBox, object>>();

        /// <summary>
        /// Subscribe to an HTML DOM event. Note you must maintain a reference to the publishing object to prevent garbage collection.
        /// <example><code>
        /// var someButton = JQueryBox.Select("#someButtonId");
        /// someButton.On("click", SomeButton_OnClick);// Note if the reference someButton(event publisher) is garbage collected, then the listener will no longer receive events.
        /// controlsKeeper.Add(someButton);
        /// 
        /// private void SomeButton_OnClick(JQueryBox sender, object e)
        /// {
        ///     sender.SetStyle("color", "red");
        /// }
        /// </code></example>
        /// </summary>
        /// <remarks>Memory leaky, since we don't know if the publishing DOM elemnt still exists, .NET objects handling events are prevented from being Garbage Collected.</remarks>
        /// <param name="eventName">HTML DOM Event Name</param>
        /// <param name="handler">Function/delegate to handle the event.</param>
        public void On(string eventName, JQueryEventHandler<JQueryBox, object> handler)
        {
            //if (!liteOnlyEvents.ContainsKey(eventName))
            //    throw new NotImplementedException($"Event {eventName} is not supported in the Lite version. Supported events: " + string.Join(", ", liteOnlyEvents));

            eventObjects.Add(this);// HACK: Prevent GC on C#/JQ object publishing the event.

            if (!onEvent.ContainsKey(eventName) || onEvent[eventName] == null)// if first event subscriber
            {
                this.InnerOn(eventName, this, nameof(InternalEventCallback));// then add JQuery listener (the list.Add is a hack to prevent GC
            }

            onEvent[eventName] = handler;
        }

        public void Off(string eventName, JQueryEventHandler<JQueryBox, object> handler)
        {
            if (!onEvent.ContainsKey(eventName))
                return;

            onEvent[eventName] -= handler;
            if (onEvent[eventName] == null) // if last subscriber removed, then remove JQuery listener
                this.InnerOff(eventName);
        }

        /// <summary>
        /// Do not use. Exposed for Javascript->C# interop.
        /// </summary>        
        [Obsolete("For internal use only.")]
        public void InternalEventCallback(string eEncoded, string eventType)
        {
            //Console.WriteLine(eventType);
            onEvent[eventType]?.Invoke(this, eEncoded);
        }

        private static List<JQueryBox> eventObjects = new List<JQueryBox>();

        /// <summary>
        /// It is recommended to use the C# events rather than this method. 
        /// </summary>
        private JQueryBox InnerOn(string events, IJSObject eventListener, string listenerFunctionName)
        {
            var newBox = new JQueryBox();
            eventObjects.Add(this);// HACK: Prevent GC on C#/JQ object publishing the event.  CONSIDER: Removing if last unsubscriber.  Really we'd want a way to detect the object no longer exists on the page.

            // {eventListener} is the JQueryBox box, and .{listeneerFunctionName} is usually "InternalEventCallback".
            // Uno WebAssembly generates a javascript wrapper for this managed type.
            // From the perspective of javascript there is a javascript method JQueryBox.InternalEventCallback which proxies calls from JS back to the managed C# object
            // The javascript JQueryBox.InternalEventCallback is registered as the handler for the event, and so proxies events firing back to this managed C# object
            // Note the JS object isn't literally "JQueryBox", InvokeJSWithInteropt takes interpolation parameters of type IJSObject and translates that into an activeObject[id] dereference.
            WebAssemblyRuntime.InvokeJSWithInterop($@"
                     
                    var handler = function(e){{
                        console.log('On Fired');
                        var eEncoded = btoa(JSON.stringify(e));                        
                        {eventListener}.{listenerFunctionName}(eEncoded, e.type);
                    }}.bind({eventListener});

                    {newBox}.{_1.jqbj} = {this}.{_1.jqbj}.on('{events}', handler);
                ");
            return newBox;
        }

        private JQueryBox InnerOff(string events)
        {
            var newBox = new JQueryBox();
            // CONSIDER: It's possible to unsubscribe only our handler, but we can't use an anonymous handler above.
            WebAssemblyRuntime.InvokeJSWithInterop($@"
                    {newBox}.{_1.jqbj} = {this}.{_1.jqbj}.off('{events}');
                ");
            return newBox;
        }

        #endregion


        #region Data

        /// <summary>
        /// Note this data is not attached to the HTML DOM element.
        /// This means you need to maintain a reference to this C# JQueryBox to keep/access this data.
        /// Use DataAdd or ManagedObjectAttach to store data attached to the HTML element.
        /// </summary>
        public dynamic DataBag = new NullingExpandoObject(); //new System.Dynamic.ExpandoObject();

        // Dynamic object that returns null for non-existant keys instead of throwing exception.
        // If a user wants a more strict behavior they can declare and add their nested expando or dictionary.
        public class NullingExpandoObject : DynamicObject
        {
            private readonly Dictionary<string, object> values
                = new Dictionary<string, object>();

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                // We don't care about the return value...
                values.TryGetValue(binder.Name, out result);
                return true;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                values[binder.Name] = value;
                return true;
            }
        }

        // TODO: Provide options between primitive types, serilized as JSON, and managed pointers
        // TODO: When another method is going to remove HTML elements, first scan for attached managed objects and free them
        /// <summary>
        /// Attaches a managed type to the HTML element's .Data property by storing a managed int pointer.
        /// The type is prevented from garbage collection since the HTML element will hold the pointer outside of GC's scope.
        /// Use of this method will cause memory leaks as objects will not be removed unless explicitely converted to regular references with ManagedObjectRemove
        /// making it available for garbage collection through normal processes.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>        
        public JQueryBox ManagedObjectAttach(string key, object value)
        {

            // We use .Normal where we're only accessing the pointer from managed code,
            // but unmanaged code intermittently holds the only pointer.
            // This prevents garbage collection during period where managed code has no reference to the object
            var managedGcHandle = GCHandle.Alloc(value, GCHandleType.Normal);  // GCHandleType.Weak
            IntPtr managedHandle = GCHandle.ToIntPtr(managedGcHandle);
            Console.WriteLine(managedHandle);
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{this}.{_1.jqbj}.data('{key}','{managedHandle}');"
            );

            return this;

        }

        /// <summary>
        /// Returns an object that was stored with AttachObject
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object ManagedObjectGet(string key)
        {
            string ptrStr = WebAssemblyRuntime.InvokeJSWithInterop(
                $@"return {this}.{_1.jqbj}.data('{key}');"
            );
            var intr = Convert.ToInt32(ptrStr);
            IntPtr pntr = new IntPtr(intr);
            var managedGcHandle = GCHandle.FromIntPtr(pntr);
            return managedGcHandle.Target;
        }

        /// <summary>
        /// Removes a managed object from an HTML element, marks the pointer as free, and returns a normal object reference.
        /// This effectively makes the object available for garbage collection under normal rules, enuring once the caller
        /// no longer holds any references to the object then it will be garbage collected when appropriate.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object ManagedObjectRemove(string key)
        {
            string ptrStr = WebAssemblyRuntime.InvokeJSWithInterop(
                $@" var ptr = {this}.{_1.jqbj}.data('{key}');
                    {this}.{_1.jqbj}.removeData('{key}');
                    return ptr;"
            );

            var managedGcHandle = GCHandle.FromIntPtr(new IntPtr(Convert.ToInt32(ptrStr)));
            var target = managedGcHandle.Target;
            managedGcHandle.Free();

            return target;
        }

        /// <summary>
        /// Attaches data to the underlying HTML element using DOM Dataset feature.  
        /// ValueTypes and strings are stored as a string.
        /// Other types are serialized using System.Json.Serializer
        /// Non-serializable types can alternatively be attached using .ManagedObjectAttach()
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public JQueryBox DataAdd(string key, object value)
        {
            // TODO: Determine if this properly handles nullable types.  Also review FuncGenericOut to determine if the casting approach and also FormatPArameter is appropriate here

            string dataString;
            if (value.GetType() == typeof(string) || value.GetType().IsValueType) {
                dataString = value.ToString();    
            }
            else
            {
                dataString = System.Text.Json.JsonSerializer.Serialize(value);
            }

            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{this}.{_1.jqbj}.data('{key}','{dataString}');"
            );

            return this;
        }

        public JQueryBox DataRemove(string key)
        {
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{this}.{_1.jqbj}.removeData('{key}');"
            );
            return this;
        }

        
        /// <typeparam name="T">Must be the same type as value passed to DataAdd()</typeparam>        
        public T DataGet<T>(string key)
        {
            object v;

            string dataString = WebAssemblyRuntime.InvokeJSWithInterop(
                $@"return {this}.{_1.jqbj}.data('{key}');"
            );

            if(typeof(T) == typeof(string) )
            {
                return (T)Convert.ChangeType(dataString, typeof(T));                
            }
            else if(typeof(T).IsValueType )
            {
                return (T)Convert.ChangeType(dataString, typeof(T));
            }
            else
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(dataString);
            }

        }


        #endregion


        #region JQuery Object Properties
        public long Length
        {
            get
            {
                return Convert.ToInt64(WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"return {this}.{_1.jqbj}.length;"
                ));
            }
        }

        public string Version =>
            WebAssemblyRuntime.InvokeJSWithInterop($@"return {this}.{_1.jqbj}.jquery;");



        #endregion

        #region DOM Manipulation

#if PRO
        /// <summary> Adds <paramref name="newSiblingHtml"/> before this element.  Returns the new HTML JQuery object. </summary>     
        /// <example>
        ///   jQuery( "<h2>Greetings</h2>" ).insertAfter( jQuery( ".container" ) );
        ///   <div class="container">
        ///   </div>
        ///   <h2>Greetings</h2>        
        /// </example>
        public JQueryBox BeforeNew(string newSiblingHtml)
        {
            return HtmlFuncJQuery("insertBefore", newSiblingHtml);
        }
        
        /// <summary> Adds <paramref name="newSiblingHtml"/> after this element.  Returns the new HTML JQuery object. </summary>             
        public JQueryBox AfterNew(string newSiblingHtml)
        {
            return HtmlFuncJQuery("insertAfter", newSiblingHtml);
        }
        
        /// <summary> Returns the appended HTML JQuery object. </summary>        
        public JQueryBox AppendNew(string newChildHtml)
        {
            return HtmlFuncJQuery("appendTo", newChildHtml);
        }

        /// <summary> Adds <paramref name="newChildHtml"/> to this container and returns reference to the new HTML JQuery object.</summary>
        public JQueryBox PrependNew(string newChildHtml)
        {
            return HtmlFuncJQuery("prependTo", newChildHtml);
        }

        public JQueryBox Clone(bool? withDataAndEvents = null, bool? deepWithDataAndEvents = null)
        {
            return FuncJQuery("clone", (withDataAndEvents ?? false).ToString(), (deepWithDataAndEvents ?? false).ToString());
        }

        /// <summary> Adds <paramref name="sibling"/> before this element. Returns <c>this</c>.</summary>        
        public JQueryBox Before(JQueryBox sibling)
        {
            return FuncJQuery("before", sibling);
        }

        /// <summary> Adds <paramref name="sibling"/> after this element.  Returns <c>this</c>.</summary>        
        public JQueryBox After(JQueryBox sibling)
        {
            return FuncJQuery("after", sibling);
        }

        public JQueryBox Detach(string selector = null)
        {
            return FuncJQuery("detach", selector);
        }

        public JQueryBox Empty()
        {
            return FuncJQuery("empty");
        }

        /// <summary> Replaces this with <paramref name="newHtml"/> and returns reference to the new HTML JQuery object.</summary>
        public JQueryBox ReplaceWithNew(string newHtml)
        {
            return HtmlFuncJQuery("replaceAll", newHtml);
        }

        public JQueryBox ReplaceWith(JQueryBox replacement)
        {
            return FuncJQuery("replaceWith", replacement);
        }

        // TODO: Test null selector
        public JQueryBox Unwrap(string selector = null)
        {
            return FuncJQuery("unwrap", selector);
        }

                // TODO: This is a good way to document the method.  Also add <example><code> INSIDE the summary showing example and resulting HTML.
        /// <summary>
        /// Calls JQuery <paramref name="this"/>.wrap( <paramref name="outerWrapper"/> ).
        /// If <paramref name="this"/> JQuery object is a collection of elements, 
        /// then <paramref name="outerWrapper"/> is cloned and wraps each individually.
        /// </summary>
        public JQueryBox WrapEach(JQueryBox outerWrapper)
        {
            return FuncJQuery("wrap", outerWrapper);
        }
        
        /// <summary>
        /// Calls JQuery <paramref name="this"/>.wrapInner( <paramref name="innerWrapper"/> ).
        /// The <paramref name="innerWrapper"/> is inserted into each element in the JQuery collection of <paramref name="this"/>,
        /// and the existing direct children of each <paramref name="this"/> element are moved within the inner most element of <paramref name="innerWrapper"/>.
        /// The HTML structure of <paramref name="innerWrapper"/> must contain only a single inner most leaf element,
        /// which will become the new parent container of the direct children of <paramref name="this"/>.
        /// </summary>
        public JQueryBox WrapChildOfEach(JQueryBox innerWrapper)
        {
            return FuncJQuery("wrapInner", innerWrapper);
        }

#endif

        // Consider: Append will accept non-HTML text and append it.  AppendTo will only accept HTML.  So currently without Append(string) we can't append non-HTML text.
        /// <summary> Adds <paramref name="child"/> to container and returns the container contents(i.e. all siblings of the appended item).</summary>        
        public JQueryBox Append(JQueryBox child)
        {
            return FuncJQuery("append", child);
        }

        /// <summary>Adds <paramref name="child"/> to container and returns the container contents(i.e. all siblings of the appended item).</summary>
        public JQueryBox Prepend(JQueryBox child)
        {
            return FuncJQuery("prepend", child);
        }

        public JQueryBox Remove(string selector = null)
        {
            return FuncJQuery("remove", selector);
        }

        /// <summary>
        /// <br>Calls JQuery <paramref name="this"/>.wrapAll( <paramref name="outerWrapper"/> ).
        /// The entire <paramref name="this"/> JQuery object HTML structure will be placed 
        /// within the innerMost element of <paramref name="outerWrapper"/>.    </br>
        /// The HTML structure of <paramref name="outerWrapper"/> must contain only a single 
        /// inner most leaf element, which will become the new parent container of <paramref name="this"/>.
        /// </summary>
        public JQueryBox WrapAll(JQueryBox outerWrapper)
        {
            return FuncJQuery("wrapAll", outerWrapper);
        }

        #endregion

        #region Element Manipulation

        public JQueryBox AddClass(string className) => FuncJQueryCallerName(className);

#if PRO
        public JQueryBox AddClasses(string[] classNames) {
            return FuncJQuery("addClass", string.Join(' ', classNames));
        }
#endif

        // Consider: exposing as indexer of Attributes collection. REview Html agility Pack        
        // TODO: Implement attribute that takes a collection of attributes
        /// <summary>
        /// 
        /// Note some values should be retrieved using <c>Property</c> instead of <c>Attribute</c>.
        /// Data-* attributes should be retrieved with <c>.Data</c>. See JQuery documentation.
        /// </summary>
        //public string Attribute(string attributeName) => FuncString("attr", attributeName);
        //public JQueryBox Attribute(string attributeName, string value) => ChainedFunc("attr", attributeName);

        public JQIndexer Attributes { get => attributes; }
        internal JQIndexer attributes;

        //public string Property(string attributeName) => FuncString("prop", attributeName);
        //public JQueryBox Property(string attributeName, string value) => ChainedFunc("prop", attributeName);


        //public JQIndexer Properties { get => properties; }
        //private JQIndexer properties;

        public O PropertyGet<O>(string key)
        {
            return FuncGenericOut<O>("prop", key);
        }

        public void PropertySet<T>(string key, T value)
        {
            //FuncGenericVoid<string, T>("prop", key, value);
            FuncGenericVoid("prop", key, value);
        }

        // TODO: Implement arrays
        // Consider: Dictionary/Indexer

        //public string Style(string propertyName) => FuncString("css", propertyName);
        //public JQueryBox Style(string propertyName, string value) => FuncJQuery("css", propertyName, value);
        //public JQIndexer Styles { get; set; } = new JQIndexer(this, "css");// can't reference this, I have to move this initilizer to the ctor

        public JQIndexer Styles { get => styles; }
        internal JQIndexer styles;




        public bool HasClass(string className)
        {
            return FuncString("hasClass", className) == "true";
        }

        public string Html
        {
            get { return FuncString("html"); }
            set { ChainedFunc("html", CleanHtml(value)); }
        }

        public string Text
        {
            get { return FuncString("text"); }
            set { ChainedFunc("text", value.Replace("'", "\\'")); }
        }

#if PRO
        public decimal Height {
            get { return Convert.ToDecimal(FuncString("height")); }
            set { ChainedFunc("height", value.ToString()); }
        }

        public decimal InnerHeight {
            get { return Convert.ToDecimal(FuncString("innerHeight")); }
            set { ChainedFunc("innerHeight", value.ToString()); }
        }

        public decimal Width {
            get { return Convert.ToDecimal(FuncString("width")); }
            set { ChainedFunc("width", value.ToString()); }
        }

        public decimal InnerWidth {
            get { return Convert.ToDecimal(FuncString("innerWidth")); }
            set { ChainedFunc("innerWidth", value.ToString()); }
        }

        public decimal OuterHeight {
            get { return Convert.ToDecimal(FuncString("outerHeight")); }
            set { ChainedFunc("outerHeight", value.ToString()); }
        }

        public decimal OuterWidth {
            get { return Convert.ToDecimal(FuncString("outerWidth")); }
            set { ChainedFunc("outerWidth", value.ToString()); }
        }

        
        public decimal ScrollLeft {
            get { return Convert.ToDecimal(FuncString("scrollLeft")); }
            set { ChainedFunc("scrollLeft", value.ToString()); }
        }

        public decimal ScrollTop {
            get { return Convert.ToDecimal(FuncString("scrollTop")); }
            set { ChainedFunc("scrollTop", value.ToString()); }
        }
        
        public JQueryBox ToggleClass(string className) {
            return ChainedFunc("toggleClass", className);
        }
        // TODO: Implement ToggleClass overloads


        // TODO: cssNumber
        // TODO: offset
        // TODO: position
#endif
        public JQueryBox RemoveAttribute(string attributeName)
        {
            return ChainedFunc("removeAttr", attributeName);
        }

        public JQueryBox RemoveClass(string className)
        {
            return ChainedFunc("removeClass", className);
        }

        public JQueryBox RemoveProp(string propName)
        {
            return ChainedFunc("removeProp", propName);
        }

        public string Value
        {
            get => FuncString("val");
            set => FuncString("val", value);
        }


        // TODO: .val() can return string, number, or array: https://api.jquery.com/val/#val1

        //public string ValueGet()
        //{
        //    return WebAssemblyRuntime.InvokeJSWithInterop(
        //        $@"return {this}.{_1.jqbj}.val();"
        //    );

        //}

        //public string Value(string value)
        //{
        //    return WebAssemblyRuntime.InvokeJSWithInterop(
        //        $@"return {this}.{_1.jqbj}.val({value});"
        //    );
        //}


        #endregion

        #region Effects

        public JQueryBox Hide() => ChainedFunc("hide");

        #endregion

        #region Traversal - Tree Traversal

        public JQueryBox Children(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox Closest(string selector) => FuncJQueryCallerName(selector);
        public JQueryBox Find(string selector) => FuncJQueryCallerName(selector);
        public JQueryBox Next(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox Parent(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox Prev(string selector = null) => FuncJQueryCallerName(selector);

        // TODO: Misc. Traversal https://api.jquery.com/category/traversing/miscellaneous-traversal/

#if PRO
        /// <summary>Implements Closest() with filter on elements positioned with relative, absolute, or fixed.  Implements jQuery's offsetParent.</summary>
        public JQueryBox ClosestPositioned() => FuncJQuery("offsetParent");
        public JQueryBox Parents(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox ParentsUntil(string stopAtSelector = null, string filterResultsSelector = null) => FuncJQueryCallerName(stopAtSelector, filterResultsSelector);
        public JQueryBox NextAll(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox NextUntil(string stopAtSelector = null, string filterResultsSelector=null) => FuncJQueryCallerName(stopAtSelector, filterResultsSelector);
        public JQueryBox PrevAll(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox PrevUntil(string stopAtSelector = null, string filterResultsSelector = null) => FuncJQueryCallerName(stopAtSelector, filterResultsSelector);
        public JQueryBox Siblings(string selector = null) => FuncJQueryCallerName(selector);
#endif


        #endregion

        #region Traversal - Filtering

        // TODO: Write unit tests

        public JQueryBox First() => FuncJQueryCallerName();
        public JQueryBox Last() => FuncJQueryCallerName();

#if PRO
        public JQueryBox Eq(int index) => FuncJQueryCallerName(index.ToString());
        public JQueryBox Even() => FuncJQueryCallerName();
        public JQueryBox Filter(string selector) => FuncJQueryCallerName(selector);
        
        public JQueryBox Has(string selector) => FuncJQueryCallerName(selector);
        public bool Is(string selector) => FuncString("is", selector) == "true";
        
        //TODO: public JQueryBox Map() => FuncJQueryCallerName();
        public JQueryBox Not(string selector) => FuncJQueryCallerName(selector);
        public JQueryBox Odd() => FuncJQueryCallerName();
        //TODO: public JQueryBox Slice() => FuncJQueryCallerName();
#endif


        #endregion






        // TODO: Some JQuery methods actually return the same instance.  We should consider testing the JS object equality
        // and returning this instead of newBox in those cases and explicitely Dispose of newBox, or not even create the return and default to returning `this`.

        #region Helpers

        // TODO: Review Uno's escapeJS method and similar methods.  Review JQuery's pattern for when embedded <script> is or is not included in HTML manipulation.
        internal static string CleanHtml(string html)
        {
            // Escape single quotes and remove control characters that break append
            return html.Replace("'", "\\'").Replace("\t", "").Replace("\r", "").Replace("\n", "");
        }

        /// <summary>Generic function for JQuery instances methods that </summary>
        /// <example>
        ///   $( ".container" ).after( "<h2>Greetings</h2>" ) );
        /// </example>
        private JQueryBox JQueryFuncHtml(string funcName, string html)
        {
            html = CleanHtml(html);
            //Console.WriteLine(html);
            var newBox = new JQueryBox();
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}('{html}');"
            );
            return newBox;
        }


        /// <example>
        ///   $( "<h2>Greetings</h2>" ).insertAfter( $( ".container" ) );
        /// </example>
        private JQueryBox HtmlFuncJQuery(string funcName, string html)
        {

            html = CleanHtml(html);
            //Console.WriteLine(html);
            var newBox = new JQueryBox();
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{newBox}.{_1.jqbj} = {_1.jQueryRef}('{html}').{funcName}({this}.{_1.jqbj});"
            );
            return newBox;
        }

        /// <summary>Generic function for JQuery instances methods that take string params and return a string. </summary>
        internal string FuncString(string funcName, params string[] parameters)
        {
            string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));

            string result = WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"return {this}.{_1.jqbj}.{funcName}({jsParameters});"
            );
            return result;
        }


        internal O FuncGenericOut<O>(string funcName, object param)
        {
            string stringResult = WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"return {this}.{_1.jqbj}.{funcName}({FormatParam(param)});"
            );
            //Console.WriteLine("Result: " + stringResult);

            Type o = Nullable.GetUnderlyingType(typeof(O));
            if (o == null)
                o = typeof(O);
            //Console.WriteLine("O: " + o.ToString());

            O safeValue = (O)((stringResult == null) ? null : Convert.ChangeType(stringResult, o));
            return safeValue;
        }

        //internal O FuncGenericOut<I, O>(string funcName, I param)
        //{

        //    // TODO: Detect if <I> param is string and escape.  
        //    // TODO: Determine if other types need special formatting such as numbers
        //    //string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));


        //    string stringResult = WebAssemblyRuntime.InvokeJSWithInterop(
        //            $@"return {this}.{_1.jqbj}.{funcName}({FormatParam(param)});"
        //    );

        //    //Console.WriteLine("Result: " + stringResult);


        //    Type o = Nullable.GetUnderlyingType(typeof(O));
        //    if (o == null)
        //        o = typeof(O);

        //    Console.WriteLine("O: " + o.ToString());
        //    O safeValue = (O)((stringResult == null) ? null : Convert.ChangeType(stringResult, o));
        //    //property.SetValue(entity, safeValue, null);


        //    return safeValue;
        //}




        internal void FuncGenericVoid(string funcName, object param1, object param2)
        {

            //Console.WriteLine($"P1: {FormatParam(param1)}");
            //Console.WriteLine($"P2: {FormatParam(param2)}");

            // TODO: Detect if <I> param is string and escape.  
            // TODO: Determine if other types need special formatting such as numbers
            //string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));

            string stringResult = WebAssemblyRuntime.InvokeJSWithInterop(
                $@"return {this}.{_1.jqbj}.{funcName}({FormatParam(param1)},{FormatParam(param2)});"
            );

            //Type o = Nullable.GetUnderlyingType(typeof(O));
            //O safeValue = (O)((stringResult == null) ? null : Convert.ChangeType(stringResult, o));
            //property.SetValue(entity, safeValue, null);


            //return safeValue;
        }


        //internal void FuncGenericVoid<I1, I2>(string funcName, I1 param1, I2 param2)
        //{

        //    //Console.WriteLine($"P1: {FormatParam(param1)}");
        //    //Console.WriteLine($"P2: {FormatParam(param2)}");



        //    // TODO: Detect if <I> param is string and escape.  
        //    // TODO: Determine if other types need special formatting such as numbers
        //    //string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));

        //    string stringResult = WebAssemblyRuntime.InvokeJSWithInterop(
        //        $@"return {this}.{_1.jqbj}.{funcName}({FormatParam(param1)},{FormatParam(param2)});"
        //    );

        //    //Type o = Nullable.GetUnderlyingType(typeof(O));
        //    //O safeValue = (O)((stringResult == null) ? null : Convert.ChangeType(stringResult, o));
        //    //property.SetValue(entity, safeValue, null);


        //    //return safeValue;
        //}









        /// <summary>
        /// Format types suitable for passing to javascript.
        /// </summary>        
        //private string FormatParam<T>(T param)
        //{

        //    string str = null;
        //    switch (param)
        //    {
        //        case string s:

        //    //if (typeof(T) == typeof(string))// TODO: Test with null?
        //    //{
        //    //str = param as string;
        //    str = $"'{ s.Replace("'", "\\'") }'";
        //            break;
        //        //}
        //        //else // all other types use .ToString
        //        //{
        //        default:
        //    str = param.ToString();
        //            break;
        //    //}
        //}

        //    return str;
        //}

        // TODO: Determine if other types need special formatting such as numbers
        private object FormatParam(object param)
        {
            string str = null;

            if (param is string s)
                str = $"'{s.Replace("'", "\\'")}'";
            else if (param is bool b)
                str = (b ? "true" : "false");
            else if (param == null)
                str = "null";
            else if (param is IJSObject)
                return param;
            else
                str = param.ToString();

            return str;
        }

        //private string FormatParameters(object p1, object p2)
        //{
        //    return $"{ FormatParam(p1) },{ FormatParam(p2) }";
        //}



        internal JQueryBox FuncJQuery(string funcName, JQueryBox jQueryBox, params string[] parameters)
        {
            JQueryBox newBox = new JQueryBox();

            string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));
            if (!jsParameters.IsNullOrWhiteSpace())
                jsParameters = "," + jsParameters;

            WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jQueryBox}.{_1.jqbj}{jsParameters});"
            );
            return newBox;
        }

        internal JQueryBox FuncJQuery(string funcName, params string[] parameters)
        {
            JQueryBox newBox = new JQueryBox();
            //Console.WriteLine(funcName);


            string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));
            //Console.WriteLine(jsParameters);
            WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jsParameters});"
            );
            return newBox;
        }

        internal JQueryBox FuncJQueryCallerName(string parameter = null, string parameter2 = null, [CallerMemberName] string funcName = null)
        {
            //Console.WriteLine("Caller: '" + funcName + "'");

            return FuncJQuery(Char.ToLowerInvariant(funcName[0]) + funcName.Substring(1), parameter, parameter2);
        }

        //private JQueryBox FuncJQueryCallerName2(FormattableString jsParameters = null, [CallerMemberName] string funcName = null)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, )

        //    // TODO: Use StringBuilder.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, formatString, params );
        //    // We can make replacements from formattableString.Arguments and create .params based on types

        //    Console.WriteLine("jsParams Format: '" + jsParameters.Format + "'");
        //    Console.WriteLine("jsParams ToString: '" + jsParameters.ToString() + "'");// CONSIDER: Uno ToStringInvariant
        //    Console.WriteLine("Caller: '" + funcName + "'");
        //    funcName = Char.ToLowerInvariant(funcName[0]) + funcName.Substring(1);

        //    JQueryBox newBox = new JQueryBox();
        //    string jsParamsString = jsParameters.Format;

        //    WebAssemblyRuntime.InvokeJSWithInterop(
        //            $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jsParamsString});"
        //    );
        //    return newBox;
        //}

        /// <summary> Does not create a new instance. </summary>
        internal JQueryBox ChainedFunc(string funcName, params string[] parameters)
        {
            string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));

            WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"{this}.{_1.jqbj}.{funcName}({jsParameters});"
            );
            return this;
        }

        internal JQueryBox ChainedFuncCallerName(string parameter = null, string parameter2 = null, [CallerMemberName] string funcName = null)
        {
            return ChainedFunc(Char.ToLowerInvariant(funcName[0]) + funcName.Substring(1), parameter, parameter2);
        }


        public class JQIndexer
        {
            private readonly JQueryBox jQueryBox;
            private readonly string funcName;

            public JQIndexer(JQueryBox jQueryBox, string funcName)
            {
                this.jQueryBox = jQueryBox;
                this.funcName = funcName;
            }

            public string this[string name]
            {
                get => jQueryBox.FuncString(funcName, name);
                set => jQueryBox.FuncString(funcName, name, value);
            }
        }


        // Debugging: C# methods that are exposed to JS must meet this criteria
        private static void EchoValidMethods()
        {
            var methods = typeof(JQueryBox)
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(method => !method.GetParameters().Any(p => p.ParameterType != typeof(string))) // we support only string parameters for now
                    .ToList();

            Console.WriteLine(string.Join(", ", methods.Select(m => m.Name)));
        }


#if PRO
        internal string GenGetInstance()
        {
            var jsHandle = _jsHandle(handle);
            return $"WasmGenerator.{ GetType().Name}.getInstance(0,{jsHandle})";
        }



        // Get jsHandle ID via reflection
        static long _jsHandle(JSObjectHandle jsHandle)
        {
            return GetPrivate<long>(jsHandle);
        }

        static IntPtr _managedHandle(JSObjectHandle jsHandle)
        {
            return GetPrivate<IntPtr>(jsHandle);
        }

        // Reflection to access private fields needed for getting JS handle ID when making instance interopt calls
        static T GetPrivate<T>(object obj, [CallerMemberName] string callerMemberName = null)
        {
            var field = obj.GetType().GetField(callerMemberName, BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            var value = (T)field.GetValue(obj);
            return value;
        }



        
        private JQueryBox Example(string propertyName, string value)
        {
            var newBox = new JQueryBox();
            // Call Find on the current instance, and add the new instance returned from find()
            string result = WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.css('{propertyName}','{value}');
                    if({newBox} == {this})
                        return 'same';
                "
            );

            if (result == "same") // if a new instance was not created, then discard our wrapper and returnt he existing object
            {
                newBox.handle.Dispose();
                return this;
            }
            else
                return newBox;

            // InvokeJSWithInterop has special behavior when used with string interpolation. 
            // It is able to inspect the interpolated string via FormatableString, which allows it to detect
            // the references to {newBox} and {this} as IJSObject instances, and replace them with .getInstance calls against the
            // client side activeInstances array.  The resulting javascript is an immediately invoked function expression. 
            // This pattern is used simply to execute the snippet and ensure the local variables don't pollute global.
            // Since {propertyName} and {value} are string types, it performs no special replacements and standard string interpolation behavior applies.

            //(function() {
            //    var __parameter_0 = WasmGenerator.JQueryBox.getInstance("162", "4");
            //    var __parameter_1 = WasmGenerator.JQueryBox.getInstance("130", "3");
            //    __parameter_0.{_1.jqbj} = __parameter_1.{_1.jqbj}.css('color', 'red');
            //    return "ok";
            //})();


        }
#endif


        //// This method demonstrates a brute force approach to using the JS handles. It might be useful in edge cases.
        //public JQueryBox Find(string selector)
        //{


        //    //long currentJsHandle = _jsHandle(Handle);

        //    //var newBox = new JQueryBox();
        //    //var newJsHandle = _jsHandle(newBox.Handle);

        //    // Call Find on the current instance, and add the new instance returned from find()
        //    //WebAssemblyRuntime.InvokeJS($@"
        //    //    (function() {{
        //    //        var current = WasmGenerator.{GetType().Name}.getInstance(0,{currentJsHandle});
        //    //        var newObj = WasmGenerator.{GetType().Name}.getInstance(0,{newJsHandle});
        //    //        newObj.{_1.jqbj} = current.{_1.jqbj}.find('{selector}');
        //    //        return 'OK';
        //    //    }})();"
        //    //);

        //    return FuncJQuery("find", selector);
        //}


        #endregion

    }


}
