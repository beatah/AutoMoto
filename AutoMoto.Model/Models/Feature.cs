using AutoMoto.Models;
using Repository.Pattern.Ef6;
using System.Collections.Generic;

namespace AutoMoto.Model.Models
{
    public class Feature : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Car> Cars { get; set; }

        public Feature()
        {
            Cars = new List<Car>();
        }
    }
}
