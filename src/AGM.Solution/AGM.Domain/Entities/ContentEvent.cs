using Newtonsoft.Json;

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

        public ContentEventTypeId? TypeId { get; set; }
        [JsonIgnore]
        public ContentEventType? Type { get; set; }
        public ContentEventSubTypeId? SubTypeId { get; set; }
        [JsonIgnore]
        public ContentEventSubType? SubType { get; set; }
        public ulong? CreatedByDiscordId { get; set; }
        public ulong? ModifiedByByDiscordId { get; set; }
        public AlbionMapId? AlbionMapId { get; set; }
        [JsonIgnore]
        public AlbionMap? AlbionMap { get; set; }
    }
}
