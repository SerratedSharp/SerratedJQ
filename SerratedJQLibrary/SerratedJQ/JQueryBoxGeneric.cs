using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Extensions;
using Uno.Foundation;

namespace SerratedSharp.SerratedJQ
{
    [Obsolete("Experimental Implementation Incomplete")]
    internal class JQueryBox<ModelType> : JQueryBox
    {

        public ModelType Model
        {
            get
            {
                JQueryBox<int>.Select("asdfasf");


                return (ModelType)JQBox.DataBag.Model;
            }
            set
            {

                JQBox.DataBag.Model = value;
            }
        }

        public JQueryBox JQBox { get; set; }

        public JQueryBox(JQueryBox jQBox)
        {
            //Model = model;
            JQBox = jQBox;
        }


        public static JQueryBox<ModelType> Select(string selector)
        {

            var jq = JQueryBox.Select(selector);
            return new JQueryBox<ModelType>(jq);// wrap JQ with strongly typed model
        }

        public JQueryBox<ModelType> Append<Q>(JQueryBox<Q> child)
        {
            return FuncJQueryChaining("append", child);
        }

        internal JQueryBox<ModelType> FuncJQueryChaining<Q>(string funcName, JQueryBox<Q> jQueryBox, params string[] parameters)
        {

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


        //public JQueryBox():base()
        //{}

        //public JQueryBox(JQueryBox jQueryBox):base(jQueryBox)
        //{

        //}
    }

}
