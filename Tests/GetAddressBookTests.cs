using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contacts.Application.AddressBook.Queries;
using Contacts.Domain.Entities;
using Contacts.Domain.Interfaces;
using MongoDB.Driver;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Tests
{
    public class GetAddressBookTests
    {
        private GetUniqueContactsHandler handler;
        private readonly Mock<IAddressBookRepository<AddressBook>> _repo;
        private readonly AddressBook a1;
        private readonly AddressBook a2;
        public GetAddressBookTests()
        {

            var mocker = new AutoMocker();
            handler = mocker.CreateInstance<GetUniqueContactsHandler>();
            _repo = mocker.GetMock<IAddressBookRepository<AddressBook>>();
            a1 = new AddressBook
            {
                UserId = 1,
                Contacts = new List<Contact>
                {
                    new Contact("Bob", "Smith", "bob@email.com", "0412344555"),
                    new Contact("Mary", "Smith", "mary@email.com", "0412344666"),
                    new Contact("Jane", "Smith", "jane@email.com", "0412344999")
                }
            };
            a2 = new AddressBook
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

            [Fact]
            public async Task GetUniqueContacts_ShouldPass()
            {
                var filter = Builders<AddressBook>.Filter.Eq(c => c.UserId, 1);
                _repo.Setup(s => s.FindByUserId(1)).ReturnsAsync(a1);
                _repo.Setup(s => s.FindByUserId(2)).ReturnsAsync(a2);


            var book = await handler.Handle(new GetUniqueContacts(1, 2), new CancellationToken());

            Assert.Equal(2, book.Contacts.Count);
            _repo.Verify(v => v.FindByUserId(It.IsAny<int>()), Times.Exactly(2));


            }
    }
}
