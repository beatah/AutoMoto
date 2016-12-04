using Repository.Pattern.Ef6;
using System.Collections.Generic;

namespace AutoMoto.Models
{
    public class Manufacturer : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Model> Models { get; set; }

        public Manufacturer()
        {
            IsActive = true;
        }

    }
}