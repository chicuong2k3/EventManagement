using EventManagement.Common.Domain.Results;
using MediatR;

namespace EventManagement.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}