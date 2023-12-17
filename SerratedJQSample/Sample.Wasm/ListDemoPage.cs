using Sample.Wasm.ClientSideModels;
using SerratedSharp.JSInteropHelpers;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
		private JQueryPlainObject Container { get; set; } // bootstrap container, I could have multiple "views" on a single view and use a SPA approach to show hide them and use a "*Page" class like this to manage them
		private List<ProductSaleRow> Rows { get; set; } = new List<ProductSaleRow>();
        private JQueryPlainObject quantityInput;
        private JQueryPlainObject priceInput;
        private JQueryPlainObject editFormRow = JQueryPlain.ParseHtmlAsJQuery($@"            
                <div class='col-md-6 col-xl-4'>
                    <label for='quantity'>Quantity:</label>
                    <input type='number' class='form-control' id='quantity'>
                </div>
                <div class='col-md-6 col-xl-4'>
                    <label for='price'>Price:</label>
                    <input type='number' class='form-control' id='price'>
                </div>
        ");

        // Optionally could us [JSExport] to expose to javascript
        public static void Init()
        {
			Get();// creates, assigns, and initializes this.singleton if first creation
		}

		// The Wasm.Bootstrap type generator only generates proxy methods for instance methods, so we create an instance for this singleton.
		public static ListDemoPage Get()
		{
			if (singleton == null)
			{
				var page = new ListDemoPage();

                singleton = page;
				singleton.InitSingleton(); // handoff to instance initializer
			}
			return singleton;
		}

		private async void InitSingleton()
        {
			Console.WriteLine("ListDemo WASM Executed.");

			Container = JQueryPlain.Select("#listcontainer");

            data.ProductSales = await RepositoryClient.GetRemoteProductSales(); // Make API call to populate data model

            foreach (var sale in data.ProductSales)
            {
                // Create row component which generates an unattached HTML DOM element and holds handle in it's .JQBox
                var newRow = new ProductSaleRow(sale);                
                Container.Append(newRow.JQueryObject);// Add element to DOM

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
            var a = JQueryPlain.Select("#sortByRep");
            a.OnClick += SortByRep_OnClick;

            var b = JQueryPlain.Select("#sortByProduct");
            b.OnClick += SortByProduct_OnClick;

            var c = JQueryPlain.Select("#sortByRevenue");
            c.OnClick += SortByRevenue_OnClick;

        }


        private void ItemRow_OnClick(JQueryPlainObject sender, ProductSaleRow component, object e)
        {          
            // Can log the event and data to the browser console for troubleshooting. These will appear as expandable JS objects in the browser console that can be inspected in great detail.
            GlobalJS.Console.Log("ItemRow_OnClick", sender.JSObject, e, sender.DataAsJSObject());

            if (sender.Data<bool?>("IsEditing") == true)// ignore click if current item being edited
            {
                Console.WriteLine("already editing");
                return;
            }
            Console.WriteLine("start editing");
            var priorEditRow = Rows.SingleOrDefault(r => r.JQueryObject.Data<bool?>("IsEditing") == true);
            if (priorEditRow != null) // Clear prior edit row styles (more appropriately would use css class instead of inline styles)
            {
                priorEditRow.JQueryObject.Css("font-style", "");
                priorEditRow.JQueryObject.Css("color", "");
                priorEditRow.JQueryObject.Data("IsEditing", false);
            }

            sender.Css("font-style", "italic");
            sender.Css("color", "blue");
            sender.Data("IsEditing", true);

            sender.Append(editFormRow);

            //If we were using the JQueryPlainObject events directly, we'd have to do this hoop jumping to get the model
            //var model = ((ProductSaleRow)sender.DataBag.Control).Model; // ProductSalesModel
            // Instead of doing dirty cast from object to model, we use the strongly typed component to access its model
            var model = component.Model;

            // prepopulate form
            quantityInput.Val(model.Quantity.ToString());
            quantityInput.Data("Model", model);
            priceInput.Val(model.Price.ToString());
            priceInput.Data("Model", model);
        }

        private void PriceInput_OnInput(JQueryPlainObject sender, object e)
        {
            var model = sender.Data<ProductSalesModel>("Model");
            model.Price = Decimal.Parse(sender.Val()); // updating the model triggers PropertyChangeNotification
        }

        private void QuantityInput_OnInput(JQueryPlainObject sender, object e)
        {
            var model = sender.Data<ProductSalesModel>("Model");            
            model.Quantity = Int32.Parse(sender.Val());

            // If we weren't using model binding/PropertyChangeNotification,
            // we could use older UI pattern of navigating to a common parent then directly modify the UI element:
            // sender.Closest(".row").Find(".br-Quantity").Text = model.Quantity.ToString();
        }

        private void SortByRep_OnClick(JQueryPlainObject sender, object e)
        {
            Console.WriteLine("By Rep");
            Rows.OrderBy(r => r.Model.Rep.Name)
                .ToList().ForEach(a => Container.Append(a.JQueryObject) // Append will move existing elements to the end, so we just need to call Append for all elements to reorder them
            );
        }

        private void SortByProduct_OnClick(JQueryPlainObject sender, object e)
        {
            Console.WriteLine("By Prod");
            Rows.OrderBy(r => r.Model.Product.Name)
                .ToList().ForEach(a => Container.Append(a.JQueryObject) 
            );
        }

        private void SortByRevenue_OnClick(JQueryPlainObject sender, object e)
        {
            Console.WriteLine("By Revenue");
            Rows.OrderByDescending(r => r.Model.Price * r.Model.Quantity)
                .ToList().ForEach(a => Container.Append(a.JQueryObject)
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
                    var elements = newRow.JQueryObject.Find($".br-{e.PropertyName}");
                    if (elements.Length == 1)
                        elements.Text(value?.ToString() ?? "");
                }

                var revenueSpan = JQueryPlain.Select("#totalRevenue");
                revenueSpan.Text( data.ProductSales.Sum(s => s.Quantity * s.Price).ToString());
            };
        }

    }
}
