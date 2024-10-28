
using AGM.Application.Features.Configuration.Commands;
using AGM.Application.Features.ContentEvents.Queries;
using AGM.DiscordBot.Common.Constants;
using AGM.DiscordBot.Factory;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Discord;
using Discord.WebSocket;
using MediatR;

namespace AGM.DiscordBot.Jobs;

public class RefreshTimerBoardJobService : CronJobService
{
    private readonly IScopedDiscordFactory _scopedFactory;
    private readonly ILogger _logger;
    private readonly DiscordSocketClient _discordSocketClient;
    public RefreshTimerBoardJobService(ILogger<RefreshTimerBoardJobService> logger, IScopedDiscordFactory scopedFactory, DiscordSocketClient discordSocketClient) : base(CronExpressionConsts.Every5Minute, TimeZoneInfo.Local, logger)
    {
        _scopedFactory = scopedFactory;
        _logger = logger;
        _discordSocketClient = discordSocketClient;
    }


    public override async Task DoWork(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{now} RefreshTimerBoardJobService is working.", DateTime.Now.ToString("T"));

        if (_discordSocketClient.ConnectionState != ConnectionState.Connected)
        {
            _logger.LogWarning("{now} Discord client not yet connected.", DateTime.Now.ToString("T"));
            return;
        }

        List<Tenant> tenants;
        using (var scope = await _scopedFactory.Create(null, null))
        {
            var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
            tenants = (await tenantRepository.GetAll()).ToList();

        }



        var currentTime = DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour);
        foreach (var tenant in tenants)
        {
            if (!tenant.ChannelEventDiscordId.HasValue)
            {
                _logger.LogWarning("{TenantName} does not have a channel set up for timers.", tenant.Name);
                continue;
            }
            var discordGuild = _discordSocketClient.GetGuild(tenant.DiscordServerId);
            if (discordGuild == null)
            {
                _logger.LogWarning("The bot is no more in Server: {TenantName} With Id: {ServerId}", tenant.Name, tenant.DiscordServerId);
                continue;
            }

            var channel = discordGuild.GetTextChannel(tenant.ChannelEventDiscordId.Value);
            if (channel == null)
            {
                _logger.LogWarning("The channel Id does not exist: {ChannelId} With Id: {ServerId}", tenant.ChannelEventDiscordId.Value, tenant.DiscordServerId);
                continue;
            }
            using (var scope = await _scopedFactory.Create(discordGuild, null))
            {
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                var timerEvents = await sender.Send(new RetrieveAllContentEventsQuery()
                {
                    From = currentTime
                });
                var embed = await BuildTimerBoard(timerEvents.Take(25));
                if (tenant.EventMessageDiscordId.HasValue)
                {

                    var existingMessage = await channel.GetMessageAsync(tenant.EventMessageDiscordId.Value);
                    if (existingMessage != null)
                    {
                        await channel.ModifyMessageAsync(existingMessage.Id, (m) => m.Embed = embed);
                        return;
                    }


                }
                //Create Message
                var message = await channel.SendMessageAsync(text: "### Albion Events", embed: embed);

                var timerSettings = new UpdateContentEventSettingsCommand
                {
                    ChannelEventDiscordId = channel.Id,
                    EventMessageDiscordId = message.Id
                };
                await sender.Send(timerSettings);
            }



        }
    }
    private async Task<Embed> BuildTimerBoard(IEnumerable<ContentEvent> contentEvents)
    {

        var Rows = contentEvents.Select(c =>
        {
            DateTimeOffset timestamp = DateTime.SpecifyKind(c.StartsOn.Value, DateTimeKind.Utc);
            var timeTag = TimestampTag.FormatFromDateTimeOffset(timestamp, TimestampTagStyles.Relative);
            return $"{Emote.Parse(c.SubType.Emoji)} {c.SubType.Name} | **{c.StartsOn.Value.ToString("H:mm")} UTC** | {timeTag} | Map: **{c.AlbionMap.Name}**";
        });

        var embed = new EmbedBuilder()
            .WithTitle("Upcoming Timers")
            .WithDescription(string.Join(Environment.NewLine, Rows))
            .WithCurrentTimestamp().Build();

        return embed;





    }


}
