using System.Collections.Generic;
using Contacts.Domain.Entities;

namespace Contacts.Infrastructure.SeedData
{
    public class AddressBookData
    {
        public static IEnumerable<AddressBook> Get()
        {
            yield return new AddressBook
            {
                UserId = 1,
                Contacts = new List<Contact>
                {
                    new Contact( "Bob", "Smith", "bob@email.com", "0412344555" ),
                    new Contact( "Mary", "Smith", "mary@email.com", "0412344666" ),
                    new Contact( "Jane", "Smith", "jane@email.com", "0412344999" )
                }
            };
            yield return new AddressBook
            {
                UserId = 2,
                Contacts = new List<Contact>
                {
                    new Contact( "John", "Smith", "john@email.com", "0412344000" ),
                    new Contact( "Mary", "Smith", "mary@email.com", "0412344666" ),
                    new Contact( "Jane", "Smith", "jane@email.com", "0412344999" )
                }
            };
        }
    }
}
