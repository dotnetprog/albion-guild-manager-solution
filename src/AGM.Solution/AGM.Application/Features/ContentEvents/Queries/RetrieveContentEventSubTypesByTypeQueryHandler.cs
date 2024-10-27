using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Queries
{
    public class RetrieveContentEventSubTypesByTypeQueryHandler : IRequestHandler<RetrieveContentEventSubTypesByTypeQuery, IReadOnlyCollection<ContentEventSubType>>
    {
        private readonly IContentEventTypeRepository _contentEventTypeRepository;

        public RetrieveContentEventSubTypesByTypeQueryHandler(IContentEventTypeRepository contentEventTypeRepository)
        {
            _contentEventTypeRepository = contentEventTypeRepository;
        }

        public async Task<IReadOnlyCollection<ContentEventSubType>> Handle(RetrieveContentEventSubTypesByTypeQuery request, CancellationToken cancellationToken)
        {
            return (await _contentEventTypeRepository.GetAllSubTypes(request.TypeId, cancellationToken)).ToArray();
        }
    }
}
