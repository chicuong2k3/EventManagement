using EventManagement.Common.Domain;
using MediatR;

namespace EventManagement.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}