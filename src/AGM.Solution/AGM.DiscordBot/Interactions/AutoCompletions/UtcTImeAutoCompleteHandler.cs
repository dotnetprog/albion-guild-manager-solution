﻿using Discord;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class UtcTimeAutoCompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var currentTime = DateTime.UtcNow;// DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour);

            var result = new List<string>();

            result.Add("Now");
            for (var i = 1; i < 12; i++)
            {
                result.Add($"In {i}h00min");
                result.Add($"In {i}h30min");
            }
            return AutocompletionResult.FromSuccess(result.Select(r => new AutocompleteResult(r.ToString(), r.ToString())).ToArray());
        }
    }
}
