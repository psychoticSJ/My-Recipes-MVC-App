using Microsoft.AspNetCore.Mvc;
using MyRecipesV2.DataAccess;
using MyRecipesV2.Models.ViewModels;
using MyRecipesV2.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyRecipesV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecipesController : Controller
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment webHostEnvironment; //to upload images on the server in a folder (in this case in the wwwroot folder). IWebHostEnvironment class helps to get the folder path on the server

        public RecipesController(IConfiguration configuration, IWebHostEnvironment webHostEnviro)
        {
            config = configuration;
            webHostEnvironment = webHostEnviro;
        }

        // GET: RecipesController        
        //[Route("/list-of-recipes")]
        public ActionResult ListOfRecipes()
        {
            return View();
        }

        // GET: RecipesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RecipesController/CreateOrUpdate
        //[Route("/create-or-edit-recipe")]
        public ActionResult CreateOrUpdate(int? id)
        {
            var objDb = new DB(config);
            RecipesViewModel recipesViewModel = new RecipesViewModel()
            {
                Recipe = new Recipes(),
                CategoryList = objDb.GetAllCategories().Select(c => new SelectListItem
                {
                    Text = c.CategoryDescription,
                    Value = c.Id.ToString()
                }),
                SubCategoryList = objDb.GetAllSubCategories().Select(c => new SelectListItem
                {
                    Text = c.SubCategoryDescription,
                    Value = c.Id.ToString()
                })
            };
            if (id != null || id > 0)
            {
                var db = new DB(config);
                recipesViewModel.Recipe = db.GetRecipe(id);
            }
            return View(recipesViewModel);
        }

        // POST: RecipesController/CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrUpdate(RecipesViewModel recipesViewModel)
        {
            try
            {                
                if (ModelState.IsValid)
                {
                    //get the wwwroot folder path to upload image to that folder
                    string webRootPath = webHostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString(); //give the uploaded file a new name
                        var uploads = Path.Combine(webRootPath, @"images/recipes"); //navigate to the wwwroot/images/products folder
                        var extension = Path.GetExtension(files[0].FileName);

                        //if file exists (this is an edit), delete existing file and upload new one
                        if (recipesViewModel.Recipe.Image1URL != null)
                        {
                            var imagePath = Path.Combine(webRootPath, recipesViewModel.Recipe.Image1URL.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        recipesViewModel.Recipe.Image1URL = @"\images\recipes\" + fileName + extension;
                        //for now i am passing the same URL to all images
                        recipesViewModel.Recipe.Image2URL = recipesViewModel.Recipe.Image1URL;
                        recipesViewModel.Recipe.Image3URL = recipesViewModel.Recipe.Image1URL;
                        recipesViewModel.Recipe.Image4URL = recipesViewModel.Recipe.Image1URL;
                        recipesViewModel.Recipe.Image5URL = recipesViewModel.Recipe.Image1URL;
                    }
                    else //update when image is not changed
                    {
                        if (recipesViewModel.Recipe.Id != 0)
                        {
                            var dB = new DB(config);
                            Recipes objRecipeFromDb = dB.GetRecipe(recipesViewModel.Recipe.Id);
                            recipesViewModel.Recipe.Image1URL = objRecipeFromDb.Image1URL;
                            //for now i am passing the same URL to all images
                            recipesViewModel.Recipe.Image2URL = recipesViewModel.Recipe.Image1URL;
                            recipesViewModel.Recipe.Image3URL = recipesViewModel.Recipe.Image1URL;
                            recipesViewModel.Recipe.Image4URL = recipesViewModel.Recipe.Image1URL;
                            recipesViewModel.Recipe.Image5URL = recipesViewModel.Recipe.Image1URL;
                        }
                    }
                    var objDb = new DB(config);
                    if (objDb.SaveRecipe(recipesViewModel))
                        return RedirectToAction(nameof(ListOfRecipes));
                    
                }
                else
                {
                    var objDb = new DB(config);
                    recipesViewModel.CategoryList = objDb.GetAllCategories().Select(c => new SelectListItem
                    {
                        Text = c.CategoryDescription,
                        Value = c.Id.ToString()
                    });
                    recipesViewModel.SubCategoryList = objDb.GetAllSubCategories().Select(c => new SelectListItem
                    {
                        Text = c.SubCategoryDescription,
                        Value = c.Id.ToString()
                    });
                }
            }
            catch
            {
                return View(recipesViewModel);
            }
            return View(recipesViewModel);
        }

        // GET: RecipesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }


        // GET: RecipesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RecipesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        public IActionResult GetRecipes([DataSourceRequest] DataSourceRequest request)
        {
            var objDb = new DB(config);
            var dtRecipes = objDb.GetAllRecipes();            
            var dtResults = dtRecipes.ToDataSourceResult(request);
            return Json(dtResults);
        }
    }
}
