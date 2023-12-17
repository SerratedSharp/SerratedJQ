
using Sample.Wasm.ClientSideModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.Wasm.ClientSideModels
{
    public static class RepoFake
    {


        private static readonly List<string> ProductNames = new List<string>
        {
            "Bake-o-nator 3000",
            "Face Fixer 3D",
            "Pancakenator",
            "Auto-Egg-Now-Not-Later",
            "Carrot Disintegrator",
            "Very Normal Coffee Maker",
            "Incredibly Pretentious Moka Pot"
        };

        private static readonly List<string> RepNames = new List<string>
        {
            "Crow T Robot",
            "Tom Servo",
            "Joel",
            "Mike"

        };


        public static List<ProductSalesModel> GetProductSales()
        {
            List<ProductModel> prods = new List<ProductModel>();
            foreach (string name in ProductNames)
            {
                prods.Add(new ProductModel { Name = name });
            }

            List<RepModel> reps = new List<RepModel>();
            foreach (string name in RepNames)
            {
                reps.Add(new RepModel { Name = name });
            }

            var ran = new Random();

            var sales = new List<ProductSalesModel>();

            for (int i = 0; i < 30; ++i)
            {
                var sale = new ProductSalesModel
                {
                    Product = prods[ran.Next(prods.Count)],
                    Price = Decimal.Parse($"{ran.Next(999)}.{ran.Next(99)}"),
                    Quantity = ran.Next(99),
                    Rep = reps[ran.Next(reps.Count)]
                };
                sales.Add(sale);
            }

            return sales;
        }

    }
}
