namespace AGM.Domain.Entities
{
    [StronglyTypedId()]
    public partial struct TenantId
    {
    }
    public class Tenant : Entity<TenantId>
    {
        public ulong DiscordServerId { get; set; }
        public string ServerName { get; set; }

        public ulong? ChannelEventDiscordId { get; set; }
        public ulong? EventMessageDiscordId { get; set; }

    }
}
