using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface ITenantRepository
    {
        Task<TenantId> Create(Tenant Tenant, CancellationToken cancellationToken = default);
        Task<Tenant> GetById(TenantId Id, CancellationToken cancellationToken = default);
        Task<Tenant> GetByDiscordId(ulong Id, CancellationToken cancellationToken = default);
        Task<Tenant> Update(Tenant Tenant, CancellationToken cancellationToken = default);
        Task<IEnumerable<Tenant>> GetAll(CancellationToken cancellationToken = default);
    }
}
