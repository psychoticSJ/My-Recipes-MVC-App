using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyRecipesV2.Models
{
public class Categories
{
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        [MaxLength(50)]
        public string CategoryDescription { get; set; }
    }
}
