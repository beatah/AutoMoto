using Repository.Pattern.Ef6;

namespace AutoMoto.Models
{
    public class Model : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
    }
}