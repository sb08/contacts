using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using MediatR;
using MongoDB.Driver;

namespace Contacts.Application.AddressBook.Queries
{
    public class GetUniqueContacts : IRequest<Domain.Entities.AddressBook>
    {
        public int Id1 { get; }
        public int Id2 { get; }

        public GetUniqueContacts(int id1, int id2)
        {
            Id1 = id1;
            Id2 = id2;
        }
    }

    public class GetUniqueContactsHandler : IRequestHandler<GetUniqueContacts, Domain.Entities.AddressBook>
    {
        private readonly IAddressBookRepository<Domain.Entities.AddressBook> _repo;
        private readonly IMongoCollection<Domain.Entities.AddressBook> _collection;

        public GetUniqueContactsHandler(IAddressBookRepository<Domain.Entities.AddressBook> repo, IMongoCollection<Domain.Entities.AddressBook> collection)
        {
            _repo = repo;
            _collection = collection;
        }

        public async Task<Domain.Entities.AddressBook> Handle(GetUniqueContacts request, CancellationToken cancellationToken)
        {
            var addressBook = new Domain.Entities.AddressBook();  //this would become a dto or viewmodel

            var a1 = await _repo.FindByUserId(request.Id1);
            var a2 = await _repo.FindByUserId(request.Id2);

            //var filter = Builders<Domain.Entities.AddressBook>.Filter.Eq(c => c.UserId, request.Id1);
            //var cursor = await _collection.FindAsync(filter);
            //var a1 = cursor.FirstOrDefault();
            //var a2 = _collection.Find(f => f.UserId == request.Id2).FirstOrDefault();

            a2?.Contacts?.ForEach(f =>
            {
                if (a1.Contacts.Any(a => a.Email == f.Email))
                    addressBook.Contacts.Add(f);
            });

            return addressBook;
        }
    }
}