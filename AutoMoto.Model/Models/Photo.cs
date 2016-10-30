using Repository.Pattern.Ef6;

namespace AutoMoto.Models
{
    public class Photo : Entity
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public Extensions Extension { get; set; }
    }
}