namespace AGM.Domain.Entities
{
    [StronglyTypedId()]
    public partial struct ContentEventId
    {
    }
    public class ContentEvent : Entity<ContentEventId>
    {
        public DateTime? StartsOn { get; set; }

        public TenantId? TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ContentEventTypeId? ContentEventTypeId { get; set; }
        public ContentEventType? ContentEventType { get; set; }

        public ulong? CreatedByDiscordId { get; set; }
        public ulong? ModifiedByByDiscordId { get; set; }
        public AlbionMapId? AlbionMapId { get; set; }
        public AlbionMap? AlbionMap { get; set; }
    }
}
