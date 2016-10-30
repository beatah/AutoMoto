using AutoMoto.Models;
using System.Threading.Tasks;

namespace AutoMoto.Contracts.Interfaces
{
    public interface IManufacturerService
    {
        void Insert(Manufacturer entity);
        void Update(Manufacturer entity);
        Task<bool> DeleteAsync(params object[] keyValues);
    }
}