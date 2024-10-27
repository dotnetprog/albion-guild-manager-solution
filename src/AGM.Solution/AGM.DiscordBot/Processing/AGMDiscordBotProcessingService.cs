using AGM.Application.Features.Configuration.Commands;
using AGM.Domain.Abstractions;
using Discord.WebSocket;
using MediatR;

namespace AGM.DiscordBot.Processing
{
    public class AGMDiscordBotProcessingService : IScopedDiscordProcessingService
    {

        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly ITenantProvider _tenantProvider;
        public AGMDiscordBotProcessingService(
            ILogger<AGMDiscordBotProcessingService> logger,
            ISender sender,
            ITenantProvider tenantProvider)
        {
            _logger = logger;
            _sender = sender;
            _tenantProvider = tenantProvider;
        }

        public async Task OnGuildJoin(SocketGuild socketGuild)
        {
            _logger.LogInformation($"Joined Discord Server {socketGuild.Name}");
            var tenant = await _sender.Send(new RegisterTenantCommand
            {
                DiscordServerId = socketGuild.Id,
                Name = socketGuild.Name
            });
            _tenantProvider.SetActiveTenant(tenant);
        }
    }
}
