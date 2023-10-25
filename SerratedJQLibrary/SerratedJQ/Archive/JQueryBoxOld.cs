using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
//using Uno.Extensions;
using Uno.Foundation;
using Uno.Foundation.Interop;
using static System.Console;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SerratedSharp.SerratedJQ.Archive;

namespace SerratedSharp.SerratedJQ.Archive
{

    [Obsolete("Use JQuery and JQueryObject instead",UrlFormat = "https://github.com/SerratedSharp/SerratedJQ#010")]
    public partial class JQueryBox : IJSObject
    {
        private const string JSClassName = "InternalSerratedJQBox";
        private static int keep = 2;
        //static JQueryBox()
        //{

        //    // Add javascript declaration that is used by WebAssembly but was declared in incorrect Uno Platform project.
        //    WebAssemblyRuntime.InvokeJS(_1.ManagedObjectJavascriptDispatcherDeclaration);

        //    WebAssemblyRuntime.InvokeJS(@$"     
        //            window.{JSClassName} = window.{JSClassName} || {{}};
        //            window.{JSClassName}.UnpinEventListener = Module.mono_bind_static_method('[SerratedSharp.SerratedJQ] SerratedSharp.SerratedJQ.JQueryBox:UnpinEventListener');                         
        //        ");

        //    //WebAssemblyRuntime.InvokeJS(SerratedSharp.JSInteropHelpers.EmbeddedFiles.ObserveRemovedJs);


        //    // Alternative export approaches

        //    //CallbacksHelper.Export(jsMethodName: "UnpinEventListener", () => JQueryBox.UnpinEventListener());

        //    //string exportScript = 
        //    //    @"var Serrated = window.Serrated || {};
        //    //        (function (Serrated) {var Callbacks = Serrated.Callbacks || {};
        //    //            Callbacks.UnpinEventListener = function(){
        //    //                InternalSJQ.Listener('UnpinEventListener');
        //    //             };
        //    //             Serrated.Callbacks = Callbacks;
        //    //        })(Serrated = window.Serrated || (window.Serrated = {}));";

        //    //  WebAssemblyRuntime.InvokeJS(@"
        //    //      SerratedSharp.SerratedJQ.JQueryBox.prototype.ClickCallback = function(eEncoded, eventType) {
        //    //          var parameters = {'eEncoded': eEncoded, 'eventType': eventType};
        //    //          var serializedParameters = JSON.stringify(parameters);
        //    //          //method name passed to .dispatch must be a public instance method (cannot be an explicit interface)
        //    //          Uno.Foundation.Interop.ManagedObject.dispatch(this.__managedHandle, 'ClickCallback', serializedParameters);
        //    //      };");

        //    if (keep == 1)// Prevent these methods from being removed by ILLinker at compile time. The condition being false prevents these from actually being called at runtime, but indeterministic so that linker doesn't remove them. 
        //    {
        //        var keeper = new JQueryBox();
        //        keeper.InternalClickCallback("", "");
        //        keeper.InternalEventCallback("", "");
        //        keeper.InternalInputCallback("", "");
        //    }

        //}

        // This constructor is internal because callers need to use factory methods such as .Select or FromHtml so that we can properly initialize handles.
        internal JQueryBox()
        {
            handle = JSObjectHandle.Create(this);
            styles = new JQIndexer(this, "css");
            //properties = new JQIndexer(this, "prop");
            attributes = new JQIndexer(this, "attr");
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

        #region Static/Factory Methods

        /// <summary>
        /// Calls $(document).find('selector')
        /// </summary>
        /// <param name="selector"></param>
        /// <returns>A JQueryBox wrapping the JQuery collection returned by .find()</returns>
        public static JQueryBox Select(string selector)
        {
            var box = new JQueryBox();

            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{box}.{_1.jqbj} = {_1.jQueryRef}(document).find({FormatParam(selector)});"
                );
            return box;
        }


        /// <summary>
        /// Note internally this routes to parseHTML(string) with keepScripts= false which strips script tags.  This doesn't cover all XSS scenarios such as sanatizing attribute embedded scripts.
        /// </summary>
        public static JQueryBox FromHtml(string html)
        {
            var box = new JQueryBox();
            // Call parseHTML to get DOM nodes, then convert to JQ collection
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{box}.{_1.jqbj} = {_1.jQueryRef}( {_1.jQueryRef}.parseHTML({FormatParam(html)}, undefined, false));"
                );

            return box;
        }

        // TODO implement optional params jQuery.parseHTML( data [, JQ context ] [, bool keepScripts ] )
        /// <summary>
        /// Similar to FromHtml, but exposes option to keep script tags(defaults to false).  This doesn't cover all XSS scenarios such as sanatizing attribute embedded scripts.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keepScripts"></param>
        /// <returns></returns>
        public static JQueryBox ParseHtml(string html, bool keepScripts = false)
        {
            var box = new JQueryBox();
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{box}.{_1.jqbj} = {_1.jQueryRef}( {_1.jQueryRef}.parseHTML({FormatParam(html)}, undefined, {FormatParam(keepScripts)}) );"
                );
            return box;
        }

        #endregion

        #region Events

        // TODO: Implement static .ready?

        // Strongly typed event handler signature 
        public delegate void JQueryEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
            where TSender : JQueryBox;

        private JQueryEventHandler<JQueryBox, object> onClick;
        public event JQueryEventHandler<JQueryBox, object> OnClick
        {
            add
            {
                if (onClick == null)// if first event subscriber
                    this.InnerOn("click", nameof(InternalClickCallback));// then add JQuery listener

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
        public event JQueryEventHandler<JQueryBox, object> OnInput
        {
            add
            {
                if (onInput == null)// if first event subscriber
                    this.InnerOn("input", nameof(InternalInputCallback));// then add JQuery listener

                onInput += value;
            }
            remove
            {
                onInput -= value;
                if (onInput == null) // if last subscriber removed, then remove JQuery listener
                    this.InnerOff("input");
            }
        }

        // generic event susbcription
        private Dictionary<string, JQueryEventHandler<JQueryBox, object>> onEvent = new Dictionary<string, JQueryEventHandler<JQueryBox, object>>();

        /// <summary>
        /// Subscribe to an HTML DOM event by HTML DOM event name.
        /// <example><code>
        /// var someButton = JQueryBox.Select("#someButtonId");
        /// someButton.On("click", SomeButton_OnClick);// Note if the reference someButton(event publisher) is garbage collected, then the listener will no longer receive events.
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
            if (!onEvent.ContainsKey(eventName) || onEvent[eventName] == null)// if first event subscriber
            {
                this.InnerOn(eventName, nameof(InternalEventCallback));// then add JQuery listener 
            }

            onEvent[eventName] += handler;
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
        /// Called only for first subscriber added. Subscribes to JS JQuery event.  Pins this object to eventOBjectsByPointer 
        /// </summary>
        private JQueryBox InnerOn(string events, string listenerFunctionName)
        {
            // {eventListener} is the JQueryBox box, and .{listeneerFunctionName} is usually "InternalEventCallback".
            // Uno WebAssembly generates a javascript wrapper for this managed type.
            // From the perspective of javascript there is a javascript method JQueryBox.InternalEventCallback which proxies calls from JS back to the managed C# object
            // The javascript JQueryBox.InternalEventCallback is registered as the handler for the event, and so proxies events firing back to this managed C# object
            // Note the JS object isn't literally "JQueryBox", InvokeJSWithInteropt takes interpolation parameters of type IJSObject and translates that into an activeObject[id] dereference.

            WebAssemblyRuntime.InvokeJSWithInterop($@"
                var handler = function(e){{
                    console.log('On Fired');
                    var eEncoded = btoa(JSON.stringify(e));
                    {this}.{listenerFunctionName}(eEncoded, e.type);
                }}.bind({this});

                {this}.{_1.jqbj}.on({FormatParam(events)}, handler);
            ");

            PinEventListener();
            return this;
        }

        /// <summary>
        /// Called only when last subscriber removed. Unsubscribes to JS JQuery event.  Unpins this object from eventObjectsByPointer 
        /// </summary>
        private JQueryBox InnerOff(string events)
        {
            //var newBox = new JQueryBox();
            // CONSIDER: It's possible to unsubscribe only our handler, but we can't use an anonymous handler above.
            // TODO: This removes all events instead of just ours
            //{newBox}.{_1.jqbj} = 

            WebAssemblyRuntime.InvokeJSWithInterop($@"
                    {this}.{_1.jqbj}.off({FormatParam(events)});
                ");

            TryUnpinEventListener();

            return this;//newBox;
        }

        /// <summary>
        /// Do not use. Exposed for Javascript->C# interop.
        /// </summary>        
        [Obsolete("For internal use only.")]
        public void InternalEventCallback(string eventB64Encoded, string eventType)
        {
            dynamic eventData = EncodedEventToDynamic(eventB64Encoded);
            onEvent[eventType]?.Invoke(this, eventData);
        }

        // Handles the JQuery event and fires the C# event, they have to be public for them to be invokable from Javascript delegates
        [Obsolete("For internal use only.")]
        public void InternalClickCallback(string eventB64Encoded, string eventType)
        {
            dynamic eventData = EncodedEventToDynamic(eventB64Encoded);
            onClick?.Invoke(this, eventData);
        }

        [Obsolete("For internal use only.")]
        public void InternalInputCallback(string eventB64Encoded, string eventType)
        {
            dynamic eventData = EncodedEventToDynamic(eventB64Encoded);
            onInput?.Invoke(this, eventData);
        }

        private dynamic EncodedEventToDynamic(string encodedEvent)
        {
            // TODO: We're only base 64 encoding this to get around a bug in Uno Platform when we try to use JSON.  Recent versions have fixed this so can likely be removed
            var base64EncodedBytes = System.Convert.FromBase64String(encodedEvent);
            string jsonString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            dynamic eventData = JsonConvert.DeserializeObject<dynamic>(jsonString);
            return eventData;
        }

        #endregion

        #region Event Listener Pinning - Memory Management

        // Called by InnerOn to "pin" this object if it has any event subscribers, to prevent it from being garbage collected.
        // This Pubisher object is also the listener of the javascript event.  The listenerFunctionName handles passing the event through to the C# event.
        // HTML Publisher Element -> JQuery JS Handler -> JQBox wrapping HTML Element(eventObject pinned, listens to JS event, publishes C# event)-> C# Event Listener 
        // This is essentially an unmanaged object -> managed object reference problem,
        // where the GC does not see the reference from unamanaged JS and thus could GC our object when it is still listening for JS events of a DOM element.

        // eventObjectsByPointer prevents the JQ Box from being garbage collected since it may have no managed references while the DOM element is still publishing events to it.
        // Indexed by ptr of the managed object.
        public static Dictionary<IntPtr, JQueryBox> eventObjectsByPointer = new Dictionary<IntPtr, JQueryBox>();

        private System.IntPtr PinEventListener()
        {
            System.IntPtr ptr = _managedHandle(this.handle);
            if (!eventObjectsByPointer.ContainsKey(ptr))
            {
                eventObjectsByPointer[ptr] = this; // prevent GC of this, indexed by it's pointer so it can be looked up later by the weak map

                // Iterate over all HTML elements and add them to WeakMap, which the Mutation Observer uses to notify the managed object when the HTML elemnt is removed
                // The key of the map is the HTML element(node) which is what the mutation observer receives during a removed element event,
                // and value is the managed pointer so it can call back to us 
                WebAssemblyRuntime.InvokeJSWithInterop($@"
                    {this}.{_1.jqbj}.each( function(index) {{ nodePtrs.set(this,'{ptr}'); }} );
                ");
                //nodePtrs.set({this}.{_1.jqbj}[0],'{ptr}');
            }
            return ptr;
        }

        // UnpinEventListener handles notifications of removed DOM/HTML elements to determine if listeners can be unpinned
        // TryUnpinEventListener handles event unssubcriptions to determine if there's no subscribers left, in which case we can unpin to make eligible for GC

        // Called by MutationObserver when DOM HTML element is removed which was related to managed object by pointer
        // TODO: Consider if this JQueryBox used a query that matched multiple elements (I believe this is handled now, write unit test for scenario)
        private static void UnpinEventListener(string pointerString)
        {
            //Console.WriteLine("Unpin called for pointer: " + pointerString);
            var handle = PointerStringToObject(pointerString) as JSObjectHandle;
            var weakRef = _target(handle);
            object jqBoxObj;
            weakRef.TryGetTarget(out jqBoxObj);
            JQueryBox jqBox = jqBoxObj as JQueryBox;

            // determine if all elements in this jQuery collection have been removed ( $element.parents('html').length > 0 )
            string countElementsInDom = WebAssemblyRuntime.InvokeJSWithInterop($@"
                    return {jqBox}.{_1.jqbj}.parents('html').length;  
            ");
            //Console.WriteLine($"countElementsInDom for ptr{ pointerString }: {countElementsInDom}");
            // if last HTML in jquery collection removed, then remove the pinned managed object
            if (int.TryParse(countElementsInDom, out int count) && count == 0)
            {
                //Console.WriteLine("Item remove for pointer: " + pointerString);
                //Console.WriteLine("MemLoad RemoveBefore: " + JQueryBox.eventObjectsByPointer.Count);
                IntPtr ptr = new IntPtr(Convert.ToInt32(pointerString));
                bool isRemoved = eventObjectsByPointer.Remove(ptr);
                //Console.WriteLine("Pointer removed? " + isRemoved);
                //Console.WriteLine("MemLoad RemoveAfter: " + JQueryBox.eventObjectsByPointer.Count);
            }
        }

        // Conditionally unpins if no remaining events, called during event unsubscription in InnerOff to unpin object if last listener removed
        private void TryUnpinEventListener()
        {
            //Console.WriteLine("TryUnpin called");
            // if there are not any events with handlers, then unpin this object
            if (!HasListeners())
            {
                System.IntPtr ptr = _managedHandle(this.handle);
                bool isRemoved = eventObjectsByPointer.Remove(ptr);
                //Console.WriteLine("TryUnpin pointer removed? " + isRemoved);
            }
        }

        // determines if any listeners are subscribed to events on this instance, used to determine if this instance can be un pinned
        public bool HasListeners()
        {
            return this.onEvent.Any(e => e.Value != null) || onInput != null || onClick != null;
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
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{this}.{_1.jqbj}.data({FormatParam(key)},'{managedHandle}');"
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
                $@"return {this}.{_1.jqbj}.data({FormatParam(key)});"
            );
            object managedObject = PointerStringToObject(ptrStr);
            return managedObject;
        }

        private static object PointerStringToObject(string ptrStr)
        {
            var intr = Convert.ToInt32(ptrStr);// TODO: ToInt32 might be wrong on 64bit systems, may truncate
            //Console.WriteLine(intr);
            IntPtr pntr = new IntPtr(intr);
            //Console.WriteLine(pntr);
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
                $@" var ptr = {this}.{_1.jqbj}.data({FormatParam(key)});
                    {this}.{_1.jqbj}.removeData({FormatParam(key)});
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
            // TODO: Determine if this properly handles nullable types.  Also review FuncGenericOut to determine if the casting approach is appropriate here

            string dataString;
            if (value.GetType() == typeof(string) || value.GetType().IsValueType)
            {
                dataString = value.ToString();
            }
            else
            {
                dataString = System.Text.Json.JsonSerializer.Serialize(value);
            }

            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{this}.{_1.jqbj}.data({FormatParam(key)},{FormatParam(dataString)});"
            );

            return this;
        }

        public JQueryBox DataRemove(string key)
        {
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{this}.{_1.jqbj}.removeData({FormatParam(key)});"
            );
            return this;
        }


        /// <typeparam name="T">Must be the same type as value passed to DataAdd()</typeparam>        
        public T DataGet<T>(string key)
        {

            string dataString = WebAssemblyRuntime.InvokeJSWithInterop(
                $@"return {this}.{_1.jqbj}.data({FormatParam(key)});"
            );

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(dataString, typeof(T));
            }
            else if (typeof(T).IsValueType)
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

        public JQueryBox AddClasses(string[] classNames)
        {
            return FuncJQuery("addClass", string.Join(' ', classNames));
        }

        // Consider: exposing as indexer of Attributes collection. REview Html agility Pack        
        // TODO: Implement attribute that takes a collection of attributes
        /// <summary>
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
            set { ChainedFunc("html", value); }
        }

        public string Text
        {
            get { return FuncString("text"); }
            set { ChainedFunc("text", value); }
        }

        public decimal Height
        {
            get { return Convert.ToDecimal(FuncString("height")); }
            set { ChainedFunc("height", value.ToString()); }
        }

        public decimal InnerHeight
        {
            get { return Convert.ToDecimal(FuncString("innerHeight")); }
            set { ChainedFunc("innerHeight", value.ToString()); }
        }

        public decimal Width
        {
            get { return Convert.ToDecimal(FuncString("width")); }
            set { ChainedFunc("width", value.ToString()); }
        }

        public decimal InnerWidth
        {
            get { return Convert.ToDecimal(FuncString("innerWidth")); }
            set { ChainedFunc("innerWidth", value.ToString()); }
        }

        public decimal OuterHeight
        {
            get { return Convert.ToDecimal(FuncString("outerHeight")); }
            set { ChainedFunc("outerHeight", value.ToString()); }
        }

        public decimal OuterWidth
        {
            get { return Convert.ToDecimal(FuncString("outerWidth")); }
            set { ChainedFunc("outerWidth", value.ToString()); }
        }

        public decimal ScrollLeft
        {
            get { return Convert.ToDecimal(FuncString("scrollLeft")); }
            set { ChainedFunc("scrollLeft", value.ToString()); }
        }

        public decimal ScrollTop
        {
            get { return Convert.ToDecimal(FuncString("scrollTop")); }
            set { ChainedFunc("scrollTop", value.ToString()); }
        }

        public JQueryBox ToggleClass(string className)
        {
            return ChainedFunc("toggleClass", className);
        }
        // TODO: Implement ToggleClass overloads

        // TODO: cssNumber
        // TODO: offset
        // TODO: position

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

        /// <summary>Implements Closest() with filter on elements positioned with relative, absolute, or fixed.  Implements jQuery's offsetParent.</summary>
        public JQueryBox ClosestPositioned() => FuncJQuery("offsetParent");
        public JQueryBox Parents(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox ParentsUntil(string stopAtSelector = null, string filterResultsSelector = null) => FuncJQueryCallerName(stopAtSelector, filterResultsSelector);
        public JQueryBox NextAll(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox NextUntil(string stopAtSelector = null, string filterResultsSelector = null) => FuncJQueryCallerName(stopAtSelector, filterResultsSelector);
        public JQueryBox PrevAll(string selector = null) => FuncJQueryCallerName(selector);
        public JQueryBox PrevUntil(string stopAtSelector = null, string filterResultsSelector = null) => FuncJQueryCallerName(stopAtSelector, filterResultsSelector);
        public JQueryBox Siblings(string selector = null) => FuncJQueryCallerName(selector);

        #endregion

        #region Traversal - Filtering

        // TODO: Write unit tests

        public JQueryBox First() => FuncJQueryCallerName();
        public JQueryBox Last() => FuncJQueryCallerName();
        public JQueryBox Eq(int index) => FuncJQueryCallerName(index.ToString());
        public JQueryBox Even() => FuncJQueryCallerName();
        public JQueryBox Filter(string selector) => FuncJQueryCallerName(selector);

        public JQueryBox Has(string selector) => FuncJQueryCallerName(selector);
        public bool Is(string selector) => FuncString("is", selector) == "true";

        //TODO: public JQueryBox Map() => FuncJQueryCallerName();
        public JQueryBox Not(string selector) => FuncJQueryCallerName(selector);
        public JQueryBox Odd() => FuncJQueryCallerName();
        //TODO: public JQueryBox Slice() => FuncJQueryCallerName();

        #endregion

        // TODO: Some JQuery methods actually return the same instance.  We should consider testing the JS object equality, see "Example()"
        // and returning this instead of newBox in those cases and explicitely Dispose of newBox, or not even create the return and default to returning `this`.

        #region Helpers

        internal static string CleanHtml(string html)
        {
            // Escape single quotes and remove control characters that break append
            return WebAssemblyRuntime.EscapeJs(html).Replace("'", @"\'");
        }

        /// <summary>Generic function for JQuery instances methods that </summary>
        /// <example>
        ///   $( ".container" ).after( "<h2>Greetings</h2>" ) );
        /// </example>
        private JQueryBox JQueryFuncHtml(string funcName, string html)
        {
            var newBox = new JQueryBox();
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({FormatParam(html)});"
            );
            return newBox;
        }

        /// <example>
        ///   $( "<h2>Greetings</h2>" ).insertAfter( $( ".container" ) );
        /// </example>
        private JQueryBox HtmlFuncJQuery(string funcName, string html)
        {
            html = CleanHtml(html);
            var newBox = new JQueryBox();
            WebAssemblyRuntime.InvokeJSWithInterop(
                $@"{newBox}.{_1.jqbj} = {_1.jQueryRef}({FormatParam(html)}).{funcName}({this}.{_1.jqbj});"
            );
            return newBox;
        }

        /// <summary>Generic function for JQuery instances methods that take string params and return a string. </summary>
        internal string FuncString(string funcName, params string[] parameters)
        {
            string jsParameters = string.Join(",", parameters.Select(p => $"{FormatParam(p)}"));

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

            Type o = Nullable.GetUnderlyingType(typeof(O));
            if (o == null)
                o = typeof(O);

            O safeValue = (O)((stringResult == null) ? null : Convert.ChangeType(stringResult, o));
            return safeValue;
        }

        internal void FuncGenericVoid(string funcName, object param1, object param2)
        {
            string stringResult = WebAssemblyRuntime.InvokeJSWithInterop(
                $@"return {this}.{_1.jqbj}.{funcName}({FormatParam(param1)},{FormatParam(param2)});"
            );
        }

        private static object FormatParam(object param)
        {
            string str = null;

            if (param is string s)
                str = "'" + WebAssemblyRuntime.EscapeJs(s).Replace("'", @"\'") + "'";
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

        internal JQueryBox FuncJQuery(string funcName, JQueryBox jQueryBox, params string[] parameters)
        {
            JQueryBox newBox = new JQueryBox();

            string jsParameters = string.Join(",", parameters.Select(p => $"{FormatParam(p)}"));
            if (!jsParameters.IsNullOrWhiteSpace())
                jsParameters = "," + jsParameters;// if any parameters being added, then comma preceding them to seperate from first parameter 

            WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jQueryBox}.{_1.jqbj}{jsParameters});"
            );
            return newBox;
        }

        internal JQueryBox FuncJQuery(string funcName, params string[] parameters)
        {
            JQueryBox newBox = new JQueryBox();
            string jsParameters = string.Join(",", parameters.Select(p => $"{FormatParam(p)}"));

            WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jsParameters});"
            );
            return newBox;
        }

        internal JQueryBox FuncJQueryCallerName(string parameter = null, string parameter2 = null, [CallerMemberName] string funcName = null)
        {
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
            string jsParameters = string.Join(",", parameters.Select(p => $"{FormatParam(p)}"));

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

        internal string GenGetInstance()
        {
            var jsHandle = _jsHandle(handle);
            return $"WasmGenerator.{GetType().Name}.getInstance(0,{jsHandle})";
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

        static WeakReference<object> _target(JSObjectHandle jsHandle)
        {
            return GetPrivate<WeakReference<object>>(jsHandle);
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

            if (result == "same") // if a new instance was not created, then discard our wrapper and return the existing object
            {
                newBox.handle.Dispose();
                return this;
            }
            else
                return newBox;


        }



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

