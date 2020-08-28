using System.Threading;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using MediatR;

namespace Contacts.Application.AddressBook.Queries
{
    public class GetAddressBookQuery : IRequest<Domain.Entities.AddressBook>
    {
        public int Id { get; }

        public GetAddressBookQuery(int id)
        {
            Id = id;
        }
    }

    public class GetAddressBookHandler : IRequestHandler<GetAddressBookQuery, Domain.Entities.AddressBook>
    {
        private readonly IAddressBookRepository<Domain.Entities.AddressBook> _repository;

        public GetAddressBookHandler(IAddressBookRepository<Domain.Entities.AddressBook> repository)
        {
            _repository = repository;
        }


        public async Task<Domain.Entities.AddressBook> Handle(GetAddressBookQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindByUserId(request.Id);
        }
    }
}
