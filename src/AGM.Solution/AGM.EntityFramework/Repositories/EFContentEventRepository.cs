using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AGM.EntityFramework.Repositories
{
    public class EFContentEventRepository : IContentEventRepository
    {
        private readonly AGMDBContext _AgmContext;
        private readonly ITenantProvider _tenantProvider;

        public EFContentEventRepository(AGMDBContext agmContext, ITenantProvider tenantProvider)
        {
            _AgmContext = agmContext;
            _tenantProvider = tenantProvider;
        }

        public async Task<ContentEventId> Create(ContentEvent ContentEvent, CancellationToken cancellationToken = default)
        {
            ContentEvent.Id = ContentEventId.New();
            await _AgmContext.ContentEvents.AddAsync(ContentEvent, cancellationToken);
            return ContentEvent.Id;
        }

        public Task Delete(ContentEventId Id, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            _AgmContext.ContentEvents.Remove(new ContentEvent { Id = Id }),
            cancellationToken);
        }

        public async Task<IEnumerable<ContentEvent>> GetAll(CancellationToken cancellationToken = default)
        {
            var tenant = _tenantProvider.GetCurrentTenant();

            return await _AgmContext.ContentEvents
                 .Where(c => c.TenantId == tenant.Id).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ContentEvent>> GetAll(DateTime From, CancellationToken cancellationToken = default)
        {
            var tenant = _tenantProvider.GetCurrentTenant();
            return await _AgmContext
                .ContentEvents
                .Include(c => c.AlbionMap)
                .Include(c => c.SubType)
                .Where(c => c.TenantId == tenant.Id && c.StartsOn >= From).OrderBy(c => c.StartsOn).ToListAsync(cancellationToken);
        }

        public async Task<ContentEvent> GetById(ContentEventId Id, CancellationToken cancellationToken = default)
        {
            return await _AgmContext.ContentEvents.FindAsync(new object[] { Id }, cancellationToken: cancellationToken);
        }

        public async Task<ContentEvent> Update(ContentEvent ContentEvent, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => _AgmContext.ContentEvents.Update(ContentEvent), cancellationToken);
            return ContentEvent;
        }
    }
}
