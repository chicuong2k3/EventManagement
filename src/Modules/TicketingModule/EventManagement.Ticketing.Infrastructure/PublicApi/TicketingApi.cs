using EventManagement.Ticketing.Application.UseCases.Customers;
using EventManagement.Ticketing.PublicApi;
using MediatR;

namespace EventManagement.Ticketing.Infrastructure.PublicApi
{
    public class TicketingApi(ISender sender) : ITicketingApi
    {
        public async Task CreateCustomerAsync(Guid customerId, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            var command = new CreateCustomerCommand(customerId, email, firstName, lastName);
            await sender.Send(command, cancellationToken);
        }
    }
}
