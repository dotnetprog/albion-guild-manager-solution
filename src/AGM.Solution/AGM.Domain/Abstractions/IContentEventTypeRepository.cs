using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface IContentEventTypeRepository
    {
        public Task<IEnumerable<ContentEventType>> GetAllTypes();
        public Task<IEnumerable<ContentEventSubType>> GetAllSubTypes(ContentEventTypeId? TypeId);
        public Task<ContentEventSubType> GetSubType(ContentEventSubTypeId SubTypeId);
        public Task<ContentEventType> GetType(ContentEventTypeId TypeId);
    }
}
