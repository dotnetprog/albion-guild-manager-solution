using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.AutoCompletions
{
    public abstract class ScopedAutoCompleteHandler : AutocompleteHandler
    {
        protected readonly IScopedDiscordFactory _ScopeFactory;
        protected ScopedAutoCompleteHandler(IScopedDiscordFactory scopeFactory)
        {
            _ScopeFactory = scopeFactory;
        }

        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            try
            {
                using var scope = await CreateScope(context);
                return await GenerateScopedSuggestionsAsync(scope.ServiceProvider, context, autocompleteInteraction, parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        protected Task<IServiceScope> CreateScope(IInteractionContext Context) => _ScopeFactory.Create(Context.Guild, (IGuildUser)Context.User);
        public abstract Task<AutocompletionResult> GenerateScopedSuggestionsAsync(IServiceProvider ScopedService, IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter);

    }
}
