using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Queries
{
    public class RetrieveAllContentEventsQuery : IRequest<IReadOnlyCollection<ContentEvent>>
    {
        public DateTime? From { get; set; }
    }
}
