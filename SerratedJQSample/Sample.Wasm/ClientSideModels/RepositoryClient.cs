
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
    public static class RepositoryClient
    {
        public static async Task<List<ProductSalesModel>> GetRemoteProductSales()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44386/ListDemo/");
            
            var response = await client.GetAsync("GetSales");
            var content = await response.Content.ReadAsStringAsync();
            var prods = JsonSerializer.Deserialize<List<ProductSalesModel>>(content);
            Console.WriteLine("Count:" + prods.Count);
            Console.WriteLine(JsonSerializer.Serialize(prods));
            foreach (var sale in prods)
            {
                Console.WriteLine("Prod:" + sale.Product.Name);
                Console.WriteLine("Rep:" + sale.Rep.Name);
            }

            return prods;
        }
    }
}
