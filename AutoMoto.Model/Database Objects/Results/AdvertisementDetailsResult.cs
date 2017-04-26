using AutoMoto.Enums;
using System;

namespace AutoMoto.Model.Database_Objects.Results
{
    public class AdvertisementDetailsResult
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int ManufacturerId { get; set; }
        public string Title { get; set; }
        public string ModelName { get; set; }
        public string City { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ManufacturerName { get; set; }
        public DateTime AddedDate { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public int EngineCap { get; set; }
        public decimal Price { get; set; }
        public FuelTypes FuelType { get; set; }
        public int Mileage { get; set; }
        public int ModelId { get; set; }

        public int Year { get; set; }

    }
}
