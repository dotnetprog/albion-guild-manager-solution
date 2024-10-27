using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface IContentEventTypeRepository
    {
        public Task<IEnumerable<ContentEventType>> GetAllTypes(CancellationToken cancellationToken = default);
        public Task<IEnumerable<ContentEventSubType>> GetAllSubTypes(ContentEventTypeId? TypeId, CancellationToken cancellationToken = default);
        public Task<ContentEventSubType> GetSubType(ContentEventSubTypeId SubTypeId, CancellationToken cancellationToken = default);
        public Task<ContentEventType> GetType(ContentEventTypeId TypeId, CancellationToken cancellationToken = default);
    }
}
