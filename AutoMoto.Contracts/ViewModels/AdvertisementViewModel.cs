using AutoMoto.Enums;
using AutoMoto.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace AutoMoto.Contracts.ViewModels
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }
        [DisplayName("Tytuł ogłoszenia")]
        public string Title { get; set; }

        [DisplayName("Opis Pojazdu")]
        public string Description { get; set; }

        public List<Address> Addresses { get; set; }
        public int Address { get; set; }
        [DisplayName("Pojemność skokowa")]
        public int EngineCap { get; set; }
        [DisplayName("Cena")]
        public decimal Price { get; set; }
        [DisplayName("Rodzaj paliwa")]
        public FuelTypes FuelType { get; set; }
        [DisplayName("Przebieg")]
        public int Mileage { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        [DisplayName("Marka")]
        public int Manufacturer { get; set; }
        public List<Models.Model> Models { get; set; }
        public int Model { get; set; }
        [DisplayName("Rok Produkcji")]
        public int Year { get; set; }
        public ICollection<HttpPostedFileBase> PhotoFiles { get; set; }
    }
}
