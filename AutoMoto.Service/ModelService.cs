using AutoMoto.Contracts.Interfaces;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class ModelService : Service<Models.Model>, IModelService
    {
        public ModelService(IRepositoryAsync<Models.Model> repository) : base(repository)
        {
        }
    }
}
