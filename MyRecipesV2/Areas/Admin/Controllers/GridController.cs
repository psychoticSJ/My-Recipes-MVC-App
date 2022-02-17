using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using MyRecipesV2.Models;
using MyRecipesV2.Models.ViewModels;

namespace MyRecipesV2.Controllers
{
    [Area("Admin")]
    public class GridController : Controller
    {
        
        public ActionResult Orders_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Enumerable.Range(0, 50).Select(i => new OrderViewModel
            {
                OrderID = i,
                Freight = i * 68,
                OrderDate = new DateTime(2021, 10, 15).AddDays(i % 7),
                ShipName = "ShipName " + i * 31,
                ShipCity = "ShipCity " + i *27
            });
            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult);
        }

       // [Route("/orders")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
