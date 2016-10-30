using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoMoto.Contracts.ViewModels
{
    public class ManufacturerViewModel
    {
        public string Action { get; set; }
        public int Id { get; set; }
        [StringLength(128)]
        [DisplayName("Nazwa")]
        [Required]
        public string Name { get; set; }
    }
}
