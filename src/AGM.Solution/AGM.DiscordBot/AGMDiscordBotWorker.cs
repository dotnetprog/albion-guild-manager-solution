using AGM.DiscordBot.Configuration;
using AGM.DiscordBot.Processing;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace AGM.DiscordBot
{
    public class AGMDiscordBotWorker : BackgroundService
    {
        private readonly ILogger<AGMDiscordBotWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly DiscordSocketClient _discordClient;
        private readonly DiscordSettings _discordSettings;

        public AGMDiscordBotWorker(ILogger<AGMDiscordBotWorker> logger,
            DiscordSocketClient discordClient,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<DiscordSettings> discordOptions)
        {
            _logger = logger;
            _discordClient = discordClient;
            _serviceScopeFactory = serviceScopeFactory;
            _discordSettings = discordOptions.Value;
        }
        private IServiceScope CreateScope(IGuild guild, IGuildUser User = null)
        {
            var Guild = new Tenant
            {
                DiscordServerId = guild.Id,
                Name = guild.Name
            };

            var scope = _serviceScopeFactory.CreateScope();
            var tenantProvider = scope.ServiceProvider.GetService<ITenantProvider>();
            tenantProvider.SetActiveTenant(Guild);
            var IdentityManager = scope.ServiceProvider.GetService<IDiscordIdentityManager>() as SocketDiscordIdentityManager;
            IdentityManager.SetUser(User);
            return scope;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _discordClient.Ready += async () =>
            {
                _logger.LogInformation($"Bot is connected and ready!");
            };
            _discordClient.JoinedGuild += async (SocketGuild) =>
            {
                using (var scope = CreateScope(SocketGuild))
                {
                    var service = scope.ServiceProvider.GetRequiredService<IScopedDiscordProcessingService>();
                    await service.OnGuildJoin(SocketGuild);
                }
            };

            await _discordClient.LoginAsync(TokenType.Bot, _discordSettings.Token, true);
            await _discordClient.StartAsync();

            await Task.Delay(Timeout.Infinite).WaitAsync(stoppingToken);
        }
    }
}
