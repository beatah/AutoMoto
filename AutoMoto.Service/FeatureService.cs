using AutoMoto.Contracts.Interfaces;
using AutoMoto.Model.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace AutoMoto.Service
{
    public class FeatureService : Service<Feature>, IFeatureService
    {
        public FeatureService(IRepositoryAsync<Feature> repository) : base(repository)
        {
        }
    }
}
