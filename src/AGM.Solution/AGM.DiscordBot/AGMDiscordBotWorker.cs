using AGM.DiscordBot.Configuration;
using AGM.DiscordBot.Factory;
using AGM.DiscordBot.Interactions;
using AGM.DiscordBot.Processing;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace AGM.DiscordBot
{
    public class AGMDiscordBotWorker : BackgroundService
    {
        private readonly ILogger<AGMDiscordBotWorker> _logger;
        private readonly IScopedDiscordFactory _scopedDiscordFactory;
        private readonly DiscordSocketClient _discordClient;
        private readonly DiscordSettings _discordSettings;
        private readonly InteractionRegisteringService _interactionRegisteringService;
        public AGMDiscordBotWorker(ILogger<AGMDiscordBotWorker> logger,
            DiscordSocketClient discordClient,
            IScopedDiscordFactory scopedDiscordFactory,
            IOptions<DiscordSettings> discordOptions,
            InteractionRegisteringService interactionRegisteringService)
        {
            _logger = logger;
            _discordClient = discordClient;
            _scopedDiscordFactory = scopedDiscordFactory;
            _discordSettings = discordOptions.Value;
            _interactionRegisteringService = interactionRegisteringService;
        }
        private async Task<IServiceScope> CreateScopeAsync(IGuild guild, IGuildUser User = null)
        {
            return await _scopedDiscordFactory.Create(guild, User);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _discordClient.Ready += async () =>
            {
                try
                {
                    _logger.LogInformation($"Bot is connected and ready!");
                    await _interactionRegisteringService.InitializeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

            };
            _discordClient.JoinedGuild += async (SocketGuild) =>
            {
                using (var scope = await CreateScopeAsync(SocketGuild))
                {
                    var service = scope.ServiceProvider.GetRequiredService<IScopedDiscordProcessingService>();
                    await service.OnGuildJoin(SocketGuild);
                }
            };
            _discordClient.Log += async (msg) =>
            {

                await Task.Run(() => _logger.LogDebug(msg.ToString()));
            };
            await _discordClient.LoginAsync(TokenType.Bot, _discordSettings.Token, true);
            await _discordClient.StartAsync();

            await Task.Delay(Timeout.Infinite).WaitAsync(stoppingToken);
        }
    }
}
