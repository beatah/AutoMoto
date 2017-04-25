using AutoMoto.Model;
using AutoMoto.Model.Models;
using AutoMoto.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoMoto.Service
{
    public class SqlDbService
    {
        private readonly IAutoStoredDbContext _storedProcedures;

        public SqlDbService(IAutoStoredDbContext storedProcedures)
        {
            _storedProcedures = storedProcedures;
        }

        public Advertisement GetAdvertisementById(int id)
        {
            return _storedProcedures.GetAdvertisementById(id);
        }
        public Manufacturer GetManufacturerById(int id)
        {
            return _storedProcedures.GetManufacturerById(id);
        }
        public IEnumerable<Models.Model> GetModelsByManufacturer(int manufacturerId)
        {
            return _storedProcedures.GetModelsByManufacturer(manufacturerId);
        }
        public IEnumerable<Models.Model> GetAllModels()
        {
            return _storedProcedures.GetAllModelsWithManufacturers().Select(x => new Models.Model()
            {
                Id = x.Id,
                ManufacturerId = x.ManufacturerId,
                Name = x.Name,
                Manufacturer = new Manufacturer() { Id = x.ManufacturerId, Name = x.ManufacturerName }
            }).ToList();

        }
        public void NewMessage(string fromUserId, string toUserId, string content, int advertisementId)
        {
            _storedProcedures.NewMessage(fromUserId, toUserId, content, advertisementId);
        }

        public void DeleteAdvertisement(int id)
        {
            _storedProcedures.DeleteAdvertisement(id);
        }
        public void InsertFeature(string name)
        {
            _storedProcedures.InsertFeature(name);
        }
        public void InsertModel(string name, string manufacturerName)
        {
            _storedProcedures.InsertModel(name, manufacturerName);
        }
        public void InsertManufacturer(string name)
        {
            _storedProcedures.InsertManufacturer(name);
        }
        public IEnumerable<Feature> GetAllFeatures()
        {
            return _storedProcedures.GetAllFeatures();
        }

        public IEnumerable<Manufacturer> GetAllManufactures()
        {
            return _storedProcedures.GetAllManufactures();
        }

        public void ChangeManufacturerStatus(int id, int status)
        {
            _storedProcedures.ChangeManufacturerStatus(id, status);
        }
        //public List<DayUser> UserDaysBetweenTwoYears(string userId, int fromDay1, int toDay1, int toDay2, int year)
        //{
        //    return _storedProcedures.UserDaysBetweenTwoYears(userId, fromDay1, toDay1, toDay2, year)
        //        .Select(x => new DayUser
        //        {
        //            DayNumber = x.DayNumber,
        //            Description = x.Description,
        //            Project = new Project { Name = x.Name, Id = x.ProjectId },
        //            ProjectId = x.ProjectId,
        //            TimeSpent = x.TimeSpent,
        //            Year = x.Year,
        //            Id = x.Id,
        //            EntryTypeId = x.EntryTypeId,
        //            EntryType = new EntryType() { Id = x.EntryTypeId, Name = x.EntryTypeName }
        //        }).ToList();


        //}
    }
}
