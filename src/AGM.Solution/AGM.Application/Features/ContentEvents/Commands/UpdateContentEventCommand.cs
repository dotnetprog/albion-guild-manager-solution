using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.ContentEvents.Commands
{
    public class UpdateContentEventCommand : IRequest
    {
        public ContentEventId ContentEventId { get; set; }
        public DateTime? StartsOn { get; set; }
        public ContentEventTypeId? TypeId { get; set; }
        public ContentEventSubTypeId? SubTypeId { get; set; }
        public AlbionMapId? AlbionMapId { get; set; }
    }
}
