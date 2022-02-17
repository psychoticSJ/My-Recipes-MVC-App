using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyRecipesV2.Models.ViewModels
{
    public class RecipesViewModel
    {
        public Recipes Recipe { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? SubCategoryList { get; set; }
    }
}
