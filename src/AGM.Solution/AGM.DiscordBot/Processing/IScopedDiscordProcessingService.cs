using Discord;
using Discord.WebSocket;

namespace AGM.DiscordBot.Processing
{
    public interface IScopedDiscordProcessingService
    {
        Task OnGuildJoin(SocketGuild socketGuild);

        Task UpdateContentEventSettingsInteraction(IChannel TimerChannel);
    }
}
