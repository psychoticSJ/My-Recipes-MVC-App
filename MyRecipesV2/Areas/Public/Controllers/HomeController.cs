using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyRecipesV2.DataAccess;
using MyRecipesV2.Models;
using MyRecipesV2.Models.ViewModels;

namespace MyRecipesV2.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly IConfiguration config;
        public HomeController(IConfiguration configuration)
        {
            config = configuration;
        }
        public IActionResult Index()
        {
            DB objDb = new(config);
            var recipesList = objDb.GetAllRecipes();
            return View(recipesList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("/Recipe-Details/{id}")]
        public IActionResult RecipeDetails(int id)
        {            
            var db = new DB(config);
            var recipe = db.GetRecipe(id);
            return View(recipe);
        }
    }
}
