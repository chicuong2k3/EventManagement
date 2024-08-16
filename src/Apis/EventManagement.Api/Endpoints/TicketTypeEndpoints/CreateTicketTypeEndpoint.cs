namespace EventManagement.Api.Endpoints.TicketTypeEndpoints;

internal sealed record CreateTicketTypeRequest(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
);
public sealed class CreateTicketTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ticket-types", async ([FromBody] CreateTicketTypeRequest request, ISender sender) =>
        {
            var command = new CreateTicketTypeCommand(
                request.EventId,
                request.Name,
                request.Price,
                request.Currency,
                request.Quantity);

            var response = await sender.Send(command);

            return response.Match(Results.Ok);

        })
        .WithName("CreateTicketType")
        .WithTags(SwaggerTags.TicketTypes)
        .WithOpenApi();
    }
}
