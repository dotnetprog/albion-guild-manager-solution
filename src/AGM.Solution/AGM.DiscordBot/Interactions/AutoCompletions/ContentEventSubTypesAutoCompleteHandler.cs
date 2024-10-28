using AGM.Application.Features.ContentEvents.Queries;
using AGM.DiscordBot.Factory;
using AGM.Domain.Entities;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public class ContentEventSubTypesAutoCompleteHandler : ScopedAutoCompleteHandler
    {
        public ContentEventSubTypesAutoCompleteHandler(IScopedDiscordFactory scopeFactory) : base(scopeFactory)
        {
        }

        public override async Task<AutocompletionResult> GenerateScopedSuggestionsAsync(IServiceProvider ScopedService, IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter)
        {
            var interaction = (context.Interaction as SocketAutocompleteInteraction);
            var selectedType = interaction.Data.Options.FirstOrDefault(o => o.Name == "content")?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(selectedType))
            {
                return AutocompletionResult.FromError(new Exception("You must set the Content Parameter first."));
            }
            var selectedTypeId = new ContentEventTypeId(Guid.Parse(selectedType));
            var userInput = (interaction?.Data?.Current?.Value?.ToString() ?? string.Empty).ToLowerInvariant();
            var sender = ScopedService.GetRequiredService<ISender>();
            var EventTypes = await sender.Send(new RetrieveAllContentEventTypesQuery());

            var selectedTypeFound = EventTypes.FirstOrDefault(e => e.Id == selectedTypeId);
            if (selectedTypeFound == null)
            {
                return AutocompletionResult.FromError(new Exception("The value provided in Content is invalid. You must select the Content from the autocomplete."));
            }
            var subTypes = await sender.Send(new RetrieveContentEventSubTypesByTypeQuery()
            {
                TypeId = selectedTypeFound.Id
            });

            var results = subTypes
                .Where(s => s.Name.ToLower().Contains(userInput))
                .Take(25)
                .Select(s => new AutocompleteResult(s.Name, s.Id.ToString()));

            return AutocompletionResult.FromSuccess(results);



        }
    }
}
