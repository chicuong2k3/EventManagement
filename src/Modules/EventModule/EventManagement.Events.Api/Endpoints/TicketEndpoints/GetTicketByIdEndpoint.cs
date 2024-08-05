using EventManagement.Events.Application.UseCases.Tickets;

namespace EventManagement.Events.Api.Endpoints.TicketEndpoints;
public sealed class GetTicketByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tickets/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetTicketByIdQuery(id));

            return response.Match(Results.Ok);
        })
        .WithName("GetTicketById")
        .WithTags(SwaggerTags.Tickets)
        .WithOpenApi();
    }
}