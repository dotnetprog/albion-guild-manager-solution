namespace AGM.Domain.Identity
{
    public class User
    {
        public ulong DiscordUserId { get; set; }
        public string? Username { get; set; }
        public string? Displayname { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
