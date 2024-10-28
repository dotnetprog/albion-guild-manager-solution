using AGM.Application.Features.ContentEvents.Queries;
using AGM.DiscordBot.Common.Constants;
using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class ContentEventTypesButGatheringAutoCompleteHandler : ScopedAutoCompleteHandler
    {
        public ContentEventTypesButGatheringAutoCompleteHandler(IScopedDiscordFactory scopeFactory) : base(scopeFactory)
        {
        }

        public override async Task<AutocompletionResult> GenerateScopedSuggestionsAsync(IServiceProvider ScopedService, IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter)
        {
            var userInput = ((context.Interaction as SocketAutocompleteInteraction)?.Data?.Current?.Value?.ToString() ?? string.Empty).ToLowerInvariant();
            var sender = ScopedService.GetRequiredService<ISender>();
            var EventTypes = await sender.Send(new RetrieveAllContentEventTypesQuery());

            var result = EventTypes
                .Where(t => t.Name != ContentEventTypeConsts.Gathering && t.Name.ToLower().Contains(userInput))
                .Take(25)
                .Select(r => new AutocompleteResult(r.Name, r.Id.ToString()));
            return AutocompletionResult.FromSuccess(result.ToArray());
        }
    }
}