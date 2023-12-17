
using Newtonsoft.Json;
using Sample.Wasm.ClientSideModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Wasm.ClientSideModels
{
    public static class RepositoryClient
    {
        // Alternative approach of prevent IL Linker trimming models which would breaks JSON deserialization
        //[DynamicDependency(DynamicallyAccessedMemberTypes.PublicNestedTypes, typeof(TrimmerDMZ))]
        public static async Task<List<ProductSalesModel>> GetRemoteProductSales()
        {
            try { 
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:5001/ListDemo/");              
                var response = await client.GetAsync("GetSales");
                var content = await response.Content.ReadAsStringAsync();
                // using System.Text.Json, deseriale the list of ProductSalesModel
                var prods = System.Text.Json.JsonSerializer.Deserialize<List<ProductSalesModel>>(content);


                //var prods = JsonConvert.DeserializeObject<List<ProductSalesModel>>(content);
                //var prods = JsonSerializer.Deserialize<List<ProductSalesModel>>(content);
                Console.WriteLine("Count:" + prods.Count);
                //Console.WriteLine(JsonSerializer.Serialize(prods));
                foreach (var sale in prods)
                {
                    Console.WriteLine("Prod:" + sale.Product.Name);
                    Console.WriteLine("Rep:" + sale.Rep.Name);
                }

                return prods;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
