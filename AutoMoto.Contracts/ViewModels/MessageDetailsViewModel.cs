
using AutoMoto.Models;
using System.Collections.Generic;

namespace AutoMoto.Contracts.ViewModels
{
    public class MessageDetailsViewModel
    {
        public List<Message> Messages { get; set; }
        public string Url { get; set; }
    }
}
