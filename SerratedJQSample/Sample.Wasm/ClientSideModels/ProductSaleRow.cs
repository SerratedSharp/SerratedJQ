using SerratedSharp.SerratedJQ.Plain;
using System;

namespace Sample.Wasm.ClientSideModels
{

    /// <summary>
    /// This is a rough example of a UI "component".  
    /// It provides it's own data model, 
    /// a data driven HTML template,
    /// events where appropriate.
    /// </summary>
    internal class ProductSaleRow
    {

        public ProductSaleRow(ProductSalesModel productSalesModel){
            Model = productSalesModel;            
        }

        public ProductSalesModel Model { get; set; }

        private JQueryPlainObject jQBox;
        public JQueryPlainObject JQBox
        {
            get
            {
                if (this.jQBox == null)
                {
                    jQBox = JQueryPlain.ParseHtmlAsJQuery(GetHtml(Model).Trim() );
                    jQBox.OnClick += JQRowOnClick;

                }
                return jQBox;
            }
            set => jQBox = value;
        }

        private static string GetHtml(ProductSalesModel model)
        {
            return $@" 
                <div class='row border rounded my-1'>
                    <div class='col-xl px-1 px-sm-3'><span>{model.Rep.Name}</span> sold <span class='br-{nameof(model.Quantity)}'>{model.Quantity}</span> of the <span>{model.Product.Name}</span> at $<span class='br-{nameof(model.Price)}'>{model.Price:0.##}</span> each.</div>                        
                </div>
            ";
        }

        // Callers could easily subscribe to row.JQBox.OnClick, but the handler of the event
        // would be on a JQBox without the model.
        // They'd be able to access the model through the DataBag, but would require
        // a dirty cast from `object` to ProductSalesModel.
        // Providing our own event here makes this more like a component and allows
        // the model to be passed to the event strongly typed.
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
