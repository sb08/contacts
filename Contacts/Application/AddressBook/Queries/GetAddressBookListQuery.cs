using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contacts.Infrastructure.SeedData;
using MediatR;

namespace Contacts.Application.AddressBook.Queries
{
    public class GetAddressBookListQuery : IRequest<List<Domain.Entities.AddressBook>>
    {
        //  pagination parameters should be used here here
    }

    public class GetAddressBookListHandler : IRequestHandler<GetAddressBookListQuery, List<Domain.Entities.AddressBook>>
    {
        private readonly IMongoDbContext _context;

        public GetAddressBookListHandler(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.AddressBook>> Handle(GetAddressBookListQuery request, CancellationToken cancellationToken)
        {
            //use mongodb driver context directly:
            //var query = collection.AsQueryable().Take(10);
            return await _context.ListAllAsync<Domain.Entities.AddressBook>("addressbook");
        }
    }
}