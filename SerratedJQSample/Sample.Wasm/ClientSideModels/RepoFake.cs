
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

        public static async Task<List<ProductSalesModel>> GetRemoteProductSales()
        {
            //List<ProductSalesModel> prods = new List<ProductSalesModel>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44386/AdvListDemo/");
            
            var response = await client.GetAsync("GetSales");
            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            var prods = JsonSerializer.Deserialize<List<ProductSalesModel>>(content);
            Console.WriteLine("Count:" + prods.Count);
            Console.WriteLine(JsonSerializer.Serialize(prods));
            foreach (var sale in prods)
            {
                Console.WriteLine("Prod:" + sale.Product.Name);
                
                Console.WriteLine("Rep:" + sale.Rep.Name);
            }

            //prods.Add(new ProductSalesModel
            //{
            //    Product = new ProductModel { Name = ""//responseString 
            //    },
            //    Price = 13,
            //    Quantity = 3,
            //    Rep = new RepModel { Name = "Remmy" }
            //});


            return prods;
        }


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
