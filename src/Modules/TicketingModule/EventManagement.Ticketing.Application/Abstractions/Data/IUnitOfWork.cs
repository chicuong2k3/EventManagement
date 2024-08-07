namespace EventManagement.Ticketing.Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
