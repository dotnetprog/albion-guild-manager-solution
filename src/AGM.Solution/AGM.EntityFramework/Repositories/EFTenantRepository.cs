using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AGM.EntityFramework.Repositories
{
    public class EFTenantRepository : ITenantRepository
    {
        private readonly AGMDBContext _AgmContext;

        public EFTenantRepository(AGMDBContext agmContext)
        {
            _AgmContext = agmContext;
        }

        public async Task<TenantId> Create(Tenant Tenant, CancellationToken cancellationToken = default)
        {
            Tenant.Id = TenantId.New();
            await _AgmContext.Tenants.AddAsync(Tenant, cancellationToken);
            return Tenant.Id;
        }

        public async Task<IEnumerable<Tenant>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _AgmContext.Tenants.ToListAsync(cancellationToken);
        }

        public async Task<Tenant> GetByDiscordId(ulong Id, CancellationToken cancellationToken = default)
        {
            return await _AgmContext.Tenants.FirstOrDefaultAsync(t => t.DiscordServerId == Id, cancellationToken);
        }

        public async Task<Tenant> GetById(TenantId Id, CancellationToken cancellationToken = default)
        {
            return await _AgmContext.Tenants.FindAsync(new object[] { Id }, cancellationToken);
        }

        public async Task<Tenant> Update(Tenant Tenant, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => _AgmContext.Tenants.Update(Tenant), cancellationToken);
            return Tenant;
        }
    }
}
