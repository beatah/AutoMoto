using Repository.Pattern.Ef6;
using System;

namespace AutoMoto.Models
{
    public class Notification : Entity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
    }
}