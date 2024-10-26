using AGM.Domain.Entities;
using MediatR;

namespace AGM.Application.Features.Configuration.Commands
{
    public class RegisterTenantCommand : IRequest<Tenant>
    {
        public ulong DiscordServerId { get; set; }
        public string? Name { get; set; }
    }
}
