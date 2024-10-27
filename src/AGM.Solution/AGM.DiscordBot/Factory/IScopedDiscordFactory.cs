using Discord;

namespace AGM.DiscordBot.Factory
{
    public interface IScopedDiscordFactory
    {
        Task<IServiceScope> Create(IGuild Guild, IGuildUser User);

    }
}
