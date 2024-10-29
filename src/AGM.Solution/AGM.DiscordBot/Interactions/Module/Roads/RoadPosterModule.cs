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
            var roadBuilder = new RoadBuilder().WithExpiresAt(expiresAt);
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
            var embed = new EmbedBuilder()
                .WithTitle("Road")
                .WithDescription(roadBuilder.BuildString())
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithAuthor(
                new EmbedAuthorBuilder()
                .WithName(SocketGuildUser.DisplayName)
                .WithIconUrl(SocketGuildUser.GetDisplayAvatarUrl()))
                .Build();
            await ReplyAsync(text: "### Road created!", embed: embed);


        }


    }
}
