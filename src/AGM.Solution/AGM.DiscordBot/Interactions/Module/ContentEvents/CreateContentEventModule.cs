using AGM.Application.Features.ContentEvents.Commands;
using AGM.Application.Features.ContentEvents.Queries;
using AGM.DiscordBot.Common.Constants;
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
        public CreateContentEventModule(IScopedDiscordFactory scopeFactory) : base(scopeFactory, "Timer")
        {
        }



        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        [SlashCommand("timer-gather", "This command add a new timer for gathering", false, RunMode.Async)]
        public async Task AddGather([Summary("resource", "The resource that needs to be collected"), Autocomplete(typeof(GatherSubTypeAutoCompleteHandler))] string SubTypeIdRaw,
                              [Summary("map", "The map which the resource is located"), Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string MapIdRaw,
                              [Summary("when", "The exact time of the when it becomes available"), Autocomplete(typeof(UtcTimeAutoCompleteHandler))] string StartsOn)
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
            if (!ValidateWhen(StartsOn))
            {
                await ShowValidation("when must respect a datetime format. please use the autocompletion.");
                return;
            }
            using var scope = await CreateScope();
            var sender = scope.ServiceProvider.GetService<ISender>();
            var SubTypeId = new ContentEventSubTypeId(Guid.Parse(SubTypeIdRaw));
            var MapId = new AlbionMapId(Guid.Parse(MapIdRaw));
            var event_time_utc = GetTimeFromWhen(StartsOn);

            var AllTypes = await sender.Send(new RetrieveAllContentEventTypesQuery());
            var GatheringType = AllTypes.FirstOrDefault(t => t.Name == ContentEventTypeConsts.Gathering);
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
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        [SlashCommand("timer-other", "This command add a new timer for gathering", false, RunMode.Async)]
        public async Task AddOther([Summary("Content", "The content you want to add"), Autocomplete(typeof(ContentEventTypesButGatheringAutoCompleteHandler))] string TypeIdRaw,
                             [Summary("Type", "The type of the content"), Autocomplete(typeof(ContentEventSubTypesAutoCompleteHandler))] string SubTypeIdRaw,
                             [Summary("map", "The map which the resource is located"), Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string MapIdRaw,
                             [Summary("when", "The exact time of the when it becomes available"), Autocomplete(typeof(SmallUtcTImeAutoCompleteHandler))] string StartsOn)
        {

            await ShowInProgress();
            if (!ValidateIsGuid(TypeIdRaw))
            {
                await ShowValidation("Content cannot be typed, it must be selected from the auto complete.");
                return;
            }
            if (!ValidateIsGuid(SubTypeIdRaw))
            {
                await ShowValidation("Type cannot be typed, it must be selected from the auto complete.");
                return;
            }
            if (!ValidateIsGuid(MapIdRaw))
            {
                await ShowValidation("map cannot be typed, it must be selected from the auto complete.");
                return;
            }
            if (!ValidateWhen(StartsOn))
            {
                await ShowValidation("when must respect a datetime format. please use the autocompletion.");
                return;
            }
            using var scope = await CreateScope();
            var sender = scope.ServiceProvider.GetService<ISender>();
            var TypeId = new ContentEventTypeId(Guid.Parse(TypeIdRaw));
            var SubTypeId = new ContentEventSubTypeId(Guid.Parse(SubTypeIdRaw));
            var MapId = new AlbionMapId(Guid.Parse(MapIdRaw));
            var event_time_utc = GetTimeFromWhen(StartsOn);

            var request = new AddContentEventCommand
            {
                AlbionMapId = MapId,
                StartsOn = event_time_utc,
                SubTypeId = SubTypeId,
                TypeId = TypeId
            };
            var Content = await sender.Send(request);
            await ShowSuccess($"Content Timer Added, Id={Content.Id}");
        }

    }

}
