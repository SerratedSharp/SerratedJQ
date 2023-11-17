
// TODO: Remove superflous method chaining
//-Removes superfluous method chaining:
//    -This refers to method chaining present in jQuery where the return from the method is always the same object reference, which is only used to facilitate chaining.
//    - It's not really a common pattern in C# except for builder/fluent or unit of work APIs.
//    - It creates ambiguity between methods where the return type is the result of the method operation, versus where the same object is being returned only to facilitate method chaining and doesn't need to be captured.  In these cases sometimes it's not clear if the operation mutated the original object reference, or if the original object is left as is and the return value is a new result of the operation.
//    -Now it's clear if a modifying operation returns void, then you know the original object was mutated.  If the operation returns a different object, then you know the original object was not mutated and you must capture the return value for the result.
//    - Eliminates unnecessary object allocations.
//    - Chaining is still possible with methods such as .Find() and .Children() since each call returns a different JQuery object/collection, and allows iterative navigation/filtering of the DOM.



//using System;
//using System.Collections.Generic;
//using static System.Console;
//using System.Linq;
//using System.Dynamic;
//using Newtonsoft.Json;
//using System.Runtime.InteropServices.JavaScript;
//using SerratedSharp.JSInteropHelpers;
//using Params = SerratedSharp.JSInteropHelpers.ParamsHelpers;

//namespace SerratedSharp.SerratedJQ;

//// Wrapper that references a JQuery instance object, i.e. the collection returned from selectors/queries
//[Obsolete("Work in progress: The API for this interface may change. Use JQueryPlain and JQueryPlainObject instead if you want to minimize impact of future changes.")]
//public class JQueryObject : IJSObjectWrapper<JQueryObject>//, IJQueryContentParameter
//{
//    internal JSObject jsObject;// reference to the jQuery javascript interop object
//    public JSObject JSObject { get { return jsObject; } }

//    // instances can only be created thru factory methods like Select()/ParseHtml()
//    internal JQueryObject() { 
//        Attributes = new JQIndexer<JQueryObject, string, string>(this, "attr");
//        Styles = new JQIndexer<JQueryObject, string, string>(this, "css");
//    }

//    // Used where we need to create a wrapper around a JSObject reference
//    public JQueryObject(JSObject jsObject):this() { this.jsObject = jsObject; }

//    // This static factory method defined by the IJSObjectWrapper enables generic code such as CallJSOfSameNameAsWrapped to automatically wrap JSObjectsWrap
//    static JQueryObject IJSObjectWrapper<JQueryObject>.WrapInstance(JSObject jsObject)
//    {
//        return new JQueryObject(jsObject);
//    }

//    /// <summary>
//    /// Copy JQueryPlainObject reference to new JQueryObject to allow access to alternativge API semantics.
//    /// </summary>
//    public JQueryObject(Plain.JQueryPlainObject jQueryObject)
//    {
//        jsObject = jQueryObject.JSObject;
//    }

//    #region Traversal - Filtering - https://api.jquery.com/category/traversing/filtering/

//    public JQueryObject First() => this.CallJSOfSameNameAsWrapped();
//    public JQueryObject Last() => this.CallJSOfSameNameAsWrapped();
//    public JQueryObject Eq(int index) => this.CallJSOfSameNameAsWrapped(index);
//    public JQueryObject Slice(int start, int end) => this.CallJSOfSameNameAsWrapped(start, end);
//    public JQueryObject Filter(string selector) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Has(string selector) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Not(string selector) => this.CallJSOfSameNameAsWrapped(selector);
//    public bool Is(string selector) => this.CallJSOfSameName<bool>(selector);
//    public JQueryObject Odd() => this.CallJSOfSameNameAsWrapped();
//    public JQueryObject Even() => this.CallJSOfSameNameAsWrapped();
//    //Map only takes a predicate: https://api.jquery.com/map/#map-callback
//    //public JQueryObject Map(

//    #endregion
//    #region Traversal - Tree Traversal - https://api.jquery.com/category/traversing/tree-traversal/

//    public JQueryObject Children(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Closest(string selector) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Find(string selector) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Next(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject NextAll(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject NextUntil(string stopAtSelector = null, string filterResultsSelector = null)
//        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
//    public JQueryObject OffsetParent() => this.CallJSOfSameNameAsWrapped();
//    public JQueryObject Parent(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Parents(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject ParentsUntil(string stopAtSelector = null, string filterResultsSelector = null)
//        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
//    public JQueryObject Prev(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject PrevAll(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject PrevUntil(string stopAtSelector = null, string filterResultsSelector = null)
//        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
//    public JQueryObject Siblings(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);

//    #endregion
//    #region Traversal - Miscellaneous - https://api.jquery.com/category/traversing/miscellaneous-traversal/

//    public JQueryObject Add(string selector) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Add(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
//    public JQueryObject Add(string selector, JQueryObject context) => this.CallJSOfSameNameAsWrapped(selector, context);
//    public JQueryObject AddBack(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Contents() => this.CallJSOfSameNameAsWrapped();
//    public JQueryObject End() => this.CallJSOfSameNameAsWrapped();

//    #endregion

//    #region General Attributes - https://api.jquery.com/category/attributes/general-attributes/

//    public JQIndexer<JQueryObject, string, string> Attributes { get; }
//    //public JQIndexer<JQueryObject,string,double> AttributesAsDouble { get; }
//    //public JQIndexer<JQueryObject, string, int> AttributesAsInt { get; }
//    //public JQIndexer<JQueryObject, string, bool> AttributesAsBool { get; }

//    public string Attr(string attributeName) => this.CallJSOfSameName<string>(attributeName);
//    public void Attr(string attributeName, string value) => this.CallJSOfSameName<object>(attributeName, value);
//    //public void Attr(string attributeName, bool value) => this.CallJSOfSameName<object>(attributeName, value);        
//    public void Attr(string attributeName, int? value) => this.CallJSOfSameName<object>(attributeName, value);
//    public void Attr(string attributeName, double? value) => this.CallJSOfSameName<object>(attributeName, value);
//    //public void Attr(string attributeName, object value) => this.CallJSOfSameName<object>(attributeName, value);
//    public void RemoveAttr(string attributeName) => this.CallJSOfSameName<object>(attributeName);
//    // TODO: Implementat validation and throw NotImplemented for return types <R> which are not supported by JQuery, perhaps using method attributes to declare valid types
//    public R Prop<R>(string propertyName) => this.CallJSOfSameName<R>(propertyName);
//    public void Prop(string propertyName, string value) => this.CallJSOfSameName<object>(propertyName, value);
//    //public void Prop(string propertyName, bool value) => this.CallJSOfSameName<object>(propertyName, value);        
//    //public void Prop(string propertyName, int? value) => this.CallJSOfSameName<object>(propertyName, value);
//    public void Prop(string propertyName, double? value) => this.CallJSOfSameName<object>(propertyName, value);
//    //public void Prop(string propertyName, object value) => this.CallJSOfSameName<object>(propertyName, value);
//    public void RemoveProp(string propertyName) => this.CallJSOfSameName<object>(propertyName);


//    public string Val() => this.CallJSOfSameName<string>();
//    public T Val<T>() => this.CallJSOfSameName<T>();
//    //his.CallJSOfSameName< (string[])JSInstanceProxy.FuncByNameAsArray(this.JSObject, "val", null); //this.CallJSOfSameName<T>();
//    public JQueryObject Val(string value) => this.CallJSOfSameNameAsWrapped(value);
//    public JQueryObject Val(double value) => this.CallJSOfSameNameAsWrapped(value);
//    public JQueryObject Val(string[] value) => this.CallJSOfSameNameAsWrapped(new object[] { value });

//    #endregion

//    #region Style Properties - https://api.jquery.com/category/manipulation/style-properties/
//    // Alsop include the couple of items from CSS that aren't in any other category: https://api.jquery.com/category/css/    

//    /// <summary> Access jQuery.css(key,value) </summary>
//    public JQIndexer<JQueryObject, string, string> Styles { get; }

//    // TODO: cssNumber https://api.jquery.com/jQuery.cssNumber/ 

//    public double Height {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double Width {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double InnerHeight {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double InnerWidth {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double OuterHeight {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double OuterWidth {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double ScrollLeft {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public double ScrollTop {
//        get => this.CallJSOfSameName<double>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    // TODO: Position Getter, returns X/Y object "coosrdinates" https://api.jquery.com/position/
//    // TODO: Offset, takes cordinates object https://api.jquery.com/offset/

//    #endregion

//    #region Instance Properties - https://api.jquery.com/category/properties/jquery-object-instance-properties/

//    public double Length => this.GetPropertyOfSameName<double>();
//    public string JQueryVersion => this.GetPropertyOfSameName<string>(propertyName: "jquery");

//    #endregion

//    #region Class Attributes - https://api.jquery.com/category/manipulation/class-attribute/
//    // CONSIDER: Validating that className parameters don't start with "." since this is a pitfall
//    public bool HasClass(string className) => this.CallJSOfSameName<bool>(className);
//    // NOTE: The native method only takes a single item or a seperate overload that takes an array, which is why we need to pass as `new object []`.  This is different from other methods that take one to many seperate params.
//    // WORKS but doesn't match interface exactly: public JQueryObject AddClass(string className, params string[] classNames) => this.CallJSOfSameNameAsWrapped( new object[] { Params.PrependToArray(className, ref classNames) } ); // new object[] { Params.Merge(className, classNames) });
//    public JQueryObject AddClass(string className) => this.CallJSOfSameNameAsWrapped(className);
//    public JQueryObject AddClass(string[] classNames) => this.CallJSOfSameNameAsWrapped(new object[] { classNames }); // Have to wrap in an extra array because javacsript .apply() will split the array into seperate params
//    public JQueryObject RemoveClass(string className) => this.CallJSOfSameNameAsWrapped(className);
//    public JQueryObject RemoveClass(string[] classNames) => this.CallJSOfSameNameAsWrapped(new object[] { classNames });
//    public JQueryObject ToggleClass(string className, bool? state = null) => this.CallJSOfSameNameAsWrapped(className, state);
//    public JQueryObject ToggleClass(string[] classNames, bool? state = null) => this.CallJSOfSameNameAsWrapped(classNames, state);

//    #endregion
//    #region Copying - https://api.jquery.com/category/manipulation/copying/

//    public JQueryObject Clone() => this.CallJSOfSameNameAsWrapped();
//    // TODO: Impkement other overloads

//    #endregion
//    #region DOM Insertion, Around, and Removal - https://api.jquery.com/category/manipulation/dom-insertion-around/

//    public JQueryObject Unwrap(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Wrap(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    public JQueryObject Wrap(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
//    public JQueryObject WrapAll(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    public JQueryObject WrapAll(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
//    public JQueryObject WrapInner(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);

//    // Only works correctly if passed HTMLElement. When passed JQ object created form our ParseHtml, it inserts only into one parent
//    //public JQueryObject WrapInner(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

//    #endregion
//    #region DOM Insertion, Inside - https://api.jquery.com/category/manipulation/dom-insertion-inside/

//    public JQueryObject Append(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
//    public JQueryObject Append(string html) => this.CallJSOfSameNameAsWrapped(html);
//    public JQueryObject Append(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.PrependToArray(jqObject, ref jqObjects));
//    public JQueryObject AppendTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    public JQueryObject AppendTo(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
//    public JQueryObject Prepend(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
//    public JQueryObject Prepend(HtmlElement html) => this.CallJSOfSameNameAsWrapped(html);
//    public JQueryObject Prepend(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
//    public JQueryObject PrependTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    public JQueryObject PrependTo(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

//    public string Html {
//        get => this.CallJSOfSameName<string>(); 
//        set => this.CallJSOfSameName<object>(value);
//    }

//    public string Text {
//        get => this.CallJSOfSameName<string>();
//        set => this.CallJSOfSameName<object>(value);
//    }

//    #endregion
//    #region DOM Insertion, Outside - https://api.jquery.com/category/manipulation/dom-insertion-outside/

//    public JQueryObject After(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
//    public JQueryObject After(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
//    public JQueryObject Before(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
//    public JQueryObject Before(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
//    public JQueryObject InsertAfter(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    public JQueryObject InsertAfter(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
//    public JQueryObject InsertBefore(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    public JQueryObject InsertBefore(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

//    #endregion
//    #region DOM Removal - https://api.jquery.com/category/manipulation/dom-removal/

//    public JQueryObject Detach(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
//    public JQueryObject Empty() => this.CallJSOfSameNameAsWrapped();
//    public JQueryObject Remove(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);

//    #endregion
//    #region DOM Replacement - https://api.jquery.com/category/manipulation/dom-replacement/
//    // TODO: Write unit tests
//    //public JQueryObject ReplaceAll(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    //public JQueryObject ReplaceAll(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
//    //public JQueryObject ReplaceWith(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
//    //public JQueryObject ReplaceWith(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

//    #endregion

//    // TODO: Determine/test handling of parameters defined in jQuery docs as:
//    //       "content": Type: htmlString or Element or Text or Array or jQuery
//    // versus "target": Type: Selector or htmlString or Element or Array or jQuery
//    // versus "wrappingElement": Type: Selector or htmlString or Element or jQuery

//    #region Specific Events - https://api.jquery.com/click/

//    // TODO: Write unit tests

//    //private JQueryEventHandler<JQueryObject, object> onClick;
//    // https://api.jquery.com/click/    
//    public event JQueryEventHandler<JQueryObject, dynamic> OnClick
//    {
//        //add { onClick = InnerOnGeneric("click", value, onClick); }
//        //remove { onClick = InnerOffGeneric("click", value, onClick); }
//        add { On("click", value); }
//        remove { Off("click", value); }
//    }

//    public event JQueryEventHandler<JQueryObject, dynamic> OnInput
//    {
//        add { On("input", value); }
//        remove { Off("input", value); }
//    }

//    public event JQueryEventHandler<JQueryObject, dynamic> OnChange
//    {
//        add { On("change", value); }
//        remove { Off("change", value); }
//    }

//    // TODO: Implement remaining events

//    #endregion

//    #region Generic Events - https://api.jquery.com/category/events/

//    // TODO: Factor out event code in JQueryObject and JQueryPlain

//    private record UniqueEvent
//    {
//        public string EventName { get; init; }
//        public string Selector { get; init; }
//    }

//    // Event subscription for any event name
//    private Dictionary<UniqueEvent, JQueryEventHandler<JQueryObject, dynamic>> onEvent = new();
//    // Used by strongly typed events such as OnClick/OnInput/OnChange
//    public void On(string eventName, JQueryEventHandler<JQueryObject, dynamic> newSubscriber)
//        => On(eventName, newSubscriber, null);
//    public void On(string eventName, string selector, JQueryEventHandler<JQueryObject, dynamic> newSubscriber)
//        => On(eventName, newSubscriber, selector);

//    private void On(string eventName, JQueryEventHandler<JQueryObject, dynamic> newSubscriber, string selector)
//    {
//        UniqueEvent eventKey = new() { EventName = eventName, Selector = selector };

//        if (!onEvent.ContainsKey(eventKey))// if first event subscriber
//            onEvent[eventKey] = null;// initialize key entry
//        onEvent[eventKey] = InnerOnGeneric(eventKey, newSubscriber, onEvent[eventKey]);
//    }

//    // TODO: Support "To remove all delegated events from an element without removing non-delegated events, use the special value "**"."
//    public void Off(string eventName, string selector, JQueryEventHandler<JQueryObject, dynamic> subscriberToRemove)
//        => Off(eventName, subscriberToRemove, selector);
//    public void Off(string eventName, JQueryEventHandler<JQueryObject, dynamic> subscriberToRemove)
//        => Off(eventName, subscriberToRemove, null);

//    private void Off(string eventName, JQueryEventHandler<JQueryObject, dynamic> subscriberToRemove, string selector)
//    {
//        UniqueEvent eventKey = new() { EventName = eventName, Selector = selector };

//        if (!onEvent.ContainsKey(eventKey) || onEvent[eventKey] == null)
//            return;

//        onEvent[eventKey] = InnerOffGeneric(eventKey, subscriberToRemove, onEvent[eventKey]);
//    }

//    public delegate void JQueryEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
//        where TSender : JQueryObject;

//    // Handler funcs generated from JS when binding our event. Kept to pass to JQuery .off(..., handler) when unbinding/unsubscribing handler
//    private readonly Dictionary<UniqueEvent, JSObject> jsHandlersByEvent = new();

//    private JQueryEventHandler<JQueryObject, dynamic> InnerOnGeneric(UniqueEvent eventKey, JQueryEventHandler<JQueryObject, dynamic> newSubscriber, JQueryEventHandler<JQueryObject, dynamic> eventCollection)
//    {
//        if (eventCollection == null)// if first event subscriber on this instance/event
//        {
//            if (jsHandlersByEvent.ContainsKey(eventKey))
//                throw new Exception($"Unexpected: jsHandlersByEvent already contains key {eventKey}");

//            // generate handler specific to this instance+event, called by JS when event occurs
//            Action<string, string, JSObject> interopListener =
//                (eventEncoded, eventType, arrayObject) =>
//                {
//                    //Console.WriteLine("Event Encoded: " + eventEncoded);
//                    // unpack the single ArrayObject into it's individual elements, wrapping them as JQueryObjects
//                    var replacements = HelpersJS.GetArrayObjectItems(arrayObject).Select(j => new JQueryObject(j)).ToList();
//                    // Deserialize the eventEncoded JSON string, and restore the native JS objects
//                    dynamic eventData = EncodedEventToDynamic(eventEncoded, replacements);

//                    onEvent[eventKey]?.Invoke(this, eventData);
//                };

//            JSObject jsHandler = InnerOn(eventKey.EventName, interopListener, eventKey.Selector);
//            jsHandlersByEvent[eventKey] = jsHandler;// store handler for later unbinding
//        }

//        eventCollection += newSubscriber;
//        return eventCollection;
//    }

//    private JQueryEventHandler<JQueryObject, dynamic> InnerOffGeneric(UniqueEvent eventKey, JQueryEventHandler<JQueryObject, dynamic> subscriberToRemove, JQueryEventHandler<JQueryObject, dynamic> eventCollection, string selector = null)
//    {
//        WriteLine(eventCollection == null ? "eventCollection is null " : "not null");

//        if (eventCollection == null)// if no subscribers on this instance/event
//            return eventCollection;

//        eventCollection -= subscriberToRemove;
//        if (eventCollection == null) // if last subscriber removed, then remove JQuery listener
//        {
//            //Console.WriteLine("jsHandlersByEvent[eventName]: " + jsHandlersByEvent[eventName]);
//            InnerOff(eventKey.EventName, jsHandlersByEvent[eventKey], eventKey.Selector);
//            jsHandlersByEvent.Remove(eventKey);
//        }

//        return eventCollection;
//    }

//    private JSObject InnerOn(string events, Action<string, string, JSObject> interopListener, string selector)
//    {
//        // TODO: Make shouldConvertHtmlElement configurable when we support HtmlElement
//        return JSInstanceProxy.BindListener(jsObject, events, shouldConvertHtmlElement: true, interopListener, selector);
//    }

//    private void InnerOff(string events, JSObject handlerToRemove, string selector)
//    {
//        JSInstanceProxy.UnbindListener(jsObject, events, handlerToRemove, selector);
//    }


//    // Event data is serialized to JSON when the event is fired from JS and passed to C#
//    // To preserve some objects as native JS references, they are extracted into a seperate array
//    // and JSON properties are replaced with `{ serratedPlaceholder: 1 }` where 1 is the index of the object in the array
//    // Then here when listening to an event we desrialize the JSON into a dynamic and restore the native JS references
//    private dynamic EncodedEventToDynamic(string encodedEvent, List<JQueryObject> replacements)
//    {
//        ExpandoObject eventData = JsonConvert.DeserializeObject<ExpandoObject>(encodedEvent);
//        ApplyReplacements(eventData, replacements, null, out bool hasPlaceholder);
//        return eventData;
//    }

//    private object ApplyReplacements(ExpandoObject currentExpando, List<JQueryObject> replacements, ExpandoObject parent, out bool hasPlaceholder)
//    {
//        // Go through properties of currentExpando,
//        // find a child expando property containing a value property named serratedPlaceholder, e.g. `target: { serratedPlaceholder : 1 }` 
//        hasPlaceholder = false;// flagged true for parent when currentExpando has a single property named serratedPlaceholder
//        if (replacements == null || replacements.Count == 0) // nothing to replace
//            return null;

//        var placeholdersFound = new Dictionary<string, object>();
//        // recursively search all properties of the ExpandoObject and find expando with property name of serratedPlaceholder
//        foreach (var property in currentExpando)
//        {
//            //Console.WriteLine("Property: " + property.Key + " + " + property.Value);

//            if (property.Key is string && property.Key == "serratedPlaceholder")
//            {
//                // Found. The entire currentExpando is the placeholder and needs to be replaced in the property of parent call.
//                // Flag out param hasPlaceholder and return the appropriate replacement so the parent call can reassign.
//                hasPlaceholder = true;
//                int index = Convert.ToInt32(property.Value);
//                //Console.WriteLine("Replacing: " + property.Key + " + " + property.Value + " with " + index);
//                return replacements[index];
//            }
//            else if (property.Value is ExpandoObject currentValue)
//            {
//                // If is expando, then recurse into it and scan its properties as well.                    
//                var replacement = ApplyReplacements(currentValue, replacements, currentExpando, out bool hasPlaceholderInner);
//                if (hasPlaceholderInner) // If child property turns out to be a placeholder, then replace it
//                    placeholdersFound[property.Key] = replacement;
//            }
//        }

//        // we can't modify expondo while iterating through it above, so stage replacement in placeholdersGound 
//        foreach (var placeHolder in placeholdersFound)// then apply them to the expando here
//        {
//            ((IDictionary<string, object>)currentExpando)[placeHolder.Key] = placeHolder.Value;
//        }

//        return null;
//    }

//    #endregion
//    #region Event Handler Attachement - https://api.jquery.com/category/events/event-handler-attachment/
//    // Incomplete - Not all memers implemented

//    // .trigger( eventType [, extraParameters ] )
//    public JQueryObject Trigger(string eventType, params object[] extraParameters) => this.CallJSOfSameNameAsWrapped(Params.Merge(eventType, new object[] { extraParameters }));

//    #endregion

//}



//    //public void InternalEventCallback(string eventEncoded, string eventType)
//    //{
//    //    dynamic eventData = EncodedEventToDynamic(eventEncoded);
//    //    onEvent[eventType]?.Invoke(this, eventData);
//    //}



//    //public class EventData
//    //{
//    //    public string Type { get; set; }
//    //    public string TimeStamp { get; set; }
//    //    public string[] CurrentTarget { get; set; }
//    //    public string[] Target { get; set; }
//    //    public string[] RelatedTarget { get; set; }
//    //    public string[] DelegateTarget { get; set; }
//    //    public string[] HandleObj { get; set; }
//    //    public string[] Data { get; set; }
//    //    public string Result { get; set; }
//    //    public string[] OriginalEvent { get; set; }
//    //    public string[] IsTrigger { get; set; }
//    //    public string[] Namespace { get; set; }
//    //    public string[] Namespace_re { get; set; }
//    //    public string[] Result_re { get; set; }
//    //    public string[] Target_re { get; set; }
//    //    public string[] DelegatedEvent { get; set; }
//    //    public string[] CurrentTarget_re { get; set; }
//    //    public string[] HandleObj_re { get; set; }
//    //    public string[] RelatedTarget_re { get; set; }
//    //    public string[] Data_re { get; set; }
//    //    public string[] IsTrigger_re { get; set; }
//    //    public string[] OriginalEvent_re { get; set; }
//    //    public string[] Type_re { get; set; }
//    //    public string[] DelegateTarget_re { get; set; }
//    //    public string[] TimeStamp_re { get; set; }
//    //}











