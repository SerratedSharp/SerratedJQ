using SerratedSharp.SerratedJQ.Plain;
using System;

namespace Sample.Wasm.ClientSideModels
{

    /// <summary>
    /// This is a rough example of an unrefined UI component.  
    /// It provides its own data model, a data driven HTML template, and events where appropriate.
    /// </summary>
    internal class ProductSaleRow
    {

        public ProductSaleRow(ProductSalesModel productSalesModel){
            Model = productSalesModel;            
        }

        public ProductSalesModel Model { get; set; }

        private JQueryPlainObject jQueryObject;
        public JQueryPlainObject JQueryObject
        {
            get
            {
                if (this.jQueryObject == null)
                {
                    jQueryObject = JQueryPlain.ParseHtmlAsJQuery(GetHtml(Model).Trim() );
                    jQueryObject.OnClick += JQRowOnClick;

                }
                return jQueryObject;
            }
            set => jQueryObject = value;
        }

        private static string GetHtml(ProductSalesModel model)
        {
            return $@" 
                <div class='row border rounded my-1'>
                    <div class='col-xl px-1 px-sm-3'><span>{model.Rep.Name}</span> sold <span class='br-{nameof(model.Quantity)}'>{model.Quantity}</span> of the <span>{model.Product.Name}</span> at $<span class='br-{nameof(model.Price)}'>{model.Price:0.##}</span> each.</div>                        
                </div>
            ";
        }

        // Exposes click event with strongly typed model included
        private void JQRowOnClick(JQueryPlainObject sender, object e)
        {
            
            // Pass thru event from JQueryPlainObject to our strongly typed event
            var ourEvent = OnClick;
            if (ourEvent != null)
            {
                ourEvent(sender, this, e); //include strongly typed `this`/ ProductSalesRow for subscribers
            }
        }

        public delegate void JQueryTypedEventHandler<in TSender, in TComponent, in TEventArgs>
            (TSender sender, TComponent component, TEventArgs e)
            where TSender : JQueryPlainObject;

        // We use explicit event so that we can only create the JQuery listener when necessary
        public event JQueryTypedEventHandler<JQueryPlainObject, ProductSaleRow, object> OnClick;

    }
}
