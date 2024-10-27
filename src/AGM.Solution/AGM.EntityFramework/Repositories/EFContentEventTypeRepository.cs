using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AGM.EntityFramework.Repositories
{
    public class EFContentEventTypeRepository : IContentEventTypeRepository
    {
        private readonly AGMDBContext _AgmContext;

        public EFContentEventTypeRepository(AGMDBContext agmContext)
        {
            _AgmContext = agmContext;
        }

        public async Task<IEnumerable<ContentEventSubType>> GetAllSubTypes(ContentEventTypeId? TypeId, CancellationToken cancellationToken = default)
        {
            var query = _AgmContext.ContentEventSubTypes.AsQueryable();
            if (TypeId.HasValue)
            {
                query = query.Where(st => st.ContentEventTypeId == TypeId);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ContentEventType>> GetAllTypes(CancellationToken cancellationToken = default)
        {
            return await _AgmContext.ContentEventTypes.ToListAsync(cancellationToken);
        }

        public async Task<ContentEventSubType> GetSubType(ContentEventSubTypeId SubTypeId, CancellationToken cancellationToken = default)
        {
            return await _AgmContext.ContentEventSubTypes.FindAsync(new object[] { SubTypeId }, cancellationToken);
        }

        public async Task<ContentEventType> GetType(ContentEventTypeId TypeId, CancellationToken cancellationToken = default)
        {
            return await _AgmContext.ContentEventTypes.FindAsync(new object[] { TypeId }, cancellationToken);
        }
    }
}
