using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
//using Uno.Extensions;
//using Uno.Foundation.Interop;
using static System.Console;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Diagnostics.Contracts;
using System.Net;
using System.Xml.Linq;
//using Uno.Foundation.Interop;

namespace SerratedSharp.SerratedJQ
{

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

        // CONSIDER: Implementing a selector builder fluent API for https://api.jquery.com/category/selectors/



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
        // TODO: Misc. Traversal 
        #endregion

        #region General Attributes - https://api.jquery.com/category/attributes/general-attributes/

        // TODO: Implement other accessors
        
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
        #endregion

        #region Class Attributes - https://api.jquery.com/category/manipulation/class-attribute/

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
        public JQueryObject WrapInner(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

        #endregion
        #region DOM Insertion, Inside - https://api.jquery.com/category/manipulation/dom-insertion-inside/

        public JQueryObject Append(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(html, htmls);
        public JQueryObject Append(JQueryObject jqObject, params JQueryBox[] jqObjects) => this.CallJSOfSameNameAsWrapped(jqObject, jqObjects);
        public JQueryObject AppendTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
        public JQueryObject AppendTo(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);
        public JQueryObject Prepend(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(html, htmls);
        public JQueryObject Prepend(JQueryObject jqObject, params JQueryBox[] jqObjects) => this.CallJSOfSameNameAsWrapped(jqObject, jqObjects);
        public JQueryObject PrependTo(string htmlOrSelector) => this.CallJSOfSameNameAsWrapped(htmlOrSelector);
        public JQueryObject PrependTo(JQueryObject jqObject) => this.CallJSOfSameNameAsWrapped(jqObject);

        public string Html
        {
            get => this.CallJSOfSameName<string>();
            set => this.CallJSOfSameName<object>(value);
        }

        public string Text
        {
            get => this.CallJSOfSameName<string>();
            set => this.CallJSOfSameName<object>(value);
        }


        #endregion
        #region DOM Insertion, Outside - https://api.jquery.com/category/manipulation/dom-insertion-outside/

        public JQueryObject After(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(html, htmls);
        public JQueryObject After(JQueryObject jqObject, params JQueryBox[] jqObjects) => this.CallJSOfSameNameAsWrapped(jqObject, jqObjects);
        public JQueryObject Before(string html, params string[] htmls) => this.CallJSOfSameNameAsWrapped(html, htmls);
        public JQueryObject Before(JQueryObject jqObject, params JQueryBox[] jqObjects) => this.CallJSOfSameNameAsWrapped(jqObject, jqObjects);
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


        // TODO: Determine/test handling of parameters defined in jQueryb docs as there:
        //       "content": Type: htmlString or Element or Text or Array or jQuery
        // versus "target": Type: Selector or htmlString or Element or Array or jQuery
        // versus "wrappingElement": Type: Selector or htmlString or Element or jQuery


        #region Static Helpers

        // Shorthand for new object[]{,,,}
        public static T[] ToParams<T>(params T[] args)
        {
            return args;
        }

        #endregion

        // CONSIDER: Can we implement a set of wrappers sharing interfaces to represent overloads like "content" of https://api.jquery.com/append/ "Type: htmlString or Element or Text or Array or jQuery"


    }


    public interface IJQueryContentParameter
    {

    }



}

