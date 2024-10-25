using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface ITenantRepository
    {
        Task<TenantId> Create(Tenant ContentEvent, CancellationToken cancellationToken = default);
        Task<Tenant> GetById(TenantId Id, CancellationToken cancellationToken = default);
        Task<Tenant> Update(Tenant ContentEvent, CancellationToken cancellationToken = default);
        Task<Tenant> Delete(TenantId Id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Tenant>> GetAll(CancellationToken cancellationToken = default);
    }
}
