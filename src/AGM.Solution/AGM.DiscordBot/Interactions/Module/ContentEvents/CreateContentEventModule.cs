using AGM.Application.Features.ContentEvents.Commands;
using AGM.Application.Features.ContentEvents.Queries;
using AGM.DiscordBot.Factory;
using AGM.DiscordBot.Interactions.AutoCompletions;
using AGM.Domain.Entities;
using Discord;
using Discord.Interactions;
using MediatR;

namespace AGM.DiscordBot.Interactions.Module.ContentEvents
{
    public class CreateContentEventModule : ScopedInteractionModule
    {
        public CreateContentEventModule(IScopedDiscordFactory scopeFactory) : base(scopeFactory)
        {
        }



        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        [SlashCommand("timer-gather", "This command add a new timer for gathering", false, RunMode.Async)]
        public async Task Run([Summary("resource", "The resource that needs to be collected"), Autocomplete(typeof(GatherSubTypeAutoCompleteHandler))] string SubTypeIdRaw,
                              [Summary("map", "The map which the resource is located"), Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string MapIdRaw,
                              [Summary("event_time_utc", "The exact time of the when it becomes available"), Autocomplete(typeof(UtcTImeAutoCompleteHandler))] string StartsOn)
        {
            await ShowInProgress();

            if (!ValidateIsGuid(SubTypeIdRaw))
            {
                await ShowValidation("resource cannot be typed, it must be selected from the auto complete.");
                return;
            }
            if (!ValidateIsGuid(MapIdRaw))
            {
                await ShowValidation("map cannot be typed, it must be selected from the auto complete.");
                return;
            }
            if (!ValidateIsDate(StartsOn))
            {
                await ShowValidation("event_time_utc must respect a datetime format. please use the autocompletion.");
                return;
            }
            using var scope = await CreateScope();
            var sender = scope.ServiceProvider.GetService<ISender>();
            var SubTypeId = new ContentEventSubTypeId(Guid.Parse(SubTypeIdRaw));
            var MapId = new AlbionMapId(Guid.Parse(MapIdRaw));
            var event_time_utc = DateTime.Parse(StartsOn);

            var AllTypes = await sender.Send(new RetrieveAllContentEventTypesQuery());
            var GatheringType = AllTypes.FirstOrDefault(t => t.Name == "Gathering");
            var GatheringSubTypes = await sender.Send(new RetrieveContentEventSubTypesByTypeQuery { TypeId = GatheringType.Id });

            if (!GatheringSubTypes.Any(t => t.Id == SubTypeId))
            {
                await ShowValidation("resource was not found under the gathering type. Please contact your admin");
                return;
            }

            var request = new AddContentEventCommand
            {
                AlbionMapId = MapId,
                StartsOn = event_time_utc,
                SubTypeId = SubTypeId,
                TypeId = GatheringType.Id
            };
            var Content = await sender.Send(request);
            await ShowSuccess($"Content Timer Added, Id={Content.Id}");
        }
        private async Task ShowSuccess(string Message)
        {
            await FollowupAsync(Message, ephemeral: true);
        }
        private async Task ShowInProgress()
        {
            var embed = new EmbedBuilder()
                .WithTitle("timer-gather notification")
                .WithDescription("In progress...")
                .WithColor(Color.Blue)
                .Build();

            await RespondAsync(embed: embed, ephemeral: true);

        }
        private bool ValidateIsDate(string input)
        {
            return DateTime.TryParse(input, out DateTime time);
        }
        private bool ValidateIsGuid(string input)
        {
            return Guid.TryParse(input, out Guid SubTypeId);
        }

        private async Task ShowValidation(string message)
        {

            var embed = new EmbedBuilder().WithTitle("Command Validation").WithDescription(message).WithColor(Color.Red).Build();
            await FollowupAsync(embed: embed, ephemeral: true);

        }



    }

}
