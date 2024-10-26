using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.Configuration.Commands
{
    public class UpdateContentEventSettingsCommand : IRequest<Tenant>
    {
        public ulong? ChannelEventDiscordId { get; set; }
        public ulong? EventMessageDiscordId { get; set; }
    }
}
