
using Dapper;
using EventManagement.Ticketing.Domain.Customers;

namespace EventManagement.Ticketing.Application.Customers;

public sealed record GetCustomerResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName
);

public sealed record GetCustomerQuery(Guid Id) : IQuery<GetCustomerResponse>;

internal sealed class GetCustomerByIdQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCustomerQuery, GetCustomerResponse>
{
    public async Task<Result<GetCustomerResponse>> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(GetCustomerResponse.Id)},
                    email AS {nameof(GetCustomerResponse.Email)},
                    first_name AS {nameof(GetCustomerResponse.FirstName)}
                    last_name AS {nameof(GetCustomerResponse.LastName)}
                FROM ticketing.customers
                WHERE id = @Id
                """
            ;

            var customer = await connection.QuerySingleOrDefaultAsync<GetCustomerResponse>(Sql, query);
            if (customer == null)
            {
                return Result.Failure<GetCustomerResponse>(CustomerErrors.NotFound(query.Id));
            }

            return customer;
        }
    }
}