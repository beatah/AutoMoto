using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    public class Car : Entity
    {
        public int EngineCap { get; set; }
        public decimal Price { get; set; }
        public FuelTypes FuelType { get; set; }
        public int Mileage { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
        public Advertisement Advertisement { get; set; }
        [Key, ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

    }
}