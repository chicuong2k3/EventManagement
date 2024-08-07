namespace EventManagement.Api.Endpoints.TicketTypeEndpoints;
public sealed class GetTicketTypeByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/ticket-types/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetTicketTypeByIdQuery(id));

            return response.Match(Results.Ok);
        })
        .WithName("GetTicketTypeById")
        .WithTags(SwaggerTags.TicketTypes)
        .WithOpenApi();
    }
}