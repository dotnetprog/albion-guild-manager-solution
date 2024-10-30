using AGM.Application.Features.Maps.Queries;
using AGM.DiscordBot.Factory;
using AGM.DiscordBot.Interactions.Module.Roads.Input;
using Discord;
using Discord.Interactions;
using MediatR;

namespace AGM.DiscordBot.Interactions.Module.Roads
{
    public class RoadPosterModule : ScopedInteractionModule
    {
        public RoadPosterModule(IScopedDiscordFactory scopeFactory) : base(scopeFactory, "Road")
        {
        }



        [EnabledInDm(false)]
        [SlashCommand("road", "This command add a new road", false, RunMode.Async)]
        public async Task AddRoad([ComplexParameter] RoadPost road)
        {
            await ShowInProgress();
            if (!ValidateWhen(road.ExpiresIn))
            {
                await ShowValidation($"{nameof(RoadPost.ExpiresIn)} must respect a datetime format. please use the autocompletion.");
                return;
            }
            var mapsResult = road.GetMaps();
            if (mapsResult.Errors.Any())
            {
                await ShowValidation(string.Join(Environment.NewLine, mapsResult.Errors.ToArray()));
                return;
            }
            var expiresAt = this.GetTimeFromWhen(road.ExpiresIn);
            var scope = await this.CreateScope();
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();
            var dbMaps = await sender.Send(new RetrieveAlbionAllMapsQuery());
            var roadBuilder = new RoadBuilder();
            DateTimeOffset timestamp = DateTime.SpecifyKind(expiresAt, DateTimeKind.Utc);
            var timeTag = TimestampTag.FormatFromDateTimeOffset(timestamp, TimestampTagStyles.Relative);
            foreach (var mapid in mapsResult.Value)
            {
                var map = dbMaps.FirstOrDefault(x => x.Id == mapid);
                if (map == null)
                {
                    await ShowValidation($"Mapid: {mapid} was not found");
                    return;
                }

                roadBuilder.AddMap(map);
            }
            var SocketGuildUser = (IGuildUser)Context.User;

            var button = new ButtonBuilder().WithLabel("Delete").WithCustomId("delete_road").WithStyle(ButtonStyle.Danger);
            var components = new ComponentBuilder().WithButton(button).Build();
            var embed = new EmbedBuilder()
                .WithTitle("Road")
                .AddField("Expires In", timeTag)
                .AddField("AuthorId", Context.User.Id)
                .WithDescription(roadBuilder.BuildString())
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithAuthor(
                new EmbedAuthorBuilder()
                .WithName(SocketGuildUser.DisplayName)
                .WithIconUrl(SocketGuildUser.GetDisplayAvatarUrl()))
                .Build();
            await ReplyAsync(text: "### Road created!", embed: embed, components: components);


        }
        [ComponentInteraction("delete_road")]
        public async Task DeleteRoad()
        {
            var guildUser = Context.User as IGuildUser;
            var channel = (ITextChannel)Context.Channel;
            var channelPermission = guildUser.GetPermissions(channel);
            var interaction = (IComponentInteraction)Context.Interaction;
            var messageEmbed = interaction.Message.Embeds.FirstOrDefault();
            var originalAuthorId = messageEmbed.Fields.FirstOrDefault(f => f.Name == "AuthorId").Value;
            var originalAuthorName = messageEmbed.Author.Value.Name;
            if (!channelPermission.ManageMessages
                && !guildUser.GuildPermissions.ManageMessages
                && originalAuthorId != guildUser.Id.ToString())
            {
                var embed = new EmbedBuilder()
                    .WithTitle($"{CommandTitle} Validation")
                    .WithDescription($"Only users that can manage messages or the Author ({originalAuthorName}) can delete a road.")
                    .WithColor(Color.Red)
                    .Build();
                await RespondAsync(embed: embed, ephemeral: true);
                return;
            }
            await interaction.Message.DeleteAsync();

        }

    }
}
