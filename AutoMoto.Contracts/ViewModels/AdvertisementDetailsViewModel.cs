using AutoMoto.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace AutoMoto.Contracts.ViewModels
{
    public class AdvertisementDetailsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Tytuł ogłoszenia")]
        public string Title { get; set; }

        [DisplayName("Opis Pojazdu")]
        public string Description { get; set; }


        [DisplayName("Pojemność skokowa")]
        public int EngineCap { get; set; }
        [DisplayName("Cena")]
        public decimal Price { get; set; }
        [DisplayName("Rodzaj paliwa")]
        public FuelTypes FuelType { get; set; }
        [DisplayName("Przebieg")]
        public int Mileage { get; set; }
        [DisplayName("Marka")]
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        [DisplayName("Rok Produkcji")]
        public int Year { get; set; }
        public ICollection<HttpPostedFileBase> PhotoFiles { get; set; }
        public bool IsFollowing { get; set; }
        public string UserId { get; set; }
    }
}
