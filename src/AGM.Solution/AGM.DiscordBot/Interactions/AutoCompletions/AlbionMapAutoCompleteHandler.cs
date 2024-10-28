using AGM.Application.Features.Maps.Queries;
using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class AlbionMapAutoCompleteHandler : ScopedAutoCompleteHandler
    {
        public AlbionMapAutoCompleteHandler(IScopedDiscordFactory scopeFactory) : base(scopeFactory)
        {
        }

        public override async Task<AutocompletionResult> GenerateScopedSuggestionsAsync(IServiceProvider ScopedService, IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter)
        {
            var userInput = ((context.Interaction as SocketAutocompleteInteraction)?.Data?.Current?.Value?.ToString() ?? string.Empty).ToLowerInvariant();
            var sender = ScopedService.GetRequiredService<ISender>();

            var Maps = await sender.Send(new RetrieveAlbionAllMapsQuery());

            var result = Maps.Where(m => m.Name.ToLower().Contains(userInput)).Take(25).Select(m => new AutocompleteResult(m.Name, m.Id.ToString()));
            return AutocompletionResult.FromSuccess(result);
        }
    }
}
