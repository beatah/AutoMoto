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
        public IEnumerable<AdvertisementDetailsResult> GetAllAdvertisementsWithDetails()
        {
            return Database.SqlQuerySmart<AdvertisementDetailsResult>("[sp_GetAllAdvertDetails]");
        }

        public IEnumerable<AdvertisementDetailsResult> GetAllAdvertisementsWithDetailsById(int id)
        {
            return Database.SqlQuerySmart<AdvertisementDetailsResult>("[sp_GetAllAdvertDetailsById]", new { id });
        }

        public IEnumerable<AdvertisementDetailsResult> GetAllAdvertisementsWithDetailsByUser(string userId)
        {
            return Database.SqlQuerySmart<AdvertisementDetailsResult>("[sp_GetAllAdvertDetailsByUser]", new { userId });
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
        public int CountFollowing(string followerId, string followeeId)
        {
            return Database.SqlQuerySmart<int>("[sp_FollowersCountByFollowee]", new { followerId, followeeId }).First();
        }
        public IEnumerable<Following> GetFollowing(string followerId, string followeeId)
        {
            return Database.SqlQuerySmart<Following>("[sp_GetFollowing]", new { followerId, followeeId });
        }
        public IEnumerable<NewFollowingNotificationResult> GetNewNotificationsFor(string userId)
        {
            return Database.SqlQuerySmart<NewFollowingNotificationResult>("[sp_GetUnreadForUser]", new { userId });
        }
        public void InsertFollowing(string followerId, string followeeId)
        {
            Database.ExecuteSqlCommandSmart("[sp_InsertFollowing]", new { followerId, followeeId });
        }
        public void DeleteFollowing(string followerId, string followeeId)
        {
            Database.ExecuteSqlCommandSmart("[sp_DeleteFollowing]", new { followerId, followeeId });
        }
        public void MarkAsRead(string userId)
        {
            Database.ExecuteSqlCommandSmart("[sp_MarkAsRead]", new { userId });
        }
    }

    public interface IAutoStoredDbContext
    {
        IEnumerable<NewFollowingNotificationResult> GetNewNotificationsFor(string userId);
        void MarkAsRead(string userId);
        void InsertFollowing(string followerId, string followeeId);
        void DeleteFollowing(string followerId, string followeeId);
        IEnumerable<Following> GetFollowing(string followerId, string followeeId);
        int CountFollowing(string followerId, string followeeId);
        IEnumerable<AdvertisementDetailsResult> GetAllAdvertisementsWithDetails();
        IEnumerable<AdvertisementDetailsResult> GetAllAdvertisementsWithDetailsById(int id);
        IEnumerable<AdvertisementDetailsResult> GetAllAdvertisementsWithDetailsByUser(string userId);
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
