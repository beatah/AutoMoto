using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;

namespace AutoMoto.Models
{
    public class Address : Entity
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}