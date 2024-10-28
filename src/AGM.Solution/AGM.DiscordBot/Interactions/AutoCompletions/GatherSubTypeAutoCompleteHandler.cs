using AGM.Application.Features.ContentEvents.Queries;
using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class GatherSubTypeAutoCompleteHandler : ScopedAutoCompleteHandler
    {
        public GatherSubTypeAutoCompleteHandler(IScopedDiscordFactory scopeFactory) : base(scopeFactory)
        {
        }

        public override async Task<AutocompletionResult> GenerateScopedSuggestionsAsync(IServiceProvider ScopedService, IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter)
        {
            var userInput = ((context.Interaction as SocketAutocompleteInteraction)?.Data?.Current?.Value?.ToString() ?? string.Empty).ToLowerInvariant();
            var sender = ScopedService.GetRequiredService<ISender>();
            var EventTypes = await sender.Send(new RetrieveAllContentEventTypesQuery());
            var gatheringType = EventTypes.FirstOrDefault(t => t.Name == "Gathering");

            var GatheringSubTypes = await sender.Send(new RetrieveContentEventSubTypesByTypeQuery { TypeId = gatheringType.Id });


            var results = GatheringSubTypes
                .Where(s => s.Name.ToLower().Contains(userInput))
                .Take(25)
                .Select(s => new AutocompleteResult(s.Name, s.Id.ToString()))
                .ToArray();


            return AutocompletionResult.FromSuccess(results);


        }
    }
}
