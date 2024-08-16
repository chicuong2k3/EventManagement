using EventManagement.Ticketing.Application.UseCases.Customers;

namespace EventManagement.Api.Endpoints.TicketEndpoints;
public sealed class GetTicketByCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tickets/code/{code}", async (string code, ISender sender) =>
        {
            var response = await sender.Send(new GetTicketByCodeQuery(code));

            return response.Match(Results.Ok);
        })
        .WithName("GetTicketByCode")
        .WithTags(SwaggerTags.Tickets)
        .WithOpenApi();
    }
}