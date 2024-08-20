using EventManagement.Common.Domain;
using MediatR;

namespace EventManagement.Common.Application.Messaging;

// this interface is used for the implementation of ValidationBehaviour
public interface ICommandBase { }

public interface ICommand : IRequest<Result>, ICommandBase
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase
{
}
