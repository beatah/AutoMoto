using AutoMoto.Model.Database_Objects.Results;
using AutoMoto.Model.Models;
using AutoMoto.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoMoto.Model
{
    public partial class AutoDbContext : IAutoStoredDbContext
    {
        public Advertisement GetAdvertisementById(int id)
        {
            return Database.SqlQuerySmart<Advertisement>("[sp_GetAdvertisementById]", new { id }).FirstOrDefault();
        }
        public Manufacturer GetManufacturerById(int id)
        {
            return Database.SqlQuerySmart<Manufacturer>("[sp_GetManufacturerById]", new { id }).FirstOrDefault();
        }
        public void DeleteAdvertisement(int id)
        {
            Database.ExecuteSqlCommandSmart("[sp_DeleteAdvertisement]", new { id });
        }
        public IEnumerable<AutoMoto.Models.Model> GetModelsByManufacturer(int manufacturerId)
        {
            return Database.SqlQuerySmart<AutoMoto.Models.Model>("[sp_GetModelsByManufacturer]", new { manufacturerId });
        }
        public IEnumerable<ModelManufacturerResult> GetAllModelsWithManufacturers()
        {
            return Database.SqlQuerySmart<ModelManufacturerResult>("[sp_GetAllModels]");
        }
        public IEnumerable<Feature> GetAllFeatures()
        {
            return Database.SqlQuerySmart<Feature>("[sp_GetAllFeatures]");
        }
        public IEnumerable<Manufacturer> GetAllManufactures()
        {
            return Database.SqlQuerySmart<Manufacturer>("[sp_GetAllManufactures]");
        }

        public void NewMessage(string fromUserId, string toUserId, string content, int advertisementId)
        {
            Database.ExecuteSqlCommandSmart("[sp_NewMessage]", new { fromUserId, toUserId, content, advertisementId });
        }
        public void InsertFeature(string name)
        {
            Database.ExecuteSqlCommandSmart("[sp_InsertFeature]", new { name });
        }
        public void InsertManufacturer(string name)
        {
            Database.ExecuteSqlCommandSmart("[sp_InsertManufacturer]", new { name });
        }

        public void ChangeManufacturerStatus(int id, int status)
        {
            Database.ExecuteSqlCommandSmart("[sp_ChangeManufacturerStatus]", new { id, status });
        }
        public void InsertModel(string name, string manufacturerName)
        {
            Database.ExecuteSqlCommandSmart("[sp_InsertModel]", new { name, manufacturerName });
        }
    }

    public interface IAutoStoredDbContext
    {
        void InsertModel(string name, string manufacturerName);
        void InsertManufacturer(string name);
        IEnumerable<Feature> GetAllFeatures();
        IEnumerable<Manufacturer> GetAllManufactures();
        Advertisement GetAdvertisementById(int id);
        IEnumerable<AutoMoto.Models.Model> GetModelsByManufacturer(int manufacturerId);
        void NewMessage(string fromUserId, string toUserId, string content, int advertisementId);
        void DeleteAdvertisement(int id);
        void InsertFeature(string name);
        Manufacturer GetManufacturerById(int id);
        void ChangeManufacturerStatus(int id, int status);
        IEnumerable<ModelManufacturerResult> GetAllModelsWithManufacturers();
    }
}
