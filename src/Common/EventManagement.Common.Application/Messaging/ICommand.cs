using EventManagement.Common.Domain.Results;
using MediatR;

namespace EventManagement.Common.Application.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
