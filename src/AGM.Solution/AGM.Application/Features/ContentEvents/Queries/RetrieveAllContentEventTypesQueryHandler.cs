using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Queries
{
    public class RetrieveAllContentEventTypesQueryHandler : IRequestHandler<RetrieveAllContentEventTypesQuery, IReadOnlyCollection<ContentEventType>>
    {
        private readonly IContentEventTypeRepository _contentEventTypeRepository;

        public RetrieveAllContentEventTypesQueryHandler(IContentEventTypeRepository contentEventTypeRepository)
        {
            _contentEventTypeRepository = contentEventTypeRepository;
        }

        public async Task<IReadOnlyCollection<ContentEventType>> Handle(RetrieveAllContentEventTypesQuery request, CancellationToken cancellationToken)
        {
            return (await _contentEventTypeRepository.GetAllTypes(cancellationToken)).ToArray();
        }
    }
}
