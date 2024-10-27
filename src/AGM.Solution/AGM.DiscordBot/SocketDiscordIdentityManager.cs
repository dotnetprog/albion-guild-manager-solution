using AGM.Domain.Abstractions;
using AGM.Domain.Identity;
using Discord;

namespace AGM.DiscordBot
{
    public class SocketDiscordIdentityManager : IDiscordIdentityManager
    {
        private IGuildUser User { get; set; }

        public void SetUser(IGuildUser user) { User = user; }

        public Task<User> GetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
