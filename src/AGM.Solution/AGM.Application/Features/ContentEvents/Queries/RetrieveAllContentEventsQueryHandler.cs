using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Queries
{
    public class RetrieveAllContentEventsQueryHandler : IRequestHandler<RetrieveAllContentEventsQuery, IReadOnlyCollection<ContentEvent>>
    {
        private readonly IContentEventRepository _contentEventRepository;

        public RetrieveAllContentEventsQueryHandler(IContentEventRepository contentEventRepository)
        {
            _contentEventRepository = contentEventRepository;
        }

        public async Task<IReadOnlyCollection<ContentEvent>> Handle(RetrieveAllContentEventsQuery request, CancellationToken cancellationToken)
        {
            var results = request.From.HasValue ?
                await _contentEventRepository.GetAll(request.From.Value, cancellationToken) :
                await _contentEventRepository.GetAll();


            return results.ToArray();

        }
    }
}
