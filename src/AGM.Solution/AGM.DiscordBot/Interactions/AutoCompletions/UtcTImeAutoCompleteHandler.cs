using Discord;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class UtcTImeAutoCompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var currentTime = DateTime.UtcNow.Date.AddHours(DateTime.Now.Hour);
            //create 12 timers from the current time;
            var result = new List<DateTime>();
            result.Add(currentTime);
            for (var i = 1; i < 12; i++)
            {
                result.Add(currentTime.AddHours(i));
            }
            return AutocompletionResult.FromSuccess(result.Select(r => new AutocompleteResult(r.ToString(), r.ToString())).ToArray());
        }
    }
}
