using AutoMoto.Models;

namespace AutoMoto.Contracts.ViewModels
{
    public class ModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
    }
}
