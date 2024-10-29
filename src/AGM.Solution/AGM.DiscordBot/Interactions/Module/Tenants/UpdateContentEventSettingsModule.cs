using AGM.DiscordBot.Factory;
using AGM.DiscordBot.Processing;
using Discord;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.Module.Tenants
{
    public class UpdateContentEventSettingsModule : ScopedInteractionModule
    {

        public UpdateContentEventSettingsModule(IScopedDiscordFactory scopedDiscordFactory) : base(scopedDiscordFactory, "ContentEvent Settings")
        {

        }


        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [SlashCommand("timer-settings", "This command configures the timer settings", false, RunMode.Async)]
        public async Task Run([ChannelTypes(ChannelType.Text)] IChannel TimerChannel)
        {
            await RespondAsync(embed:
                new EmbedBuilder()
                .WithTitle("Timer Settings Notification")
                .WithDescription("In progress...")
                .WithColor(Color.Blue)
                .Build(), ephemeral: true);
            using var scope = await CreateScope();
            var processingService = scope.ServiceProvider.GetRequiredService<IScopedDiscordProcessingService>();
            await processingService.UpdateContentEventSettingsInteraction(TimerChannel);
            await FollowupAsync(embed:
                new EmbedBuilder()
                .WithTitle("Timer Settings Notification")
                .WithDescription("Update Succeded !")
                .WithColor(Color.Green)
                .Build(), ephemeral: true);
        }

    }
}
