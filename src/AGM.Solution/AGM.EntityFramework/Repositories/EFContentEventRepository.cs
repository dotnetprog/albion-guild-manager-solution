using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AGM.EntityFramework.Repositories
{
    public class EFContentEventRepository : IContentEventRepository
    {
        private readonly AGMDBContext _AgmContext;

        public EFContentEventRepository(AGMDBContext agmContext)
        {
            _AgmContext = agmContext;
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
            return await _AgmContext.ContentEvents.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ContentEvent>> GetAll(DateTime From, CancellationToken cancellationToken = default)
        {
            return await _AgmContext.ContentEvents.Where(c => c.StartsOn >= From).ToListAsync(cancellationToken);
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
