﻿using AutoMoto.Model.Models;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AutoMoto.Models
{
    public class Advertisement : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime AddedDate { get; set; }
        public string Description { get; set; }
        public AspNetUser User { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public bool IsActive { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }

    }
}