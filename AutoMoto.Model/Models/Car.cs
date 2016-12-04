using AutoMoto.Enums;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public Advertisement Advertisement { get; set; }
        public int Id { get; set; }

        public int Year { get; set; }

    }
}