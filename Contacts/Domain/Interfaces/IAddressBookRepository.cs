using System;
using System.Threading.Tasks;

namespace Contacts.Domain.Interfaces
{
    public interface IAddressBookRepository<T> : IDisposable where T : IAggregate
    {
        Task<T> FindByUserId(int Id);
        Task<T> UpsertBook(T doc);
    }
}
