using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Commands
{
    public class DeleteContentEventCommand : IRequest
    {
        public ContentEventId ContentEventId { get; set; }
    }
}
