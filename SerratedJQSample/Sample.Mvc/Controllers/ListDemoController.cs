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
    public class ListDemoController : Controller
    {
        private readonly ILogger<ListDemoController> _logger;

        public ListDemoController(ILogger<ListDemoController> logger)
        {
            _logger = logger;
        }

        public IActionResult ListDemo()
        {
            return View();
        }

        // API endpoint called by client side WASM
        public JsonResult GetSales()
        {
            var sales = RepoFake.GetProductSales();
            return Json(sales);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
