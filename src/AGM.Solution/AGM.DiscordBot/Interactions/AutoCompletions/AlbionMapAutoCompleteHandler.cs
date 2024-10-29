using AGM.Application.Features.Maps.Queries;
using AGM.DiscordBot.Factory;
using AGM.Domain.Enums;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using System.Text.RegularExpressions;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class AlbionMapAutoCompleteHandler : ScopedAutoCompleteHandler
    {
        public AlbionMapAutoCompleteHandler(IScopedDiscordFactory scopeFactory) : base(scopeFactory)
        {
        }


        private readonly Regex _roadRegex = new Regex(@"^[a-zA-Z]{2}$");

        public override async Task<AutocompletionResult> GenerateScopedSuggestionsAsync(IServiceProvider ScopedService, IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter)
        {
            var userInput = ((context.Interaction as SocketAutocompleteInteraction)?.Data?.Current?.Value?.ToString() ?? string.Empty).ToLowerInvariant();
            var sender = ScopedService.GetRequiredService<ISender>();

            var Maps = await sender.Send(new RetrieveAlbionAllMapsQuery());

            if (_roadRegex.IsMatch(userInput))
            {

                var roads = Maps
                    .Where(m => m.Type == AlbionMapType.Road)
                    .Select(m => new { r = m, s = m.Name.Split('-') })
                    .ToList();
                var results = roads.Where(r => r.s[0].ToLower()[0] == userInput[0] && r.s[1].ToLower()[0] == userInput[1]).Select(r => r.r);
                if (results.Any())
                {
                    return AutocompletionResult.FromSuccess(
                   results
                   .Take(25)
                   .Select(m =>
                   new AutocompleteResult(m.Name, m.Id.ToString()))
                   .ToArray());
                }

            }




            var result = Maps.Where(m => m.Name.ToLower().Contains(userInput)).Take(25).Select(m => new AutocompleteResult(m.Name, m.Id.ToString()));
            return AutocompletionResult.FromSuccess(result);
        }
    }
}
