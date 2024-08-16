using System.Data.Common;

namespace EventManagement.Ticketing.Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
