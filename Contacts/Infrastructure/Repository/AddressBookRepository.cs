using System;
using System.Threading.Tasks;
using Contacts.Domain.Entities;
using Contacts.Domain.Interfaces;
using MongoDB.Driver;

namespace Contacts.Infrastructure.Repository
{
    public class AddressBookRepository : IAddressBookRepository<AddressBook>
    {
        private readonly IMongoCollection<AddressBook> _collection;

        public AddressBookRepository(IMongoCollection<AddressBook> collection)
        {
            _collection = collection;
        }

        public async Task<AddressBook> FindByUserId(int Id)
        {
            var filter = Builders<AddressBook>.Filter.Where(x => x.UserId ==Id);
            var result =  await _collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<AddressBook> UpsertBook(AddressBook book)
        {
            try
            {
                await _collection.ReplaceOneAsync(
                    doc => doc.UserId == book.UserId,
                    book,
                    new ReplaceOptions { IsUpsert = true });

                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region IDisposable Members

        private Boolean _disposed = false;        // To detect redundant calls
        // IDisposable
        protected void Dispose(Boolean disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(Boolean disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion   
    }
}
