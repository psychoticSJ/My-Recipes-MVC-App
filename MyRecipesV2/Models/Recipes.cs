using System.ComponentModel.DataAnnotations;

namespace MyRecipesV2.Models
{
    public class Recipes
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Ingredients { get; set; }
        public int Category { get; set; }

        [Display(Name ="Sub Category")]
        public int? SubCategory { get; set; }

        [Display(Name = "Number Of Servings")]
        public int NumberOfServings { get; set; }

        [Display(Name = "Prep Time")]
        public int PrepTime { get; set; }

        [Display(Name = "Cook Time")]
        public int CookTime { get; set; }
        public int TotalTime { get; set; }

        [Required]
        [Display(Name = "Directions - Step 1")]
        public string Step1 { get; set; }

        [Required]
        [Display(Name = "Directions - Step 2")]
        public string Step2 { get; set; }

        [Display(Name = "Directions - Step 3")]
        public string? Step3 { get; set; }

        [Display(Name = "Directions - Step 4")]
        public string? Step4 { get; set; }

        [Display(Name = "Directions - Step 5")]
        public string? Step5 { get; set; }

        [Display(Name = "Nutrition Facts")]
        public string? NutritionFacts { get; set; }
        public string? Image1URL { get; set; }
        public string? Image2URL { get; set; }
        public string? Image3URL { get; set; }
        public string? Image4URL { get; set; }
        public string? Image5URL { get; set; }
        public string? VideoURL { get; set; }
        public string? Tags { get; set; }

        [Display(Name = "Submitted By")]
        public string? SubmittedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }
}
