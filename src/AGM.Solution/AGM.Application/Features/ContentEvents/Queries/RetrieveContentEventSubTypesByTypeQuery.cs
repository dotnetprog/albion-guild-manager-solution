using AGM.Application.Features.Caching.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Queries
{
    public class RetrieveContentEventSubTypesByTypeQuery : IRequest<IReadOnlyCollection<ContentEventSubType>>, ICacheableQuery
    {
        public ContentEventTypeId TypeId { get; set; }
        public string CacheKey => $"{nameof(RetrieveContentEventSubTypesByTypeQuery)}.{TypeId.Value}";

        public bool BypassCache { get; set; }
    }
}
