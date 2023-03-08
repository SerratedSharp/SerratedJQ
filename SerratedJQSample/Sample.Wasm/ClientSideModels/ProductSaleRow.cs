using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Wasm.ClientSideModels
{



    internal class ProductSaleRow //: JQueryBox
    {
        

        //public ProductSaleRow():base(){}

        public ProductSaleRow(ProductSalesModel productSalesModel) //: base(CreateJQBox(productSalesModel))
        {
            Model = productSalesModel;            
        }

        //private static JQueryBox CreateJQBox(ProductSalesModel productSalesModel)
        //{
        //    return JQueryBox.FromHtml(GetHtml(productSalesModel));
        //}



        public ProductSalesModel Model { get; set; }

        private JQueryBox jQBox;
        public JQueryBox JQBox
        {
            get
            {
                if (this.jQBox == null)
                {
                    jQBox = JQueryBox.FromHtml(GetHtml(Model));
                    jQBox.DataBag.Control = this;
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



    }
}
