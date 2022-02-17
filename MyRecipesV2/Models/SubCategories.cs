using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyRecipesV2.Models
{
    public class SubCategories
{
        public int Id { get; set; }
        [Display(Name = "Sub Category Name")]
        [MaxLength(50)]
        public string SubCategoryDescription { get; set; }
    }
}
