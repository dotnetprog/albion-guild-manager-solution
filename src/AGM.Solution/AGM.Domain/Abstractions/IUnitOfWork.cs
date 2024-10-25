namespace AGM.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task SaveChanchesAsync(CancellationToken cancellationToken = default);
    }
}
