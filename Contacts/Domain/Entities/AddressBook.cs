using System.Collections.Generic;
using Contacts.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contacts.Domain.Entities
{
    public partial class AddressBook : IAggregate
    {
        public AddressBook()
        {
            Contacts = new List<Contact>();
        }

        [BsonId]
        public ObjectId ObjectId { get; set; }
        public int UserId { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
