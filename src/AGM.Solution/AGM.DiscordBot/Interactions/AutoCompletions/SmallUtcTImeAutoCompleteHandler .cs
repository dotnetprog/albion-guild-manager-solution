using Discord;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class SmallUtcTImeAutoCompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var currentTime = DateTime.UtcNow;
            //create 12 timers from the current time;
            var result = new List<string>();
            result.Add("Now");
            for (var i = 1; i < 13; i++)
            {
                var mins = i * 5;
                result.Add($"In 0h{mins.ToString("00")}");
            }
            return AutocompletionResult.FromSuccess(result.Select(r => new AutocompleteResult(r.ToString(), r.ToString())).ToArray());
        }
    }
}
