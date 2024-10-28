using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.Maps.Queries
{
    public class RetrieveAlbionAllMapsQueryHandler : IRequestHandler<RetrieveAlbionAllMapsQuery, IReadOnlyCollection<AlbionMap>>
    {
        private readonly IAlbionMapRepository _mapRepository;

        public RetrieveAlbionAllMapsQueryHandler(IAlbionMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public async Task<IReadOnlyCollection<AlbionMap>> Handle(RetrieveAlbionAllMapsQuery request, CancellationToken cancellationToken)
        {
            var maps = await _mapRepository.GetAll();
            return maps.ToArray();
        }
    }
}
