using System.ComponentModel;

namespace AutoMoto.Contracts.ViewModels
{
    public class ManufacturerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
    }
}
