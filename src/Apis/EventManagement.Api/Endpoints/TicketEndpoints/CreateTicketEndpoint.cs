
namespace EventManagement.Api.Endpoints.TicketEndpoints;

internal sealed record CreateTicketRequest(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
);
public sealed class CreateTicketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tickets", async ([FromBody] CreateTicketRequest request, ISender sender) =>
        {
            var command = new CreateTicketCommand(
                request.EventId,
                request.Name,
                request.Price,
                request.Currency,
                request.Quantity);

            var response = await sender.Send(command);

            return response.Match(Results.Created, $"/tickets");

        })
        .WithName("CreateTicket")
        .WithTags(SwaggerTags.Tickets)
        .WithOpenApi();
    }
}
