using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Commands
{
    public class AddContentEventCommand : IRequest<ContentEvent>
    {
        public ContentEventTypeId TypeId { get; set; }
        public ContentEventSubTypeId SubTypeId { get; set; }
        public DateTime StartsOn { get; set; }
        public AlbionMapId? AlbionMapId { get; set; }
    }
}
