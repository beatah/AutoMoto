using Repository.Pattern.Ef6;
using System.Collections.ObjectModel;
using AutoMoto.Model.Enums;

namespace AutoMoto.Model.Models
{
    public class Pizza : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection<Ingredient> Ingredients { get; set; }
        public double Price { get; set; }
        public SizeEnum Size { get; set; }

    }
}
