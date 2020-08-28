using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace Contacts.Infrastructure.SeedData
{
    public static class CreateAndSeedExtension
    {
        public static void CreateAndSeedDbOnFirstRun(this IServiceProvider services)
        {
            //Log.Information("CreateAndSeedDbOnFirstRun");

            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            var databaseCreationService = services.GetService(typeof(IDatabaseCreationService)) as IDatabaseCreationService;

            if (databaseCreationService == null) return;
                databaseCreationService.RestoreAddressBookData();
        }
    }

    public interface IDatabaseCreationService
    {
        void RestoreAddressBookData();
    }

    public class DatabaseCreationService : IDatabaseCreationService
    {
        private readonly IMongoDbContext _dbContext;
        private readonly IEnumerable<string> _collections;

        public DatabaseCreationService(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
            _collections = _dbContext.ListCollections();
        }

        public void RestoreAddressBookData()
        {
            if (_collections.Contains("addressbook")) return;

            //Log.Information("Creating a new addressbook collection");

            _dbContext.InsertMany(AddressBookData.Get(), "addressbook");
        }

    }
}