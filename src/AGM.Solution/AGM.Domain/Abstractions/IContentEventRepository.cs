using AGM.Domain.Entities;

namespace AGM.Domain.Abstractions
{
    public interface IContentEventRepository
    {
        Task<ContentEventId> Create(ContentEvent ContentEvent, CancellationToken cancellationToken = default);
        Task<ContentEvent> GetById(ContentEventId Id, CancellationToken cancellationToken = default);
        Task<ContentEvent> Update(ContentEvent ContentEvent, CancellationToken cancellationToken = default);
        Task Delete(ContentEventId Id, CancellationToken cancellationToken = default);

        Task<IEnumerable<ContentEvent>> GetAll(CancellationToken cancellationToken = default);
        Task<IEnumerable<ContentEvent>> GetAll(DateTime From, CancellationToken cancellationToken = default);
    }
}
