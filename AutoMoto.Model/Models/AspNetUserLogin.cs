using Repository.Pattern.Ef6;

namespace AutoMoto.Model.Models
{
    public class AspNetUserLogin : Entity
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
