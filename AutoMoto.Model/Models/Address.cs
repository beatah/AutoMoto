using AutoMoto.Model.Models;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AutoMoto.Models
{
    public class Address : Entity
    {
        public int Id { get; set; }
        public AspNetUser User { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}