using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.Module
{
    public abstract class ScopedInteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        protected readonly IScopedDiscordFactory _ScopeFactory;
        protected ScopedInteractionModule(IScopedDiscordFactory scopeFactory)
        {
            _ScopeFactory = scopeFactory;
        }
        protected Task<IServiceScope> CreateScope() => _ScopeFactory.Create(Context.Guild, (IGuildUser)Context.User);

    }
}
