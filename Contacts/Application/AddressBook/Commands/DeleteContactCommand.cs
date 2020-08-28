using System;
using System.Threading;
using System.Threading.Tasks;
using Contacts.Infrastructure.SeedData;
using MediatR;

namespace Contacts.Application.AddressBook.Commands
{
    public class DeleteContactCommand : INotification
    {
        public Guid UserId { get; }
        public Guid ContactId { get; }

        public DeleteContactCommand(Guid userId, Guid contactId)
        {
            UserId = userId;
            ContactId = contactId;
        }
    }

    public class DeleteContactCommandHandler : INotificationHandler<DeleteContactCommand>
    {
        private readonly IMongoDbContext _context;

        public DeleteContactCommandHandler(IMongoDbContext context)
        {
            _context = context;
        }

        public Task Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}