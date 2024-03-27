using System;
using System.Collections.Generic;
using static System.Console;
using System.Linq;
using System.Dynamic;
using Newtonsoft.Json;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.JSInteropHelpers;
using Params = SerratedSharp.JSInteropHelpers.ParamsHelpers;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace SerratedSharp.SerratedJQ.Plain;

// Wrapper that references a JQuery instance object, i.e. the collection returned from selectors/queries
public class JQueryPlainObject : IJSObjectWrapper<JQueryPlainObject>, IJQueryContentParameter
{
    internal JSObject jsObject;// reference to the jQuery javascript interop object
    
    /// <summary>
    /// Handle to the underlying javascript jQuery object
    /// </summary>
    public JSObject JSObject { get { return jsObject; } }

    // instances can only be created thru factory methods like Select()/ParseHtml()
    internal JQueryPlainObject() { }
    public JQueryPlainObject(JSObject jsObject) { this.jsObject = jsObject; }
    // This static factory method defined by the IJSObjectWrapper enables generic code such as CallJSOfSameNameAsWrapped to automatically wrap JSObjectsWrap
    static JQueryPlainObject IJSObjectWrapper<JQueryPlainObject>.WrapInstance(JSObject jsObject)
    {
        return new JQueryPlainObject(jsObject);
    }

    /// <summary>
    /// Copy JQueryObject reference to new JQueryPlainObject to allow access to alternative API semantics.
    /// </summary>
    //public JQueryPlainObject(JQueryObject jQueryObject)
    //{
    //    jsObject = jQueryObject.JSObject;
    //}

    #region Traversal - Filtering - https://api.jquery.com/category/traversing/filtering/

    public JQueryPlainObject First() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject Last() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject Eq(int index) => this.CallJSOfSameNameAsWrapped(index);
    public JQueryPlainObject Slice(int start, int end) => this.CallJSOfSameNameAsWrapped(start, end);
    public JQueryPlainObject Filter(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Has(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Not(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public bool Is(string selector) => this.CallJSOfSameName<bool>(selector);
    public JQueryPlainObject Odd() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject Even() => this.CallJSOfSameNameAsWrapped();
    //Map only takes a predicate: https://api.jquery.com/map/#map-callback
    //public JQueryObject Map(

    // We should be able to implement variations of methods that take a parameter of:
    //      A collection of Elements: https://api.jquery.com/filter/#filter-elements
    //      A collection of JQuery object collection: https://api.jquery.com/filter/#filter-selection
    //      Unsure if we can implement a predicate parameter: https://api.jquery.com/filter/#filter-function
    //public JQueryObject Filter(Func<int, bool> predicate) => this.CallJSOfSameNameAsWrapped(predicate);
    //public JQueryObject Filter(Func<int, JQueryObject, bool> predicate) => this.CallJSOfSameNameAsWrapped(predicate);

    #endregion
    #region Traversal - Tree Traversal - https://api.jquery.com/category/traversing/tree-traversal/

    public JQueryPlainObject Children(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Closest(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Find(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Next(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject NextAll(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject NextUntil(string stopAtSelector = null, string filterResultsSelector = null)
        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
    public JQueryPlainObject OffsetParent() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject Parent(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Parents(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject ParentsUntil(string stopAtSelector = null, string filterResultsSelector = null)
        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
    public JQueryPlainObject Prev(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject PrevAll(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject PrevUntil(string stopAtSelector = null, string filterResultsSelector = null)
        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
    public JQueryPlainObject Siblings(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);

    #endregion
    #region Traversal - Miscellaneous - https://api.jquery.com/category/traversing/miscellaneous-traversal/

    public JQueryPlainObject Add(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Add(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryPlainObject Add(string selector, JQueryPlainObject context) => this.CallJSOfSameNameAsWrapped(selector, context);
    public JQueryPlainObject AddBack(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Contents() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject End() => this.CallJSOfSameNameAsWrapped();

    #endregion

    #region General Attributes - https://api.jquery.com/category/attributes/general-attributes/

    public string Attr(string attributeName) => this.CallJSOfSameName<string>(attributeName);
    public void Attr(string attributeName, string value) => this.CallJSOfSameName<object>(attributeName, value);
    //public void Attr(string attributeName, bool value) => this.CallJSOfSameName<object>(attributeName, value);        
    public void Attr(string attributeName, int? value) => this.CallJSOfSameName<object>(attributeName, value);
    public void Attr(string attributeName, double? value) => this.CallJSOfSameName<object>(attributeName, value);
    //public void Attr(string attributeName, object value) => this.CallJSOfSameName<object>(attributeName, value);
    public void RemoveAttr(string attributeName) => this.CallJSOfSameName<object>(attributeName);
    // TODO: Implementat validation and throw NotImplemented for return types <R> which are not supported by JQuery, perhaps using method attributes to declare valid types
    public R Prop<R>(string propertyName) => this.CallJSOfSameName<R>(propertyName);
    public void Prop(string propertyName, string value) => this.CallJSOfSameName<object>(propertyName, value);
    //public void Prop(string propertyName, bool value) => this.CallJSOfSameName<object>(propertyName, value);        
    //public void Prop(string propertyName, int? value) => this.CallJSOfSameName<object>(propertyName, value);
    public void Prop(string propertyName, double? value) => this.CallJSOfSameName<object>(propertyName, value);
    //public void Prop(string propertyName, object value) => this.CallJSOfSameName<object>(propertyName, value);
    public void RemoveProp(string propertyName) => this.CallJSOfSameName<object>(propertyName);


    public string Val() => this.CallJSOfSameName<string>();
    public T Val<T>() => this.CallJSOfSameName<T>();
    
    public JQueryPlainObject Val(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject Val(double value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject Val(string[] value) => this.CallJSOfSameNameAsWrapped(new object[] { value });

    #endregion

    #region Style Properties - https://api.jquery.com/category/manipulation/style-properties/
    // Also include the couple of items from CSS that aren't in any other category: https://api.jquery.com/category/css/    

    public string Css(string propertyName) => this.CallJSOfSameName<string>(propertyName);
    public JQueryPlainObject Css(string propertyName, string value) => this.CallJSOfSameNameAsWrapped(propertyName, value);

    // TODO: cssNumber https://api.jquery.com/jQuery.cssNumber/ 

    public double Height() => this.CallJSOfSameName<double>();
    public JQueryPlainObject Height(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject Height(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double Width() => this.CallJSOfSameName<double>();
    public JQueryPlainObject Width(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject Width(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double InnerHeight() => this.CallJSOfSameName<double>();
    public JQueryPlainObject InnerHeight(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject InnerHeight(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double InnerWidth() => this.CallJSOfSameName<double>();
    public JQueryPlainObject InnerWidth(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject InnerWidth(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double OuterHeight() => this.CallJSOfSameName<double>();
    public JQueryPlainObject OuterHeight(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject OuterHeight(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double OuterWidth() => this.CallJSOfSameName<double>();
    public JQueryPlainObject OuterWidth(string value) => this.CallJSOfSameNameAsWrapped(value);
    public JQueryPlainObject OuterWidth(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double ScrollLeft() => this.CallJSOfSameName<double>();
    public JQueryPlainObject ScrollLeft(double value) => this.CallJSOfSameNameAsWrapped(value);

    public double ScrollTop() => this.CallJSOfSameName<double>();
    public JQueryPlainObject ScrollTop(double value) => this.CallJSOfSameNameAsWrapped(value);

    // TODO: Position Getter, returns X/Y object "coosrdinates" https://api.jquery.com/position/
    // TODO: Offset, takes cordinates object https://api.jquery.com/offset/

    #endregion

    #region Instance Properties - https://api.jquery.com/category/properties/jquery-object-instance-properties/

    public double Length => this.GetPropertyOfSameName<double>();
    public string JQueryVersion => this.GetPropertyOfSameName<string>(propertyName: "jquery");

    #endregion

    #region Class Attributes - https://api.jquery.com/category/manipulation/class-attribute/
    // CONSIDER: Validating that className parameters don't start with "." since this is a pitfall
    public bool HasClass(string className) => this.CallJSOfSameName<bool>(className);
    // NOTE: The native method only takes a single item or a seperate overload that takes an array, which is why we need to pass as `new object []`.  This is different from other methods that take one to many seperate params.
    // WORKS but doesn't match interface exactly: public JQueryPlainObject AddClass(string className, params string[] classNames) => this.CallJSOfSameNameAsWrapped( new object[] { Params.PrependToArray(className, ref classNames) } ); // new object[] { Params.Merge(className, classNames) });
    public JQueryPlainObject AddClass(string className) => this.CallJSOfSameNameAsWrapped(className);
    public JQueryPlainObject AddClass(string[] classNames) => this.CallJSOfSameNameAsWrapped(new object[] { classNames }); // Have to wrap in an extra array because javacsript .apply() will split the array into seperate params
    public JQueryPlainObject RemoveClass(string className) => this.CallJSOfSameNameAsWrapped(className);
    public JQueryPlainObject RemoveClass(string[] classNames) => this.CallJSOfSameNameAsWrapped(new object[] { classNames });
    public JQueryPlainObject ToggleClass(string className, bool? state = null) => this.CallJSOfSameNameAsWrapped(className, state);
    public JQueryPlainObject ToggleClass(string[] classNames, bool? state = null) => this.CallJSOfSameNameAsWrapped(classNames, state);

    #endregion
    #region Copying - https://api.jquery.com/category/manipulation/copying/

    public JQueryPlainObject Clone() => this.CallJSOfSameNameAsWrapped();
    // TODO: Impkement other overloads

    #endregion
    #region DOM Insertion, Around, and Removal - https://api.jquery.com/category/manipulation/dom-insertion-around/

    public JQueryPlainObject Unwrap(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Wrap(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryPlainObject Wrap(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryPlainObject WrapAll(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryPlainObject WrapAll(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryPlainObject WrapInner(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);

    // Only works correctly if passed HTMLElement. When passed JQ object created form our ParseHtml, it inserts only into one parent
    //public JQueryObject WrapInner(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    #endregion
    #region DOM Insertion, Inside - https://api.jquery.com/category/manipulation/dom-insertion-inside/

    public JQueryPlainObject Append(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryPlainObject Append(string html) => this.CallJSOfSameNameAsWrapped(html);
    // TODO: Follow this pattern for implementing other methods that take JQuery's "content" params that support both JQuery objects and HtmlElement objects
    public JQueryPlainObject Append(IJQueryContentParameter contentObject, params IJQueryContentParameter[] contentObjects) => this.CallJSOfSameNameAsWrapped(Params.PrependToArray(contentObject, ref contentObjects));
    //public JQueryPlainObject Append(HtmlElement html) => this.CallJSOfSameNameAsWrapped(html);
    //public JQueryPlainObject Append(JQueryPlainObject jqObject, params JQueryPlainObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.PrependToArray(jqObject, ref jqObjects));
    public JQueryPlainObject AppendTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryPlainObject AppendTo(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryPlainObject Prepend(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryPlainObject Prepend(HtmlElement html) => this.CallJSOfSameNameAsWrapped(html);
    public JQueryPlainObject Prepend(JQueryPlainObject jqObject, params JQueryPlainObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
    public JQueryPlainObject PrependTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryPlainObject PrependTo(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    public string Html() => this.CallJSOfSameName<string>();
    public JQueryPlainObject Html(string htmlString) => this.CallJSOfSameNameAsWrapped(htmlString);

    public string Text() => this.CallJSOfSameName<string>();
    public JQueryPlainObject Text(string text) => this.CallJSOfSameNameAsWrapped(text);

    #endregion
    #region DOM Insertion, Outside - https://api.jquery.com/category/manipulation/dom-insertion-outside/

    public JQueryPlainObject After(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryPlainObject After(JQueryPlainObject jqObject, params JQueryPlainObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
    public JQueryPlainObject Before(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryPlainObject Before(JQueryPlainObject jqObject, params JQueryPlainObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
    public JQueryPlainObject InsertAfter(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryPlainObject InsertAfter(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryPlainObject InsertBefore(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryPlainObject InsertBefore(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    #endregion
    #region DOM Removal - https://api.jquery.com/category/manipulation/dom-removal/

    public JQueryPlainObject Detach(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryPlainObject Empty() => this.CallJSOfSameNameAsWrapped();
    public JQueryPlainObject Remove(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);

    #endregion
    #region DOM Replacement - https://api.jquery.com/category/manipulation/dom-replacement/
    // TODO: Write unit tests
    //public JQueryPlainObject ReplaceAll(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    //public JQueryPlainObject ReplaceAll(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    //public JQueryPlainObject ReplaceWith(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    //public JQueryPlainObject ReplaceWith(JQueryPlainObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    #endregion

    // TODO: Determine/test handling of parameters defined in jQuery docs as:
    //       "content": Type: htmlString or Element or Text or Array or jQuery
    // versus "target": Type: Selector or htmlString or Element or Array or jQuery
    // versus "wrappingElement": Type: Selector or htmlString or Element or jQuery

    #region Specific Events - https://api.jquery.com/click/

    //private JQueryEventHandler<JQueryObject, object> onClick;
    // https://api.jquery.com/click/    
    public event JQueryEventHandler<JQueryPlainObject, dynamic> OnClick
    {
        add { On("click", value); }
        remove { Off("click", value); }
    }

    public event JQueryEventHandler<JQueryPlainObject, dynamic> OnInput
    {
        add { On("input", value); }
        remove { Off("input", value); }
    }

    public event JQueryEventHandler<JQueryPlainObject, dynamic> OnChange
    {
        add { On("change", value); }
        remove { Off("change", value); }
    }

    // TODO: Implement remaining events

    #endregion

    #region Generic Events - https://api.jquery.com/category/events/

    private record UniqueEvent
    {
        public string EventName { get; init; }
        public string Selector { get; init; }
    }

    // Event subscription for any event name
    private Dictionary<UniqueEvent, JQueryEventHandler<JQueryPlainObject, dynamic>> onEvent = new();
    // Used by strongly typed events such as OnClick/OnInput/OnChange
    public void On(string eventName, JQueryEventHandler<JQueryPlainObject, dynamic> newSubscriber)
        => On(eventName, newSubscriber, null);
    public void On(string eventName, string selector, JQueryEventHandler<JQueryPlainObject, dynamic> newSubscriber)
        => On(eventName, newSubscriber, selector);

    private void On(string eventName, JQueryEventHandler<JQueryPlainObject, dynamic> newSubscriber, string selector)
    {
        UniqueEvent eventKey = new() { EventName = eventName, Selector = selector };

        if (!onEvent.ContainsKey(eventKey))// if first event subscriber
            onEvent[eventKey] = null;// initialize key entry
        onEvent[eventKey] = InnerOnGeneric(eventKey, newSubscriber, onEvent[eventKey]);
    }

    // TODO: Support "To remove all delegated events from an element without removing non-delegated events, use the special value "**"."
    public void Off(string eventName, string selector, JQueryEventHandler<JQueryPlainObject, dynamic> subscriberToRemove)
        => Off(eventName, subscriberToRemove, selector);
    public void Off(string eventName, JQueryEventHandler<JQueryPlainObject, dynamic> subscriberToRemove)
        => Off(eventName, subscriberToRemove, null);

    private void Off(string eventName, JQueryEventHandler<JQueryPlainObject, dynamic> subscriberToRemove, string selector)
    {
        UniqueEvent eventKey = new() { EventName = eventName, Selector = selector };

        if (!onEvent.ContainsKey(eventKey) || onEvent[eventKey] == null)
            return;

        onEvent[eventKey] = InnerOffGeneric(eventKey, subscriberToRemove, onEvent[eventKey]);
    }

    public delegate void JQueryEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
        where TSender : JQueryPlainObject;

    // Handler funcs generated from JS when binding our event. Kept to pass to JQuery .off(..., handler) when unbinding/unsubscribing handler
    private readonly Dictionary<UniqueEvent, JSObject> jsHandlersByEvent = new();

    private JQueryEventHandler<JQueryPlainObject, dynamic> InnerOnGeneric(UniqueEvent eventKey, JQueryEventHandler<JQueryPlainObject, dynamic> newSubscriber, JQueryEventHandler<JQueryPlainObject, dynamic> eventCollection)
    {
        if (eventCollection == null)// if first event subscriber on this instance/event
        {
            if (jsHandlersByEvent.ContainsKey(eventKey))
                throw new Exception($"Unexpected: jsHandlersByEvent already contains key {eventKey}");

            // generate handler specific to this instance+event, called by JS when event occurs
            Action<string, string, JSObject> interopListener =
                (eventEncoded, eventType, arrayObject) =>
               {
                   //Console.WriteLine("Event Encoded: " + eventEncoded);
                   // unpack the single ArrayObject into it's individual elements, wrapping them as JQueryObjects
                   var replacements = HelpersJS.GetArrayObjectItems(arrayObject).Select(j => new JQueryPlainObject(j)).ToList();
                   // Deserialize the eventEncoded JSON string, and restore the native JS objects
                   dynamic eventData = EncodedEventToDynamic(eventEncoded, replacements);

                   onEvent[eventKey]?.Invoke(this, eventData);
               };

            JSObject jsHandler = InnerOn(eventKey.EventName, interopListener, eventKey.Selector);
            jsHandlersByEvent[eventKey] = jsHandler;// store handler for later unbinding
        }

        eventCollection += newSubscriber;
        return eventCollection;
    }

    private JQueryEventHandler<JQueryPlainObject, dynamic> InnerOffGeneric(UniqueEvent eventKey, JQueryEventHandler<JQueryPlainObject, dynamic> subscriberToRemove, JQueryEventHandler<JQueryPlainObject, dynamic> eventCollection, string selector = null)
    {
        WriteLine(eventCollection == null ? "eventCollection is null " : "not null");

        if (eventCollection == null)// if no subscribers on this instance/event
            return eventCollection;

        eventCollection -= subscriberToRemove;
        if (eventCollection == null) // if last subscriber removed, then remove JQuery listener
        {
            //Console.WriteLine("jsHandlersByEvent[eventName]: " + jsHandlersByEvent[eventName]);
            InnerOff(eventKey.EventName, jsHandlersByEvent[eventKey], eventKey.Selector);
            jsHandlersByEvent.Remove(eventKey);
        }

        return eventCollection;
    }

    private JSObject InnerOn(string events, Action<string, string, JSObject> interopListener, string selector)
    {
        // TODO: Make shouldConvertHtmlElement configurable when we support HtmlElement
        return JQueryProxy.BindListener(jsObject, events, shouldConvertHtmlElement: true, interopListener, selector);
    }

    private void InnerOff(string events, JSObject handlerToRemove, string selector)
    {
        JQueryProxy.UnbindListener(jsObject, events, handlerToRemove, selector);
    }


    // Event data is serialized to JSON when the event is fired from JS and passed to C#
    // To preserve some objects as native JS references, they are extracted into a seperate array
    // and JSON properties are replaced with `{ serratedPlaceholder: 1 }` where 1 is the index of the object in the array
    // Then here when listening to an event we desrialize the JSON into a dynamic and restore the native JS references    
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(JQueryPlainObject))]
    private dynamic EncodedEventToDynamic(string encodedEvent, List<JQueryPlainObject> replacements)
    {         
        ExpandoObject eventData = JsonConvert.DeserializeObject<ExpandoObject>(encodedEvent);
        ApplyReplacements(eventData, replacements, null, out bool hasPlaceholder);
        return eventData;    
    }

    private object ApplyReplacements(ExpandoObject currentExpando, List<JQueryPlainObject> replacements, ExpandoObject parent, out bool hasPlaceholder)
    {
        // Go through properties of currentExpando,
        // find a child expando property containing a value property named serratedPlaceholder, e.g. `target: { serratedPlaceholder : 1 }` 
        hasPlaceholder = false;// flagged true for parent when currentExpando has a single property named serratedPlaceholder
        if (replacements == null || replacements.Count == 0) // nothing to replace
            return null;

        var placeholdersFound = new Dictionary<string, object>();
        // recursively search all properties of the ExpandoObject and find expando with property name of serratedPlaceholder
        foreach (var property in currentExpando)
        {
            //Console.WriteLine("Property: " + property.Key + " + " + property.Value);

            if (property.Key is string && property.Key == "serratedPlaceholder")
            {
                // Found. The entire currentExpando is the placeholder and needs to be replaced in the property of parent call.
                // Flag out param hasPlaceholder and return the appropriate replacement so the parent call can reassign.
                hasPlaceholder = true;
                int index = Convert.ToInt32(property.Value);
                //Console.WriteLine("Replacing: " + property.Key + " + " + property.Value + " with " + index);
                return replacements[index];
            }
            else if (property.Value is ExpandoObject currentValue)
            {
                // If is expando, then recurse into it and scan its properties as well.                    
                var replacement = ApplyReplacements(currentValue, replacements, currentExpando, out bool hasPlaceholderInner);
                if (hasPlaceholderInner) // If child property turns out to be a placeholder, then replace it
                    placeholdersFound[property.Key] = replacement;
            }
        }

        // we can't modify expondo while iterating through it above, so stage replacement in placeholdersGound 
        foreach (var placeHolder in placeholdersFound)// then apply them to the expando here
        {
            ((IDictionary<string, object>)currentExpando)[placeHolder.Key] = placeHolder.Value;
        }

        return null;
    }

    #endregion
    #region Event Handler Attachement - https://api.jquery.com/category/events/event-handler-attachment/
    // Incomplete - Not all members implemented

    // .trigger( eventType [, extraParameters ] )
    public JQueryPlainObject Trigger(string eventType, params object[] extraParameters) => this.CallJSOfSameNameAsWrapped(Params.Merge(eventType, new object[] { extraParameters }));

    #endregion
    #region Data - https://api.jquery.com/category/data/

    public T Data<T>(string key) => this.CallJSOfSameName<T>(key);
    public JQueryPlainObject Data(string key, object value) => this.CallJSOfSameNameAsWrapped(key, value);
    /// <summary>
    /// Calls JQuery.Data() returning the entire data object as a JSObject reference.  Use JSObject.GetPropertyAs* methods to access properties or pass reference to GlobalJS.Console.Log(obj) to log entire object graph in browser console.
    /// </summary>
    public JSObject DataAsJSObject() => this.CallJSOfSameName<JSObject>(funcName: "data");

    #endregion


}





