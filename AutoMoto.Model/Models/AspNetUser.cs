using AutoMoto.Models;
using Newtonsoft.Json;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMoto.Model.Models
{
    public class AspNetUser : Entity
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        public ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

        public Address Address { get; set; }
        public int AddressId { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<UserNotification> Notifications { get; set; }
        public ICollection<Following> Followers { get; set; }
        public ICollection<Following> Followees { get; set; }
        [JsonIgnore]
        public ICollection<Advertisement> Advertisements { get; set; }

        public AspNetUser()
        {

            AspNetUserClaims = new List<AspNetUserClaim>();
            AspNetRoles = new List<AspNetRole>();
            AspNetUserLogins = new List<AspNetUserLogin>();
            Followers = new List<Following>();
            Followees = new List<Following>();
        }
    }


}