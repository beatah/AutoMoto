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


        public IEnumerable<Advertisement> GetAllAdvertisementsWithDetails()
        {
            var all = _storedProcedures.GetAllAdvertisementsWithDetails();
            return AdvertisementResultToEntity(all.ToList());
        }
        public IEnumerable<Advertisement> GetAllAdvertisementsWithDetailsByUser(string userId)
        {
            var all = _storedProcedures.GetAllAdvertisementsWithDetailsByUser(userId);
            return AdvertisementResultToEntity(all.ToList());
        }

        public IEnumerable<Notification> GetNewNotificationsFor(string userId)
        {
            var all = _storedProcedures.GetNewNotificationsFor(userId);
            return
                all.Select(
                    x =>
                        new FollowingNotification()
                        {
                            AdvertisementId = x.AdvertisementId,
                            Advertisement = new Advertisement()
                            {
                                Title = x.Title,
                                Id = x.AdvertisementId,
                                User = new AspNetUser() { FirstName = x.FirstName, LastName = x.LastName }
                            },
                            Id = x.Id,
                            DateTime = x.DateTime
                        });
        }

        public Advertisement GetAllAdvertisementsWithDetailsById(int id)
        {
            var all = _storedProcedures.GetAllAdvertisementsWithDetailsById(id);
            return AdvertisementResultToEntity(all.ToList()).First();
        }

        public bool IsFollowingExists(string followerId, string followeeId)
        {
            return _storedProcedures.CountFollowing(followerId, followeeId) == 1;
        }

        public void InsertFollowing(string followerId, string followeeId)
        {
            _storedProcedures.InsertFollowing(followerId, followeeId);
        }
        public void DeleteFollowing(string followerId, string followeeId)
        {
            _storedProcedures.DeleteFollowing(followerId, followeeId);
        }
        public void MarkAsRead(string userId)
        {
            _storedProcedures.MarkAsRead(userId);
        }
        //public Following GetFollowing(string followerId, string followeeId)
        //{
        //    return _storedProcedures.GetFollowing(followerId, followeeId).First();
        //}


        private static List<Advertisement> AdvertisementResultToEntity(List<Model.Database_Objects.Results.AdvertisementDetailsResult> all)
        {
            var advertisements = all.GroupBy(p => p.Id)
                            .Select(g => g.First())
                            .Select(x => new Advertisement()
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Description = x.Description,
                                IsActive = x.IsActive,
                                AddedDate = x.AddedDate,
                                Car = new Car()
                                {
                                    EngineCap = x.EngineCap,
                                    FuelType = x.FuelType,
                                    Mileage = x.Mileage,
                                    ModelId = x.ModelId,
                                    Model = new Models.Model()
                                    {
                                        Id = x.ModelId,
                                        Name = x.ModelName,
                                        ManufacturerId = x.ManufacturerId,
                                        Manufacturer = new Manufacturer()
                                        {
                                            Id = x.ManufacturerId,
                                            Name = x.ManufacturerName
                                        }
                                    },
                                    Price = x.Price,
                                    Year = x.Year
                                },
                                UserId = x.UserId,
                                User =
                                    new AspNetUser()
                                    {
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        Id = x.UserId,
                                        AddressId = x.AddressId,
                                        Address = new Address() { Id = x.AddressId, City = x.City }
                                    },
                            }).ToList();
            foreach (var advertisement in advertisements)
            {
                advertisement.Photos =
                    all.Where(x => x.Id == advertisement.Id)
                        .Select(x => new Photo() { Content = x.Content, Extension = x.Extension })
                        .ToList();
            }

            return advertisements;
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
