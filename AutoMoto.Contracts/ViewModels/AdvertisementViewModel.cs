using AutoMoto.Enums;
using AutoMoto.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace AutoMoto.Contracts.ViewModels
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }
        [DisplayName("Tytuł ogłoszenia")]
        [Required(ErrorMessage = "Pole tytuł jest wymagane")]
        public string Title { get; set; }

        [DisplayName("Opis Pojazdu")]
        [Required(ErrorMessage = "Pole opis pojazdu jest wymagane")]
        public string Description { get; set; }


        [DisplayName("Pojemność skokowa")]
        [Required(ErrorMessage = "Pole pojemność skokowa jest wymagane")]
        public int EngineCap { get; set; }
        [DisplayName("Cena")]
        [Range(1, int.MaxValue, ErrorMessage = "Cena musi być większa od {1}")]
        public decimal Price { get; set; }
        [DisplayName("Rodzaj paliwa")]
        [Required(ErrorMessage = "Pole rodzaj paliwa jest wymagane")]
        public FuelTypes FuelType { get; set; }
        [DisplayName("Przebieg")]
        [Required(ErrorMessage = "Pole Przebieg jest wymagane")]
        public int Mileage { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        [DisplayName("Marka")]
        public int Manufacturer { get; set; }
        public List<Models.Model> Models { get; set; }
        public int Model { get; set; }
        [DisplayName("Rok Produkcji")]
        [Required(ErrorMessage = "Pole rok produkcji jest wymagane")]
        public int Year { get; set; }
        public ICollection<HttpPostedFileBase> PhotoFiles { get; set; }
    }
}
