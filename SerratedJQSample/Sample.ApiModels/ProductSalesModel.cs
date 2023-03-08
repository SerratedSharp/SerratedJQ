
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Wasm.ClientSideModels
{
    public class ProductModel
    {
        public string Name { get; set; }
    }

    public class RepModel
    {
        public string Name { get; set; }
    }


    public class ProductSalesModel : INotifyPropertyChanged
    {
        private RepModel rep;
        private decimal price;
        private int quantity;
        private ProductModel product;

        public ProductModel Product
        {
            get => product;
            set
            {
                if (product == value) return;
                product = value;
                Notify();
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity == value) return;
                quantity = value;
                Notify();
            }
        }
        public decimal Price
        {
            get => price;
            set
            {
                if (price == value) return;
                price = value;
                Notify();
            }
        }
        public RepModel Rep
        {
            get => rep;
            set
            {
                if (rep == value) return;
                rep = value;
                Notify();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataModel
    {
        public List<ProductSalesModel> ProductSales { get; set; }//= RepoFake.GetProductSales();
    }



}
