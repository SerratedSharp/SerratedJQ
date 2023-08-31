//using Uno.Extensions;
using Uno.Foundation;
using Uno.Foundation.Interop;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

//using Newtonsoft.Json;

namespace SerratedSharp.SerratedJQ
{
    // The main purpose of this experiment was to create a wrapper that also included a strongly typed model object.
    // This would allow strongly typed components which also included model data attached to them.
    // The recommended approach is just for consumers to create their own strongly typed object with a property reference to the JQueryBox as demonstrated in JQ Sample.
    //
    // The core problem with the below approach is many queries against the DOM produce a new JQueryBox, so performing
    // a JQuery select that selects a prior strongly typed object has no knowledge of the original C# wrapper or data attached to it.
    //
    // Additionally when we perform a .Select or .Find, we don't necesarily know what strongly typed model C# type should wrap the DOM reference.
    // I.e. if we initially generated a JQueryBoxV2.Select<PaymentComponent>('#payment') and later do a .Find('#payment'), the caller needs to know that
    // the generic type was <PAymentComponent>.  For this reason, likely attaching multiple models to an object and allowing feature detection style approach
    // from the caller, such as JQueryBox.HasModel<PaymentComponent>/.GetModel<PaymentComponent> would be more appropriated so that multipel models can be added to a DOM ref.

    // Even if we select the same DOM element, a new unique JQuery javascript reference is created to wrap that DOM element.
    // So when selecting the same element twice, our C# wrapper generates both a new C# wrapper and a new JQuery reference.
    //
    // Note while $('#someElement div') === $('#someElement').find('div) is false, $('#someElement div')[0] === $('#someElement').find('div)[0] is true.
    // This is because the instance of the JQuery reference is unique, but the actual HTML DOM reference is identical.
    // Again, this reason a proper solution would probably use a Symbol or Dataset to attach something to the DOM element itself.
    //
    // Possible solutions:
    // - A serialized C# JSON object string
    // - A managed handle represnted as an integer (problematic without an approach to ensure the C# object isn't garbage collected)
    // 
    // How do event handlers work now?  How do we have a reference from a JS object back to a C# callback(which intrinsically includes a instance referenc
    // and not worry about the GC?

    // MAde private since this is not ready for consumption

    [Obsolete("Experimental Implementation Incomplete")]
    internal class JQueryBoxV2<ModelType> : JQueryBox
    {
        public ModelType Model
        {
            get {
                return (ModelType)this.DataBag.Model;
            }
            set {
                this.DataBag.Model = value;
            }
        }


        private static int keep = 2;
        static JQueryBoxV2()
        {
            WebAssemblyRuntime.InvokeJS(_1.ManagedObjectJavascriptDispatcherDeclaration);

            if (keep == 1)// Prevent these methods from being removed by ILLinker at compile time. The condition being false prevents these from being clled at runtime. 
            {
                var keeper = new JQueryBox();
                keeper.InternalClickCallback("", "");
                keeper.InternalEventCallback("", "");
                keeper.InternalInputCallback("", "");
            }

        }
        
        internal JQueryBoxV2()
        {
            handle = JSObjectHandle.Create(this);
            styles = new JQIndexer(this, "css");
            //properties = new JQIndexer(this, "prop");
            attributes = new JQIndexer(this, "attr");
        }


        public static JQueryBox<ModelType> Select(string selector)
        {

            var jq = JQueryBox.Select(selector);
            return new JQueryBox<ModelType>(jq);// wrap JQ with strongly typed model
        }

        public JQueryBoxV2<ModelType> Append<Q>(JQueryBox<Q> child)
        {
            return FuncJQueryChaining("append", child);
        }

        internal JQueryBoxV2<ModelType> FuncJQueryChaining<Q>(string funcName, JQueryBox<Q> jQueryBox, params string[] parameters)
        {
            // We stop creating a new object where we expect the behavior of JQuery it to return a ref to the same instance.
            // Instead we return our on C# ref, instead of potentially generating a new one that looks like a
            //  different instance reference but wraps the same JQuery object.

            //var newBox = new JQueryBox();
            //var newModelBox = new JQueryBox<ModelType>(newBox);

            string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));
            if (!jsParameters.IsNullOrWhiteSpace())
                jsParameters = "," + jsParameters;

            WebAssemblyRuntime.InvokeJSWithInterop(
                   $@"{this}.{_1.jqbj}.{funcName}({jQueryBox}.{_1.jqbj}{jsParameters});"
            );

            return this;

            //WebAssemblyRuntime.InvokeJSWithInterop(
            //        $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jQueryBox}.{_1.jqbj}{jsParameters});"
            //);

            //string isSameObject = WebAssemblyRuntime.InvokeJSWithInterop(
            //        $@"{newBox}.{_1.jqbj} === {this}.{_1.jqbj}.{funcName};"
            //);

            //// if is same JS object returned from method chaining, then dispose of the new one and return the original
            //if (isSameObject == "true")
            //{
            //    newBox.handle.Dispose();
            //    return this;
            //}
            //else
            //{
            //    var newTyped = new JQueryBox<ModelType>(newBox);
            //    newTyped.Model = this.Model;
            //    return newTyped;
            //}



        }

        // These methods were originally declared in based JQueryBox,
        // so that all references to any JQueryBox could select/create generic strongly typed objects
        // I.e. a <div> with a JQueryBox<PaymentComponent> could have other HTML elements bound to
        // a different type inside it.  So JQueryBox.Select<PaymentComponent>('#payment').Find<CustomButton>('#submitButton')
        // is conceivable.  So these methods being declared in a non-generic JQueryBox means the type
        // <ModelType> parameter would be specified at call time for each call, allowing consumer to specify
        // the strongly typed model they want generated.


        public static JQueryBox<ModelType> Select<ModelType>(string selector)
        {
            var jq = JQueryBox.Select(selector);
            return new JQueryBox<ModelType>(jq);// wrap JQ with strongly typed model
        }


        public static JQueryBox<ModelType> FromHtml<ModelType>(string html)
        {
            var jq = JQueryBox.FromHtml(html);
            return new JQueryBox<ModelType>(jq);// wrap JQ with strongly typed model
        }

        public JQueryBoxV2<ModelType> Find<ModelType>(string selector) => FuncJQueryCallerName<ModelType>(selector);


        internal JQueryBoxV2<ModelType> FuncJQueryCallerName<ModelType>(string parameter = null, string parameter2 = null, [CallerMemberName] string funcName = null)
        {
            //Console.WriteLine("Caller: '" + funcName + "'");

            return FuncJQuery<ModelType>(Char.ToLowerInvariant(funcName[0]) + funcName.Substring(1), parameter, parameter2);
        }

        internal JQueryBoxV2<ModelType> FuncJQuery<ModelType>(string funcName, params string[] parameters)
        {
            JQueryBoxV2<ModelType> newBox = new JQueryBoxV2<ModelType>();
            //Console.WriteLine(funcName);

            string jsParameters = string.Join(",", parameters.Select(p => $"'{p}'"));
            //Console.WriteLine(jsParameters);
            WebAssemblyRuntime.InvokeJSWithInterop(
                    $@"{newBox}.{_1.jqbj} = {this}.{_1.jqbj}.{funcName}({jsParameters});"
            );
            return newBox;
        }



    }


}

