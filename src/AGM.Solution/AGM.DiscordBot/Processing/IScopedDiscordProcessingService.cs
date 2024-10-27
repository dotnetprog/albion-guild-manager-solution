using Discord.WebSocket;

namespace AGM.DiscordBot.Processing
{
    public interface IScopedDiscordProcessingService
    {
        Task OnGuildJoin(SocketGuild socketGuild);
    }
}
