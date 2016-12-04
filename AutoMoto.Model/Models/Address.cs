using AutoMoto.Model.Models;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    public class Address : Entity
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}