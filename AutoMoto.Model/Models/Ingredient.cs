using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;

namespace AutoMoto.Model.Models
{
    public class Ingredient : Entity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(128)]
        [Required]
        public string Name { get; set; }
    }
}