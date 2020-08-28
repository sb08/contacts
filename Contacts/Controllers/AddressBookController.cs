using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Application.AddressBook.Commands;
using Contacts.Application.AddressBook.Queries;
using Contacts.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ApiController
    {
        [HttpGet]
        [Produces(typeof(List<AddressBook>))]
        public async Task<ActionResult> Get()
        {
            var result = await Mediator.Send(new GetAddressBookListQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Produces(typeof(AddressBook))]
        public async Task<ActionResult> Get(int id)
        {
            var result = await Mediator.Send(new GetAddressBookQuery(id));
            return Ok(result);
        }

        [Produces(typeof(AddressBook))]
        [HttpGet("{id}/compare/{id2}")]
        public async Task<ActionResult> GetUniqueContacts(int id, int id2)
        {
            return Ok(await Mediator.Send(new GetUniqueContacts(id, id2)));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid userId, Guid contactId)
        {
            await Mediator.Publish(new DeleteContactCommand(userId,  contactId));
            return NoContent();
        }
    }
}
