using Sample.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sample.Wasm.ClientSideModels;

namespace Sample.Mvc.Controllers
{
    public class AdvListDemoController : Controller
    {
        private readonly ILogger<AdvListDemoController> _logger;

        public AdvListDemoController(ILogger<AdvListDemoController> logger)
        {
            _logger = logger;
        }



        public IActionResult AdvListDemo()
        {
            return View();
        }

        public JsonResult GetSales()
        {
            ProductSalesModel sale = new ProductSalesModel
            {
                Product = new ProductModel { Name = "PName" }
                ,Price = 13
                ,Quantity = 5
                ,Rep = new RepModel { Name = "Bob"}
            };

            var sales = RepoFake.GetProductSales(); //new List<ProductSalesModel>{ sale };
            return Json(sales);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
