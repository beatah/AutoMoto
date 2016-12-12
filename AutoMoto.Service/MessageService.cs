using AutoMoto.Contracts.Interfaces;
using AutoMoto.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class MessageService : Service<Message>, IMessageService
    {
        public MessageService(IRepositoryAsync<Message> repository) : base(repository)
        {
        }
    }
}
