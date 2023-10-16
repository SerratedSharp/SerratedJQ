using System;
using System.Collections.Generic;
using static System.Console;
using System.Linq;
using System.Dynamic;
using Newtonsoft.Json;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.JSInteropHelpers;
using Params = SerratedSharp.JSInteropHelpers.ParamsHelpers;


namespace SerratedSharp.SerratedJQ;


// Wrapper that references a JQuery instance object, i.e. the collection returned from selectors/queries
public class JQueryObject : IJSObjectWrapper<JQueryObject>, IJQueryContentParameter
{
    internal JSObject jsObject;// reference to the jQuery javascript interop object
    public JSObject JSObject { get { return jsObject; } }

    // instances can only be created thru factory methods like Select()/ParseHtml()
    internal JQueryObject() { }
    public JQueryObject(JSObject jsObject) { this.jsObject = jsObject; }
    // This static factory method defined by the IJSObjectWrapper enables generic code such as CallJSOfSameNameAsWrapped to automatically wrap JSObjectsWrap
    static JQueryObject IJSObjectWrapper<JQueryObject>.WrapInstance(JSObject jsObject)
    {
        return new JQueryObject(jsObject); 
    }

    #region Traversal - Filtering - https://api.jquery.com/category/traversing/filtering/

    public JQueryObject First()                     => this.CallJSOfSameNameAsWrapped();
    public JQueryObject Last()                      => this.CallJSOfSameNameAsWrapped();
    public JQueryObject Eq(int index)               => this.CallJSOfSameNameAsWrapped(index);
    public JQueryObject Slice(int start, int end)   => this.CallJSOfSameNameAsWrapped(start, end);
    public JQueryObject Filter(string selector)     => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Has(string selector)        => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Not(string selector)        => this.CallJSOfSameNameAsWrapped(selector);
    public bool Is(string selector)                 => this.CallJSOfSameName<bool>(selector);
    public JQueryObject Odd()                       => this.CallJSOfSameNameAsWrapped();
    public JQueryObject Even()                      => this.CallJSOfSameNameAsWrapped();
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

    public JQueryObject Children(string selector = null)    => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Closest(string selector)            => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Find(string selector)               => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Next(string selector = null)        => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject NextAll(string selector = null)     => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject NextUntil(string stopAtSelector = null, string filterResultsSelector = null)
        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
    public JQueryObject OffsetParent()                      => this.CallJSOfSameNameAsWrapped();
    public JQueryObject Parent(string selector = null)      => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Parents(string selector = null)     => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject ParentsUntil(string stopAtSelector = null, string filterResultsSelector = null)
        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
    public JQueryObject Prev(string selector = null)        => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject PrevAll(string selector = null)     => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject PrevUntil(string stopAtSelector = null, string filterResultsSelector = null)
        => this.CallJSOfSameNameAsWrapped(stopAtSelector, filterResultsSelector);
    public JQueryObject Siblings(string selector = null)     => this.CallJSOfSameNameAsWrapped(selector);

    #endregion
    #region Traversal - Miscellaneous - https://api.jquery.com/category/traversing/miscellaneous-traversal/
        
    public JQueryObject Add(string selector) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Add(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryObject Add(string selector, JQueryObject context) => this.CallJSOfSameNameAsWrapped(selector, context);
    public JQueryObject AddBack(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Contents() => this.CallJSOfSameNameAsWrapped();
    public JQueryObject End() => this.CallJSOfSameNameAsWrapped();
        
    #endregion

    #region General Attributes - https://api.jquery.com/category/attributes/general-attributes/

    public string Attr(string attributeName) => this.CallJSOfSameName<string>(attributeName);
    public void Attr(string attributeName, string? value) => this.CallJSOfSameName<object>(attributeName, value);
    //public void Attr(string attributeName, bool value) => this.CallJSOfSameName<object>(attributeName, value);        
    public void Attr(string attributeName, int? value) => this.CallJSOfSameName<object>(attributeName, value);
    public void Attr(string attributeName, decimal? value) => this.CallJSOfSameName<object>(attributeName, value);
    //public void Attr(string attributeName, object value) => this.CallJSOfSameName<object>(attributeName, value);
    public void RemoveAttr(string attributeName) => this.CallJSOfSameName<object>(attributeName);
            
        
    public string Val
    {
        get => this.CallJSOfSameName<string>();
        set => this.CallJSOfSameName<object>(value);
    }


    #endregion

    #region Style Properties - https://api.jquery.com/category/manipulation/style-properties/

    // TODO: css https://api.jquery.com/category/manipulation/style-properties/
    // TODO: cssNumber https://api.jquery.com/jQuery.cssNumber/ 

    public decimal Height
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal Width
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal InnerHeight
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal InnerWidth
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal OuterHeight
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal OuterWidth
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal ScrollLeft
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    public decimal ScrollTop
    {
        get => this.CallJSOfSameName<decimal>();
        set => this.CallJSOfSameName<object>(value);
    }

    // TODO: Position Getter, returns X/Y object "coosrdinates" https://api.jquery.com/position/
    // TODO: Offset, takes cordinates object https://api.jquery.com/offset/

    #endregion

    #region Instance Properties - https://api.jquery.com/category/properties/jquery-object-instance-properties/
    // TODO: Length and JQueryVersion

    // Length
    public double Length => this.GetPropertyOfSameName<double>();
    // jQuery
    public string JQueryVersion => this.GetPropertyOfSameName<string>(propertyName: "jquery");

    #endregion

    #region Class Attributes - https://api.jquery.com/category/manipulation/class-attribute/
    // CONSIDER: Validating that className parameters don't start with "." since this is a pitfall
    public bool HasClass(string className) => this.CallJSOfSameName<bool>(className);
    public void AddClass(string className) => this.CallJSOfSameName<object>(className);
    public void AddClass(string[] classNames) => this.CallJSOfSameName<object>(classNames);
    public void RemoveClass(string className) => this.CallJSOfSameName<object>(className);
    public void RemoveClass(string[] classNames) => this.CallJSOfSameName<object>(classNames);
    public void ToggleClass(string className, bool? state = null) => this.CallJSOfSameName<object>(className, state);
    public void ToggleClass(string[] classNames, bool? state = null) => this.CallJSOfSameName<object>(classNames, state);

    #endregion
    #region Copying - https://api.jquery.com/category/manipulation/copying/

    public JQueryObject Clone() => this.CallJSOfSameNameAsWrapped();

    #endregion
    #region DOM Insertion, Around, and Removal - https://api.jquery.com/category/manipulation/dom-insertion-around/

    public JQueryObject Unwrap(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Wrap(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject Wrap(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryObject WrapAll(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject WrapAll(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryObject WrapInner(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
        
    // Only works correctly if passed HTMLElement. When passed JQ object created form our ParseHtml, it inserts only into one parent
    //public JQueryObject WrapInner(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    #endregion
    #region DOM Insertion, Inside - https://api.jquery.com/category/manipulation/dom-insertion-inside/

    //public JQueryObject Append(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(html, htmls);
    public void Append(string html) => this.CallJSOfSameNameAsWrapped(html);
    //public JQueryObject Append(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    public void Append(JQueryObject jqObject, params JQueryObject[] jqObjects)
        => this.CallJSOfSameNameAsWrapped(Params.PrependToArray(jqObject, ref jqObjects));
        
    public JQueryObject AppendTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject AppendTo(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryObject Prepend(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryObject Prepend(HtmlElement html) => this.CallJSOfSameNameAsWrapped(html);
    public JQueryObject Prepend(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped( Params.Merge( jqObject, jqObjects));
    public JQueryObject PrependTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject PrependTo(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    public string Html // rename InnerHtml
    {
        get => this.CallJSOfSameName<string>(funcName:"html");
        set => this.CallJSOfSameName<object>(value, funcName: "html");
    }

    public string Text
    {
        get => this.CallJSOfSameName<string>();
        set => this.CallJSOfSameName<object>(value);
    }

    #endregion
    #region DOM Insertion, Outside - https://api.jquery.com/category/manipulation/dom-insertion-outside/

    public JQueryObject After(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryObject After(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
    public JQueryObject Before(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(Params.Merge(html, htmls));
    public JQueryObject Before(JQueryObject jqObject, params JQueryObject[] jqObjects) => this.CallJSOfSameNameAsWrapped(Params.Merge(jqObject, jqObjects));
    public JQueryObject InsertAfter(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject InsertAfter(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryObject InsertBefore(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject InsertBefore(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    #endregion
    #region DOM Removal - https://api.jquery.com/category/manipulation/dom-removal/

    public JQueryObject Detach(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);
    public JQueryObject Empty() => this.CallJSOfSameNameAsWrapped();
    public JQueryObject Remove(string selector = null) => this.CallJSOfSameNameAsWrapped(selector);

    #endregion
    #region DOM Replacement - https://api.jquery.com/category/manipulation/dom-replacement/

    public JQueryObject ReplaceAll(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject ReplaceAll(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
    public JQueryObject ReplaceWith(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
    public JQueryObject ReplaceWith(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

    #endregion

    // TODO: Determine/test handling of parameters defined in jQueryb docs as:
    //       "content": Type: htmlString or Element or Text or Array or jQuery
    // versus "target": Type: Selector or htmlString or Element or Array or jQuery
    // versus "wrappingElement": Type: Selector or htmlString or Element or jQuery

    #region Events - https://api.jquery.com/click/

    //private JQueryEventHandler<JQueryObject, object> onClick;
    // https://api.jquery.com/click/
    public event JQueryEventHandler<JQueryObject, dynamic> OnClick {
        //add { onClick = InnerOnGeneric("click", value, onClick); }
        //remove { onClick = InnerOffGeneric("click", value, onClick); }
        add { On("click", value); }
        remove { Off("click", value); }
    }
        
    public event JQueryEventHandler<JQueryObject, dynamic> OnInput {
        add { On("input", value); }
        remove { Off("input", value); }
    }

    public event JQueryEventHandler<JQueryObject, dynamic> OnChange {
        add { On("change", value); }
        remove { Off("change", value); }
    }

    // TODO: Implement remaining events

    #endregion

    #region Events - https://api.jquery.com/category/events/

    // Event subscription for any event name
    private Dictionary<string, JQueryEventHandler<JQueryObject, dynamic>> onEvent = new Dictionary<string, JQueryEventHandler<JQueryObject, dynamic>>();
    public void On(string eventName, JQueryEventHandler<JQueryObject, dynamic> newSubscriber)
    {
        if (!onEvent.ContainsKey(eventName))// if first event subscriber
        {
            onEvent[eventName] = null;// initialize key entry
        }
        onEvent[eventName] = InnerOnGeneric(eventName, newSubscriber, onEvent[eventName]);
    }
    public void Off(string eventName, JQueryEventHandler<JQueryObject, dynamic> subscriberToRemove)
    {
        if (!onEvent.ContainsKey(eventName) || onEvent[eventName] == null)
            return;

        onEvent[eventName] = InnerOffGeneric(eventName, subscriberToRemove, onEvent[eventName]);
    }

    public delegate void JQueryEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e)
        where TSender : JQueryObject;

    //public void InternalEventCallback(string eventEncoded, string eventType)
    //{
    //    dynamic eventData = EncodedEventToDynamic(eventEncoded);
    //    onEvent[eventType]?.Invoke(this, eventData);
    //}

    // Handler funcs generated from JS when binding our event. Kept to pass to JQuery .off(..., handler) when unbinding/unsubscribing handler
    private readonly Dictionary<string, JSObject> jsHandlersByEvent = new Dictionary<string, JSObject>();

    private JQueryEventHandler<JQueryObject, dynamic> InnerOnGeneric(string eventName, JQueryEventHandler<JQueryObject, dynamic> newSubscriber, JQueryEventHandler<JQueryObject, dynamic> eventCollection)
    {
        if (eventCollection == null)// if first event subscriber on this instance/event
        {
            if (jsHandlersByEvent.ContainsKey(eventName))
                throw new Exception($"Unexpected: jsHandlersByEvent already contains key {eventName}");

            // generate handler specific to this instance+event, called by JS when event occurs
            Action<string, string, JSObject> interopListener = 
                (eventEncoded, eventType, arrayObject) => {
                    //Console.WriteLine("Event Encoded: " + eventEncoded);
                    // unpack the single ArrayObject into it's individual elements, wrapping them as JQueryObjects
                    var replacements = HelpersProxy.GetArrayObjectItems(arrayObject).Select(j=>new JQueryObject(j)).ToList();
                    // Deserialize the eventEncoded JSON string, and restore the native JS objects
                    dynamic eventData = EncodedEventToDynamic(eventEncoded, replacements);

                    onEvent[eventName]?.Invoke(this, eventData);
                };

            JSObject jsHandler = this.InnerOn(eventName,interopListener);
            jsHandlersByEvent[eventName] = jsHandler;// store handler for later unbinding
        }
            
        eventCollection += newSubscriber;            
        return eventCollection;
    }

    private JQueryEventHandler<JQueryObject, dynamic> InnerOffGeneric(string eventName, JQueryEventHandler<JQueryObject, dynamic> subscriberToRemove, JQueryEventHandler<JQueryObject, dynamic> eventCollection)
    {
        Console.WriteLine(eventCollection == null ? "eventCollection is null " : "not null");

        if (eventCollection == null)// if no subscribers on this instance/event
            return eventCollection;

        eventCollection -= subscriberToRemove;
        if (eventCollection == null) // if last subscriber removed, then remove JQuery listener
        {
            //Console.WriteLine("jsHandlersByEvent[eventName]: " + jsHandlersByEvent[eventName]);
            this.InnerOff(eventName, jsHandlersByEvent[eventName]);
            jsHandlersByEvent.Remove(eventName);
        }

        return eventCollection;

    }

    private JSObject InnerOn(string events, Action<string, string, JSObject> interopListener)
    {
        // TODO: Make shouldConvertHtmlElement configurable when we support HtmlElement
        return JSInstanceProxy.BindListener(this.jsObject, events, shouldConvertHtmlElement:true, interopListener);
    }

    private void InnerOff(string events, JSObject handlerToRemove)
    {
        JSInstanceProxy.UnbindListener(this.jsObject, events, handlerToRemove);
    }


    // Event data is serialized to JSON when the event is fired from JS and passed to C#
    // To preserve some objects as native JS references, they are extracted into a seperate array
    // and JSON properties are replaced with `{ serratedPlaceholder: 1 }` where 1 is the index of the object in the array
    // Then here when listening to an event we desrialize the JSON into a dynamic and restore the native JS references
    private dynamic EncodedEventToDynamic(string encodedEvent, List<JQueryObject> replacements)
    {
        ExpandoObject eventData = JsonConvert.DeserializeObject<ExpandoObject>(encodedEvent);
        ApplyReplacements(eventData, replacements, null, out bool hasPlaceholder);
        return eventData;
    }

    private object ApplyReplacements(ExpandoObject currentExpando, List<JQueryObject> replacements, ExpandoObject parent, out bool hasPlaceholder)
    {
        // Go through properties of currentExpando,
        // find a child expando property containing a value property named serratedPlaceholder, e.g. `target: { serratedPlaceholder : 1 }` 
        hasPlaceholder = false;// flagged true for parent when currentExpando has a single property named serratedPlaceholder
        if (replacements == null || replacements.Count == 0) // nothing to replace
            return null;

        Dictionary<string, object?> placeholdersFound = new Dictionary<string, object?>();
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
                if(hasPlaceholderInner) // If child property turns out to be a placeholder, then replace it
                {
                    placeholdersFound[property.Key] = replacement;                        
                }                    
            }
        }

        // we can't modify expondo while iterating through it above, so stage replacement in placeholdersGound 
        foreach (var placeHolder in placeholdersFound)// then apply them to the expando here
        {
            ((IDictionary<String, Object?>)currentExpando)[placeHolder.Key] = placeHolder.Value;
        }
            
        return null;
    }

    #endregion


    #region Static Helpers

        
    private static T[] ToParams<T>(params T[] args)
    {
        return args;
    }

    #endregion

    // CONSIDER: Can we implement a set of wrappers sharing interfaces to represent overloads like "content" of https://api.jquery.com/append/ "Type: htmlString or Element or Text or Array or jQuery"


}


public interface IJQueryContentParameter
{

}





