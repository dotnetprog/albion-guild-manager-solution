using AGM.Domain.Identity;

namespace AGM.Domain.Abstractions
{
    public interface IDiscordIdentityManager
    {
        Task<User> GetCurrentUser();
    }
}
