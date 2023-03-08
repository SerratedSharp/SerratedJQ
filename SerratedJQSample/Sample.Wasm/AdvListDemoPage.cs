using Sample.Wasm.ClientSideModels;
using SerratedSharp.SerratedJQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uno.Foundation;
using Uno.Foundation.Interop;

namespace Sample.Wasm
{

	/// <summary>
	/// This class has the client side C# specific to the ListDemo page.  
	/// </summary>
    public class AdvListDemoPage // :IJSObject
    {
		// Uncomment to support exposing a wrapper for this as a javascript object.
        //private ListDemoPage()
        //{
        //    Handle = JSObjectHandle.Create(this);
        //}
        //public JSObjectHandle Handle { get; }


        private static AdvListDemoPage singleton = null;// We use a singleton so the majority of state/members are instance instead of static
		private const string globalJSVarName = "spaWasm";

		// Client side state
		private DataModel data = new DataModel();

		// UI references
		private JQueryBox Container { get; set; } // bootstrap container, I could have multiple "views" on a single view and use a SPA approach to show hide them and use a "*Page" class like this to manage them
		private List<ProductSaleRow> Rows { get; set; } = new List<ProductSaleRow>();
        private JQueryBox quantityInput;
        private JQueryBox priceInput;
        private JQueryBox editFormRow = JQueryBox.FromHtml($@"
            <!--<div id='salesEditor' class='form-row'>-->
                <div class='col-md-6 col-xl-4'>
                    <label for='quantity'>Quantity:</label>
                    <input type='number' class='form-control' id='quantity'>
                </div>
                <div class='col-md-6 col-xl-4'>
                    <label for='price'>Price:</label>
                    <input type='number' class='form-control' id='price'>
                </div>
            <!--</div>-->
        ");



        public static void Init() // static initializer exported by Program CallbacksHelper.Export and called from javascript once HTML page and JQuery are loaded/ready. Most page initialization occurs in InitSingleton()
        {
			Get();// creates, assigns, and initializes this.singleton if first creation
		}

		// The Wasm.Bootstrap type generator only generates proxy methods for instance methods, so we create an instance for this singleton.
		public static AdvListDemoPage Get()
		{
			if (singleton == null)
			{
				var page = new AdvListDemoPage();

                // Optionally establish a global JS var `listDemoWasm` that expooses this singleton instance to javascript, allowing us to interact with the client side C# object from javascript.
                // Add `:IJSObject` to ListDemoPage class to support this, and uncomment the private constructor and handle.
                // This generates a javascript wrapper for this object.  InvokeJSWithInteropt detects string interpolation such as {page} for IJSObjects and will insert the JS handle reference.
                //WebAssemblyRuntime.InvokeJSWithInterop($@"
                //    listDemoWasm = {page};
                //");// This pattern is sufficient for a singleton, but would need to be more sophisticated for a non-singleton instances, such as .

                singleton = page;
				singleton.InitSingleton(); // handoff to instance initilizer
			}
			return singleton;
		}

		private async void InitSingleton()
        {
			Console.WriteLine("ListDemo WASM Executed.");

			Container = JQueryBox.Select("#listcontainer");//selector);

            data.ProductSales = await RepoFake.GetRemoteProductSales(); // Make API call to 

            foreach (var sale in data.ProductSales)
            {
                //	//Container.AppendNew(@$"
                //var newRowObj = JQueryBox.FromHtml($@" 
                //                 <div class='row border rounded my-1'>
                //                     <div class='col-xl px-1 px-sm-3'><span>{sale.Rep.Name}</span> sold <span class='br-{nameof(sale.Quantity)}'>{sale.Quantity}</span> of the <span>{sale.Product.Name}</span> at $<span class='br-{nameof(sale.Price)}'>{sale.Price:0.##}</span> each.</div>                        
                //                 </div>
                //             ");
                //var newRow = new JQueryBox<ProductSalesModel>(newRowObj);
                var newRow = new ProductSaleRow(sale);

                Container.Append(newRow.JQBox);

                
                // Setup model binding, to update row HTML on model's property changed event.  This could be encapsulated and refined using a more sophisticated model binding syntax.
                sale.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    var property = sender.GetType().GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal))
                    {
                        var value = property.GetValue(sender);
                        var elements = newRow.JQBox.Find($".br-{e.PropertyName}");
                        if (elements.Length == 1)
                            elements.Text = value?.ToString() ?? "";
                    }

                    var revenueSpan = JQueryBox.Select("#totalRevenue");
                    revenueSpan.Text = data.ProductSales.Sum(s => s.Quantity * s.Price).ToString();
                };

                // save a reference to the JQuery row object
                Rows.Add(newRow);

                newRow.JQBox.OnClick += ItemRow_OnClick;
                newRow.Model = sale;
                newRow.Model = sale;
            }

            // Form input handlers for live updating view as edited
            quantityInput = editFormRow.Find("#quantity");
            quantityInput.OnInput += QuantityInput_OnInput; // wire up to HTML DOM `input` event, fires for each keystroke 
            
            priceInput = editFormRow.Find("#price");
            priceInput.OnInput += PriceInput_OnInput;



            var a = JQueryBox.Select("#sortByRep");
            a.OnClick += SortByRep_OnClick;
            //pinned.Add(a);

            var b = JQueryBox.Select("#sortByProduct");
            b.OnClick += SortByProduct_OnClick;
            //pinned.Add(b);

            var c = JQueryBox.Select("#sortByRevenue");
            c.OnClick += SortByRevenue_OnClick;
            //pinned.Add(c);


        }

        private static List<JQueryBox> pinned = new List<JQueryBox>();



        private void ItemRow_OnClick(JQueryBox sender, object e)
        {
            if (sender.DataBag.IsEditing == true )// ignore click if current item being edited
                return;

            var priorEditRow = Rows.SingleOrDefault(r => r.JQBox.DataBag.IsEditing == true);
            if (priorEditRow != null) // Clear prior edit row styles (more appropriately would use css class instead of inline styles)
            {
                priorEditRow.JQBox.Styles["font-style"] = "";
				priorEditRow.JQBox.Styles["color"] = "";
				priorEditRow.JQBox.DataBag.IsEditing = false;
            }

			sender.Styles["font-style"] = "italic";
			sender.Styles["color"] = "blue";
            sender.DataBag.IsEditing = true;
            sender.Append(editFormRow);

            // Note we could encapsulate product rows in their own class and include statically typed model properties to avoid use of dynamic DataBag
            var model = ((ProductSaleRow)sender.DataBag.Control).Model; // ProductSalesModel
            editFormRow.DataBag.Model = model; // Give form a reference to selected client side model.
            editFormRow.DataBag.EditRow = sender;
            
            // prepopulate form
			quantityInput.Value = model.Quantity.ToString();
            quantityInput.DataBag.Model = model;
            priceInput.Value = model.Price.ToString();
            priceInput.DataBag.Model = model;
            
        }

        private void PriceInput_OnInput(JQueryBox sender, object e)
        {
            var model = (ProductSalesModel)sender.DataBag.Model;
            model.Price = Decimal.Parse(sender.Value); // updating the model triggers PropertyChangeNotification
        }

        private void QuantityInput_OnInput(JQueryBox sender, object e)
        {
            var model = (ProductSalesModel)sender.DataBag.Model;            
            model.Quantity = Int32.Parse(sender.Value);

            // If we weren't using model binding/PropertyChangeNotification, we could use older UI pattern of navigating to a common parent then directly modify the UI element:
            sender.Closest(".row").Find(".br-Quantity").Text = model.Quantity.ToString();
        }

        private void SortByRep_OnClick(JQueryBox sender, object e)
        {
            Console.WriteLine("By Rep");
            Rows.OrderBy(j => ((ProductSalesModel)j.Model).Rep.Name)
                .ToList().ForEach(a => Container.Append(a.JQBox) // Append will move existing elements to the end, so we just need to call Append for all elements to reorder them
            );
        }


        private void SortByProduct_OnClick(JQueryBox sender, object e)
        {
            Console.WriteLine("By Prod");
            Rows.OrderBy(j => ((ProductSalesModel)j.Model).Product.Name)
                .ToList().ForEach(a => Container.Append(a.JQBox) 
            );
        }

        private void SortByRevenue_OnClick(JQueryBox sender, object e)
        {
            Console.WriteLine("By Revenue");
            Rows.OrderByDescending(j => ((ProductSalesModel)j.Model).Price * ((ProductSalesModel)j.Model).Quantity)
                .ToList().ForEach(a => Container.Append(a.JQBox)
                );
        }


    }
}
