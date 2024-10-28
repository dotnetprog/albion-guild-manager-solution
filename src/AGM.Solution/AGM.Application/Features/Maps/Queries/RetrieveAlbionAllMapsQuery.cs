using AGM.Application.Features.Caching.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.Maps.Queries
{
    public class RetrieveAlbionAllMapsQuery : IRequest<IReadOnlyCollection<AlbionMap>>, ICacheableQuery
    {
        public string CacheKey => $"{nameof(RetrieveAlbionAllMapsQuery)})";

        public bool BypassCache { get; set; }
    }
}
