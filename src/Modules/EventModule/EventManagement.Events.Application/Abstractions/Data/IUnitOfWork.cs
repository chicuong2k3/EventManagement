namespace EventManagement.Events.Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
