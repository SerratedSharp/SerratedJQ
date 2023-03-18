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
    public class ListDemoPage
    {

        private static ListDemoPage singleton = null;// We use a singleton so the majority of state/members are instance instead of static
		private const string globalJSVarName = "spaWasm";

		// Client side state
		private DataModel data = new DataModel();

		// UI references
		private JQueryBox Container { get; set; } // bootstrap container, I could have multiple "views" on a single view and use a SPA approach to show hide them and use a "*Page" class like this to manage them
		private List<ProductSaleRow> Rows { get; set; } = new List<ProductSaleRow>();
        private JQueryBox quantityInput;
        private JQueryBox priceInput;
        private JQueryBox editFormRow = JQueryBox.FromHtml($@"            
                <div class='col-md-6 col-xl-4'>
                    <label for='quantity'>Quantity:</label>
                    <input type='number' class='form-control' id='quantity'>
                </div>
                <div class='col-md-6 col-xl-4'>
                    <label for='price'>Price:</label>
                    <input type='number' class='form-control' id='price'>
                </div>
        ");



        public static void Init() // static initializer exported by Program CallbacksHelper.Export and called from javascript once HTML page and JQuery are loaded/ready. Most page initialization occurs in InitSingleton()
        {
			Get();// creates, assigns, and initializes this.singleton if first creation
		}

		// The Wasm.Bootstrap type generator only generates proxy methods for instance methods, so we create an instance for this singleton.
		public static ListDemoPage Get()
		{
			if (singleton == null)
			{
				var page = new ListDemoPage();

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

			Container = JQueryBox.Select("#listcontainer");

            data.ProductSales = await RepositoryClient.GetRemoteProductSales(); // Make API call to populate data model

            foreach (var sale in data.ProductSales)
            {
                // Create row component which generates an unattached HTML DOM element and holds handle in it's .JQBox
                var newRow = new ProductSaleRow(sale);                
                Container.Append(newRow.JQBox);// Add element to DOM

                // This is not necesary, but demonstrates possibility to do live updates to HTML based on the model
                SetupModelBinding(sale, newRow);

                // save a reference to the JQuery row object
                Rows.Add(newRow);

                //newRow.JQBox.OnClick += ItemRow_OnClick;
                newRow.OnClick += ItemRow_OnClick;// Subcribe to the component's strongly typed event instead of the JQBox loosely typed event
                newRow.Model = sale;
            }

            // HTML DOM `input` events for Price/Quantity fields
            quantityInput = editFormRow.Find("#quantity");
            quantityInput.OnInput += QuantityInput_OnInput; 
            
            priceInput = editFormRow.Find("#price");
            priceInput.OnInput += PriceInput_OnInput;

            // Click events for column headers to sort
            var a = JQueryBox.Select("#sortByRep");
            a.OnClick += SortByRep_OnClick;

            var b = JQueryBox.Select("#sortByProduct");
            b.OnClick += SortByProduct_OnClick;

            var c = JQueryBox.Select("#sortByRevenue");
            c.OnClick += SortByRevenue_OnClick;

        }


        private void ItemRow_OnClick(JQueryBox sender, ProductSaleRow component, object e)
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
            
            //If we were using the JQueryBox events directly, we'd have to do this hoop jumping to get the model
            //var model = ((ProductSaleRow)sender.DataBag.Control).Model; // ProductSalesModel
            // Instead of doing dirty cast from object to model, the component implemented as strongly typed event handle passing su trhe component
            var model = component.Model;

            // prepopulate form
            quantityInput.Value = model.Quantity.ToString();
            quantityInput.DataBag.Model = model;
            priceInput.Value = model.Price.ToString();
            priceInput.DataBag.Model = model;

            // Alternatively to creating a strongly typed component, you could use the loosely typed databag if you know event handlers are only wired to JQueryBoxes that have consistent models on the DataBag
            //var model = ((ProductSaleRow)sender.DataBag.Control).Model; // ProductSalesModel
            //editFormRow.DataBag.Model = model; // Give form a reference to selected client side model.
            //editFormRow.DataBag.EditRow = sender;
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

            // If we weren't using model binding/PropertyChangeNotification,
            // we could use older UI pattern of navigating to a common parent then directly modify the UI element:
            // sender.Closest(".row").Find(".br-Quantity").Text = model.Quantity.ToString();
        }

        private void SortByRep_OnClick(JQueryBox sender, object e)
        {
            Console.WriteLine("By Rep");
            Rows.OrderBy(r => r.Model.Rep.Name)
                .ToList().ForEach(a => Container.Append(a.JQBox) // Append will move existing elements to the end, so we just need to call Append for all elements to reorder them
            );
        }

        private void SortByProduct_OnClick(JQueryBox sender, object e)
        {
            Console.WriteLine("By Prod");
            Rows.OrderBy(r => r.Model.Product.Name)
                .ToList().ForEach(a => Container.Append(a.JQBox) 
            );
        }

        private void SortByRevenue_OnClick(JQueryBox sender, object e)
        {
            Console.WriteLine("By Revenue");
            Rows.OrderByDescending(r => r.Model.Price * r.Model.Quantity)
                .ToList().ForEach(a => Container.Append(a.JQBox)
                );
        }


        private void SetupModelBinding(ProductSalesModel sale, ProductSaleRow newRow)
        {
            // Setup model binding, to update edit row and summary HTML on model's property changed event.
            // This could be encapsulated and refined using a more sophisticated model binding syntax.  PropertyChange events are generally tedious.                
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
        }

        // Make a strongly typed edit row

        //private void priceInput_OnInput(JQueryBox sender, object e)
        //{
        //    Model.Price = Int32.Parse(sender.Value);
        //    var ourEvent = OnInput;
        //    if (ourEvent != null)
        //    {
        //        ourEvent(sender, this, e);
        //    }
        //}

        //private void quantityInput_OnInput(JQueryBox sender, object e)
        //{
        //    Model.Quantity = Int32.Parse(sender.Value);
        //    var ourEvent = OnInput;
        //    if (ourEvent != null)
        //    {
        //        ourEvent(sender, this, e);
        //    }
        //}

        //private void InvokeOnInput(JQueryBox sender, object e)
        //{
        //    var ourEvent = OnInput;
        //    if (ourEvent != null)
        //    {
        //        ourEvent(sender, this, e);
        //    }
        //}
        //
        //public event JQueryTypedEventHandler<JQueryBox, ProductSaleRow, object> OnInput;



    }
}
