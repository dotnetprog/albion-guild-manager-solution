using AGM.Application.Features.Caching.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Queries
{
    public class RetrieveAllContentEventTypesQuery : IRequest<IReadOnlyCollection<ContentEventType>>, ICacheableQuery
    {
        public string CacheKey => nameof(RetrieveAllContentEventTypesQuery);

        public bool BypassCache { get; set; }
    }
}
