using MongoDB.Bson;

namespace Contacts.Domain.Interfaces
{
    public interface IAggregate
    {
        ObjectId ObjectId { get; set; }
    }
}
