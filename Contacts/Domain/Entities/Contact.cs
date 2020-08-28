using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contacts.Domain.Entities
{
    public class Contact
    {
        public Contact(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        [BsonId]
        public ObjectId ObjectId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
