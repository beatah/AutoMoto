using AutoMoto.Model.Models;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Models
{
    public class Following : Entity
    {

        public AspNetUser Follower { get; set; }
        public AspNetUser Followee { get; set; }

        [Key]
        [Column(Order = 1)]
        public string FollowerId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string FolloweeId { get; set; }
    }
}