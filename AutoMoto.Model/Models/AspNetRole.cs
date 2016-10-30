using Repository.Pattern.Ef6;
using System.Collections.Generic;

namespace AutoMoto.Model.Models
{
    public class AspNetRole : Entity
    {
        public AspNetRole()
        {
            AspNetUsers = new List<AspNetUser>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
